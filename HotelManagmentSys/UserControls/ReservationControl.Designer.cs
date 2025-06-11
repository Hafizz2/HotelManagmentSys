namespace HotelManagmentSys
{
    partial class ReservationControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            PriceTxt = new TextBox();
            DOBpick = new DateTimePicker();
            label12 = new Label();
            RoomNumberCbx = new ComboBox();
            RoomTypeCbx = new ComboBox();
            CountryBx = new TextBox();
            StateBx = new TextBox();
            CityBx = new TextBox();
            PhoneTxt = new TextBox();
            FemaleRad = new RadioButton();
            MaleRad = new RadioButton();
            LastNameTxt = new TextBox();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            SubmitBtn = new Button();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            FirstNameTxt = new TextBox();
            label2 = new Label();
            label1 = new Label();
            CloseBtn = new Button();
            label13 = new Label();
            TotalPaymentLabel = new Label();
            CheckIn = new DateTimePicker();
            CheckOut = new DateTimePicker();
            SuspendLayout();
            // 
            // PriceTxt
            // 
            PriceTxt.Font = new Font("Segoe UI", 10F);
            PriceTxt.Location = new Point(486, 313);
            PriceTxt.Name = "PriceTxt";
            PriceTxt.Size = new Size(62, 30);
            PriceTxt.TabIndex = 55;
            // 
            // DOBpick
            // 
            DOBpick.Format = DateTimePickerFormat.Short;
            DOBpick.Location = new Point(500, 137);
            DOBpick.MinDate = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            DOBpick.Name = "DOBpick";
            DOBpick.Size = new Size(173, 27);
            DOBpick.TabIndex = 54;
            DOBpick.Value = new DateTime(2007, 12, 31, 0, 0, 0, 0);
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 10F);
            label12.Location = new Point(375, 141);
            label12.Name = "label12";
            label12.Size = new Size(119, 23);
            label12.TabIndex = 53;
            label12.Text = "Date Of Birth :";
            // 
            // RoomNumberCbx
            // 
            RoomNumberCbx.FormattingEnabled = true;
            RoomNumberCbx.Location = new Point(146, 316);
            RoomNumberCbx.Name = "RoomNumberCbx";
            RoomNumberCbx.Size = new Size(151, 28);
            RoomNumberCbx.TabIndex = 52;
            RoomNumberCbx.SelectedIndexChanged += RoomNumberCbx_SelectedIndexChanged;
            // 
            // RoomTypeCbx
            // 
            RoomTypeCbx.FormattingEnabled = true;
            RoomTypeCbx.Location = new Point(146, 268);
            RoomTypeCbx.Name = "RoomTypeCbx";
            RoomTypeCbx.Size = new Size(151, 28);
            RoomTypeCbx.TabIndex = 51;
            RoomTypeCbx.SelectedIndexChanged += RoomTypeCbx_SelectedIndexChanged;
            // 
            // CountryBx
            // 
            CountryBx.Location = new Point(524, 224);
            CountryBx.Name = "CountryBx";
            CountryBx.PlaceholderText = "Country";
            CountryBx.Size = new Size(100, 27);
            CountryBx.TabIndex = 50;
            // 
            // StateBx
            // 
            StateBx.Location = new Point(346, 224);
            StateBx.Name = "StateBx";
            StateBx.PlaceholderText = "State";
            StateBx.Size = new Size(100, 27);
            StateBx.TabIndex = 49;
            // 
            // CityBx
            // 
            CityBx.Location = new Point(146, 222);
            CityBx.Name = "CityBx";
            CityBx.PlaceholderText = "City";
            CityBx.Size = new Size(100, 27);
            CityBx.TabIndex = 48;
            // 
            // PhoneTxt
            // 
            PhoneTxt.Location = new Point(146, 177);
            PhoneTxt.Name = "PhoneTxt";
            PhoneTxt.Size = new Size(202, 27);
            PhoneTxt.TabIndex = 47;
            // 
            // FemaleRad
            // 
            FemaleRad.AutoSize = true;
            FemaleRad.Location = new Point(270, 136);
            FemaleRad.Name = "FemaleRad";
            FemaleRad.Size = new Size(78, 24);
            FemaleRad.TabIndex = 46;
            FemaleRad.TabStop = true;
            FemaleRad.Text = "Female";
            FemaleRad.UseVisualStyleBackColor = true;
            // 
            // MaleRad
            // 
            MaleRad.AutoSize = true;
            MaleRad.Location = new Point(146, 137);
            MaleRad.Name = "MaleRad";
            MaleRad.Size = new Size(63, 24);
            MaleRad.TabIndex = 45;
            MaleRad.TabStop = true;
            MaleRad.Text = "Male";
            MaleRad.UseVisualStyleBackColor = true;
            // 
            // LastNameTxt
            // 
            LastNameTxt.Font = new Font("Segoe UI", 12F);
            LastNameTxt.Location = new Point(474, 85);
            LastNameTxt.Name = "LastNameTxt";
            LastNameTxt.Size = new Size(199, 34);
            LastNameTxt.TabIndex = 44;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 10F);
            label11.Location = new Point(365, 316);
            label11.Name = "label11";
            label11.Size = new Size(115, 23);
            label11.TabIndex = 43;
            label11.Text = "Price per Day:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 10F);
            label10.Location = new Point(437, 370);
            label10.Name = "label10";
            label10.Size = new Size(130, 23);
            label10.TabIndex = 42;
            label10.Text = "Check-out Date";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10F);
            label9.Location = new Point(128, 370);
            label9.Name = "label9";
            label9.Size = new Size(118, 23);
            label9.TabIndex = 41;
            label9.Text = "Check-in Date";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10F);
            label8.Location = new Point(13, 316);
            label8.Name = "label8";
            label8.Size = new Size(137, 23);
            label8.TabIndex = 40;
            label8.Text = "Room Number : ";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10F);
            label7.Location = new Point(13, 269);
            label7.Name = "label7";
            label7.Size = new Size(109, 23);
            label7.TabIndex = 39;
            label7.Text = "Room Type : ";
            // 
            // SubmitBtn
            // 
            SubmitBtn.BackColor = Color.ForestGreen;
            SubmitBtn.FlatStyle = FlatStyle.Flat;
            SubmitBtn.Font = new Font("Segoe UI", 10F);
            SubmitBtn.ForeColor = SystemColors.ControlLightLight;
            SubmitBtn.Location = new Point(295, 513);
            SubmitBtn.Name = "SubmitBtn";
            SubmitBtn.Size = new Size(94, 36);
            SubmitBtn.TabIndex = 38;
            SubmitBtn.Text = "Submit";
            SubmitBtn.UseVisualStyleBackColor = false;
            SubmitBtn.Click += SubmitBtn_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10F);
            label6.Location = new Point(13, 223);
            label6.Name = "label6";
            label6.Size = new Size(84, 23);
            label6.TabIndex = 37;
            label6.Text = "Address : ";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F);
            label5.Location = new Point(13, 178);
            label5.Name = "label5";
            label5.Size = new Size(141, 23);
            label5.TabIndex = 36;
            label5.Text = "Phone Number : ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F);
            label4.Location = new Point(13, 137);
            label4.Name = "label4";
            label4.Size = new Size(80, 23);
            label4.TabIndex = 35;
            label4.Text = "Gender : ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F);
            label3.Location = new Point(375, 93);
            label3.Name = "label3";
            label3.Size = new Size(105, 23);
            label3.TabIndex = 34;
            label3.Text = "Last Name : ";
            // 
            // FirstNameTxt
            // 
            FirstNameTxt.Font = new Font("Segoe UI", 12F);
            FirstNameTxt.Location = new Point(146, 85);
            FirstNameTxt.Name = "FirstNameTxt";
            FirstNameTxt.Size = new Size(202, 34);
            FirstNameTxt.TabIndex = 33;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(13, 93);
            label2.Name = "label2";
            label2.Size = new Size(106, 23);
            label2.TabIndex = 32;
            label2.Text = "First Name : ";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(254, 41);
            label1.Name = "label1";
            label1.Size = new Size(211, 31);
            label1.TabIndex = 29;
            label1.Text = "Guest Information";
            // 
            // CloseBtn
            // 
            CloseBtn.BackColor = Color.OrangeRed;
            CloseBtn.FlatStyle = FlatStyle.Flat;
            CloseBtn.ForeColor = SystemColors.ControlLightLight;
            CloseBtn.Location = new Point(622, 3);
            CloseBtn.Name = "CloseBtn";
            CloseBtn.Size = new Size(68, 40);
            CloseBtn.TabIndex = 56;
            CloseBtn.Text = "Close";
            CloseBtn.UseVisualStyleBackColor = false;
            CloseBtn.Click += CloseBtn_Click;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 12F);
            label13.Location = new Point(222, 463);
            label13.Name = "label13";
            label13.Size = new Size(107, 28);
            label13.TabIndex = 57;
            label13.Text = "Total Cost :";
            // 
            // TotalPaymentLabel
            // 
            TotalPaymentLabel.AutoSize = true;
            TotalPaymentLabel.Font = new Font("Segoe UI", 15.8F, FontStyle.Bold);
            TotalPaymentLabel.ForeColor = Color.OrangeRed;
            TotalPaymentLabel.Location = new Point(325, 457);
            TotalPaymentLabel.Name = "TotalPaymentLabel";
            TotalPaymentLabel.Size = new Size(81, 37);
            TotalPaymentLabel.TabIndex = 58;
            TotalPaymentLabel.Text = "4000";
            // 
            // CheckIn
            // 
            CheckIn.Location = new Point(60, 396);
            CheckIn.Name = "CheckIn";
            CheckIn.Size = new Size(250, 27);
            CheckIn.TabIndex = 59;
            CheckIn.ValueChanged += CheckIn_ValueChanged;
            // 
            // CheckOut
            // 
            CheckOut.Location = new Point(375, 396);
            CheckOut.Name = "CheckOut";
            CheckOut.Size = new Size(250, 27);
            CheckOut.TabIndex = 60;
            CheckOut.ValueChanged += CheckOut_ValueChanged;
            // 
            // ReservationControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(CheckOut);
            Controls.Add(CheckIn);
            Controls.Add(TotalPaymentLabel);
            Controls.Add(label13);
            Controls.Add(CloseBtn);
            Controls.Add(PriceTxt);
            Controls.Add(DOBpick);
            Controls.Add(label12);
            Controls.Add(RoomNumberCbx);
            Controls.Add(RoomTypeCbx);
            Controls.Add(CountryBx);
            Controls.Add(StateBx);
            Controls.Add(CityBx);
            Controls.Add(PhoneTxt);
            Controls.Add(FemaleRad);
            Controls.Add(MaleRad);
            Controls.Add(LastNameTxt);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(SubmitBtn);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(FirstNameTxt);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "ReservationControl";
            Size = new Size(693, 579);
            MouseDown += ReservationControl_MouseDown;
            MouseMove += ReservationControl_MouseMove;
            MouseUp += ReservationControl_MouseUp;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox PriceTxt;
        private DateTimePicker DOBpick;
        private Label label12;
        private ComboBox RoomNumberCbx;
        private ComboBox RoomTypeCbx;
        private TextBox CountryBx;
        private TextBox StateBx;
        private TextBox CityBx;
        private TextBox PhoneTxt;
        private RadioButton FemaleRad;
        private RadioButton MaleRad;
        private TextBox LastNameTxt;
        private Label label11;
        private Label label10;
        private Label label9;
        private Label label8;
        private Label label7;
        private Button SubmitBtn;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private TextBox FirstNameTxt;
        private Label label2;
        private Label label1;
        private Button CloseBtn;
        private Label label13;
        private Label TotalPaymentLabel;
        private DateTimePicker CheckIn;
        private DateTimePicker CheckOut;
    }
}
