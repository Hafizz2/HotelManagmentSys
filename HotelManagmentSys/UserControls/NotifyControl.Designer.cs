namespace HotelManagmentSys
{
    partial class NotifyControl
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
            titleLabel = new Label();
            panel1 = new Panel();
            messageLabel = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            titleLabel.ForeColor = Color.ForestGreen;
            titleLabel.Location = new Point(3, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(135, 31);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "This Is Title";
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(messageLabel);
            panel1.Controls.Add(titleLabel);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(406, 69);
            panel1.TabIndex = 1;
            // 
            // messageLabel
            // 
            messageLabel.AutoSize = true;
            messageLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            messageLabel.Location = new Point(3, 31);
            messageLabel.Name = "messageLabel";
            messageLabel.Size = new Size(145, 28);
            messageLabel.TabIndex = 1;
            messageLabel.Text = "This Is Message";
            // 
            // NotifyControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.ForestGreen;
            Controls.Add(panel1);
            Name = "NotifyControl";
            Size = new Size(412, 75);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label titleLabel;
        private Panel panel1;
        private Label messageLabel;
    }
}
