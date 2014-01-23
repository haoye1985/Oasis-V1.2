using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Oasis_v1._1
{
    public partial class fmNetworkCreation : Form
    {
        InfraNetwork network;

        public fmNetworkCreation()
        {
            InitializeComponent();
        }

        private void btnNetworkOK_Click(object sender, EventArgs e)
        {
            network = new InfraNetwork();
            NetworkParaSetting(network);
        }

        public void NetworkParaSetting(InfraNetwork Network)
        {
            Network.NetworkID = Network.GenerateID();
            txtNetworkID.Text = Network.NetworkID;
            txtNetworkName.Text = "Default Network";
            Network.NetworkName = txtNetworkName.Text;
            cbNetworkType.SelectedIndex = 0;
            cbTorlerance.SelectedIndex = 0;

        }

        private void btnNetworkCancel_Click(object sender, EventArgs e)
        {
            if (network != null)
            {
                States.Network = network;
                Forms.dockMain.nodeCreationToolStripMenuItem.Enabled = true;
                Forms.dockMain.linkCreationToolStripMenuItem.Enabled = true;
                Forms.dockMain.checkNetworkFeaturesToolStripMenuItem.Enabled = true;
                Forms.dockMain.tsBuilderMap.Enabled = true;
                Forms.dockMain.featureConnectivityAnalysisToolStripMenuItem.Enabled = true;
                Forms.dockMap.lblMainStatus.Text = network.NetworkName + " Created!";
                this.Close();
            }
            else
            {
                Forms.dockMap.lblMainStatus.Text = "Network Not Created!";
                this.Close();
            }
        }

        private void cbNetworkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbNetworkType.Text)
            {
                case "Road":
                    network.NetworkWorkingType = NetworkType.Road_NetWork;
                    break;
                case "Rail":
                    network.NetworkWorkingType = NetworkType.Railway_Network;
                    break;
                case "Tube":
                    network.NetworkWorkingType = NetworkType.Tube_Network;
                    break;
                case "":
                    network.NetworkWorkingType = NetworkType.Default_Network;
                    break;
                default:
                    network.NetworkWorkingType = NetworkType.Other_Network;
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
             switch (cbTorlerance.Text)
            {
                case "5(small)":
                    network.ExpansionFactor = 5;
                    break;
                case "10(medium)":
                    network.ExpansionFactor = 10;
                    break;
                case "20(Large)":
                    network.ExpansionFactor = 20;
                    break;
                default:
                    network.ExpansionFactor = 5;
                    break;
            }
        }

        private void CheckBoxMap_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBoxMap.Checked)
            {
                Forms.dockMain.tsBuilderMap.CheckState = CheckState.Checked;
            }
            else
            {
                Forms.dockMain.tsBuilderMap.CheckState = CheckState.Unchecked;
            }
        }




    }
}
