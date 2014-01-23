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

        #endregion

        #region Constructor

        public NetworkDesign(MapBox mapbox)
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
            Coordinate tempCoord = coordinate;

            if (Forms.dockMain.nodeCreationToolStripMenuItem.Checked)
                ObjectCreation.AddNode(_mapbox, coordinate, e.Location);

            if (Forms.dockMain.linkCreationToolStripMenuItem.Checked)
            {
                _mapbox.Cursor = Cursors.Cross;
                if (States.Network.IsNodeCollectionEmpty())
                {
                    _pointArray.Clear();
                    _pointArray.Add(coordinate);
                    _pointArray.Add(coordinate);
                }
                else
                {
                    foreach (var node in States.Network.InfraNodes)
                    {
                        if (node.EnvelopeContains(coordinate))
                        {
                            _pointArray.Clear();
                            _pointArray.Add(node.Coordinate);
                            _pointArray.Add(node.Coordinate);
                        }
                        else
                        {
                            _pointArray.Clear();
                            _pointArray.Add(coordinate);
                            _pointArray.Add(coordinate);
                        }
                    }
                }
                
                _mapbox.Invalidate();
            }


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
                if (States.Network.IsNodeCollectionEmpty())
                {
                    if (_pointArray.Count == 2)
                    {
                        _pointArray[_pointArray.Count - 1] = coordinate;
                        p1 = _mapbox.Map.WorldToImage(_pointArray[0]);
                        p2 = _mapbox.Map.WorldToImage(_pointArray[1]);
                        ObjectCreation.AddLink(_mapbox, _pointArray[0], _pointArray[1]);
                        _pointArray.Clear();
                    }
                }
                else
                {
                    foreach (var node in States.Network.InfraNodes)
                    {
                        if (node.EnvelopeContains(coordinate))
                        {
                            if (_pointArray.Count == 2)
                            {
                                _pointArray[_pointArray.Count - 1] = node.Coordinate;
                                p1 = _mapbox.Map.WorldToImage(_pointArray[0]);
                                p2 = _mapbox.Map.WorldToImage(_pointArray[1]);
                                ObjectCreation.AddLink(_mapbox, _pointArray[0], _pointArray[1]);
                                _pointArray.Clear();
                            }
                        }
                        else
                        {
                            if (_pointArray.Count == 2)
                            {
                                _pointArray[_pointArray.Count - 1] = coordinate;
                                p1 = _mapbox.Map.WorldToImage(_pointArray[0]);
                                p2 = _mapbox.Map.WorldToImage(_pointArray[1]);
                                ObjectCreation.AddLink(_mapbox, _pointArray[0], _pointArray[1]);
                                _pointArray.Clear();
                            }
                        }
                    }
 
                }

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
