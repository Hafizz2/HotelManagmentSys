using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagmentSys.Models
{
   public class Staff
    {

        // STAFF table properties
        public int StaffId { get; set; } // Auto-incremented, not required for insert
        public int AccountId { get; set; } // Foreign key to ACCOUNT.Id
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        // ACCOUNT table properties
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
