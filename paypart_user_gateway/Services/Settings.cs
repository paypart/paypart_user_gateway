
using System;
using System.Linq;

namespace paypart_user_gateway.Services
{
    public class Settings
    {
        public string mongoUrl;
        public string connectionString;
        public string database;
        public string brokerList;
        public string addBillerTopic;
        public int redisCancellationToken;
        public int pLength;
        public string notifyUrl;
        public string resetNotifyBody;
        public string resetNotifySubject;
        public string resetNotifynewpass;
    }
}
