
using Couchbase.Lite;
using Couchbase.Lite.Query;
using MAUIApp.Models;
using Couchbase.Lite.Sync;

namespace MauiCouchbaseApp.Services
{
    public class DatabaseService
    {
        private static Database _database;
        private const string DbName = "UserProfile";
        private static Replicator _replicator;

        public static void Init()
        {
            if (_database != null) return;

            var config = new DatabaseConfiguration
            {
                Directory = FileSystem.AppDataDirectory,
                //EncryptionKey = new EncryptionKey("YourStrongPassword")
            };

            _database = new Database(DbName, config);
        }

        public static void AddUser(UserProfile userprofiledata)
        {
            var mutableDocument = new MutableDocument(userprofiledata.Id);
            mutableDocument.SetString("name", userprofiledata.Name);
            mutableDocument.SetString("email", userprofiledata.Email);
            mutableDocument.SetString("address", userprofiledata.Address);
            mutableDocument.SetString("type", "user");
            //if (userprofiledata.ImageData != null)
            //{
            //    mutableDocument.SetBlob("imageData", new Blob("image/jpeg", userprofiledata.ImageData));
            //}
            _database.Save(mutableDocument);
        }

        public static List<UserProfile> GetUser()
        {
            var users = new List<UserProfile>();

            var query = QueryBuilder.Select(SelectResult.Expression(Meta.ID),
                                             SelectResult.Property("name"),
                                             SelectResult.Property("email"),
                                             SelectResult.Property("address")
                                             )
                .From(DataSource.Database(_database));
                

            var result = query.Execute();

            foreach (var row in result)
            {
                users.Add(new UserProfile
                {
                    Id = row.GetString("id"),
                    Name = row.GetString("name"),
                    Email = row.GetString("email"),
                    Address = row.GetString("address"),
                   // ImageData = row.GetBlob("imageData").Content,
                   

                });
            }

            return users;
        }

        public static void StartSync(string syncUrl, string username, string password)
        {
            var targetEndpoint = new URLEndpoint(new Uri(syncUrl));
            var config = new ReplicatorConfiguration(_database, targetEndpoint)
            {
                ReplicatorType = ReplicatorType.PushAndPull,
                Continuous = true,
                Authenticator = new BasicAuthenticator(username, password)
            };

            _replicator = new Replicator(config);
            _replicator.Start();
        }

        public static void StopSync() => _replicator?.Stop();
        public static void Close() => _database?.Close();
    }
}



