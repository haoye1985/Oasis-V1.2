using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System.Drawing;
using System.Drawing.Drawing2D;
using SharpMap.Layers;
using SharpMap.Data.Providers;

namespace Oasis_v1._1
{
    public class InfraLink
    {
        #region Fields

        private readonly double expansionFactor = 1.0;

        private int _ID;

        private IPoint _startPoint;

        private IPoint _endPoint;

        //private IPoint _centroid;

        private GeometryFactory factory = new GeometryFactory();

        private List<Coordinate> _pointArray = new List<Coordinate>();

        private Envelope _hitTestStartZone = new Envelope();

        private Envelope _hitTestEndZone = new Envelope();

        private Coordinate[] coordinates = new Coordinate[2];

        private List<IGeometry> _linkSegment = new List<IGeometry>();

        private ILineString _lineSegment;

        public InfraNode FromNode { get; set; }

        public InfraNode ToNode { get; set; }

        public float RiskAij{get; set;}

        public bool FromNodeConnected { get; set; }

        public bool ToNodeConnected { get; set; }

        public bool LinkConnected { get; set; }

        #endregion

        #region Constructors

        public InfraLink(IPoint start, IPoint end)
        {
            _startPoint = start;
            _endPoint = end;
            Initialise();
        }

        public InfraLink(Coordinate start, Coordinate end)
        {
            _startPoint = factory.CreatePoint(start);
            _endPoint = factory.CreatePoint(end);
            Initialise();
        }

        public InfraLink(InfraLink infralink)
        {
            _startPoint = infralink.StartPoint;
            _startPoint = infralink.EndPoint;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Construct the infralink parameters
        /// </summary>
        private void Initialise()
        {
            // Obtain the coordinate to link vertices
            coordinates[0] = _startPoint.Coordinate;
            coordinates[1] = _endPoint.Coordinate;

            FromNode = new InfraNode(_startPoint.Coordinate);
            ToNode = new InfraNode(_endPoint.Coordinate);

            _hitTestStartZone = new Envelope(_startPoint.Coordinate);
            _hitTestEndZone = new Envelope(_endPoint.Coordinate);

            _lineSegment = factory.CreateLineString(coordinates);
            _linkSegment.Add(_startPoint);
            _linkSegment.Add(_endPoint);
            _linkSegment.Add(_lineSegment);

            _hitTestStartZone.ExpandBy(expansionFactor);
            _hitTestEndZone.ExpandBy(expansionFactor);
        }

        /// <summary>
        /// return wheher the position included in the zone of starting point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool EnvelopeStartContains(Coordinate p)
        {
            bool isContained;
            return isContained = _hitTestStartZone.Contains(p);
        }

        /// <summary>
        /// return wheher the position included in the zone of ending point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool EnvelopeEndContains(Coordinate p)
        {
            bool isContained;
            return isContained = _hitTestEndZone.Contains(p);
        }

        #endregion

        #region Properties


        public IPoint StartPoint
        {
            get { return _startPoint; }
            set { _startPoint = value; }
        }

        public IPoint EndPoint
        {
            get { return _endPoint; }
            set { _endPoint = value; }
        }

        public ILineString Line
        {
            get { return _lineSegment; }
            set { _lineSegment = value; }
        }

        public List<IGeometry> Geometries
        {
            get { return _linkSegment; }
            set { _linkSegment = value; }
        }

        public int ID
        {
            get
            { return _ID; }
            set
            { _ID = value; }
        }

        #endregion

    }
}
