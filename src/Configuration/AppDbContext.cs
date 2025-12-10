using api_slim.src.Models;
using MongoDB.Driver;

namespace api_slim.src.Configuration
{
    public class AppDbContext
    {
        public static string? ConnectionString { get; set; }
        public static string? DatabaseName { get; set; }
        public static bool IsSSL { get; set; }
        private IMongoDatabase Database { get; }

        public AppDbContext()
        {
            try
            {
                MongoClientSettings mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
                if (IsSSL)
                {
                    mongoClientSettings.SslSettings = new SslSettings
                    {
                        EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
                    };
                }
                
                var mongoClient = new MongoClient(mongoClientSettings);
                Database = mongoClient.GetDatabase(DatabaseName);
            }
            catch
            {
                throw new Exception("Failed to connect to database.");
            }
        }

        public IMongoCollection<User> Users
        {
            get { return Database.GetCollection<User>("users"); }
        }

        #region FIN
        public IMongoCollection<AccountsReceivable> AccountsReceivables
        {
            get { return Database.GetCollection<AccountsReceivable>("accounts_receivables"); }
        }
        #endregion
    }
}