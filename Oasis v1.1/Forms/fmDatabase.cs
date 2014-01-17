using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace Oasis_v1._1
{
    public partial class fmDatabase : Form
    {
        DbConnection dbConnection;
        List<PostGisMetaItem> pgitems = new List<PostGisMetaItem>();

        public fmDatabase()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            txtStatus.Clear();
            dbConnection = new DbConnection(txtHost.Text, txtPort.Text, txtDatabase.Text, txtUserName.Text, txtPassword.Text);
            dbConnection.GetConnect();

            if (dbConnection.IsConnection)
            {
                stripDB.Text = "Database connection is sucessful!";
                txtStatus.BackColor = Color.LawnGreen;
            }
            else
            {
                stripDB.Text = "Database connection is failed!";
                txtStatus.BackColor = Color.Red;
            }

            PostGISHelper pgh = new PostGISHelper(dbConnection.ConnectionString);
            var listString = pgh.GetExistingLayerInformation();
            pgitems = PostGISHelper.FormMetaToView(listString);

            intialExample();
        }

        public void intialExample()
        {
            this.olvLayers.AddDecoration(new EditingCellBorderDecoration(true));
            this.olvLayers.SetObjects(pgitems);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            dbConnection = new DbConnection(txtHost.Text, txtPort.Text, txtDatabase.Text, txtUserName.Text, txtPassword.Text);
            PostGISHelper pgHelper = new PostGISHelper(dbConnection.ConnectionString);
            pgHelper.LoadPostGisSharpLayer("notts_acc05", Forms.dockMap.mapBox1, Forms.dockNode.dataGridViewPersistent1);
        }
    }
}
