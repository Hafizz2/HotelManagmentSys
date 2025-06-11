using HotelManagmentSys.Controller;
using HotelManagmentSys.DTOs;
using HotelManagmentSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagmentSys
{
    public partial class EditProfileControl : UserControl
    {
        DTOs.UserLoginInfoDTO staff;
        bool drag = false;
        Point startPoint = new Point();

        public EditProfileControl(UserLoginInfoDTO staffed)
        {
            InitializeComponent();
            this.staff = staffed; // Correct assignment
            SettNamebx.Text = staff.FirstName;
            LastTxtbx.Text = staff.LastName;
            AddressTxt.Text = staff.Address;

            // Hook up buttons safely
            CloseBtn.Click += CloseBtn_Click_Internal;
            SubmitBtn.Click += SubmitBtn_Click;
        }

        public event EventHandler SubmitBtnClicked;
        public event EventHandler CloseBtn_Click;

        // Inside your edit profile user control's SubmitBtn_Click event handler

        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            // 1. Basic UI Validation for required profile fields
            if (string.IsNullOrWhiteSpace(SettNamebx.Text) ||
                string.IsNullOrWhiteSpace(LastTxtbx.Text) ||
                string.IsNullOrWhiteSpace(AddressTxt.Text))
            {
                MessageBox.Show("Please Fill All Required Profile Fields (First Name, Last Name, Address).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            string newPassword = Newpassbx.Text; 
            string oldPasswordInput = Oldpassbx.Text; 

            bool passwordChangeRequested = !string.IsNullOrEmpty(newPassword);

            // Instantiate DataAccess
            var staffDataAccess = new StaffDataAccess();

            if (passwordChangeRequested)
            {
                // Validation for password change fields
                if (string.IsNullOrEmpty(oldPasswordInput))
                {
                    MessageBox.Show("Please enter your current password to change it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (oldPasswordInput != AppContextt.CurrentUser.Password) 
                {
                    MessageBox.Show("The current password you entered is incorrect.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Optional: Add check if newPassword is same as oldPassword
                if (newPassword == AppContextt.CurrentUser.Password)
                {
                    MessageBox.Show("New password cannot be the same as the current password.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

            }

            var updatedStaffInfo = new UserLoginInfoDTO
            {
                AccountId = AppContextt.CurrentUser.AccountId, // Use the logged-in AccountId
                FirstName = SettNamebx.Text,
                LastName = LastTxtbx.Text,
                Address = AddressTxt.Text,
            };

            var (message, success) = staffDataAccess.UpdateStaff(updatedStaffInfo, passwordChangeRequested ? newPassword : null);

            MessageBox.Show(message, success ? "Success" : "Failed", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (success)
            {
              
                AppContextt.CurrentUser.FirstName = updatedStaffInfo.FirstName;
                AppContextt.CurrentUser.LastName = updatedStaffInfo.LastName;
                AppContextt.CurrentUser.Address = updatedStaffInfo.Address;
                AppContextt.CurrentUser.PhoneNumber = updatedStaffInfo.PhoneNumber;

                if (passwordChangeRequested)
                {
                    
                    AppContextt.CurrentUser.Password = newPassword;
                    Newpassbx.Clear(); 
                    Oldpassbx.Clear(); 
                }

                // Notify MainForm to reload if needed
                SubmitBtnClicked?.Invoke(this, e);
            }
        }
        private void CloseBtn_Click_Internal(object sender, EventArgs e)
        {
            // Optional: clear fields if needed before closing
            AddressTxt.Clear();
            SettNamebx.Clear();
            Oldpassbx.Clear();
            Newpassbx.Clear();
            LastTxtbx.Clear();

            this.Dispose();
        }

        // Drag logic
        private void EditProfileControl_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            startPoint = e.Location;
        }

        private void EditProfileControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag && this.Parent != null)
            {
                Point currentScreenPos = this.PointToScreen(e.Location);
                Point newLocation = this.Parent.PointToClient(new Point(currentScreenPos.X - startPoint.X, currentScreenPos.Y - startPoint.Y));
                this.Location = new Point(
                    Math.Max(0, Math.Min(this.Parent.Width - this.Width, newLocation.X)),
                    Math.Max(0, Math.Min(this.Parent.Height - this.Height, newLocation.Y))
                );
            }
        }

        private void EditProfileControl_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }
    }

}
