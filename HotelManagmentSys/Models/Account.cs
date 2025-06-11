using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagmentSys.Models
{
    public class Account
    {
        //Auto-Properties
        public int AccountID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public String Role { get; set; }

    }
}
