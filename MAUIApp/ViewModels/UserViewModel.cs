
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using MAUIApp.Models;
using MauiCouchbaseApp.Services;

namespace MAUIApp.ViewModels
{
    public partial class UserViewModel : ObservableObject
    {
        

        public ICommand AddTaskCommand { get; }
        public ICommand ToggleDoneCommand { get; }
        public ICommand SelectImageCommand { get; }
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string email;
        [ObservableProperty]
        private string address;
        [ObservableProperty]
        private string imageData;
        [ObservableProperty]
        private ImageSource selectedImage;
        private ObservableCollection<UserProfile> _users;
        public ObservableCollection<UserProfile> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users)); // important!
            }
        }
        //public ObservableCollection<UserProfile> Users { get; set; } = new();
        public UserViewModel()
        {
            try
            {

                DatabaseService.Init();
                Users = new ObservableCollection<UserProfile>();
                LoadTasks();
                //Sync with server
                //DatabaseService.StartSync("ws://your-couchbase-sync-gateway:4984/db", "username", "password");

                AddTaskCommand = new Command<string>((title) =>
                {
                    var adduserdetail = new UserProfile()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = this.Name,
                        Email = this.Email,
                        Address = this.Address,
                        // ImageData = this.ImageData,
                    };
                    DatabaseService.AddUser(adduserdetail);
                    LoadTasks();
                });
                ToggleDoneCommand = new Command<UserProfile>((user) =>
                {
                    DatabaseService.GetUser();
                    // LoadTasks();
                });
                SelectImageCommand = new Command(async () => await SelectImageAsync());
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadTasks()
        {
            var users = DatabaseService.GetUser();
            ObservableCollection<UserProfile> profiles = new ObservableCollection<UserProfile>(users);
            Users = profiles;
            
        }
        private async Task<bool> RequestPermissionsAsync()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Photos>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Photos>();
                }

                return status == PermissionStatus.Granted;
            }

            return true; // Assume granted on other platforms
        }
        private async Task SelectImageAsync()
        {
            try
            {
                FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please select an image"
                });

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    SelectedImage = ImageSource.FromStream(() => stream);

                    using var ms = new MemoryStream();
                    await stream.CopyToAsync(ms);
                    var imageData = ms.ToArray();
                    // Save or process imageData
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // MediaPicker is not supported on this device
                Console.WriteLine($"Feature not supported: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                Console.WriteLine($"Permission error: {pEx.Message}");
            }
            catch (Exception ex)
            {
                // Something else went wrong
                Console.WriteLine($"Image pick error: {ex.Message}");
            }
        }
    }
}


