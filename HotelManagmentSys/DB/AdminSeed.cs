using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using HotelManagmentSys.DataAccess;
using HotelManagmentSys.DB;
using HotelManagmentSys.Models;

namespace HotelManagmentSys.DataAccess
{
    public class AdminSeeder
    {
        private static readonly string AdminUsername = "admin";

        public void SeedAdmin()
        {
            try
            {
                using (SqlConnection con = new DBConnection().OpenConnection())
                {
                    
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            
                            string checkSql = "SELECT COUNT(*) FROM ACCOUNT WHERE UserName = @Username";
                            using (SqlCommand checkCmd = new SqlCommand(checkSql, con, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@Username", AdminUsername);
                                int count = (int)checkCmd.ExecuteScalar();

                                if (count > 0)
                                {
                                    transaction.Commit();
                                    return; 
                                }
                            }

                            int InsertedAdminId;

                            
                            Account admin = new Account
                            {
                                UserName = AdminUsername,
                                Password = "admin123", 
                                Role = "Manager"
                            };

                            string insertSql = @"INSERT INTO ACCOUNT (UserName, Password, Role)
                                                OUTPUT INSERTED.AccountID
                                                VALUES (@UserName, @Password, @Role);";

                            using (SqlCommand cmd = new SqlCommand(insertSql, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@UserName", admin.UserName);
                                cmd.Parameters.AddWithValue("@Password", admin.Password);
                                cmd.Parameters.AddWithValue("@Role", admin.Role);
                                InsertedAdminId = (int)cmd.ExecuteScalar(); 
                            }

                            
                            string staffSql = @"INSERT INTO STAFF (AccountId, FirstName, LastName, Address, PhoneNumber)
                                                VALUES (@AccountId, @FirstName, @LastName, @Address, @PhoneNumber)";

                            using (SqlCommand staffCmd = new SqlCommand(staffSql, con, transaction))
                            {
                                staffCmd.Parameters.AddWithValue("@AccountId", InsertedAdminId);
                                staffCmd.Parameters.AddWithValue("@FirstName", "Mejmeria");
                                staffCmd.Parameters.AddWithValue("@LastName", "Mukera");
                                staffCmd.Parameters.AddWithValue("@Address", "Addis Ababa, Ethiopia");
                                staffCmd.Parameters.AddWithValue("@PhoneNumber", "+251911000000");

                                int rowsAffected = staffCmd.ExecuteNonQuery();
                                if (rowsAffected != 1)
                                {
                                    throw new Exception("Staff insert failed: No rows affected.");
                                }
                            }

                            
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            
                            transaction.Rollback();
                            throw; 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Admin seeding failed: {ex.Message}", "Seeding Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}