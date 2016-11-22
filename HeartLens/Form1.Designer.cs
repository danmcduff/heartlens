namespace HeartLens
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerFrameUpdate = new System.Windows.Forms.Timer(this.components);
            this.textBoxHeartRate = new System.Windows.Forms.TextBox();
            this.DeviceComboBox = new System.Windows.Forms.ComboBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.pictureBoxFace = new System.Windows.Forms.PictureBox();
            this.groupBoxVideo = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFace)).BeginInit();
            this.SuspendLayout();
            // 
            // timerFrameUpdate
            // 
            this.timerFrameUpdate.Enabled = true;
            this.timerFrameUpdate.Interval = 1000;
            this.timerFrameUpdate.Tick += new System.EventHandler(this.timerFrameUpdate_Tick);
            // 
            // textBoxHeartRate
            // 
            this.textBoxHeartRate.Location = new System.Drawing.Point(12, 12);
            this.textBoxHeartRate.Name = "textBoxHeartRate";
            this.textBoxHeartRate.Size = new System.Drawing.Size(100, 20);
            this.textBoxHeartRate.TabIndex = 0;
            // 
            // DeviceComboBox
            // 
            this.DeviceComboBox.FormattingEnabled = true;
            this.DeviceComboBox.Location = new System.Drawing.Point(164, 12);
            this.DeviceComboBox.Name = "DeviceComboBox";
            this.DeviceComboBox.Size = new System.Drawing.Size(121, 21);
            this.DeviceComboBox.TabIndex = 3;
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(305, 7);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(54, 28);
            this.buttonOk.TabIndex = 4;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // pictureBoxFace
            // 
            this.pictureBoxFace.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBoxFace.Location = new System.Drawing.Point(12, 39);
            this.pictureBoxFace.Name = "pictureBoxFace";
            this.pictureBoxFace.Size = new System.Drawing.Size(292, 304);
            this.pictureBoxFace.TabIndex = 5;
            this.pictureBoxFace.TabStop = false;
            // 
            // groupBoxVideo
            // 
            this.groupBoxVideo.Location = new System.Drawing.Point(327, 41);
            this.groupBoxVideo.Name = "groupBoxVideo";
            this.groupBoxVideo.Size = new System.Drawing.Size(667, 487);
            this.groupBoxVideo.TabIndex = 6;
            this.groupBoxVideo.TabStop = false;
            this.groupBoxVideo.Text = "groupBox1";
            this.groupBoxVideo.Enter += new System.EventHandler(this.groupBoxVideo_Enter);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 638);
            this.Controls.Add(this.groupBoxVideo);
            this.Controls.Add(this.pictureBoxFace);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.DeviceComboBox);
            this.Controls.Add(this.textBoxHeartRate);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFace)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerFrameUpdate;
        private System.Windows.Forms.TextBox textBoxHeartRate;
        private System.Windows.Forms.ComboBox DeviceComboBox;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.PictureBox pictureBoxFace;
        private System.Windows.Forms.GroupBox groupBoxVideo;
    }
}

