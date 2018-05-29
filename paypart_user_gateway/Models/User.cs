using System;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace paypart_user_gateway.Models
{
    public enum Status
    {
        Pending,
        Active,
        Deleted
    }
    public class User
    {
        [Key]
        //[BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public int _id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int role_id { get; set; }
        public int billerid { get; set; }
        public DateTime datecreated { get; set; }
        public Nullable<DateTime> lastlogin { get; set; }
        public string createdby { get; set; }
        public int status { get; set; }

        public UserRole role { get; set; }
    }
}
