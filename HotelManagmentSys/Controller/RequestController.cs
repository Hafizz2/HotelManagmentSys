using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using HotelManagmentSys.DB;
using HotelManagmentSys.DTOs;


namespace HotelManagmentSys.Controller
{
     class RequestController

    {  private DBConnection db = new DBConnection();

            public (string Message, bool Success) CreateNewRequest(int customerId, int roomId, string requestType)
            {
                try
                {
                    using (SqlConnection conn = db.OpenConnection())
                    {
                        string query = @"
                        INSERT INTO REQUEST (CustomerId, RoomId, RequestType, RequestDate, Status)
                        VALUES (@CustomerId, @RoomId, @RequestType, @RequestDate, @Status)";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@CustomerId", customerId);
                            cmd.Parameters.AddWithValue("@RoomId", roomId);
                            cmd.Parameters.AddWithValue("@RequestType", requestType);
                            cmd.Parameters.AddWithValue("@RequestDate", DateTime.Now); // Request time is now
                            cmd.Parameters.AddWithValue("@Status", "Pending");         // Default status
                           
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                return ($"{requestType} request submitted successfully. Status: Pending.", true);
                            }
                            else
                            {
                                return ("Failed to submit request.", false);
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Log the exception details for debugging
                    MessageBox.Show($"Database error creating request: {ex.Message}");
                    return ($"Database error submitting request: {ex.Message}", false);
                }
                catch (Exception ex)
                {
                    // Log the exception details for debugging
                    MessageBox.Show($"Unexpected error creating request: {ex.Message}");
                    return ($"An unexpected error occurred: {ex.Message}", false);
                }
            }

        // --- NEW: Method to get requests for a specific customer ---
        public List<RecentCustomerRequestDTO> GetRequestsForCustomer(int customerId)
        {
            List<RecentCustomerRequestDTO> requests = new List<RecentCustomerRequestDTO>();
            using (SqlConnection conn = db.OpenConnection())
            {
                string query = @"
                    SELECT
                        RQ.RequestId,
                        R.RoomNumber,
                        RQ.RequestType,
                        RQ.Status,
                        RQ.RequestDate
                    FROM REQUEST AS RQ
                    INNER JOIN ROOM AS R ON RQ.RoomId = R.RoomId
                    WHERE RQ.CustomerId = @CustomerId -- Filter by the customer's ID
                    ORDER BY RQ.RequestDate DESC;"; 

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);

                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                requests.Add(new RecentCustomerRequestDTO
                                {
                                    RequestId = Convert.ToInt32(reader["RequestId"]),
                                    RoomNumber = reader["RoomNumber"].ToString(),
                                    RequestDescription = reader["RequestType"].ToString(),
                                    RequestStatus = reader["Status"].ToString(),
                                    RequestDate = Convert.ToDateTime(reader["RequestDate"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading your requests: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return requests;
        }

        // --- NEW: Method to check for existing active requests ---
        public bool HasActiveRequest(int customerId, int roomId, string requestType)
        {
            using (SqlConnection conn = db.OpenConnection())
            {
                // We consider a request "active" if its status is 'Pending' or 'In Progress'.
                // Adjust these statuses if your system uses different terms for active requests.
                string query = @"
                    SELECT COUNT(RequestId)
                    FROM REQUEST
                    WHERE CustomerId = @CustomerId
                      AND RoomId = @RoomId
                      AND RequestType = @RequestType
                      AND Status IN ('Pending', 'In Progress');"; // Define 'active' statuses

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cmd.Parameters.AddWithValue("@RoomId", roomId);
                    cmd.Parameters.AddWithValue("@RequestType", requestType);

                    try
                    {
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        return count > 0; // Returns true if an active request of this type exists
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error checking for active request: {ex.Message}");
                        // You might want to log this error. Default to false to avoid blocking legitimate requests.
                        return false;
                    }
                }
            }
        }

        public List<ReceptionRequestDTO> GetAllRequestsForReception()
        {
            List<ReceptionRequestDTO> requests = new List<ReceptionRequestDTO>();
            try
            {
                using (SqlConnection conn = db.OpenConnection())
                {
                    string query = @"
                        SELECT
                            RQ.RequestId,
                            C.FirstName AS CustomerFirstName,
                            R.RoomNumber,
                            RQ.RequestType,
                            RQ.Status AS RequestStatus,
                            RQ.RequestDate
                        FROM REQUEST AS RQ
                        INNER JOIN ROOM AS R ON RQ.RoomId = R.RoomId
                        INNER JOIN Customer AS C ON RQ.CustomerId = C.CustomerId -- Join with Customer table
                        ORDER BY RQ.RequestDate DESC, RQ.Status ASC; -- Order by most recent and then status
                    "; // You can add TOP N here if you only want to load recent requests, e.g., TOP 200

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                requests.Add(new ReceptionRequestDTO
                                {
                                    RequestId = Convert.ToInt32(reader["RequestId"]),
                                    CustomerFirstName = reader["CustomerFirstName"].ToString(),
                                    RoomNumber = reader["RoomNumber"].ToString(),
                                    RequestType = reader["RequestType"].ToString(),
                                    RequestStatus = reader["RequestStatus"].ToString(), // Map to new DTO property
                                    RequestDate = Convert.ToDateTime(reader["RequestDate"])
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error fetching reception requests: {ex.Message}");
                MessageBox.Show($"Database Error loading reception requests: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error fetching reception requests: {ex.Message}");
                MessageBox.Show($"An unexpected error occurred loading reception requests: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return requests;
        }


        public (string Message, bool Success) UpdateRequestStatus(int requestId, string newStatus)
        {
            try
            {
                using (SqlConnection conn = db.OpenConnection())
                {
                    string query = @"
                        UPDATE REQUEST
                        SET Status = @NewStatus
                        WHERE RequestId = @RequestId;";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@NewStatus", newStatus);
                        cmd.Parameters.AddWithValue("@RequestId", requestId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return ($"Request {requestId} status updated to '{newStatus}'.", true);
                        }
                        else
                        {
                            return ($"Request {requestId} not found or no change made.", false);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error updating request status: {ex.Message}");
                return ($"Database error: {ex.Message}", false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred updating request status: {ex.Message}");
                return ($"An unexpected error occurred: {ex.Message}", false);
            }
        }
    }
}

