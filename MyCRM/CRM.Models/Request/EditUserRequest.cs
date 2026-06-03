using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MyCRM.Models.Request
{
    public class EditUserRequest : AddUserRequest
    {
     
        public int Id { get; set; }

    }
}
