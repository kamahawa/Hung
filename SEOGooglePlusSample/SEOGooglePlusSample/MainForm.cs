using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEOGooglePlusSample
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Load();
        }

        private void _btnStart_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormSEO f = new FormSEO();
            f.Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Load()
        {
            string fileName = "account.xml";
            if (File.Exists(fileName))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(fileName);
                dtgAccount.DataSource = ds;
                dtgAccount.DataMember = "account";
            }
            else
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable("account");
                dt.Columns.Add("Username", typeof(string));
                dt.Columns.Add("Password", typeof(string));
                ds.Tables.Add(dt);
                dtgAccount.DataSource = ds;
                dtgAccount.DataMember = "account";
            }
               
        }

        private void Save()
        {
            string path = "account.xml";
            DataSet ds = (DataSet)dtgAccount.DataSource;
            ds.WriteXml(path);
        }

        private void btnGetIdCommunities_Click(object sender, EventArgs e)
        {
            this.Hide();
            int member = 0;
            try
            {
                member = Int32.Parse(txtMember.Text);
            }
            catch(Exception ex)
            { }
            FormProgress f = new FormProgress(txtSearch.Text, member);
            f.ShowDialog();
            this.Show();
        }
    }
}
