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
    public partial class FrmDeleteUser : Form
    {
        private readonly String _databasePath = Application.StartupPath + "/face_store.db";
        public FrmDeleteUser()
        {
            InitializeComponent();
        }

        private void FrmDeleteUser_Load(object sender, EventArgs e)
        {
            LoadAllUsers();
        }

        private void LoadAllUsers()
        {
            IDataStoreAccess dataStore = new DataStoreAccess(_databasePath);
            var allUsernames = dataStore.GetAllUsernames();
            lstBoxUsernames.DataSource = allUsernames;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            IDataStoreAccess dataStore = new DataStoreAccess(_databasePath);
            var selectedUsername = (String)lstBoxUsernames.SelectedValue;
            if (String.Empty != selectedUsername)
            {
                var messageBox = MessageBox.Show(String.Format("Are you sure you want to delete {0}?", selectedUsername.ToUpper()), "Delete User",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (messageBox == DialogResult.Yes)
                {
                    if (dataStore.DeleteUser(selectedUsername))
                        MessageBox.Show(selectedUsername.ToUpper() + " has been deleted", "User Deleted",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else MessageBox.Show("Error: User cannot be deleted", "Error",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                LoadAllUsers();
            }
        }
    }
}
