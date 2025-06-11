namespace HotelManagmentSys
{
    partial class AddStaffContrl
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
            label1 = new Label();
            CloseBtn = new Button();
            Addressbx = new TextBox();
            label3 = new Label();
            LastNamebx = new TextBox();
            label2 = new Label();
            FirstNamebx = new TextBox();
            label12 = new Label();
            label14 = new Label();
            label15 = new Label();
            SettNewpassbx = new TextBox();
            SettConfpassbx = new TextBox();
            SubmitBtn = new Button();
            label4 = new Label();
            RoleComboBox = new ComboBox();
            Phonebx = new TextBox();
            label5 = new Label();
            Usernamebx = new TextBox();
            label6 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(129, 38);
            label1.Name = "label1";
            label1.Size = new Size(315, 31);
            label1.TabIndex = 59;
            label1.Text = "Add Staffs To Your Database";
            // 
            // CloseBtn
            // 
            CloseBtn.BackColor = Color.ForestGreen;
            CloseBtn.FlatStyle = FlatStyle.Flat;
            CloseBtn.ForeColor = SystemColors.ControlLightLight;
            CloseBtn.Location = new Point(487, 3);
            CloseBtn.Name = "CloseBtn";
            CloseBtn.Size = new Size(80, 40);
            CloseBtn.TabIndex = 60;
            CloseBtn.Text = "❌ Close";
            CloseBtn.UseVisualStyleBackColor = false;
            CloseBtn.Click += CloseBtn_Click;
            // 
            // Addressbx
            // 
            Addressbx.Location = new Point(273, 204);
            Addressbx.Name = "Addressbx";
            Addressbx.Size = new Size(258, 27);
            Addressbx.TabIndex = 74;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(57, 161);
            label3.Name = "label3";
            label3.Size = new Size(119, 28);
            label3.TabIndex = 73;
            label3.Text = "Last Name :";
            // 
            // LastNamebx
            // 
            LastNamebx.Location = new Point(273, 162);
            LastNamebx.Name = "LastNamebx";
            LastNamebx.Size = new Size(258, 27);
            LastNamebx.TabIndex = 72;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(57, 204);
            label2.Name = "label2";
            label2.Size = new Size(102, 28);
            label2.TabIndex = 71;
            label2.Text = "Address  :";
            // 
            // FirstNamebx
            // 
            FirstNamebx.Location = new Point(273, 118);
            FirstNamebx.Name = "FirstNamebx";
            FirstNamebx.Size = new Size(258, 27);
            FirstNamebx.TabIndex = 67;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label12.Location = new Point(57, 118);
            label12.Name = "label12";
            label12.Size = new Size(121, 28);
            label12.TabIndex = 63;
            label12.Text = "First Name :";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label14.Location = new Point(55, 378);
            label14.Name = "label14";
            label14.Size = new Size(155, 28);
            label14.TabIndex = 65;
            label14.Text = "New Password :";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label15.Location = new Point(56, 430);
            label15.Name = "label15";
            label15.Size = new Size(180, 28);
            label15.TabIndex = 66;
            label15.Text = "Confirm Passwod :";
            // 
            // SettNewpassbx
            // 
            SettNewpassbx.Location = new Point(273, 382);
            SettNewpassbx.Name = "SettNewpassbx";
            SettNewpassbx.PasswordChar = '*';
            SettNewpassbx.Size = new Size(258, 27);
            SettNewpassbx.TabIndex = 69;
            // 
            // SettConfpassbx
            // 
            SettConfpassbx.Location = new Point(272, 430);
            SettConfpassbx.Name = "SettConfpassbx";
            SettConfpassbx.PasswordChar = '*';
            SettConfpassbx.Size = new Size(259, 27);
            SettConfpassbx.TabIndex = 70;
            // 
            // SubmitBtn
            // 
            SubmitBtn.BackColor = Color.ForestGreen;
            SubmitBtn.FlatStyle = FlatStyle.Flat;
            SubmitBtn.ForeColor = SystemColors.ControlLightLight;
            SubmitBtn.Location = new Point(235, 488);
            SubmitBtn.Name = "SubmitBtn";
            SubmitBtn.Size = new Size(105, 40);
            SubmitBtn.TabIndex = 75;
            SubmitBtn.Text = "✅ Submit";
            SubmitBtn.UseVisualStyleBackColor = false;
            SubmitBtn.Click += SubmitBtn_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label4.Location = new Point(56, 330);
            label4.Name = "label4";
            label4.Size = new Size(134, 28);
            label4.TabIndex = 76;
            label4.Text = "Role of User :";
            // 
            // RoleComboBox
            // 
            RoleComboBox.FormattingEnabled = true;
            RoleComboBox.Items.AddRange(new object[] { "Manager", "Reception" });
            RoleComboBox.Location = new Point(272, 334);
            RoleComboBox.Name = "RoleComboBox";
            RoleComboBox.Size = new Size(257, 28);
            RoleComboBox.TabIndex = 77;
            RoleComboBox.Text = "Choose here";
            // 
            // Phonebx
            // 
            Phonebx.Location = new Point(273, 246);
            Phonebx.Name = "Phonebx";
            Phonebx.Size = new Size(258, 27);
            Phonebx.TabIndex = 79;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(57, 246);
            label5.Name = "label5";
            label5.Size = new Size(169, 28);
            label5.TabIndex = 78;
            label5.Text = "Phone Number  :";
            // 
            // Usernamebx
            // 
            Usernamebx.Location = new Point(272, 288);
            Usernamebx.Name = "Usernamebx";
            Usernamebx.Size = new Size(258, 27);
            Usernamebx.TabIndex = 81;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(56, 288);
            label6.Name = "label6";
            label6.Size = new Size(124, 28);
            label6.TabIndex = 80;
            label6.Text = "User Name :";
            // 
            // AddStaffContrl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            BorderStyle = BorderStyle.Fixed3D;
            Controls.Add(Usernamebx);
            Controls.Add(label6);
            Controls.Add(Phonebx);
            Controls.Add(label5);
            Controls.Add(RoleComboBox);
            Controls.Add(label4);
            Controls.Add(SubmitBtn);
            Controls.Add(Addressbx);
            Controls.Add(label3);
            Controls.Add(LastNamebx);
            Controls.Add(label2);
            Controls.Add(SettConfpassbx);
            Controls.Add(SettNewpassbx);
            Controls.Add(FirstNamebx);
            Controls.Add(label15);
            Controls.Add(label14);
            Controls.Add(label12);
            Controls.Add(CloseBtn);
            Controls.Add(label1);
            Name = "AddStaffContrl";
            Size = new Size(570, 542);
            MouseDown += AddStaffContrl_MouseDown;
            MouseMove += AddStaffContrl_MouseMove;
            MouseUp += AddStaffContrl_MouseUp;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button CloseBtn;
        private TextBox Addressbx;
        private Label label3;
        private TextBox LastNamebx;
        private Label label2;
        private TextBox FirstNamebx;
        private Label label12;
        private Label label14;
        private Label label15;
        private TextBox SettNewpassbx;
        private TextBox SettConfpassbx;
        private Button SubmitBtn;
        private Label label4;
        private ComboBox RoleComboBox;
        private TextBox Phonebx;
        private Label label5;
        private TextBox Usernamebx;
        private Label label6;
    }
}
