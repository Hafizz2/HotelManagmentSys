
namespace HotelManagmentSys.DTOs
{


   public class UserLoginInfoDTO
    {
        public int AccountId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } 
        public string Role { get; set; } 

        public string Status { get; set; }

        public int? StaffId { get; set; } 
        public int CustomerId { get; set; } 

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; } 
        public string PhoneNumber { get; set; }
        public string Gender { get; set; } 
        public string City { get; set; } // Customer specific
        public string State { get; set; } // Customer specific
        public string Country { get; set; } // Customer specific
        public int? Age { get; set; } // Customer specific

    }

    public class StaffEditDTO
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

        public static implicit operator StaffEditDTO(UserLoginInfoDTO v)
        {
            throw new NotImplementedException();
        }
    }

    public class TodaysReservationDTO
    {
        public string RoomNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime CheckInDate { get; set; }
    }

    public class RecentCustomerRequestDTO
    {
        public int RequestId {  get; set; }
        public string RoomNumber { get; set; }
        public string RequestDescription { get; set; } 
        public string RequestStatus { get; set; }
        public DateTime RequestDate { get; set; } 

    }

    public class ActiveReservationDTO
    {
        public int ReservationId { get; set; } 
        public int RoomId { get; set; }       
        public int CustomerId { get; set; }
        public string RoomNumber { get; set; }
        public string CustomerName { get; set; } 
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string ReservationStatus { get; set; } // e.g., "Confirmed", "CheckedOut", "Cancelled"
    }

    public class ReceptionRequestDTO 
    {
        public int RequestId { get; set; }
        public string CustomerFirstName { get; set; } 
        public string RoomNumber { get; set; }
        public string RequestType { get; set; }
        public string RequestStatus { get; set; } 
        public DateTime RequestDate { get; set; }
    }

}
