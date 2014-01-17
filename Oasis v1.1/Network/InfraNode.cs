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

            private readonly double expansionFactor = 1.0;
          
            private IPoint _point;
            private Coordinate _coordinate;
            private GeometryFactory factory = new GeometryFactory();
            private Envelope _hitTestZone = new Envelope();
            private bool _isSelected;

            private int _ID;
            private string _nodeName = "NodeName";

        #endregion

        #region Constructors

            public InfraNode(IPoint point)
            {
                _point = point;
                _coordinate = _point.Coordinate;
                _hitTestZone = new Envelope(_point.Coordinate);
                _hitTestZone.ExpandBy(expansionFactor);
            }

            public InfraNode(Coordinate coordinate)
            {
                _point = factory.CreatePoint(coordinate);
                _coordinate = _point.Coordinate;
                _hitTestZone = new Envelope(_point.Coordinate);
                _hitTestZone.ExpandBy(expansionFactor);
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

        public IPoint Point
        {
            get
            {
                return _point;
            }
            set
            {
                _point = value;
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
