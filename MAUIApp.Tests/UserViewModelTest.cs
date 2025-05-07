using System.Collections.ObjectModel;
using MAUIApp.Models;
using MAUIApp.ViewModels;
using MauiCouchbaseApp.Services;
using Moq;
using Xunit;

namespace MAUIApp.ViewModels.Tests
{
    public class UserViewModelTest
    {
        private readonly Mock<IDatabaseService> _mockDatabaseService;

        public UserViewModelTest()
        {
            _mockDatabaseService = new Mock<IDatabaseService>();
            DatabaseService.Instance = _mockDatabaseService.Object;
        }

        [Fact]
        public void Constructor_ShouldInitializeUsersCollection()
        {
            // Arrange
            _mockDatabaseService.Setup(service => service.GetUser()).Returns(new List<UserProfile>());

            // Act
            var viewModel = new UserViewModel();

            // Assert
            Assert.NotNull(viewModel.Users);
            Assert.Empty(viewModel.Users);
        }

        [Fact]
        public void AddTaskCommand_ShouldAddUserToDatabase()
        {
            // Arrange
            var viewModel = new UserViewModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St"
            };

            _mockDatabaseService.Setup(service => service.GetUser()).Returns(new List<UserProfile>());
            _mockDatabaseService.Setup(service => service.AddUser(It.IsAny<UserProfile>()));

            // Act
            viewModel.AddTaskCommand.Execute(null);

            // Assert
            _mockDatabaseService.Verify(service => service.AddUser(It.Is<UserProfile>(user =>
                user.Name == "John Doe" &&
                user.Email == "john.doe@example.com" &&
                user.Address == "123 Main St")), Times.Once);
        }

        [Fact]
        public void ToggleDoneCommand_ShouldCallGetUser()
        {
            // Arrange
            var user = new UserProfile { Id = "1", Name = "Test User" };
            var viewModel = new UserViewModel();

            _mockDatabaseService.Setup(service => service.GetUser()).Returns(new List<UserProfile> { user });

            // Act
            viewModel.ToggleDoneCommand.Execute(user);

            // Assert
            _mockDatabaseService.Verify(service => service.GetUser(), Times.Once);
        }

        [Fact]
        public async Task SelectImageCommand_ShouldSetSelectedImage()
        {
            // Arrange
            var viewModel = new UserViewModel();
            var mockMediaPicker = new Mock<IMediaPicker>();
            var mockFileResult = new Mock<FileResult>();

            mockFileResult.Setup(file => file.OpenReadAsync()).ReturnsAsync(new MemoryStream());
            mockMediaPicker.Setup(picker => picker.PickPhotoAsync(It.IsAny<MediaPickerOptions>())).ReturnsAsync(mockFileResult.Object);

            MediaPicker.Instance = mockMediaPicker.Object;

            // Act
            await viewModel.SelectImageCommand.ExecuteAsync();

            // Assert
            Assert.NotNull(viewModel.SelectedImage);
        }
    }
}