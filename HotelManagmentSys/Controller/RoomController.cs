using HotelManagmentSys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagmentSys.Controller
{
    public class RoomController
    {

        public (string Message, bool Success) AddRoom(Room room)
        {
            using (SqlConnection conn = new DB.DBConnection().OpenConnection())
            {
                string query = @"INSERT INTO ROOM (RoomNumber, RoomType, Price, Status)
                                 VALUES (@RoomNumber, @RoomType, @Price, @Status)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomNumber", room.RoomNumber);
                    cmd.Parameters.AddWithValue("@RoomType", room.RoomType);
                    cmd.Parameters.AddWithValue("@Price", room.Price);
                    cmd.Parameters.AddWithValue("@Status", "Free");

                    try
                    {
                        cmd.ExecuteNonQuery();
                        // If we get here, the insert was successful.
                        return ("Room added successfully.", true);
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627 || ex.Number == 2601) // Unique constraint violation
                        {
                            // Return a more specific error message in this case
                            return ($"Error adding room: Room number '{room.RoomNumber}' already exists.  Please use a different room number.", false);
                        }
                        // For other SQL exceptions, include the error message.
                        return ($"Database error adding room: {ex.Message}", false);
                    }
                    catch (Exception ex)
                    {
                        // For general exceptions, include the error message.
                        return ($"An unexpected error occurred: {ex.Message}", false);
                    }
                }
            }
        }

        // --- NEW: GetAllRooms Method ---
        public List<Room> GetAllRooms()
        {
            List<Room> rooms = new List<Room>();
            using (SqlConnection conn = new DB.DBConnection().OpenConnection())
            {
                string query = "SELECT RoomId ,RoomNumber, RoomType, Price, Status FROM ROOM";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rooms.Add(new Room
                                {
                                    RoomId = Convert.ToInt32(reader["RoomId"]), 
                                    RoomNumber = reader["RoomNumber"].ToString(),
                                    RoomType = reader["RoomType"].ToString(),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    Status = reader["Status"].ToString() // Read the Status
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error getting all rooms: {ex.Message}"); // Log the error
                                                                                     // You might want to return an empty list or throw an exception here
                    }
                }
            }
            return rooms;
        }

        // --- NEW: UpdateRoom Method ---
        // This will NOT update the Status column, as per your requirement.
        public (string Message, bool Success) UpdateRoom(Room room)
        {
            using (SqlConnection conn = new DB.DBConnection().OpenConnection())
            {
                string query = @"UPDATE ROOM
                             SET RoomType = @RoomType, Price = @Price
                             WHERE RoomId = @RoomId"; // Update based on RoomNumber

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomType", room.RoomType);
                    cmd.Parameters.AddWithValue("@Price", room.Price);
                    cmd.Parameters.AddWithValue("@RoomId", room.RoomId);// Identify the room to update

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return ($"Room {room.RoomId} updated successfully.", true);
                        }
                        return ($"Room {room.RoomId} not found or no changes made.", false);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating room: {ex.Message}"); // Log the error
                        return ($"An error occurred while updating room: {ex.Message}", false);
                    }
                }
            }
        }

        // --- NEW: DeleteRoom Method ---
        public (string Message, bool Success) DeleteRoom(int roomId)
        {
            using (SqlConnection conn = new DB.DBConnection().OpenConnection())
            {
                string query = "DELETE FROM ROOM WHERE RoomId = @RoomId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RoomId", roomId);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return ($"Room {roomId} deleted successfully.", true);
                        }
                        return ($"Room {roomId} not found.", false);
                    }
                    catch (SqlException sqlEx)
                    {
                        // Check for foreign key constraints (e.g., if room has reservations)
                        if (sqlEx.Number == 547) // Foreign Key Violation
                        {
                            return ("Cannot delete room: It is currently involved in reservations or other records.", false);
                        }
                        MessageBox.Show($"SQL Error deleting room: {sqlEx.Message}"); // Log the error
                        return ($"Database error deleting room: {sqlEx.Message}", false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"General Error deleting room: {ex.Message}"); // Log the error
                        return ($"An unexpected error occurred: {ex.Message}", false);
                    }
                }
            }
        }



    }
}