using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HotelManagmentSys.Models;
using HotelManagmentSys.DataAccess;

namespace HotelManagmentSys
{
    public partial class AddStaffContrl : UserControl
    {
        bool drag = false;
        private Point startPoint;
        public event EventHandler StaffDataAdded;

        public AddStaffContrl()
        {
            InitializeComponent();
            
        }

        protected virtual void OnStaffDataAdded()
        {
            // Check if there are any subscribers before invoking the event
            StaffDataAdded?.Invoke(this, EventArgs.Empty);
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Addressbx.Clear();
            LastNamebx.Clear();
            FirstNamebx.Clear();
            SettNewpassbx.Clear();
            SettConfpassbx.Clear();
            RoleComboBox.SelectedIndex = -1;
            Phonebx.Clear();
            Usernamebx.Clear();
            
        }

        private void AddStaffContrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                drag = true;
                startPoint = e.Location;
            }
        }

        private void AddStaffContrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag && this.Parent != null)
            {
                // Calculate the new position
               
                int newLeft = this.Left + (e.X - startPoint.X);
                int newTop = this.Top + (e.Y - startPoint.Y);

                // Boundary check
                newLeft = Math.Max(0, Math.Min(this.Parent.Width - this.Width, newLeft));
                newTop = Math.Max(0, Math.Min(this.Parent.Height - this.Height, newTop));

                this.Left = newLeft;
                this.Top = newTop;
            }
        }

        private void AddStaffContrl_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }


        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(Usernamebx.Text) ||
                string.IsNullOrWhiteSpace(SettConfpassbx.Text) ||
                string.IsNullOrWhiteSpace(RoleComboBox.SelectedItem?.ToString()) ||
                string.IsNullOrWhiteSpace(FirstNamebx.Text) ||
                string.IsNullOrWhiteSpace(LastNamebx.Text) ||
                string.IsNullOrWhiteSpace(Addressbx.Text) ||
                string.IsNullOrWhiteSpace(Phonebx.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Create Staff object
            Staff staff = new Staff
            {
                UserName = Usernamebx.Text,
                Password = SettConfpassbx.Text, // TODO: Hash password
                Role = RoleComboBox.SelectedItem.ToString(),
                FirstName = FirstNamebx.Text,
                LastName = LastNamebx.Text,
                Address = Addressbx.Text,
                PhoneNumber = Phonebx.Text
            };

            // Save to database
            try
            {
               new Controller.StaffDataAccess().AddStaff(staff);
                
                MessageBox.Show("Staff added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OnStaffDataAdded();

                Addressbx.Clear();
                LastNamebx.Clear();
                FirstNamebx.Clear();
                SettNewpassbx.Clear();
                SettConfpassbx.Clear();
               RoleComboBox.SelectedIndex = -1;
                Phonebx.Clear();
                Usernamebx.Clear();
    }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
    }
}
