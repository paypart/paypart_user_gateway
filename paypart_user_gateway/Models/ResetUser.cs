using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace paypart_user_gateway.Models
{
    public class ResetUser
    {
        public string email { get; set; }
        public string newpass { get; set; }
    }
}
