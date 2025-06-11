using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagmentSys
{
    public partial class NotifyControl: UserControl
    {
        private System.Windows.Forms.Timer hideTimer = new System.Windows.Forms.Timer();
        private Form parentForm;
        public NotifyControl()
        {
            InitializeComponent();
            this.Visible = false;

            hideTimer.Interval = 3000; 
            hideTimer.Tick += (s, e) =>
            {
                this.Visible = false;
                hideTimer.Stop();
            };
        }

        public void Attach(Form form)
        {
            parentForm = form;
            if (!form.Controls.Contains(this))
            {
                form.Controls.Add(this);
            }
        }

        public void ShowMessage(string message, string title = "Success", bool isSuccess = true)
        {
            if (parentForm != null)
            {
                this.Location = new Point(20, parentForm.ClientSize.Height - this.Height - 20);
            }

            titleLabel.Text = title;
            messageLabel.Text = message;

           

            titleLabel.ForeColor = isSuccess ? Color.Green : Color.Red;

            this.Visible = true;
            this.BringToFront();
            hideTimer.Start();
        }
    }
}
