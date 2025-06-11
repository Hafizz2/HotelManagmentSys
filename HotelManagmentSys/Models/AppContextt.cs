using HotelManagmentSys.DTOs;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagmentSys.Models
{
    // AppContext.cs (Create this file)
    public static class AppContextt
    {
        // This will hold the information of the currently logged-in user
        // Make sure your UserLoginInfoDTO (or whatever you named it) is accessible here
        public static UserLoginInfoDTO CurrentUser { get; private set; } // Using your renamed DTO

        // Method to set the current user after successful login
        public static void SetCurrentUser(UserLoginInfoDTO user)
        {
            CurrentUser = user;
        }

        // Method to clear the current user on logout
        public static void ClearCurrentUser()
        {
            CurrentUser = null;
        }

        }

}
