using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpMap.Forms;
using SharpMap.Layers;
using BruTile.Web;
using BruTile;

namespace Oasis_v1._1
{
    public partial class MainWin : Form
    {
        private NetworkDesign dl;
        private TileAsyncLayer tileLayer = new TileAsyncLayer(new GoogleTileSource(GoogleMapType.GoogleMap), "TileLayer - Google");

        public MainWin()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetMainFormStyle();
        }

        #region Methods

            private void SetMainFormStyle()
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.WindowState = FormWindowState.Maximized;
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            }

            public void Initialize()
            {
                Forms.dockManager.ShowDocumentDocks();
            }

        #endregion

        #region Button Events

            private void googleToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (Forms.dockMap.mapBox1.Map.BackgroundLayer == null)
                {
                    LayerHelper.LoadTileWebLayer(Forms.dockMap.mapBox1, TileWebObjectType.GoogleMap);
                }
                else
                {
                    Forms.dockMap.mapBox1.Map.BackgroundLayer.Clear();
                    LayerHelper.LoadTileWebLayer(Forms.dockMap.mapBox1, TileWebObjectType.GoogleMap);
                }
            }

            private void oSMToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (Forms.dockMap.mapBox1.Map.BackgroundLayer == null)
                {
                    LayerHelper.LoadTileWebLayer(Forms.dockMap.mapBox1, TileWebObjectType.OpenStreetMap);
                }
                else
                {
                    Forms.dockMap.mapBox1.Map.BackgroundLayer.Clear();
                    LayerHelper.LoadTileWebLayer(Forms.dockMap.mapBox1, TileWebObjectType.OpenStreetMap);
                }
            }

            private void bingToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (Forms.dockMap.mapBox1.Map.BackgroundLayer == null)
                {
                    LayerHelper.LoadTileWebLayer(Forms.dockMap.mapBox1, TileWebObjectType.BingMap);
                }
                else
                {
                    Forms.dockMap.mapBox1.Map.BackgroundLayer.Clear();
                    LayerHelper.LoadTileWebLayer(Forms.dockMap.mapBox1, TileWebObjectType.BingMap);
                }
            }

            private void panMapToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Forms.dockMap.mapBox1.ActiveTool = MapBox.Tools.Pan;
            }

            private void panToSelectionToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Forms.dockMap.mapBox1.ActiveTool = MapBox.Tools.ZoomWindow;
            }

            private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Forms.dockMap.mapBox1.ActiveTool = MapBox.Tools.ZoomIn;
            }

            private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Forms.dockMap.mapBox1.ActiveTool = MapBox.Tools.ZoomOut;
            }

            private void exitToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Close();
            }

            private void shapeFileToolStripMenuItem_Click(object sender, EventArgs e)
            {
                SharpMap.Layers.VectorLayer vlay = new SharpMap.Layers.VectorLayer("States");
                vlay.DataSource = new SharpMap.Data.Providers.ShapeFile(@"C:\\Hao's Code\\MapControls\\SampleData\\Notts_acc05.shp", true);
                Forms.dockMap.mapBox1.Map.Layers.Add(vlay);
                Forms.dockMap.mapBox1.Map.ZoomToExtents();
                Forms.dockMap.mapBox1.Refresh();
            }

        #endregion

            private void postGISInfoToolStripMenuItem_Click(object sender, EventArgs e)
            {
                fmDatabase fmDb = new fmDatabase();
                fmDb.Show();
            }

            private void linkCreationToolStripMenuItem_Click(object sender, EventArgs e)
            {
                NetworkUpdator.CreateInfraLinkColumn(Forms.dockLink.dataGridViewPersistent1);
                linkCreationToolStripMenuItem.CheckState = CheckState.Checked;
                nodeCreationToolStripMenuItem.CheckState = CheckState.Unchecked;
            }

            private void nodeCreationToolStripMenuItem_Click(object sender, EventArgs e)
            {
                NetworkUpdator.CreateInfraNodeColumn(Forms.dockNode.dataGridViewPersistent1);
                nodeCreationToolStripMenuItem.CheckState = CheckState.Checked;
                linkCreationToolStripMenuItem.CheckState = CheckState.Unchecked;
                
            }

            private void networkEditModeToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (!(nodeCreationToolStripMenuItem.Enabled && linkCreationToolStripMenuItem.Enabled))
                {
                    editNetworkCreation networkCreation = new editNetworkCreation();
                    networkCreation.Show();
                    dl = new NetworkDesign(Forms.dockMap.mapBox1);
                    Forms.dockMap.lblMainStatus.Text = "Network Builder Mode";    
                }
            }

            private void clearLayersToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (Forms.dockMap.mapBox1.Map.Layers != null)
                {
                    Forms.dockMap.mapBox1.Map.Dispose();
                    Forms.dockMap.mapBox1.Refresh();
                }
            }

            private void settingToolStripMenuItem_Click(object sender, EventArgs e)
            {
                Pan.Settings.SettingsState.ShowEditor();
                //Forms.dockMap.Update();
            }

            private void checkNetworkFeaturesToolStripMenuItem_Click(object sender, EventArgs e)
            {
                if (States.Network == null)
                {
                    MessageBox.Show("The network is empty!");
                }
                else
                {
                    int nodeCount = States.Network.NodesCollection.InfraNodes.Count;
                    int linkCount = States.Network.LinksCollection.InfraLinks.Count;
                    MessageBox.Show("The current network contains " + nodeCount + " nodes and " + linkCount + " links!");
                }
            }

            private void tsBuilderMap_Click(object sender, EventArgs e)
            {
                if (tsBuilderMap.CheckState == CheckState.Unchecked)
                {
                    tsBuilderMap.CheckState = CheckState.Checked;
                }
                else
                {
                    tsBuilderMap.CheckState = CheckState.Unchecked;
                }
            }

            private void toolStripButton3_Click(object sender, EventArgs e)
            {
                Forms.dockMap.mapBox1.ActiveTool = MapBox.Tools.ZoomIn;
            }

            private void toolStripButton4_Click(object sender, EventArgs e)
            {
                Forms.dockMap.mapBox1.ActiveTool = MapBox.Tools.ZoomOut;
            }

            private void toolStripButton5_Click(object sender, EventArgs e)
            {
                Forms.dockMap.mapBox1.ActiveTool = MapBox.Tools.Pan;
            }

            private void toolStripButton6_Click(object sender, EventArgs e)
            {

            }

            private void featureConnectivityAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
            {
                NetworkAnalysis na = new NetworkAnalysis(States.Network);

                if (na.IsAllConnected())
                {
                    MessageBox.Show("The entire network is connected!");
                }
                else
                {
                    MessageBox.Show("the entire netwokr is not connected, the connected links");
                }
            }

    }
}
