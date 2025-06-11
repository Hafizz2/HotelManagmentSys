using HotelManagmentSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagmentSys
{
     class ReservationDTO
    {
            public Customer Customer { get; set; }
            public Reservation Reservation { get; set; }
            public Account Account { get; set; }
        }

    }