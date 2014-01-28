using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpMap.Forms;
using GeoAPI.Geometries;
using System.Windows.Forms;
using System.Drawing;
using SharpMap.Layers;
using BruTile.Web;
using Dataweb.NShape;
using Dataweb.NShape.Advanced;
using Dataweb.Utilities;
using GeoAPI.CoordinateSystems.Transformations;

namespace Oasis_v1._1
{
    public class NetworkDesign
    {
        #region Fields

        private MapBox _mapbox;
        private PointF p1;
        private PointF p2;
        public Graphics p;
        private TileAsyncLayer tileLayer = new TileAsyncLayer(new GoogleTileSource(GoogleMapType.GoogleMap), "TileLayer - Google");
        private List<Coordinate> _pointArray = new List<Coordinate>(2);
        private bool _isFirstMouseClicked;
        private bool _isSecondMouseClicked;

        private Coordinate startCoordinate = new Coordinate();
        private Coordinate endCoordinate = new Coordinate();

        #endregion

        #region Constructor

        public NetworkDesign(MapBox mapbox)
        {
            _mapbox = mapbox;
            _mapbox.MouseDown += new MapBox.MouseEventHandler(OnMouseDown);
            _mapbox.MouseMove += new MapBox.MouseEventHandler(OnMouseMove);
            _mapbox.MouseUp += new MapBox.MouseEventHandler(OnMouseUp);
            _mapbox.Paint += new PaintEventHandler(OnPaint);
        }

        #endregion

        #region Events

        public void OnMouseDown(Coordinate coordinate, MouseEventArgs e)
        {
            if (Forms.dockMain.nodeCreationToolStripMenuItem.Checked)
                ObjectCreation.AddNode(_mapbox, coordinate, e.Location);

            if (Forms.dockMain.linkCreationToolStripMenuItem.Checked)
            {
                _mapbox.Cursor = Cursors.Cross;
                _isFirstMouseClicked = true;

                if (States.Network.IsNodeCollectionEmpty())
                {
                    startCoordinate = coordinate;
                    endCoordinate = coordinate;
                }
                else
                {
                    for (int i = 0; i < States.Network.InfraNodes.Count; i++)
                    {
                        if (States.Network.InfraNodes[i].EnvelopeContains(coordinate))
                        {
                            string NodeID = States.Network.InfraNodes[i].Coordinate.ToString();
                            startCoordinate = States.Network.InfraNodes[i].Coordinate;
                            endCoordinate = States.Network.InfraNodes[i].Coordinate;
                            Forms.dockMap.lblMainStatus.Text = NodeID + "Contained!";
                            break;
                        }
                        else
                        {
                            startCoordinate = coordinate;
                            endCoordinate = coordinate;
                            Forms.dockMap.lblMainStatus.Text = "Not Contained!";
                        }
                    }

                }              
            }

            _mapbox.Invalidate();
        }

        public void OnMouseMove(Coordinate coordinate, MouseEventArgs e)
        {
            if (Forms.dockMain.linkCreationToolStripMenuItem.Checked)
            {
                if (!States.Network.IsNodeCollectionEmpty())
                {
                    for (int i = 0; i < States.Network.InfraNodes.Count; i++)
                    {
                        if (States.Network.InfraNodes[i].EnvelopeContains(coordinate))
                        {
                            string NodeID = States.Network.InfraNodes[i].Coordinate.ToString();
                            endCoordinate = States.Network.InfraNodes[i].Coordinate;
                            Forms.dockMap.lblMainStatus.Text = "Contained!" + NodeID;
                            break;
                        }
                        else
                        {
                            endCoordinate = coordinate;
                            Forms.dockMap.lblMainStatus.Text = "Not Contained!";                       
                        }
                    }
                }
            }
            _mapbox.Invalidate();
        }

        public void OnMouseUp(Coordinate coordinate, MouseEventArgs e)
        {
            if (Forms.dockMain.linkCreationToolStripMenuItem.Checked)
            {
                _isFirstMouseClicked = false;
                _isSecondMouseClicked = true;

                if (States.Network.IsNodeCollectionEmpty())
                {
                        endCoordinate = coordinate;
                        p1 = _mapbox.Map.WorldToImage(startCoordinate);
                        p2 = _mapbox.Map.WorldToImage(endCoordinate);
                        ObjectCreation.AddLink(_mapbox, startCoordinate, endCoordinate);
                }
                else
                {
                    for (int i = 0; i < States.Network.InfraNodes.Count; i++)
                    {
                        if (States.Network.InfraNodes[i].EnvelopeContains(coordinate))
                        {
                            endCoordinate = States.Network.InfraNodes[i].Coordinate;
                            p1 = _mapbox.Map.WorldToImage(startCoordinate);
                            p2 = _mapbox.Map.WorldToImage(endCoordinate);
                            ObjectCreation.AddLink(_mapbox, startCoordinate, endCoordinate);
                        }
                        else
                        {
                            endCoordinate = coordinate;
                            p1 = _mapbox.Map.WorldToImage(startCoordinate);
                            p2 = _mapbox.Map.WorldToImage(endCoordinate);
                        }
                    }
                }

                startCoordinate = null;
                endCoordinate = null;
                _isSecondMouseClicked = false;
                _mapbox.Invalidate();
            }
        }

        public void OnPaint(object sender, PaintEventArgs e)
        {
            p = e.Graphics;
            p.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            if (Forms.dockMain.tsBuilderMap.Checked)
            {
                tileLayer.Render(p, _mapbox.Map);
                //IMathTransform mathTransform = ProjectionHelper.Wgs84toGoogleMercator.MathTransform;
                //Envelope geom = GeometryTransform.TransformBox(new Envelope(-9.205626, -9.123736, 38.690993, 38.740837), mathTransform);
                //_mapbox.Map.ZoomToBox(geom);
                //_mapbox.Map.Zoom = 20000;
            }

            //if (Forms.dockMain.linkCreationToolStripMenuItem.Checked && _pointArray.Count >0)
            if (_isFirstMouseClicked || _isSecondMouseClicked)
            {
                var p1 = _mapbox.Map.WorldToImage(startCoordinate);
                var p2 = _mapbox.Map.WorldToImage(endCoordinate);
                p.DrawLine(new Pen(Color.Green, 1F), p1, p2);
            }

            if (!States.Network.IsCollectionEmpty())
            {
                NetworkVisualiser.DrawNetwork(p, _mapbox.Map, States.Network);
            }
        }

        #endregion

        #region Properties

                public Graphics pp
                {
                    get { return p; }
                    set { p = value; }
                }

        #endregion

    }
}
