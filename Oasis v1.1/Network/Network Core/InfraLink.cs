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

        // Basic Variable
        private int _ID;
        private string _name;

        // Geometry Variables
        private InfraNode _fromNode = new InfraNode();
        private InfraNode _toNode = new InfraNode();
        private InfraNode _centroidNode = new InfraNode();
        private Coordinate[] _coordConnection = new Coordinate[2];
        private ILineString _infraSegment;

        // Infrastructure Variables
        public float RiskAij { get; set; }

        // Other Variables
        private GeometryFactory factory = new GeometryFactory();

        public bool FromNodeConnected { get; set; }
        public bool ToNodeConnected { get; set; }
        public bool LinkConnected { get; set; }

        #endregion

        #region Constructors

        public InfraLink(){}

        public InfraLink(IPoint startLinkPoint, IPoint endLinkPoint)
        {
            _coordConnection[0] = startLinkPoint.Coordinate;
            _coordConnection[1] = endLinkPoint.Coordinate;
            _fromNode = new InfraNode(_coordConnection[0]);
            _toNode = new InfraNode(_coordConnection[1]);         
            _infraSegment = factory.CreateLineString(_coordConnection);
        }

        public InfraLink(Coordinate startLinkCoordinate, Coordinate endLinkCooridnate)
        {
            _coordConnection[0] = startLinkCoordinate;
            _coordConnection[1] = endLinkCooridnate;
            _fromNode = new InfraNode(_coordConnection[0]);
            _toNode = new InfraNode(_coordConnection[1]);
            _infraSegment = factory.CreateLineString(_coordConnection);
        }

        public InfraLink(InfraLink infraLink)
        {
            _coordConnection[0] = infraLink.FromNode.Coordinate;
            _coordConnection[1] = infraLink.ToNode.Coordinate;
            _fromNode = new InfraNode(_coordConnection[0]);
            _toNode = new InfraNode(_coordConnection[1]);
            _infraSegment = factory.CreateLineString(_coordConnection);
        }

        #endregion

        #region Properties

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public InfraNode FromNode
        {
            get {return _fromNode;}
            set {_fromNode = value;}
        }

        public InfraNode ToNode
        {
            get { return _toNode; }
            set { _toNode = value; }
        }

        public ILineString InfraSegment
        {
            get { return _infraSegment; }
            set { _infraSegment = value; }
        }

        #endregion

    }
}
