using HotelManagmentSys.DTOs;
using HotelManagmentSys.Models;
using System;
using System.Data;
using System.Data.SqlClient;
namespace HotelManagmentSys.Controller
{
    
    public class StaffDataAccess
    {
        private readonly DB.DBConnection db = new DB.DBConnection();
        public void AddStaff(Staff staff)
        {
            SqlConnection conn = db.OpenConnection();

            SqlTransaction transaction = conn.BeginTransaction(); // Ensure atomicity

            try
            {
                // 1. Insert into ACCOUNT
                string insertAccountQuery = @"INSERT INTO Account (UserName, Password, Role)
                                      OUTPUT INSERTED.AccountID
                                      VALUES (@UserName, @Password, @Role)";

                SqlCommand accountCmd = new SqlCommand(insertAccountQuery, conn, transaction);
                accountCmd.Parameters.AddWithValue("@UserName", staff.UserName);
                accountCmd.Parameters.AddWithValue("@Password", staff.Password);
                accountCmd.Parameters.AddWithValue("@Role", staff.Role);

                int accountId = (int)accountCmd.ExecuteScalar(); // Get the new AccountID

                // 2. Insert into STAFF using AccountID
                string insertStaffQuery = @"INSERT INTO Staff (FirstName, LastName, Address, PhoneNumber, AccountID)
                                    VALUES (@FirstName, @LastName, @Address, @PhoneNumber, @AccountID)";
                SqlCommand staffCmd = new SqlCommand(insertStaffQuery, conn, transaction);
                staffCmd.Parameters.AddWithValue("@FirstName", staff.FirstName);
                staffCmd.Parameters.AddWithValue("@LastName", staff.LastName);
                staffCmd.Parameters.AddWithValue("@Address", staff.Address);
                staffCmd.Parameters.AddWithValue("@PhoneNumber", staff.PhoneNumber);
                staffCmd.Parameters.AddWithValue("@AccountID", accountId);

                staffCmd.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                db.CloseConnection();
            }
        }

        public (string, bool) UpdateStaff(UserLoginInfoDTO staff, string newPassword = null)
        {
            SqlConnection connection = null;
            DB.DBConnection dbConnection = new DB.DBConnection(); // Instantiate your DBConnection class

            try
            {
                connection = dbConnection.OpenConnection();
                string updateSql;

                // Determine if password needs to be updated
                if (!string.IsNullOrEmpty(newPassword))
                {
                    updateSql = @"UPDATE STAFF
                              SET FirstName = @FirstName, LastName = @LastName,
                                  Address = @Address
                              WHERE AccountId = @AccountId;

                              UPDATE ACCOUNT
                              SET Password = @NewPassword 
                              WHERE AccountID = @AccountId;";
                }
                else
                {
                    // SQL to only update STAFF table (no password change)
                    updateSql = @"UPDATE STAFF
                              SET FirstName = @FirstName, LastName = @LastName,
                                  Address = @Address
                                   WHERE AccountId = @AccountId;";
                }

                using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
                {
                    // Add parameters for the STAFF table update (always included)
                    updateCommand.Parameters.AddWithValue("@FirstName", staff.FirstName);
                    updateCommand.Parameters.AddWithValue("@LastName", staff.LastName);
                    updateCommand.Parameters.AddWithValue("@Address", staff.Address);
                    updateCommand.Parameters.AddWithValue("@AccountId", staff.AccountId);

                    // Add parameter for the password update, only if a new password is provided
                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        updateCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                    }

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return ("Profile Updated Successfully.", true);
                    }
                    else
                    {
                        // This could happen if AccountId is wrong, or if no data actually changed
                        // (e.g., if you try to update with the exact same values)
                        return ("No changes detected or AccountId not found.", false);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the detailed exception internally for debugging
                Console.WriteLine($"Error in StaffDataAccess.UpdateStaff: {ex.Message}");
                return ("Unable to Update Profile.\nError: " + ex.Message, false);
            }
            finally
            {
                // Ensure the connection is always closed
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    dbConnection.CloseConnection();
                }
            }
        }



        public List<UserLoginInfoDTO> GetAllStaffDetails()
        {
            List<UserLoginInfoDTO> staffList = new List<UserLoginInfoDTO>();
            SqlConnection connection = null;
            DB.DBConnection dbConnection = new DB.DBConnection();

            try
            {
                connection = dbConnection.OpenConnection();
                // Join ACCOUNT and STAFF tables to get full details including Role and Status
                string sql = @"SELECT A.AccountID, A.UserName, A.Role, A.Status, 
                                   S.StaffId, S.FirstName, S.LastName, S.Address, S.PhoneNumber
                           FROM ACCOUNT A
                           JOIN STAFF S ON A.AccountID = S.AccountID
                           WHERE A.Role <> 'Customer'"; // Still exclude customer accounts from staff list

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            staffList.Add(new UserLoginInfoDTO
                            {
                                AccountId = Convert.ToInt32(reader["AccountID"]),
                                UserName = reader["UserName"].ToString(),
                                Role = reader["Role"].ToString(),
                                Status = reader["Status"].ToString(), 
                                StaffId = Convert.ToInt32(reader["StaffId"]),
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Address = reader["Address"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error getting all staff details: {ex.Message}");
                // Log the exception. You might want to return an empty list or throw for calling code to handle.
                // For now, return empty list on error.
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    dbConnection.CloseConnection();
                }
            }
            return staffList;
        }

        // --- UPDATED: DeactivateStaffAccount (sets Status = 'Inactive') ---
        public (string Message, bool Success) DeactivateStaffAccount(int accountId)
        {
            SqlConnection connection = null;
            DB.DBConnection dbConnection = new DB.DBConnection();

            try
            {
                connection = dbConnection.OpenConnection();
                // Update the Status column in the ACCOUNT table
                string sql = @"UPDATE ACCOUNT SET Status = 'Inactive' WHERE AccountID = @AccountId";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@AccountId", accountId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return ($"Account {accountId} successfully deactivated.", true);
                    }
                    else
                    {
                        return ($"Account {accountId} not found or already inactive.", false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deactivating account: {ex.Message}");
                return ($"Error deactivating account: {ex.Message}", false);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    dbConnection.CloseConnection();
                }
            }
        }

        // --- UPDATED: ActivateStaffAccount (sets Status = 'Active', no role parameter needed) ---
        public (string Message, bool Success) ActivateStaffAccount(int accountId) // Removed newRole parameter
        {
            SqlConnection connection = null;
            DB.DBConnection dbConnection = new DB.DBConnection();

            try
            {
                connection = dbConnection.OpenConnection();
                // Update the Status column in the ACCOUNT table
                string sql = @"UPDATE ACCOUNT SET Status = 'Active' WHERE AccountID = @AccountId";

                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@AccountId", accountId);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return ($"Account {accountId} successfully activated.", true);
                    }
                    else
                    {
                        return ($"Account {accountId} not found or already active.", false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error activating account: {ex.Message}");
                return ($"Error activating account: {ex.Message}", false);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    dbConnection.CloseConnection();
                }
            }
        }
    }

}
