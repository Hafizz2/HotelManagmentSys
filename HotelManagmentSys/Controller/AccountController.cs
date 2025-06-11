using HotelManagmentSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using HotelManagmentSys.DTOs;
using HotelManagmentSys.DB;

namespace HotelManagmentSys.Controller
{
    class AccountController
    {

        public (UserLoginInfoDTO User, string Message) Login(string username, string password)
        {
            using (SqlConnection con = new DB.DBConnection().OpenConnection())
            {
                // Direct query on ACCOUNT table with plain text password (INSECURE)
                string sql = @"SELECT AccountID, UserName, Password, Role , Status
                           FROM ACCOUNT
                           WHERE UserName = @username AND Password = @password AND Status = 'Active'";

                using (SqlCommand cmd = new SqlCommand(sql, con))

                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password); 
                    try
                    {
                        //con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                var userAccount = new UserLoginInfoDTO
                                {
                                    AccountId = Convert.ToInt32(reader["AccountID"]),
                                    UserName = reader["UserName"].ToString(),
                                    Password = reader["Password"].ToString(), 
                                    Role = reader["Role"].ToString(),
                                    Status = reader["Status"].ToString()
                                };

                              
                                con.Close();

                                con.Open();
                                if (userAccount.Role == "Customer")
                                {
                                    // Fetch from CUSTOMER table
                                    using (SqlCommand customerCmd = new SqlCommand(
                                        "SELECT CustomerId, FirstName, LastName, Gender, PhoneNumber, City, State, Country, Age FROM CUSTOMER WHERE AccountId = @AccountId", con))
                                    {
                                        customerCmd.Parameters.AddWithValue("@AccountId", userAccount.AccountId);
                                        using (SqlDataReader customerReader = customerCmd.ExecuteReader())
                                        {
                                            if (customerReader.Read())
                                            {
                                                userAccount.CustomerId = Convert.ToInt32(customerReader["CustomerId"]);
                                                userAccount.FirstName = customerReader["FirstName"].ToString();
                                                userAccount.LastName = customerReader["LastName"].ToString();
                                                userAccount.Gender = customerReader["Gender"].ToString();
                                                userAccount.PhoneNumber = customerReader["PhoneNumber"].ToString();
                                                userAccount.City = customerReader["City"].ToString();
                                                userAccount.State = customerReader["State"].ToString();
                                                userAccount.Country = customerReader["Country"].ToString();
                                                userAccount.Age = Convert.ToInt32(customerReader["Age"]);
                                            }
                                        }
                                    }
                                }
                                else if (userAccount.Role == "Manager" || userAccount.Role == "Reception") // Staff roles
                                {
                                    using (SqlCommand staffCmd = new SqlCommand(
                                        "SELECT StaffId, FirstName, LastName, Address, PhoneNumber FROM STAFF WHERE AccountId = @AccountId", con))
                                    {
                                        staffCmd.Parameters.AddWithValue("@AccountId", userAccount.AccountId);
                                        using (SqlDataReader staffReader = staffCmd.ExecuteReader())
                                        {
                                            if (staffReader.Read())
                                            {
                                                userAccount.StaffId = Convert.ToInt32(staffReader["StaffId"]);
                                                userAccount.FirstName = staffReader["FirstName"].ToString();
                                                userAccount.LastName = staffReader["LastName"].ToString();
                                                userAccount.Address = staffReader["Address"].ToString();
                                                userAccount.PhoneNumber = staffReader["PhoneNumber"].ToString();
                                            }
                                        }
                                    }
                                }

                                return (userAccount, "Login Successful.");
                            }
                            else
                            {
                                return (null, "Invalid username or password Or Deactivated Account"); // No matching account
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Database error during login: {ex.Message}");
                        return (null, "Error: An unexpected error occurred. Please try again.");
                    }
                }
            }
        }

}}
