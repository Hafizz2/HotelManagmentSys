using HotelManagmentSys.DTOs;
using HotelManagmentSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace HotelManagmentSys.Controller
{
    class ReservationController
    {
        private readonly DB.DBConnection db = new DB.DBConnection();
        

            public (string Message, bool Success) ProcessNewReservation(Customer customer, Reservation reservation)
            {
                SqlConnection conn = null;
                SqlTransaction transaction = null;

                try
                {
                    conn = db.OpenConnection();
                    transaction = conn.BeginTransaction(); // Ensure atomicity

                    int accountId = 0;
                    int customerId = 0;
                    int reservationId = 0;

                    // 1. Insert into ACCOUNT
                    string insertAccountQuery = @"INSERT INTO ACCOUNT (UserName, Password, Role, Status)
                                             OUTPUT INSERTED.AccountID
                                             VALUES (@UserName, @Password, @Role , @Status)";

                    using (SqlCommand accountCmd = new SqlCommand(insertAccountQuery, conn, transaction))
                    {
                        accountCmd.Parameters.AddWithValue("@UserName", customer.UserName);
                        accountCmd.Parameters.AddWithValue("@Password", customer.Password); // Consider hashing this!
                        accountCmd.Parameters.AddWithValue("@Role", customer.Role);
                        accountCmd.Parameters.AddWithValue("@Status","Active");
                        accountId = (int)accountCmd.ExecuteScalar();
                    }
                    customer.AccountID = accountId; // Assign the newly generated AccountID back to the customer object

                    // 2. Insert into CUSTOMER using AccountID
                    string insertCustomerQuery = @"INSERT INTO CUSTOMER (FirstName, LastName, Gender, PhoneNumber, City, State, Country, Age, AccountID)
                                               OUTPUT INSERTED.CustomerId
                                               VALUES (@FirstName, @LastName, @Gender, @PhoneNumber, @City, @State, @Country, @Age, @AccountID)";

                    using (SqlCommand customerCmd = new SqlCommand(insertCustomerQuery, conn, transaction))
                    {
                        customerCmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        customerCmd.Parameters.AddWithValue("@LastName", customer.LastName);
                        customerCmd.Parameters.AddWithValue("@Gender", customer.Gender);
                        customerCmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                        customerCmd.Parameters.AddWithValue("@City", customer.City);
                        customerCmd.Parameters.AddWithValue("@State", customer.State);
                        customerCmd.Parameters.AddWithValue("@Country", customer.Country);
                        customerCmd.Parameters.AddWithValue("@Age", customer.Age);
                        customerCmd.Parameters.AddWithValue("@AccountID", customer.AccountID); // Use the retrieved AccountID
                        customerId = (int)customerCmd.ExecuteScalar();
                    }
                    reservation.CustomerId = customerId; // Assign the newly generated CustomerId to the reservation

                    // 3. Insert into Reservation
                    string insertReservationQuery = @"INSERT INTO Reservation (CustomerId, RoomId, CheckInDate, CheckOutDate, TotalPayment, ReservationStatus)
                                                 OUTPUT INSERTED.ReservationId
                                                 VALUES (@CustomerId, @RoomId, @CheckInDate, @CheckOutDate, @TotalPayment, @ReservationStatus)";

                    using (SqlCommand rsrvCmd = new SqlCommand(insertReservationQuery, conn, transaction))
                    {
                        rsrvCmd.Parameters.AddWithValue("@CustomerId", reservation.CustomerId);
                        rsrvCmd.Parameters.AddWithValue("@RoomId", reservation.RoomId);
                        rsrvCmd.Parameters.AddWithValue("@CheckInDate", reservation.CheckInDate);
                        rsrvCmd.Parameters.AddWithValue("@CheckOutDate", reservation.CheckOutDate);
                        rsrvCmd.Parameters.AddWithValue("@TotalPayment", reservation.TotalPayment);
                        rsrvCmd.Parameters.AddWithValue("@ReservationStatus", "Confirmed"); // Default to 'Confirmed'
                        reservationId = (int)rsrvCmd.ExecuteScalar();
                    }
                    reservation.ReservationId = reservationId; // Assign the newly generated ReservationId

                    // 4. Update Room Status
                    string updateRoomQuery = @"UPDATE ROOM SET Status = @Status WHERE RoomId = @RoomId";

                    using (SqlCommand roomCmd = new SqlCommand(updateRoomQuery, conn, transaction))
                    {
                        roomCmd.Parameters.AddWithValue("@RoomId", reservation.RoomId); // Use RoomId from the reservation
                        roomCmd.Parameters.AddWithValue("@Status", "Occupied"); // Set status to 'Occupied'
                        roomCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return ("Reservation processed successfully!", true);
                }
                catch (SqlException ex)
                {
                    transaction?.Rollback();
                    // Check for specific SQL errors like unique constraints etc.
                    if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint violation (e.g., username already exists)
                    {
                        return ($"Database error: Duplicate entry for Account or Customer. Details: {ex.Message}", false);
                    }
                    return ($"Database error during reservation: {ex.Message}", false);
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    return ($"An unexpected error occurred during reservation: {ex.Message}", false);
                }
                finally
                {
                    db.CloseConnection(); // Ensure connection is closed even on error
                }
            }

            // --- NEW: Method to get Available Rooms (for ComboBoxes) ---
            public List<Room> GetAvailableRooms()
            {
                List<Room> availableRooms = new List<Room>();
                using (SqlConnection conn = db.OpenConnection())
                {
                    string query = "SELECT RoomId, RoomNumber, RoomType, Price FROM ROOM WHERE Status = 'Free'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    availableRooms.Add(new Room
                                    {
                                        RoomId = Convert.ToInt32(reader["RoomId"]),
                                        RoomNumber = reader["RoomNumber"].ToString(),
                                        RoomType = reader["RoomType"].ToString(),
                                        Price = Convert.ToDecimal(reader["Price"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error fetching available rooms: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                return availableRooms;
            }

            // --- NEW: Method to get Room Price by RoomId (for price display) ---
            public decimal GetRoomPrice(int roomId)
            {
                using (SqlConnection conn = db.OpenConnection())
                {
                    string query = "SELECT Price FROM ROOM WHERE RoomId = @RoomId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@RoomId", roomId);
                        try
                        {
                            object result = cmd.ExecuteScalar();
                            return result != DBNull.Value && result != null ? Convert.ToDecimal(result) : 0m;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error getting room price for RoomId {roomId}: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return 0m;
                        }
                    }
                }
            }


        // --- NEW: Method to Get Active Reservations for Display Grid ---
        public List<ActiveReservationDTO> GetActiveReservations()
        {
            List<ActiveReservationDTO> activeReservations = new List<ActiveReservationDTO>();
            using (SqlConnection conn = new DB.DBConnection().OpenConnection())
            {
                // Join Reservation, Room, and Customer tables
                // Filter for reservations that are 'Confirmed' or 'CheckedIn' but not 'CheckedOut' or 'Cancelled'
                // Adjust ReservationStatus values as per your DB
                string query = @"
                    SELECT
                        RES.ReservationId,
                        RES.RoomId,
                        R.RoomNumber,
                        C.CustomerId,
                        C.FirstName + ' ' + C.LastName AS CustomerName, 
                        RES.CheckInDate,
                        RES.CheckOutDate,
                        RES.ReservationStatus
                    FROM RESERVATION AS RES
                    INNER JOIN ROOM AS R ON RES.RoomId = R.RoomId
                    INNER JOIN CUSTOMER AS C ON RES.CustomerId = C.CustomerId
                    WHERE RES.ReservationStatus IN ('Confirmed', 'CheckedIn') 
                    ORDER BY RES.CheckInDate ASC"; // Order by check-in date

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                activeReservations.Add(new ActiveReservationDTO
                                {
                                    ReservationId = Convert.ToInt32(reader["ReservationId"]),
                                    RoomId = Convert.ToInt32(reader["RoomId"]),
                                    CustomerId = Convert.ToInt32(reader["CustomerId"]), // <--- THIS MAPPING IS CRUCIAL
                                    RoomNumber = reader["RoomNumber"].ToString(),
                                    CustomerName = reader["CustomerName"].ToString(),
                                    CheckInDate = Convert.ToDateTime(reader["CheckInDate"]),
                                    CheckOutDate = Convert.ToDateTime(reader["CheckOutDate"]),
                                    ReservationStatus = reader["ReservationStatus"].ToString()
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading active reservations: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return activeReservations;
        }

        // --- NEW: ProcessCheckout Method ---
        public (string Message, bool Success) ProcessCheckout(int reservationId, int roomId, int customerId)
        {
            SqlConnection conn = null;
            SqlTransaction transaction = null;

            try
            {
                conn = db.OpenConnection();
                transaction = conn.BeginTransaction();

                // 1. Update Reservation Status and Actual Check-Out Date
                string updateReservationQuery = @"UPDATE Reservation
                                                  SET ReservationStatus = 'CheckedOut',
                                                      CheckOutDate = GETDATE()
                                                  WHERE ReservationId = @ReservationId";
                using (SqlCommand resCmd = new SqlCommand(updateReservationQuery, conn, transaction))
                {
                    resCmd.Parameters.AddWithValue("@ReservationId", reservationId);
                    int resRowsAffected = resCmd.ExecuteNonQuery();
                    if (resRowsAffected == 0)
                    {
                        throw new Exception($"Reservation ID {reservationId} not found or not updated.");
                    }
                }

                // 2. Update Room Status
                string updateRoomQuery = @"UPDATE ROOM
                                           SET Status = 'Free'
                                           WHERE RoomId = @RoomId";
                using (SqlCommand roomCmd = new SqlCommand(updateRoomQuery, conn, transaction))
                {
                    roomCmd.Parameters.AddWithValue("@RoomId", roomId);
                    int roomRowsAffected = roomCmd.ExecuteNonQuery();
                    if (roomRowsAffected == 0)
                    {
                        throw new Exception($"Room ID {roomId} not found or not updated.");
                    }
                }

                // 3. Get the AccountId for the given CustomerId
                int accountId = 0;
                string getAccountIdQuery = @"SELECT AccountId FROM Customer WHERE CustomerId = @CustomerId";
                using (SqlCommand getAccCmd = new SqlCommand(getAccountIdQuery, conn, transaction))
                {
                    getAccCmd.Parameters.AddWithValue("@CustomerId", customerId);
                    object result = getAccCmd.ExecuteScalar();
                    if (result == null || !int.TryParse(result.ToString(), out accountId))
                    {
                        throw new Exception($"Account not found for Customer ID {customerId}. Cannot deactivate account.");
                    }
                }

                // 4. Deactivate Customer's Account Status in the Account table
                string updateAccountQuery = @"UPDATE Account
                                              SET Status = 'Deactivated'
                                              WHERE AccountId = @AccountId";
                using (SqlCommand accCmd = new SqlCommand(updateAccountQuery, conn, transaction))
                {
                    accCmd.Parameters.AddWithValue("@AccountId", accountId);
                    int accRowsAffected = accCmd.ExecuteNonQuery();
                    if (accRowsAffected == 0)
                    {
                        throw new Exception($"Account ID {accountId} not found or not deactivated.");
                    }
                }

                transaction.Commit();
                return ($"Checkout successful for Reservation ID {reservationId}. Room is now Free and Customer account deactivated.", true);
            }
            catch (SqlException ex)
            {
                transaction?.Rollback();
                return ($"Database error during checkout: {ex.Message}", false);
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                return ($"An unexpected error occurred during checkout: {ex.Message}", false);
            }
            finally
            {
                db.CloseConnection();
            }
        }


            public decimal GenerateReservationReport(string periodType)
        {
            decimal totalRevenue = 0;
            string whereClause = "";
            DateTime startDate;
            DateTime endDate;

            DateTime today = DateTime.Today; // Get today's date

            // Determine date range based on periodType
            switch (periodType.ToLower())
            {
                case "daily": 
                    startDate = today.Date;
                    endDate = today.Date.AddDays(1).AddSeconds(-1); 
                    break;
                case "weekly":
                    // Start of the current week (e.g., Monday). Adjust if your week starts on Sunday.
                    int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
                    startDate = today.AddDays(-1 * diff).Date;
                    endDate = startDate.AddDays(7).AddSeconds(-1); // End of Sunday (if week starts Monday)
                    break;
                case "monthly":
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddSeconds(-1); // End of current month
                    break;
                case "yearly":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = startDate.AddYears(1).AddSeconds(-1); // End of current year
                    break;
                default:
                    MessageBox.Show("Invalid report period type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0; // Return 0 if invalid type
            }

            // Filter by CheckInDate and only for CheckedOut reservations (money generated)
            whereClause = "WHERE RES.CheckInDate BETWEEN @StartDate AND @EndDate AND RES.ReservationStatus = 'CheckedOut'";

            string query = $@"
                SELECT
                    SUM(RES.TotalPayment)
                FROM RESERVATION AS RES
                {whereClause}";

            using (SqlConnection conn = db.OpenConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

                    try
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            totalRevenue = Convert.ToDecimal(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error calculating total revenue: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return totalRevenue;
        }

        // --- NEW: Method to get the active RoomId for a customer ---
        public int GetActiveRoomIdForCustomer(int customerId)
        {
            int roomId = 0; // Default to 0 if no active room is found
            using (SqlConnection conn = db.OpenConnection())
            {
                string query = @"
                    SELECT TOP 1 RES.RoomId
                    FROM RESERVATION AS RES
                    WHERE RES.CustomerId = @CustomerId
                      AND RES.ReservationStatus IN ('Confirmed') 
                      AND GETDATE() BETWEEN RES.CheckInDate AND RES.CheckOutDate -- Ensure current date is within the reservation period
                    ORDER BY RES.CheckInDate DESC; -- Get the most recent valid active reservation if multiple (unlikely but safe)
                ";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    try
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            roomId = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log this exception, but don't stop the app. A customer just might not have an active room.
                        Console.WriteLine($"Error getting active room ID for customer {customerId}: {ex.Message}");
                        // You might want to log this to a file for production
                    }
                }
            }
            return roomId;
        }
    }

  }
    

