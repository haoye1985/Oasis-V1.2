using System;
using System.Collections.Generic;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
using System.Drawing;
using System.Drawing.Drawing2D;
using SharpMap.Layers;
using SharpMap.Data.Providers;

namespace Oasis_v1._1
{
    public class InfraNode
    {
        #region Fields

            private IPoint _geometry;
            private Coordinate _coordinate;


            private string _nodeName = "NodeName";

            private GeometryFactory factory = new GeometryFactory();
            private Envelope _hitTestZone = new Envelope();
            private bool _isSelected;

            private int _ID;
            public float RiskXi { get; set; }
            public float RiskCi { get; set; }

        #endregion

        #region Constructors

            public InfraNode() { }

            public InfraNode(IPoint point)
            {
                _geometry = point;
                _coordinate = _geometry.Coordinate;
                _hitTestZone = new Envelope(_geometry.Coordinate);
                _hitTestZone.ExpandBy(NetworkConstants.ExpansionFactor);
            }

            public InfraNode(Coordinate coordinate)
            {
                _geometry = factory.CreatePoint(coordinate);
                _coordinate = _geometry.Coordinate;
                _hitTestZone = new Envelope(_geometry.Coordinate);
                _hitTestZone.ExpandBy(NetworkConstants.ExpansionFactor);
            }
       
        #endregion

        #region Methods

            public bool EnvelopeContains(Coordinate p)
            {
                bool isContained;
                return isContained = _hitTestZone.Contains(p);
            }

        #endregion

        #region Properties

        public int ID
        {
            get
            { return _ID; }
            set
            { _ID = value; }
        }

        public string NodeName
        {
            get { return _nodeName; }
            set { _nodeName = value; }
        }

        public Coordinate Coordinate
        {
            get { return _coordinate; }
            set { _coordinate = value; }
        }

        public IPoint Geometry
        {
            get
            {
                return _geometry;
            }
            set
            {
                _geometry = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = true;
            }
        }

        #endregion
    }
}
