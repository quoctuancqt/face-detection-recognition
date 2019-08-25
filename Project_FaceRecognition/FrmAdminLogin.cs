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
    public partial class FrmAdminLogin : Form
    {
        public FrmAdminLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == (String) Properties.Settings.Default["adminUsername"] &&
                txtUsername.Text == (String) Properties.Settings.Default["adminUsername"])
            {
                var adminOptionsForm = new FrmAdminOptions();
                Hide();
                if (adminOptionsForm.ShowDialog() == DialogResult.Cancel)
                {
                    Show();
                }
            }
            else
            {
                MessageBox.Show("Invalid Username/Password");
            }
        }

        private void FrmAdminLogin_Load(object sender, EventArgs e)
        {
            Hide();
            //Show SplashScreen first
            var splashScreen = new FrmSplashScreen(this);
            splashScreen.Show();
        }

        private void btnUserLogin_Click(object sender, EventArgs e)
        {
            Hide();
            var mainForm = new FrmMain(true);
            if (mainForm.ShowDialog() == DialogResult.Cancel)
            {
                Show();
            }
        }
    }
}
