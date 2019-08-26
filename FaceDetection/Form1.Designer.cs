namespace FaceDetection
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnAddFace = new System.Windows.Forms.Button();
            this.ptbAddFace = new System.Windows.Forms.PictureBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ptbMatchFace = new System.Windows.Forms.PictureBox();
            this.lblMatchFace = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbAddFace)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbMatchFace)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(640, 480);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // btnAddFace
            // 
            this.btnAddFace.Location = new System.Drawing.Point(65, 183);
            this.btnAddFace.Name = "btnAddFace";
            this.btnAddFace.Size = new System.Drawing.Size(75, 23);
            this.btnAddFace.TabIndex = 1;
            this.btnAddFace.Text = "Add Face";
            this.btnAddFace.UseVisualStyleBackColor = true;
            this.btnAddFace.Click += new System.EventHandler(this.BtnAddFace_Click);
            // 
            // ptbAddFace
            // 
            this.ptbAddFace.Location = new System.Drawing.Point(6, 21);
            this.ptbAddFace.Name = "ptbAddFace";
            this.ptbAddFace.Size = new System.Drawing.Size(134, 119);
            this.ptbAddFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptbAddFace.TabIndex = 2;
            this.ptbAddFace.TabStop = false;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(6, 155);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(134, 20);
            this.txtUserName.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ptbAddFace);
            this.groupBox1.Controls.Add(this.btnAddFace);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Location = new System.Drawing.Point(646, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 212);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Train Face";
            // 
            // ptbMatchFace
            // 
            this.ptbMatchFace.Location = new System.Drawing.Point(652, 269);
            this.ptbMatchFace.Name = "ptbMatchFace";
            this.ptbMatchFace.Size = new System.Drawing.Size(134, 94);
            this.ptbMatchFace.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptbMatchFace.TabIndex = 5;
            this.ptbMatchFace.TabStop = false;
            // 
            // lblMatchFace
            // 
            this.lblMatchFace.AutoSize = true;
            this.lblMatchFace.Location = new System.Drawing.Point(652, 232);
            this.lblMatchFace.Name = "lblMatchFace";
            this.lblMatchFace.Size = new System.Drawing.Size(64, 13);
            this.lblMatchFace.TabIndex = 6;
            this.lblMatchFace.Text = "Match Face";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(809, 484);
            this.Controls.Add(this.lblMatchFace);
            this.Controls.Add(this.ptbMatchFace);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Face detection";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbAddFace)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbMatchFace)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnAddFace;
        private System.Windows.Forms.PictureBox ptbAddFace;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox ptbMatchFace;
        private System.Windows.Forms.Label lblMatchFace;
    }
}

