using Microsoft.Extensions.Options;
using MongoDB.Driver;
using paypart_user_gateway.Models;

namespace paypart_user_gateway.Services
{
    public class UserMongoContext
    {
        private readonly IMongoDatabase _database = null;

        public UserMongoContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.connectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.database);
        }

        public IMongoCollection<User> Users
        {
            get
            {
                return _database.GetCollection<User>("users");
            }
        }
    }
}
