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
    public class DrawNetwork
    {
        #region Fields

        private MapBox _mapbox;
        private PointF p1;
        private PointF p2;
        public Graphics p;
        private TileAsyncLayer tileLayer = new TileAsyncLayer(new GoogleTileSource(GoogleMapType.GoogleMap), "TileLayer - Google");
        private List<Coordinate> _pointArray = new List<Coordinate>();

        #endregion

        #region Constructor

        public DrawNetwork(MapBox mapbox)
        {
            _pointArray = new List<Coordinate>(2);
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
            {
                // add infranode onto map
                _mapbox.Cursor = Cursors.Cross;
                InfraNode node = new InfraNode(coordinate);
                States.Network.AddNode(node);

                // add infranode onto diagram
                Forms.dockDiagram.pDiagram1.AddRectangle(e.X, e.Y);

                // add infranode information to other controls
                ControlHelper.AddNetworkNode(Forms.dockNode.dataGridViewPersistent1, node);
                _mapbox.Invalidate();
            }
            else if (Forms.dockMain.linkCreationToolStripMenuItem.Checked)
            {
                _mapbox.Cursor = Cursors.Cross;

                _pointArray.Add(coordinate);
                _pointArray.Add(coordinate);

                /*
                if (States.Network.InfraNodes.Count > 0)
                {
                    foreach (var node in States.Network.InfraNodes)
                    {
                        if (node.EnvelopeContains(coordinate))
                        {
                            _pointArray.Add(node.Coordinate);
                            _pointArray.Add(node.Coordinate);
                        }
                    }
                }
                else
                {
                    _pointArray.Add(coordinate);
                    _pointArray.Add(coordinate);
                }
                 */
                _mapbox.Invalidate();
            }


        }

        public void OnMouseDrag(Coordinate coordinate, MouseEventArgs e)
        {

        }

        public void OnMouseMove(Coordinate coordinate, MouseEventArgs e)
        {
            if (Forms.dockMain.linkCreationToolStripMenuItem.Checked)
            {
                if (_pointArray.Count == 2)
                {
                    _pointArray[_pointArray.Count - 1] = _mapbox.Map.ImageToWorld(e.Location);
                    _mapbox.Invalidate();
                }
            }
        }

        public void OnMouseUp(Coordinate coordinate, MouseEventArgs e)
        {
            if (Forms.dockMain.linkCreationToolStripMenuItem.Checked)
            {
                if (_pointArray.Count == 2)
                {
                    _pointArray[_pointArray.Count - 1] = coordinate;
                    p1 = _mapbox.Map.WorldToImage(_pointArray[0]);
                    p2 = _mapbox.Map.WorldToImage(_pointArray[1]);
                    InfraLink link = new InfraLink(_pointArray[0], _pointArray[1]);
                    States.Network.AddLink(link);

                    PointF centroid = _mapbox.Map.WorldToImage(link.Line.Centroid.Coordinate);
                    Forms.dockDiagram.pDiagram1.AddRectangle((int)centroid.X, (int)centroid.Y);

                    // add infralink information to other controls
                    ControlHelper.AddNetworkLink(Forms.dockLink.dataGridViewPersistent1, link);
                    _mapbox.Invalidate();

                    _mapbox.Invalidate();
                }
                _pointArray.Clear();
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

            if (Forms.dockMain.linkCreationToolStripMenuItem.Checked && _pointArray.Count >0)
            {
                var p1 = _mapbox.Map.WorldToImage(_pointArray[0]);
                var p2 = _mapbox.Map.WorldToImage(_pointArray[1]);
                p.DrawLine(new Pen(Color.Green, 1F), p1, p2);
            }

            if (States.Network.InfraNodes.Count > 0 || States.Network.InfraLinks.Count > 0)
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
