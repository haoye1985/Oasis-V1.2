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
        #region Private Field

        private string _networkID;
        private string _networkName;
        private NetworkType _networkType = NetworkType.Default_Network;
        private InfraNodesCollection _networkNodesCollection = new InfraNodesCollection();
        private InfraLinksCollection _networkLinksCollection = new InfraLinksCollection();

        #endregion

        #region Constructor

        public InfraNetwork() {}

        public InfraNetwork(InfraNodesCollection Nodes, InfraLinksCollection Links)
        {
            _networkNodesCollection.InfraNodes = Nodes.InfraNodes;
            _networkLinksCollection.InfraLinks = Links.InfraLinks;
        }

        #endregion

        #region Methods

        public void AddNodeCollection(InfraNodesCollection Nodes)
        {
            _networkNodesCollection = Nodes;
        }

        public void AddLinkCollection(InfraLinksCollection Links)
        {
            _networkLinksCollection = Links;
        }

        public void AddNodeToNetwork(InfraNode node)
        {
            _networkNodesCollection.AddNodeToCollection(node);
        }

        public void AddLinkToNetwork(InfraLink link)
        {
            _networkLinksCollection.AddLinkToCollection(link);
        }

        public void DeleteLinkFromNetwork(InfraLink link)
        {
            _networkLinksCollection.RemoveLinkFromCollection(link);
        }

        public void DeleteNodeFromNetwork(InfraNode node)
        {
            _networkNodesCollection.RemoveNodeFromCollection(node);
        }

        public string GenerateID()
        {
            return Guid.NewGuid().ToString();
        }

        public bool IsCollectionEmpty()
        {
            return (_networkNodesCollection.InfraNodes.Count == 0) && (_networkLinksCollection.InfraLinks.Count == 0);
        }

        public bool IsNodeCollectionEmpty()
        {
            return _networkNodesCollection.InfraNodes.Count == 0;
        }

        public bool IsLinkCollectionEmpty()
        {
            return _networkLinksCollection.InfraLinks.Count == 0;
        }

        public void DeleteAllLink()
        {
            for (int i = 0; i < _networkLinksCollection.InfraLinks.Count; i++)
            {
                _networkLinksCollection.RemoveLinkFromCollection(_networkLinksCollection.InfraLinks[i]);
            }
        }

        public void DeleteAllNodes()
        {
            for (int i = 0; i < _networkNodesCollection.InfraNodes.Count; i++)
            {
                _networkNodesCollection.RemoveNodeFromCollection(_networkNodesCollection.InfraNodes[i]);
            }
        }

        #endregion

        #region Properties

        public string NetworkID
        {
            get { return _networkID; }
            set { _networkID = value; }
        }

        public string NetworkName
        {
            get { return _networkName; }
            set { _networkName = value; }
        }

        public NetworkType NetworkWorkingType
        {
            get { return _networkType; }
            set { _networkType = value; }
        }

        public InfraNodesCollection NodesCollection
        {
            get { return _networkNodesCollection; }
            set { _networkNodesCollection = value; }
        }

        public InfraLinksCollection LinksCollection
        {
            get { return _networkLinksCollection; }
            set { _networkLinksCollection = value; }
        }

        public string InfraNodesCount
        {
            get { return _networkNodesCollection.InfraNodes.Count.ToString(); }
        }

        public string InfraLinksCount
        {
            get { return _networkLinksCollection.InfraLinks.Count.ToString(); }
        }

        #endregion

    }
}
