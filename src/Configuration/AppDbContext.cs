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

        #region MASTER
        public IMongoCollection<User> Users
        {
            get { return Database.GetCollection<User>("users"); }
        }
        public IMongoCollection<GenericTable> GenericTables
        {
            get { return Database.GetCollection<GenericTable>("generic_tables"); }
        }
        public IMongoCollection<Plan> Plans
        {
            get { return Database.GetCollection<Plan>("plans"); }
        }
        public IMongoCollection<Procedure> Procedures
        {
            get { return Database.GetCollection<Procedure>("procedures"); }
        }
        public IMongoCollection<Billing> Billings
        {
            get { return Database.GetCollection<Billing>("billings"); }
        }
        public IMongoCollection<Seller> Sellers
        {
            get { return Database.GetCollection<Seller>("sellers"); }
        }
        public IMongoCollection<Commission> Commissions
        {
            get { return Database.GetCollection<Commission>("commissions"); }
        }
        public IMongoCollection<AccreditedNetwork> AccreditedNetworks
        {
            get { return Database.GetCollection<AccreditedNetwork>("accredited_networks"); }
        }
        public IMongoCollection<Address> Addresses
        {
            get { return Database.GetCollection<Address>("addresses"); }
        }
        public IMongoCollection<Contact> Contacts
        {
            get { return Database.GetCollection<Contact>("contact"); }
        }
        public IMongoCollection<SellerRepresentative> SellerRepresentatives
        {
            get { return Database.GetCollection<SellerRepresentative>("seller_representatives"); }
        }
        public IMongoCollection<ServiceModule> ServiceModules
        {
            get { return Database.GetCollection<ServiceModule>("service_modules"); }
        }
        public IMongoCollection<Professional> Professionals
        {
            get { return Database.GetCollection<Professional>("professionals"); }
        }
        #endregion

        #region FIN
        public IMongoCollection<AccountsReceivable> AccountsReceivables
        {
            get { return Database.GetCollection<AccountsReceivable>("accounts_receivables"); }
        }
        #endregion
    }
}