using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoAPI.Geometries;

namespace Oasis_v1._1
{
    public class InfraNetwork
    {
        private List<InfraNode> _infraNodes;
        private List<InfraLink> _infraLinks;

        private List<IPoint> _PointGeometries = new List<IPoint>();
        private List<ILineString> _LineGemetries = new List<ILineString>();
        private List<IPoint> _centroids = new List<IPoint>();

        #region Constructor

        public InfraNetwork()
        {
            _infraNodes = new List<InfraNode>();
            _infraLinks = new List<InfraLink>();
            Initialise();
        }

        #endregion

        private void Initialise()
        {
            DoObtainGeometry();
        }

        private void DoObtainGeometry()
        {
            foreach (var infranode in _infraNodes)
            {
                _PointGeometries.Add(infranode.Point);
            }

            foreach (var infralink in _infraLinks)
            {
                _LineGemetries.Add(infralink.Line);
            }

            foreach (var infralink in _infraLinks)
            {
                _centroids.Add(infralink.Line.Centroid);
            }
        }

        #region Methods

        public void AddNode(InfraNode node)
        {
            _infraNodes.Add(node);
            _PointGeometries.Add(node.Point);
        }

        public void AddLink(InfraLink link)
        {
            _infraLinks.Add(link);
            _LineGemetries.Add(link.Line);
            _centroids.Add(link.Line.Centroid);
        }

        public void DeleteLink(InfraLink link)
        {
            _infraLinks.Remove(link);
        }

        public void DeleteNode(InfraNode node)
        {
            _infraNodes.Remove(node);
        }

        public void DeleteAllLink()
        {
            for (int i = 0; i < _infraLinks.Count; i++)
            {
                _infraLinks.Remove(_infraLinks[i]);
            }
        }

        public void DeleteAllNodes()
        {
            for (int i = 0; i < _infraNodes.Count; i++)
            {
                _infraNodes.Remove(_infraNodes[i]);
            }
        }

        #endregion

        #region Properties

        public List<InfraNode> InfraNodes
        {
            get
            {
                return _infraNodes;
            }
            set
            {
                _infraNodes = value;
            }
        }

        public List<InfraLink> InfraLinks
        {
            get
            {
                return _infraLinks;
            }
            set
            {
                _infraLinks = value;
            }
        }

        public List<IPoint> NodeGeometry
        {
            get { return _PointGeometries; }
            set { _PointGeometries = value; }
        }

        public List<ILineString> LineGeometry
        {
            get { return _LineGemetries; }
            set { _LineGemetries = value; }
        }

        public List<IPoint> Centriods
        {
            get { return _centroids; }
            set { _centroids = value; }
        }

        #endregion

    }
}
