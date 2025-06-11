using HotelManagmentSys.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagmentSys.Controller
{
     class DashboardController
    {
        // --- Method to Get Today's Reservations ---
        public DataTable GetTodaysReservationsDataTable()
        {
            DataTable reservationsTable = new DataTable();
            using (SqlConnection conn = new DB.DBConnection().OpenConnection())
            {
                // SQL query to join Reservations, Rooms, and Customers tables
                // Filter for today's check-in date.
                // IMPORTANT: Alias columns to match your DataGridView's DataPropertyName
                string query = @"
                    SELECT
                        RES.ReservationId, -- Include ID if you might need it later
                        R.RoomNumber,
                        C.FirstName + ' ' + C.LastName AS CustomerName, -- Concatenate for full name
                        RES.CheckInDate,
                        RES.CheckOutDate,
                        RES.ReservationStatus,
                        RES.RoomId, -- Include RoomId for checkout logic
                        RES.CustomerId -- Include CustomerId if needed elsewhere
                    FROM RESERVATION AS RES
                    INNER JOIN ROOM AS R ON RES.RoomId = R.RoomId
                    INNER JOIN CUSTOMER AS C ON RES.CustomerId = C.CustomerId
                    WHERE CAST(RES.CheckInDate AS DATE) = CAST(GETDATE() AS DATE)
                    AND RES.ReservationStatus IN ('Checked In', 'Confirmed') -- Filter for relevant statuses
                    ORDER BY RES.CheckInDate ASC, R.RoomNumber ASC;"; // Order for better display

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(reservationsTable); // Fill the DataTable with data
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading today's reservations (DataTable): {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // In a real application, you'd log this error.
                    }
                }
            }
            return reservationsTable;
        }
        // --- Method to Get Recent Customer Requests ---
        public DataTable GetRecentCustomerRequestsDataTable(int limit = 5)
        {
            DataTable requestsTable = new DataTable();
            using (SqlConnection conn = new DB.DBConnection().OpenConnection())
            {
                 string query = $@"
                    SELECT TOP (@Limit)
                        RQ.RequestId,
                        R.RoomNumber,
                        RQ.RequestType,
                        RQ.Status AS RequestStatus, -- Alias 'Status' to 'RequestStatus' for DGV binding
                        RQ.RequestDate
                    FROM REQUEST AS RQ
                    INNER JOIN ROOM AS R ON RQ.RoomId = R.RoomId
                    ORDER BY RQ.RequestDate DESC;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Limit", limit);
                    try
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(requestsTable); // Fill the DataTable with data
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading recent customer requests (DataTable): {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // In a real application, you'd log this error.
                    }
                }
            }
            return requestsTable;
        }
        public int GetTotalRoomsCount()
                {
                using (SqlConnection conn = new DB.DBConnection().OpenConnection())
                {
                    string query = "SELECT COUNT(*) FROM ROOM";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            object result = cmd.ExecuteScalar(); // Returns the single count
                            return result != null ? Convert.ToInt32(result) : 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error getting total rooms count: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return 0; // Return 0 in case of error
                        }
                    }
                }
            }

            // --- NEW: Method to get Reserved Rooms Count ---
            public int GetReservedRoomsCount()
            {
                using (SqlConnection conn = new DB.DBConnection().OpenConnection())
                {
                    string query = "SELECT COUNT(*) FROM ROOM WHERE Status = 'Occupied'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            object result = cmd.ExecuteScalar();
                            return result != null ? Convert.ToInt32(result) : 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error getting reserved rooms count: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return 0;
                        }
                    }
                }
            }

            // --- NEW: Method to get Available Rooms Count ---
            public int GetAvailableRoomsCount()
            {
                using (SqlConnection conn = new DB.DBConnection().OpenConnection())
                {
                    string query = "SELECT COUNT(*) FROM ROOM WHERE Status = 'Free'";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            object result = cmd.ExecuteScalar();
                            return result != null ? Convert.ToInt32(result) : 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error getting available rooms count: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return 0;
                        }
                    }
                }
            }

            // --- NEW: Method to get Total Customers Count ---
            public int GetTotalCustomersCount()
            {
                using (SqlConnection conn = new DB.DBConnection().OpenConnection())
                {
                    string query = "SELECT COUNT(*) FROM CUSTOMER";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            object result = cmd.ExecuteScalar();
                            return result != null ? Convert.ToInt32(result) : 0;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error getting total customers count: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return 0;
                        }
                    }
                }
            }
     }
}

