using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagmentSys.Models
{
    class Customer
    {
        public int AccountID { get; set; }  // Auto-generated
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }

        // ACCOUNT table properties
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }


    }
}
