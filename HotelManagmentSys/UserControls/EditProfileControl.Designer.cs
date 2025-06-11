namespace HotelManagmentSys
{
    partial class EditProfileControl
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
            Newpassbx = new TextBox();
            Oldpassbx = new TextBox();
            SettNamebx = new TextBox();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            CloseBtn = new Button();
            label1 = new Label();
            LastTxtbx = new TextBox();
            label2 = new Label();
            AddressTxt = new TextBox();
            label3 = new Label();
            SubmitBtn = new Button();
            SuspendLayout();
            // 
            // Newpassbx
            // 
            Newpassbx.Location = new Point(285, 287);
            Newpassbx.Name = "Newpassbx";
            Newpassbx.PasswordChar = '*';
            Newpassbx.Size = new Size(258, 27);
            Newpassbx.TabIndex = 29;
            // 
            // Oldpassbx
            // 
            Oldpassbx.Location = new Point(285, 246);
            Oldpassbx.Name = "Oldpassbx";
            Oldpassbx.PasswordChar = '*';
            Oldpassbx.Size = new Size(258, 27);
            Oldpassbx.TabIndex = 28;
            // 
            // SettNamebx
            // 
            SettNamebx.Location = new Point(285, 117);
            SettNamebx.Name = "SettNamebx";
            SettNamebx.Size = new Size(258, 27);
            SettNamebx.TabIndex = 27;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label14.Location = new Point(69, 287);
            label14.Name = "label14";
            label14.Size = new Size(155, 28);
            label14.TabIndex = 25;
            label14.Text = "New Password :";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            label13.Location = new Point(69, 246);
            label13.Name = "label13";
            label13.Size = new Size(152, 28);
            label13.TabIndex = 24;
            label13.Text = "Old Password : ";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label12.Location = new Point(69, 117);
            label12.Name = "label12";
            label12.Size = new Size(121, 28);
            label12.TabIndex = 23;
            label12.Text = "First Name :";
            // 
            // CloseBtn
            // 
            CloseBtn.BackColor = Color.ForestGreen;
            CloseBtn.FlatStyle = FlatStyle.Flat;
            CloseBtn.ForeColor = SystemColors.ControlLightLight;
            CloseBtn.Location = new Point(524, 3);
            CloseBtn.Name = "CloseBtn";
            CloseBtn.Size = new Size(68, 40);
            CloseBtn.TabIndex = 57;
            CloseBtn.Text = "Close";
            CloseBtn.UseVisualStyleBackColor = false;
            CloseBtn.Click += CloseBtn_Click_Internal;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(182, 53);
            label1.Name = "label1";
            label1.Size = new Size(247, 31);
            label1.TabIndex = 58;
            label1.Text = "Edit Your Information";
            // 
            // LastTxtbx
            // 
            LastTxtbx.Location = new Point(285, 161);
            LastTxtbx.Name = "LastTxtbx";
            LastTxtbx.Size = new Size(258, 27);
            LastTxtbx.TabIndex = 60;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(69, 203);
            label2.Name = "label2";
            label2.Size = new Size(102, 28);
            label2.TabIndex = 59;
            label2.Text = "Address  :";
            // 
            // AddressTxt
            // 
            AddressTxt.Location = new Point(285, 203);
            AddressTxt.Name = "AddressTxt";
            AddressTxt.Size = new Size(258, 27);
            AddressTxt.TabIndex = 62;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(69, 160);
            label3.Name = "label3";
            label3.Size = new Size(119, 28);
            label3.TabIndex = 61;
            label3.Text = "Last Name :";
            // 
            // SubmitBtn
            // 
            SubmitBtn.BackColor = Color.ForestGreen;
            SubmitBtn.FlatStyle = FlatStyle.Flat;
            SubmitBtn.ForeColor = SystemColors.ControlLightLight;
            SubmitBtn.Location = new Point(228, 340);
            SubmitBtn.Name = "SubmitBtn";
            SubmitBtn.Size = new Size(105, 40);
            SubmitBtn.TabIndex = 63;
            SubmitBtn.Text = "Submit";
            SubmitBtn.UseVisualStyleBackColor = false;
            // 
            // EditProfileControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            Controls.Add(SubmitBtn);
            Controls.Add(AddressTxt);
            Controls.Add(label3);
            Controls.Add(LastTxtbx);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(CloseBtn);
            Controls.Add(Newpassbx);
            Controls.Add(Oldpassbx);
            Controls.Add(SettNamebx);
            Controls.Add(label14);
            Controls.Add(label13);
            Controls.Add(label12);
            Name = "EditProfileControl";
            Size = new Size(595, 472);
            MouseDown += EditProfileControl_MouseDown;
            MouseMove += EditProfileControl_MouseMove;
            MouseUp += EditProfileControl_MouseUp;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox Newpassbx;
        private TextBox Oldpassbx;
        private TextBox SettNamebx;
        private Label label14;
        private Label label13;
        private Label label12;
        private Button CloseBtn;
        private Label label1;
        private TextBox LastTxtbx;
        private Label label2;
        private TextBox AddressTxt;
        private Label label3;
        private Button SubmitBtn;
    }
}
