using Azure;
using Azure.Core;
using HotelManagmentSys.Controller;
using HotelManagmentSys.DB;
using HotelManagmentSys.DTOs;
using HotelManagmentSys.Models;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Logging;
using System.Data;
using System.Globalization;
using System.Runtime;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagmentSys
{
    public partial class Home : Form
    {
        private StaffDataAccess _staffDataAccess;
        private RoomController _roomController;
        private int _selectedRoomId = 0;
        private DashboardController _dashboardController;
        private ReservationController _reservationController;
        private RequestController _requestController;
        private int _currentCustomerId;
        private int _currentCustomerRoomId;
        private List<UserLoginInfoDTO> _allStaff; 


        public Home()
        {
            InitializeComponent();
            LoginPanel.BringToFront();
            new DataAccess.AdminSeeder().SeedAdmin();
            _staffDataAccess = new StaffDataAccess();
            _roomController = new RoomController();
            SetDefaultState();
            _dashboardController = new DashboardController();
            _reservationController = new ReservationController();
          
            InitializeReportControls();
            _requestController = new RequestController();
            InitializeRequestButtons();

            


        }


        private void RefreshDatas()
        {
            SetupDataGridView();
            SetupRoomDataGridView();
            DashDataGridViews();
            LoadStaffData();
            LoadRoomsData();
            LoadDashboardData();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            LoadStaffData();
            LoadRoomsData();
            LoadDashboardData();
            
            // Select a default period and generate report
            if (cmbReportPeriod.Items.Count > 0)
            {
                cmbReportPeriod.SelectedIndex = 0; // Select 'Weekly' by default
            }
        }
        private void HideAllRoleUserControls()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is UserControl)
                {
                    ctrl.Visible = false;
                }
            }
        }


        private void EditProfileControl_SubmitBtnClicked(object sender, EventArgs e)
        {
            if (AppContextt.CurrentUser != null)
            {
                LoadProfile(AppContextt.CurrentUser);
                MessageBox.Show("Profile data reloaded on tab.", "Refresh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {

            // Inside your login button's Click event handler
            string username = UsernameTxtbx.Text.Trim();
            string password = PassTxtbx.Text;
            NotifyControl notify = new NotifyControl();
            notify.Attach(this);

            tabControl1.TabPages.Clear();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                notify.ShowMessage("Username and Password are required.", "Error", false);
                return;
            }

            try
            {
                var authService = new AccountController(); 
                var (loggedInUser, message) = authService.Login(username, password); 

                if (loggedInUser == null) 
                {
                    notify.ShowMessage(message, "Error", false); 
                    return;
                }

                AppContextt.SetCurrentUser(loggedInUser); 

                switch (loggedInUser.Role) 
                {
                    case "Manager":
                        this.Text = "Manager Dashboard";
                        tabControl1.TabPages.Add(DashboardTab);
                        tabControl1.TabPages.Add(ManagestaffTab);
                        tabControl1.TabPages.Add(ModifyroomTab);
                        tabControl1.TabPages.Add(ReportTab);
                        tabControl1.TabPages.Add(ProfileTab);
                        break;

                    case "Customer":
                        this.Text = "Shebelle Hotel";
                        tabControl1.TabPages.Add(CustomerHomTab);
                        InitializeCustomerInfo();
                        LoadMyRequests();
                        
                        namelbl.Text = AppContextt.CurrentUser.FirstName.ToString();
                        break;

                    case "Reception":
                        this.Text = "Reception Dashboard";
                        tabControl1.TabPages.Add(DashboardTab);
                        tabControl1.TabPages.Add(ReservationTab);
                        tabControl1.TabPages.Add(RequestTab);
                        tabControl1.TabPages.Add(ProfileTab);
                        SetupReceptionRequestsDisplay();
                        LoadReceptionRequests();

                        break;

                    default:
                        notify.ShowMessage("Unknown role assigned.", "Error", false);
                        AppContextt.ClearCurrentUser(); 
                        return;
                }

                notify.ShowMessage("Login successful!", "Success", true);
                LoadProfile(loggedInUser);
                RefreshDatas();
                LoginPanel.Visible = false;
                btnLogout.Visible = true;
                tabControl1.Visible = true;
                tabControl1.SelectedIndex = 0; 
                

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:{ex.Message}");
                AppContextt.ClearCurrentUser();
            }



        }


        private void btnLogout_Click(object sender, EventArgs e)
        {
            AppContextt.ClearCurrentUser(); // Clear the stored user data
            UsernameTxtbx.Clear();
            PassTxtbx.Clear();
            tabControl1.TabPages.Clear();
            tabControl1.Visible = false;
            btnLogout.Visible = false;
            LoginPanel.Visible = true;
            LoginPanel.BringToFront();
            this.Text = "Login Here";

            HideAllRoleUserControls();


            NotifyControl notify = new NotifyControl();
            notify.Attach(this);
            notify.ShowMessage("Logout successful!", "Success", true);
        }

        private void UsernameTxtbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(UsernameTxtbx.Text))
                {
                    UsernameTxtbx.Focus();
                }
                else
                {
                    PassTxtbx.Focus();
                }

            }
        }

        private void PassTxtbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {

                if (string.IsNullOrWhiteSpace(UsernameTxtbx.Text))
                {
                    UsernameTxtbx.Focus();
                }
                else
                {
                    LoginBtn_Click(sender, e);
                }

            }
        }


        public void LoadProfile(UserLoginInfoDTO account)
        {

            FirstNbx.Text = account.FirstName;
            LastNbx.Text = account.LastName;
            Addressbx.Text = account.Address;
            Oldpassbx.Text = account.Password;

        }


        private void ReserveBtn_Click(object sender, EventArgs e)
        {
            HideAllRoleUserControls();
            // Create the user control
            ReservationControl reserve = new ReservationControl();
            this.Controls.Add(reserve);
            reserve.BringToFront();
        }
        private void SubmitBtn_Click(object sender, EventArgs e)
        {
            var control = Controls["EditProfileControl"];
            Controls.Remove(control);
            control.Dispose();
        }


        private void SettNameBtn_Click(object sender, EventArgs e)
        {
            if (Controls.ContainsKey("EditProfileControl")) return;
            EditProfileControl editControl = new EditProfileControl
                (
                new UserLoginInfoDTO
                {
                    FirstName = FirstNbx.Text,
                    LastName = LastNbx.Text,
                    Address = Addressbx.Text,
                }
                );
            Controls.Add(editControl);
            editControl.BringToFront();
            
        }


        // The event handler method that will be called when StaffDataAdded is raised
        private void HandleStaffDataAdded(object sender, EventArgs e)
        {
            MessageBox.Show("New staff member added, refreshing dashboard data!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadStaffData(); // Call your main refresh method
        }


        private void AddStaffBtn_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is AddStaffContrl existingAddStaffControl) // Use pattern matching for type check and variable assignment
                {
                    existingAddStaffControl.BringToFront();
                    existingAddStaffControl.Visible = true;
                    return; 
                }
            }

            AddStaffContrl addStaffControl = new AddStaffContrl(); // Create the user control instance

            addStaffControl.StaffDataAdded += HandleStaffDataAdded;
            this.Controls.Add(addStaffControl);
            addStaffControl.BringToFront();
        }

        private void ShowPass_MouseDown(object sender, MouseEventArgs e)
        {
            PassTxtbx.UseSystemPasswordChar = false;
        }

        private void ShowPass_MouseUp(object sender, MouseEventArgs e)
        {
            PassTxtbx.UseSystemPasswordChar = true;
        }

        private void eyeBtn_MouseDown(object sender, MouseEventArgs e)
        {
            Oldpassbx.UseSystemPasswordChar = false;
        }

        private void eyeBtn_MouseUp(object sender, MouseEventArgs e)
        {
            Oldpassbx.UseSystemPasswordChar = true;
        }


        private void LoadStaffData()
        {
            _allStaff = _staffDataAccess.GetAllStaffDetails(); // Assuming this method exists in your StaffController

            staffDataGridView.DataSource = null;
            staffDataGridView.DataSource = _allStaff;
            ApplyStaffFilter();
        }

        private void SesarchTxtbx_TextChanged(object sender, EventArgs e)
        {
            ApplyStaffFilter(); 
        }

        private void ApplyStaffFilter()
        {
            string searchText = SesarchTxtbx.Text.Trim(); 

            if (string.IsNullOrEmpty(searchText))
            {
                // If search box is empty, show all original staff
                staffDataGridView.DataSource = _allStaff;
            }
            else
            {
                // Filter the _allStaff list using LINQ
                List<UserLoginInfoDTO> filteredStaff = _allStaff
                    .Where(staff =>
                        staff.FirstName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        staff.LastName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        staff.Role.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        staff.UserName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        staff.Status.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                    )
                    .ToList();

                staffDataGridView.DataSource = filteredStaff;
            }

            staffDataGridView.Refresh(); // Ensure the DataGridView updates its display
        }

        
        private void SetupDataGridView()
        {
            //For Staff Data
            staffDataGridView.AutoGenerateColumns = false;
            staffDataGridView.Columns.Clear();

            staffDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "AccountId", DataPropertyName = "AccountId", HeaderText = "Account ID", ReadOnly = true });
            staffDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "FirstName", DataPropertyName = "FirstName", HeaderText = "First Name", ReadOnly = true });
            staffDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "LastName", DataPropertyName = "LastName", HeaderText = "Last Name", ReadOnly = true });
            staffDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Role", DataPropertyName = "Role", HeaderText = "Role", ReadOnly = true });
            staffDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Status", ReadOnly = true });

            // Add a single, dynamic Toggle Status button column
            DataGridViewButtonColumn toggleStatusBtnCol = new DataGridViewButtonColumn();
            toggleStatusBtnCol.Name = "ToggleStatusCol"; // Name for identification
            toggleStatusBtnCol.HeaderText = "Action";
            toggleStatusBtnCol.UseColumnTextForButtonValue = false; // IMPORTANT: Text will be set dynamically per row
            staffDataGridView.Columns.Add(toggleStatusBtnCol);

            // Subscribe to CellContentClick for button clicks
            staffDataGridView.CellContentClick += StaffDataGridView_CellContentClick;
            // NEW: Subscribe to CellFormatting to set button text dynamically
            staffDataGridView.CellFormatting += StaffDataGridView_CellFormatting;



            // Setup for ActiveReservationsDGV
            ActiveReservationsDGV.AutoGenerateColumns = false;
            ActiveReservationsDGV.Columns.Clear();

            ActiveReservationsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "ResId", DataPropertyName = "ReservationId", HeaderText = "Res ID", Visible = false }); // Hidden, but needed for checkout
            ActiveReservationsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "RoomId", DataPropertyName = "RoomId", HeaderText = "Room ID", Visible = false }); // Hidden, but needed for checkout
            ActiveReservationsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "RoomNumber", DataPropertyName = "RoomNumber", HeaderText = "Room No." });
            ActiveReservationsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "CustomerName", DataPropertyName = "CustomerName", HeaderText = "Customer Name" });
            ActiveReservationsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "CheckInDate", DataPropertyName = "CheckInDate", HeaderText = "Check-In", DefaultCellStyle = { Format = "yyyy-MM-dd" } });
            ActiveReservationsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "CheckOutDate", DataPropertyName = "CheckOutDate", HeaderText = "Check-Out", DefaultCellStyle = { Format = "yyyy-MM-dd" } });
            ActiveReservationsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReservationStatus", DataPropertyName = "ReservationStatus", HeaderText = "Status" });

            // Add Checkout Button Column
            DataGridViewButtonColumn checkoutBtnCol = new DataGridViewButtonColumn();
            checkoutBtnCol.Name = "CheckoutCol";
            checkoutBtnCol.HeaderText = "Action";
            checkoutBtnCol.Text = "Check Out";
            checkoutBtnCol.UseColumnTextForButtonValue = true;
            ActiveReservationsDGV.Columns.Add(checkoutBtnCol);

            // Make columns read-only (except the button)
            foreach (DataGridViewColumn col in ActiveReservationsDGV.Columns)
            {
                if (col.Name != "CheckoutCol") // Allow button to be clickable
                {
                    col.ReadOnly = true;
                }
            }

            // Subscribe to CellContentClick for the Checkout button
            ActiveReservationsDGV.CellContentClick += ActiveReservationsDGV_CellContentClick;


        }

        private void StaffDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if it's our "ToggleStatusCol" and a valid row
            if (e.RowIndex >= 0 && staffDataGridView.Columns[e.ColumnIndex].Name == "ToggleStatusCol")
            {
                // Get the data item for the current row
                UserLoginInfoDTO staff = staffDataGridView.Rows[e.RowIndex].DataBoundItem as UserLoginInfoDTO;

                if (staff != null)
                {
                    // Set the button text based on the staff's status
                    if (staff.Status == "Active")
                    {
                        e.Value = "Deactivate";
                    }
                    else if (staff.Status == "Inactive")
                    {
                        e.Value = "Activate";
                    }
                    else // Fallback for unexpected status
                    {
                        e.Value = "Active";
                        
                    }
                    e.FormattingApplied = true; // Indicate that we've handled the formatting
                }
            }
        }

        private void StaffDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if it's our "ToggleStatusCol" and a valid row
            if (e.RowIndex >= 0 && staffDataGridView.Columns[e.ColumnIndex].Name == "ToggleStatusCol")
            {
                UserLoginInfoDTO selectedStaff = staffDataGridView.Rows[e.RowIndex].DataBoundItem as UserLoginInfoDTO;
                if (selectedStaff == null) return;

                // Prevent deactivating self
                if (selectedStaff.AccountId == AppContextt.CurrentUser.AccountId)
                {
                    MessageBox.Show("You cannot change the status of your own account.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var (message, success) = (string.Empty, false); // Initialize result variables

                if (selectedStaff.Status == "Active")
                {
                    // If currently Active, user wants to Deactivate
                    if (MessageBox.Show($"Are you sure you want to DEACTIVATE {selectedStaff.FirstName} {selectedStaff.LastName}?", "Confirm Deactivation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        (message, success) = _staffDataAccess.DeactivateStaffAccount(selectedStaff.AccountId);
                    }
                }
                else if (selectedStaff.Status == "Inactive")
                {
                    // If currently Inactive, user wants to Activate
                    if (MessageBox.Show($"Are you sure you want to ACTIVATE {selectedStaff.FirstName} {selectedStaff.LastName}?", "Confirm Activation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        (message, success) = _staffDataAccess.ActivateStaffAccount(selectedStaff.AccountId);
                    }
                }
                else
                {
                    // Handle unexpected status (e.g., if database has an unknown status)
                    MessageBox.Show($"Cannot process status for {selectedStaff.FirstName} {selectedStaff.LastName}: Unknown Status '{selectedStaff.Status}'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Display outcome and refresh data if successful
                if (!string.IsNullOrEmpty(message)) // Only show message if an action was attempted
                {
                    MessageBox.Show(message, success ? "Success" : "Failed", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                }

                if (success)
                {
                    LoadStaffData(); // Reload data to show updated status and button text
                }
            }
        }


        private void SetupRoomDataGridView()
        {
            RoomsDataGridView.AutoGenerateColumns = false;
            RoomsDataGridView.Columns.Clear();

            // Add columns. RoomId is included, RoomNumber is also displayed.
            RoomsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "RoomId", DataPropertyName = "RoomId", HeaderText = "ID", ReadOnly = true }); // Display RoomId
            RoomsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "RoomNumber", DataPropertyName = "RoomNumber", HeaderText = "Room No.", ReadOnly = true });
            RoomsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "RoomType", DataPropertyName = "RoomType", HeaderText = "Type", ReadOnly = true });
            RoomsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", DataPropertyName = "Price", HeaderText = "Price", ReadOnly = true });
            RoomsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", DataPropertyName = "Status", HeaderText = "Status", ReadOnly = true });

            // Add a Delete Button Column per row
            DataGridViewButtonColumn deleteBtnCol = new DataGridViewButtonColumn();
            deleteBtnCol.Name = "DeleteCol";
            deleteBtnCol.HeaderText = "Delete";
            deleteBtnCol.Text = "Delete";
            deleteBtnCol.UseColumnTextForButtonValue = true;
            RoomsDataGridView.Columns.Add(deleteBtnCol);

            // --- Subscribe to Events ---
            RoomsDataGridView.CellClick += RoomsDataGridView_CellClick; // For populating fields
            RoomsDataGridView.CellContentClick += RoomsDataGridView_CellContentClick; // For delete button in row
        }

        // --- B. Load Data into DataGridView ---
        private void LoadRoomsData()
        {
            List<Room> rooms = _roomController.GetAllRooms();
            RoomsDataGridView.DataSource = null;
            RoomsDataGridView.DataSource = rooms;

        }

        

        private void SetDefaultState()
        {
            RoomNumberTxt.Clear();
            RoomTypeCbx.SelectedIndex = -1;
            PriceTxt.Clear();

            RoomNumberTxt.ReadOnly = false; 
                                            

            SaveUpdateButton.Text = "Save"; 
            _selectedRoomId = 0;           
        }

        
        private void RoomsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0 && RoomsDataGridView.Columns[e.ColumnIndex].Name != "DeleteCol")
            {
                DataGridViewRow selectedRow = RoomsDataGridView.Rows[e.RowIndex];
                Room selectedRoom = selectedRow.DataBoundItem as Room;

                if (selectedRoom != null)
                {
                    
                    RoomNumberTxt.Text = selectedRoom.RoomNumber;
                    RoomTypeCbx.SelectedItem = selectedRoom.RoomType;
                    PriceTxt.Text = selectedRoom.Price.ToString(CultureInfo.InvariantCulture);

                    
                    RoomNumberTxt.ReadOnly = true;

                  
                    SaveUpdateButton.Text = "Update";

                 
                    _selectedRoomId = selectedRoom.RoomId;
                }
            }
        }

        
        private void RoomsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0 && RoomsDataGridView.Columns[e.ColumnIndex].Name == "DeleteCol")
            {
                Room selectedRoom = RoomsDataGridView.Rows[e.RowIndex].DataBoundItem as Room;

                if (selectedRoom != null)
                {
                    if (MessageBox.Show($"Are you sure you want to DELETE Room Number '{selectedRoom.RoomNumber}' (ID: {selectedRoom.RoomId})?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        var (message, success) = _roomController.DeleteRoom(selectedRoom.RoomId); // Pass RoomId for deletion
                        MessageBox.Show(message, success ? "Success" : "Failed", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                        if (success)
                        {
                            LoadRoomsData(); 
                            SetDefaultState(); 
                        }
                    }
                }
            }
        }

        
        private void SaveUpdateButton_Click(object sender, EventArgs e)
        {
            // 1. Input Validation
            if (string.IsNullOrWhiteSpace(RoomNumberTxt.Text) ||
                string.IsNullOrWhiteSpace(RoomTypeCbx.Text) ||
                string.IsNullOrWhiteSpace(PriceTxt.Text))
            {
                MessageBox.Show("Please fill in Room Number, Room Type, and Price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(PriceTxt.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
            {
                MessageBox.Show("Please enter a valid number for Price.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (price < 0)
            {
                MessageBox.Show("Price cannot be negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Create Room object from UI inputs
            Room room = new Room
            {
                RoomNumber = RoomNumberTxt.Text.Trim(),
                RoomType = RoomTypeCbx.Text.Trim(),
                Price = price,
            };

            (string Message, bool Success) response;

            if (SaveUpdateButton.Text == "Save")
            {
                // Add New Room
                response = _roomController.AddRoom(room);
            }
            else // Button text is "Update"
            {
                // Update Existing Room
                if (_selectedRoomId == 0) // Sanity check: ensure a room ID is selected
                {
                    MessageBox.Show("No room selected for update. Please select a room from the list.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                room.RoomId = _selectedRoomId; // Assign the stored primary key
                response = _roomController.UpdateRoom(room);
            }

            // 3. Display Feedback and Refresh Data
            MessageBox.Show(response.Message, response.Success ? "Success" : "Failed", MessageBoxButtons.OK, response.Success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (response.Success)
            {
                LoadRoomsData(); // Reload DataGridView to reflect changes
                SetDefaultState(); // Reset input fields and button
                RefreshDatas();
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            SetDefaultState(); // Reset UI to its default "Add New Room" state
        }

        private void DashDataGridViews()
        {
            // Setup for TodaysReservationsDGV
            TodaysReservationsDGV.AutoGenerateColumns = false;
            TodaysReservationsDGV.Columns.Clear();
            TodaysReservationsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "ResRoomNumber", DataPropertyName = "RoomNumber", HeaderText = "Room No." });
            TodaysReservationsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "ResCustomerName", DataPropertyName = "CustomerName", HeaderText = "Customer Name" });
            TodaysReservationsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "ResCheckInDate", DataPropertyName = "CheckInDate", HeaderText = "Check-In Date", DefaultCellStyle = { Format = "yyyy-MM-dd" } }); // Format date

            // Setup for RecentRequestsDGV
            RecentRequestsDGV.AutoGenerateColumns = false;
            RecentRequestsDGV.Columns.Clear();
            RecentRequestsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReqRoomNumber", DataPropertyName = "RoomNumber", HeaderText = "Room No." });
            RecentRequestsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReqType", DataPropertyName = "RequestType", HeaderText = "Request" });
            RecentRequestsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReqStatus", DataPropertyName = "RequestStatus", HeaderText = "Status" });

            //

            // Setup for Request Data grid
            RequestsDGV.AutoGenerateColumns = false; // We'll define columns manually
            RequestsDGV.Columns.Clear(); // Clear any existing columns

            RequestsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReqId", DataPropertyName = "RequestId", HeaderText = "ID", Width = 50 });
            RequestsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "RoomNo", DataPropertyName = "RoomNumber", HeaderText = "Room No.", Width = 70 });
            RequestsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReqType", DataPropertyName = "RequestDescription", HeaderText = "Request Type", Width = 150 });
            RequestsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReqDate", DataPropertyName = "RequestDate", HeaderText = "Date", DefaultCellStyle = { Format = "yyyy-MM-dd HH:mm" }, Width = 120 });
            RequestsDGV.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReqStatus", DataPropertyName = "RequestStatus", HeaderText = "Status", Width = 100 });

            // Make columns read-only
            foreach (DataGridViewColumn col in RequestsDGV.Columns)
            {
                col.ReadOnly = true;
            }

            // Make columns read-only for both DataGridViews if they are just for display
            foreach (DataGridViewColumn col in TodaysReservationsDGV.Columns) col.ReadOnly = true;
            foreach (DataGridViewColumn col in RecentRequestsDGV.Columns) col.ReadOnly = true;
            
        }

        // --- B. Load Data into DataGridViews ---
        // Assuming this is within your Dashboard or Main Form class
        // Make sure you have 'using System.Data;' at the top of your file.

        private void LoadDashboardData()
        {
            try
            {
               DataTable todaysReservationsTable = _dashboardController.GetTodaysReservationsDataTable(); // Assuming it's in _reservationController
                TodaysReservationsDGV.DataSource = null;

                TodaysReservationsDGV.DataSource = todaysReservationsTable;


                // Load Recent Customer Requests (e.g., top 10) using the DataTable method
                // Make sure your _dashboardController or _requestController
                // has the GetRecentCustomerRequestsDataTable() method as discussed.
                DataTable recentRequestsTable = _dashboardController.GetRecentCustomerRequestsDataTable(10); // Assuming it's in _requestController
                RecentRequestsDGV.DataSource = null;

                RecentRequestsDGV.DataSource = recentRequestsTable;


                // Load Counts for Labels using ExecuteScalar methods (these remain unchanged)
                TotalRoomsLabel.Text = _dashboardController.GetTotalRoomsCount().ToString();
                ReservedRoomsLabel.Text = _dashboardController.GetReservedRoomsCount().ToString();
                AvailableRoomsLabel.Text = _dashboardController.GetAvailableRoomsCount().ToString();
                TotalCustomersLabel.Text = _dashboardController.GetTotalCustomersCount().ToString();

                // Load Active Reservations (assuming this still uses List<DTO> for now)
                List<ActiveReservationDTO> activeReservations = _reservationController.GetActiveReservations();
                ActiveReservationsDGV.DataSource = null;
                ActiveReservationsDGV.DataSource = activeReservations;
            }
            catch (Exception ex)
            {
                // It's good to log the full exception details in a real app, not just show message
                MessageBox.Show($"Failed to load dashboard data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ActiveReservationsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if it's the "CheckoutCol" and a valid row
            if (e.RowIndex >= 0 && ActiveReservationsDGV.Columns[e.ColumnIndex].Name == "CheckoutCol")
            {
                ActiveReservationDTO selectedReservation = ActiveReservationsDGV.Rows[e.RowIndex].DataBoundItem as ActiveReservationDTO;

                if (selectedReservation != null)
                {
                    // Optional: More specific confirmation based on reservation status
                    if (selectedReservation.ReservationStatus == "CheckedOut")
                    {
                        MessageBox.Show("This reservation is already checked out.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (selectedReservation.ReservationStatus == "Cancelled")
                    {
                        MessageBox.Show("This reservation has been cancelled.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (MessageBox.Show($"Confirm checkout for Room No. {selectedReservation.RoomNumber} ({selectedReservation.CustomerName})?", "Confirm Checkout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        // Call the ReservationController to process checkout
                        var (message, success) = _reservationController.ProcessCheckout(selectedReservation.ReservationId, selectedReservation.RoomId , selectedReservation.CustomerId);

                        MessageBox.Show(message, success ? "Success" : "Failed", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                        if (success)
                        {
                            LoadDashboardData(); // Refresh all dashboard data after checkout
                        }
                    }
                }
            }


        }

        private void InitializeReportControls()
        {
            // Populate ComboBox if not done in designer
            if (cmbReportPeriod.Items.Count == 0)
            {
                cmbReportPeriod.Items.Add("Daily");
                cmbReportPeriod.Items.Add("Weekly");
                cmbReportPeriod.Items.Add("Monthly");
                cmbReportPeriod.Items.Add("Yearly");
            }

            // Wire up event handler
            cmbReportPeriod.SelectedIndexChanged += cmbReportPeriod_SelectedIndexChanged;
        }

        private void cmbReportPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void GenerateReport()
        {
            if (cmbReportPeriod.SelectedItem == null)
            {
                lblTotalRevenue.Text = "Total Revenue: $0.00";
                return;
            }

            string periodType = cmbReportPeriod.SelectedItem.ToString();

            // Call the simplified controller method
            decimal totalRevenue = _reservationController.GenerateReservationReport(periodType);

            lblTotalRevenue.Text = $"Total Revenue: {totalRevenue.ToString("C", CultureInfo.CurrentCulture)}";
        }
       
        private void InitializeCustomerInfo()
        {
            if (AppContextt.CurrentUser != null && AppContextt.CurrentUser.CustomerId > 0)
            {
                _currentCustomerId = AppContextt.CurrentUser.CustomerId;

                // Use ReservationController to find the customer's active room ID
                _currentCustomerRoomId = _reservationController.GetActiveRoomIdForCustomer(_currentCustomerId);

                if (_currentCustomerRoomId == 0) // Or whatever indicates no active room
                {
                    MessageBox.Show("No active room found for your account. You cannot submit requests at this time.", "No Active Room", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Optionally disable all request buttons
                    DisableRequestButtons();
                }
            }
            else
            {
                MessageBox.Show("Customer information not found. Please log in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //DisableRequestButtons();
                //// Potentially close the form or redirect to login
                //this.Close();
            }
        }

        private void DisableRequestButtons()
        {
            // Iterate through all controls or specific buttons to disable them
            foreach (Control control in this.Controls) // Adjust scope if buttons are in a panel
            {
                if (control is Button btn && (btn.Name.Contains("Service") || btn.Name.Contains("Checkout") || btn.Name.Contains("Security") || btn.Name.Contains("Complaint"))) // Or check for a specific tag
                {
                    btn.Enabled = false;
                }
            }
        }

        // --- NEW: Method to load and display requests for the current customer ---
        public void LoadMyRequests()

        {
            if (_currentCustomerId > 0)
            {
                var requests = _requestController.GetRequestsForCustomer(_currentCustomerId);
                RequestsDGV.DataSource = null;
                RequestsDGV.DataSource = requests;

                if (requests.Count == 0)
                {
                    // Optional: Show a message if no requests
                    MessageBox.Show("You currently have no requests.", "No Requests Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // This case should be handled by InitializeCustomerInfo, but good to have
                RequestsDGV.DataSource = null;
            }
        }

        private void InitializeRequestButtons()
        {
            // Wire up your buttons to a common event handler or individual handlers
            Request1.Click += RequestButton_Click;
            Request2.Click += RequestButton_Click;
            Request3.Click += RequestButton_Click;
            Request4.Click += RequestButton_Click;
            Request5.Click += RequestButton_Click;
            Request6.Click += RequestButton_Click;
            Request6.Click += RequestButton_Click;
            Request7.Click += RequestButton_Click;
        }


        // --- Common Event Handler for Request Buttons ---
        private void RequestButton_Click(object sender, EventArgs e)
        {
            if (_currentCustomerRoomId == 0) // Basic check for active room
            {
                MessageBox.Show("You need to have an active reservation/room to submit requests.", "Cannot Submit Request", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                string requestType = clickedButton.Text;

                // --- NEW CHECK: Prevent adding if an active request already exists ---
                if (_requestController.HasActiveRequest(_currentCustomerId, _currentCustomerRoomId, requestType))
                {
                    MessageBox.Show($"You already have an active '{requestType}' request. Please wait for it to be completed.", "Duplicate Request", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return; 
                }
                 // Call CreateNewRequest without the description parameter
                var (message, success) = _requestController.CreateNewRequest(_currentCustomerId, _currentCustomerRoomId, requestType);

                MessageBox.Show(message, success ? "Request Submitted" : "Request Failed", MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                if (success)
                {
                    LoadMyRequests(); 
                }

            }
        }


        private void SetupReceptionRequestsDisplay()
        {
            dgvReceptionRequests.AutoGenerateColumns = false;
            dgvReceptionRequests.Columns.Clear();

            // Request ID (Visible for debugging/reference, can be hidden later)
            dgvReceptionRequests.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReqId", DataPropertyName = "RequestId", HeaderText = "ID", Width = 50, Visible = true });
            // New Customer Name Columns
            dgvReceptionRequests.Columns.Add(new DataGridViewTextBoxColumn { Name = "CustFName", DataPropertyName = "CustomerFirstName", HeaderText = "First Name", Width = 100, ReadOnly = true });
            dgvReceptionRequests.Columns.Add(new DataGridViewTextBoxColumn { Name = "RoomNo", DataPropertyName = "RoomNumber", HeaderText = "Room No.", Width = 80, ReadOnly = true });
            dgvReceptionRequests.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReqType", DataPropertyName = "RequestType", HeaderText = "Request Type", Width = 150, ReadOnly = true });
            dgvReceptionRequests.Columns.Add(new DataGridViewTextBoxColumn { Name = "ReqDate", DataPropertyName = "RequestDate", HeaderText = "Date", DefaultCellStyle = { Format = "yyyy-MM-dd HH:mm" }, Width = 130, ReadOnly = true });

            // --- Status ComboBox Column ---
            DataGridViewComboBoxColumn statusColumn = new DataGridViewComboBoxColumn();
            statusColumn.Name = "ReqStatus";
            statusColumn.HeaderText = "Status";
            statusColumn.DataPropertyName = "RequestStatus"; 
            statusColumn.Width = 120;
            statusColumn.FlatStyle = FlatStyle.Flat;

            // Add the possible statuses. These should ideally match values in your database's REQUEST.Status column.
            statusColumn.Items.Add("Pending");      // Initial status for new requests
            statusColumn.Items.Add("In Progress");  // Reception starts working on it
            statusColumn.Items.Add("On way");       // If a staff member is en route
            statusColumn.Items.Add("Delivered");    // If the service/item is delivered
            statusColumn.Items.Add("Completed");    // A final status indicating task done (can be same as Delivered)
            statusColumn.Items.Add("Cancelled");    // If the request was cancelled

            dgvReceptionRequests.Columns.Add(statusColumn);

            // Make all other columns read-only as only status is editable here
            foreach (DataGridViewColumn col in dgvReceptionRequests.Columns)
            {
                if (col.Name != "ReqStatus") // Allow ReqStatus to be editable
                {
                    col.ReadOnly = true;
                }
            }

            dgvReceptionRequests.AllowUserToAddRows = false;
            dgvReceptionRequests.AllowUserToDeleteRows = false;
            dgvReceptionRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReceptionRequests.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        public void LoadReceptionRequests()
        {
            List<ReceptionRequestDTO> requests = _requestController.GetAllRequestsForReception();
            dgvReceptionRequests.DataSource = null;

            dgvReceptionRequests.DataSource = requests;

            
        }

        private void dgvReceptionRequests_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvReceptionRequests.IsCurrentCellDirty)
            {
                dgvReceptionRequests.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

         private void dgvReceptionRequests_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dgvReceptionRequests.Columns[e.ColumnIndex].Name == "ReqStatus")
            {
                if (dgvReceptionRequests.Rows[e.RowIndex].Cells["ReqStatus"].Value == null)
                {
                    MessageBox.Show("Please select a valid status.", "Invalid Status", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadReceptionRequests(); // Reload to revert invalid change
                    return;
                }

                // Get the RequestId for the row
                int requestId = Convert.ToInt32(dgvReceptionRequests.Rows[e.RowIndex].Cells["ReqId"].Value);

                // Get the new status value from the ComboBox
                string newStatus = dgvReceptionRequests.Rows[e.RowIndex].Cells["ReqStatus"].Value.ToString();

                // Call the controller to update the database
                var (message, success) = _requestController.UpdateRequestStatus(requestId, newStatus);

                if (success)
                {
                    MessageBox.Show(message, "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                else
                {
                    MessageBox.Show($"Failed to update request {requestId}: {message}", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadReceptionRequests(); 
                }
            }
        }

    }
}



