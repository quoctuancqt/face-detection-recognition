using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Project_FaceRecognition
{
    public partial class FrmAdminOptions : Form
    {
        public FrmAdminOptions()
        {
            InitializeComponent();
        }

        private void btnManageBooks_Click(object sender, EventArgs e)
        {
            Hide();
            var uploadDocForm = new FrmUploadDocument();
            var result = uploadDocForm.ShowDialog();
            if (result == DialogResult.OK || result == DialogResult.Cancel)
            {
                Show();
            }
        }

        private void btnRegisterValidate_Click(object sender, EventArgs e)
        {
            Hide();
            var mainForm = new FrmMain(false);
            if (mainForm.ShowDialog() == DialogResult.Cancel)
            {
                Show();
            }
        }

        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            Hide();
            var deleteUserForm = new FrmDeleteUser();
            if (deleteUserForm.ShowDialog() == DialogResult.Cancel)
            {
                Show();
            }
        }
    }
}
