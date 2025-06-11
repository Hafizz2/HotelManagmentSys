using HotelManagmentSys.Controller;
using HotelManagmentSys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagmentSys
{
    public partial class ReservationControl : UserControl
    {

        bool drag = false;
        Point startPoint = new Point();
        private ReservationController _reservationController;
        private List<Room> _availableRooms; // To hold all available rooms fetched from DB
        private Room _selectedRoom; // To store the currently selected room object


        public ReservationControl()
        {
            InitializeComponent();
            _reservationController = new ReservationController();
            InitializeReservationForm(); // Call a method to set up and load data
        }

        private void InitializeReservationForm()
        {
            //LoadRoomTypes();
            LoadAvailableRooms(); // Load all available rooms initially
            DOBpick.MaxDate = DateTime.Today.AddYears(-18);
            CheckIn.Value = DateTime.Today; // Default check-in to today
            CheckOut.Value = DateTime.Today.AddDays(1); // Default check-out to tomorrow
            CalculateTotalPayment(); // Initial calculation
        }

        // --- Private Helper Methods for UI Logic ---

        private void LoadAvailableRooms()
        {
            _availableRooms = _reservationController.GetAvailableRooms();
            // Clear previous items from RoomTypeCbx to repopulate
            RoomTypeCbx.Items.Clear();

            // Get distinct room types from available rooms
            var distinctRoomTypes = _availableRooms.Select(r => r.RoomType).Distinct().ToList();
            foreach (var type in distinctRoomTypes)
            {
                RoomTypeCbx.Items.Add(type);
            }
            RoomTypeCbx.SelectedIndex = -1; // No selection initially
            RoomNumberCbx.Items.Clear(); // Clear room numbers until type is selected
            PriceTxt.Text = "";
            TotalPaymentLabel.Text = "";
            _selectedRoom = null; // Clear selected room
        }
       
        private void LoadRoomNumbers(string roomType)
        {
            RoomNumberCbx.Items.Clear();
            var roomsOfType = _availableRooms.Where(r => r.RoomType == roomType).ToList();
            foreach (var room in roomsOfType)
            {
                // Display RoomNumber, but store the whole Room object or its ID
                // For simplicity in ComboBox, we'll display RoomNumber and store RoomId as ValueMember
                RoomNumberCbx.Items.Add(room.RoomNumber); // Display room number
            }
            RoomNumberCbx.SelectedIndex = -1; // No selection initially
        }


        private void CalculateTotalPayment()
        {
            if (_selectedRoom == null) // Only check if a room is selected
            {
                TotalPaymentLabel.Text = "0.00";
                return;
            }

            DateTime checkIn = CheckIn.Value.Date;
            DateTime checkOut = CheckOut.Value.Date;

            if (checkOut <= checkIn)
            {
                MessageBox.Show("Check-out date must be after check-in date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TotalPaymentLabel.Text = "0.00";
                return;
            }

            TimeSpan duration = checkOut - checkIn;
            // Ensure at least 1 night if check-out is exactly the next day (e.g., 24 hours apart)
            // Or if it's a partial day (e.g., check-in 10AM, check-out 2PM next day -> 1 night)
            int numberOfNights = (int)Math.Ceiling(duration.TotalDays);
            if (numberOfNights == 0 && duration.TotalDays > 0) numberOfNights = 1; // For same-day checkout but duration > 0

            decimal pricePerNight = _selectedRoom.Price;
            decimal total = pricePerNight * numberOfNights;

            // Changed "Birr" to "C" for standard currency formatting
            TotalPaymentLabel.Text = total.ToString("C", CultureInfo.CurrentCulture);
        }

        private void ClearFormFields()
        {
            FirstNameTxt.Clear();
            LastNameTxt.Clear();
            PhoneTxt.Clear();
            CityBx.Clear();
            StateBx.Clear();
            CountryBx.Clear();
            MaleRad.Checked = true; // Default gender
            FemaleRad.Checked = false;

            RoomTypeCbx.SelectedIndex = -1;
            RoomNumberCbx.Items.Clear(); // Clear room numbers
            PriceTxt.Text = "";
            TotalPaymentLabel.Text = "";
            CheckIn.Value = DateTime.Today;
            CheckOut.Value = DateTime.Today.AddDays(1);
            _selectedRoom = null; // Clear the selected room
            LoadAvailableRooms(); // Reload available rooms in case statuses changed
        }

        // --- UI Event Handlers ---

        private void RoomTypeCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RoomTypeCbx.SelectedItem != null)
            {
                string selectedType = RoomTypeCbx.SelectedItem.ToString();
                LoadRoomNumbers(selectedType);
                RoomNumberCbx.SelectedIndex = -1; // Reset room number selection
                PriceTxt.Text = ""; // Clear price when type changes
                _selectedRoom = null; // Clear selected room
                CalculateTotalPayment(); // Recalculate (will likely be 0)
            }
        }

        private void RoomNumberCbx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RoomNumberCbx.SelectedItem != null && _availableRooms != null)
            {
                string selectedRoomNumber = RoomNumberCbx.SelectedItem.ToString();
                // Find the full Room object based on the selected RoomNumber
                _selectedRoom = _availableRooms.FirstOrDefault(r => r.RoomNumber == selectedRoomNumber);

                if (_selectedRoom != null)
                {
                    PriceTxt.Text = _selectedRoom.Price.ToString("C", CultureInfo.CurrentCulture); // Display price per night
                    CalculateTotalPayment(); // Recalculate total based on new room price
                }
                else
                {
                    PriceTxt.Text = "";
                    TotalPaymentLabel.Text = "0.00";
                }
            }
        }

        private void CheckIn_ValueChanged(object sender, EventArgs e)
        {
            // Ensure CheckOut is not before CheckIn
            if (CheckIn.Value.Date > CheckOut.Value.Date)
            {
                CheckOut.Value = CheckIn.Value.AddDays(1);
            }
            CalculateTotalPayment();
        }

        private void CheckOut_ValueChanged(object sender, EventArgs e)
        {
            // Ensure CheckOut is not before CheckIn
            if (CheckOut.Value.Date <= CheckIn.Value.Date)
            {
                MessageBox.Show("Check-out date must be after check-in date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CheckOut.Value = CheckIn.Value.AddDays(1); // Set to next day by default
            }
            CalculateTotalPayment();
        }

        // --- Submit Button Click Event ---
        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            // 1. Input Validation
            if (string.IsNullOrWhiteSpace(FirstNameTxt.Text) ||
                string.IsNullOrWhiteSpace(LastNameTxt.Text) ||
                RoomTypeCbx.SelectedItem == null || // Check if an item is selected
                RoomNumberCbx.SelectedItem == null || // Check if a room number is selected
                _selectedRoom == null || // Ensure a Room object is truly selected
                string.IsNullOrWhiteSpace(PhoneTxt.Text) ||
                string.IsNullOrWhiteSpace(CityBx.Text) ||
                string.IsNullOrWhiteSpace(StateBx.Text) ||
                string.IsNullOrWhiteSpace(CountryBx.Text))
            {
                MessageBox.Show("Please fill in all customer fields and select a room.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (CheckIn.Value.Date >= CheckOut.Value.Date)
            {
                MessageBox.Show("Check-out date must be after check-in date.", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal totalPayment;
            if (!decimal.TryParse(TotalPaymentLabel.Text.Replace(CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol, ""), NumberStyles.Currency, CultureInfo.CurrentCulture, out totalPayment) || totalPayment <= 0)
            {
                MessageBox.Show("Total payment calculation error or payment is zero/negative. Please check room selection and dates.", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // 2. Prepare Customer and Reservation Objects
            DateTime today = DateTime.Today;
            DateTime dob = DOBpick.Value;
            int age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--; // Adjust for birthday not yet passed this year

            string gender = MaleRad.Checked ? "Male" : "Female";

            Customer newCustomer = new Customer
            {
                UserName = GenerateUsername(), // From your helper method
                Password = GeneratePassword(), // From your helper method (Remember to hash for real apps!)
                Role = "Customer", // Default role
                FirstName = FirstNameTxt.Text,
                LastName = LastNameTxt.Text,
                Gender = gender,
                PhoneNumber = PhoneTxt.Text,
                City = CityBx.Text,
                State = StateBx.Text,
                Country = CountryBx.Text,
                Age = age
            };

            Reservation newReservation = new Reservation
            {
                RoomId = _selectedRoom.RoomId, // Use the RoomId from the selected Room object
                CheckInDate = CheckIn.Value.Date,
                CheckOutDate = CheckOut.Value.Date,
                TotalPayment = totalPayment, // Use the calculated total payment
                ReservationStatus = "Confirmed" // Default status for new reservation
            };

            // 3. Call Controller to process reservation
            try
            {
                var (message, success) = _reservationController.ProcessNewReservation(newCustomer, newReservation);

                MessageBox.Show(message, success ? "Success" : "Failed", MessageBoxButtons.OK, success ? MessageBoxIcon.Information: MessageBoxIcon.Error);

                if (success)
                {
                    ClearFormFields(); // Clear all fields and reset UI
                    MessageBox.Show($"The New Customer Account Credential\n" +
                $"Username: {newCustomer.UserName}\n" +
                $"Password: {newCustomer.Password}",
                "Account Created Successfully", // This is the title of the message box
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);// Optionally refresh dashboard data if this screen is part of it
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private string GenerateUsername()
        {
            return "User" + new Random().Next(1000, 9999);
        }
        private string GeneratePassword()
        {
            Random rnd = new Random();
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string digits = rnd.Next(10, 100).ToString(); // 2 digits
            string alpha = new string(Enumerable.Range(0, 2).Select(x => letters[rnd.Next(letters.Length)]).ToArray());
            return digits + alpha;
        }




        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            CountryBx.Clear();
            StateBx.Clear();
            FirstNameTxt.Clear();
            LastNameTxt.Clear();
            PhoneTxt.Clear();
            CityBx.Clear();

        }

        private void ReservationControl_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            startPoint = e.Location;
        }

        private void ReservationControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                if (this.Parent != null)
                {
                    Point currentScreenPos = this.PointToScreen(e.Location);
                    Point newLocation = this.Parent.PointToClient(new Point(currentScreenPos.X - startPoint.X, currentScreenPos.Y - startPoint.Y));

                    // Boundaries
                    int newX = Math.Max(0, Math.Min(this.Parent.Width - this.Width, newLocation.X));
                    int newY = Math.Max(0, Math.Min(this.Parent.Height - this.Height, newLocation.Y));

                    this.Location = new Point(newX, newY);
                }

            }
        }

        private void ReservationControl_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

    }
}

