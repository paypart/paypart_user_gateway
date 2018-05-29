using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace paypart_user_gateway.Models
{
    public class UserRole
    {
        //public int id { get; set; }
        //[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]

        [Key]
        public int _id { get; set; }
        public string role { get; set; }
        public int status { get; set; }
    }
}
