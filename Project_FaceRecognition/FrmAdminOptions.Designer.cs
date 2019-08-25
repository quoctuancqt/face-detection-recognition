namespace Project_FaceRecognition
{
    partial class FrmAdminOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAdminOptions));
            this.btnRegisterValidate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnManageBooks = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRegisterValidate
            // 
            this.btnRegisterValidate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRegisterValidate.BackgroundImage")));
            this.btnRegisterValidate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRegisterValidate.Location = new System.Drawing.Point(85, 29);
            this.btnRegisterValidate.Name = "btnRegisterValidate";
            this.btnRegisterValidate.Size = new System.Drawing.Size(99, 87);
            this.btnRegisterValidate.TabIndex = 0;
            this.btnRegisterValidate.UseVisualStyleBackColor = true;
            this.btnRegisterValidate.Click += new System.EventHandler(this.btnRegisterValidate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(43, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Register/Validate User";
            // 
            // btnDeleteUser
            // 
            this.btnDeleteUser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeleteUser.BackgroundImage")));
            this.btnDeleteUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDeleteUser.Location = new System.Drawing.Point(303, 29);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(102, 87);
            this.btnDeleteUser.TabIndex = 2;
            this.btnDeleteUser.UseVisualStyleBackColor = true;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(302, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Delete User";
            // 
            // btnManageBooks
            // 
            this.btnManageBooks.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnManageBooks.BackgroundImage")));
            this.btnManageBooks.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnManageBooks.Location = new System.Drawing.Point(503, 29);
            this.btnManageBooks.Name = "btnManageBooks";
            this.btnManageBooks.Size = new System.Drawing.Size(101, 87);
            this.btnManageBooks.TabIndex = 4;
            this.btnManageBooks.UseVisualStyleBackColor = true;
            this.btnManageBooks.Click += new System.EventHandler(this.btnManageBooks_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(489, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Manage Library";
            // 
            // FrmAdminOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 185);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnManageBooks);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDeleteUser);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRegisterValidate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAdminOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administrative Options";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRegisterValidate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnManageBooks;
        private System.Windows.Forms.Label label3;
    }
}