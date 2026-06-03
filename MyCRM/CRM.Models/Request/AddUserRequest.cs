using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Models.Request
{
    public class AddUserRequest
    {
     
        public string Name { get; set; }
        public string LoginAct { get; set; }
        public string? LoginPwd { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int AccountNoExpired { get; set; }
        public int CredentialsNoExpired { get; set; }
        public int AccountNoLocked { get; set; }
        public int AccountEnabled { get; set; }
    }
}
