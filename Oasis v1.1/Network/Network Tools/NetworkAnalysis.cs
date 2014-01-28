using System;
using System.Collections.Generic;


namespace Oasis_v1._1
{
    public class NetworkAnalysis
    {
        private bool _isGraphConnected;
        private int isolatedNodes;
        private InfraNetwork _network = new InfraNetwork();

        public NetworkAnalysis(InfraNetwork network)
        {
            _network = network;
        }

        public bool IsAllConnected()
        {
            List<InfraLink> connectedLinks = new List<InfraLink>();

            foreach (var link in _network.InfraLinks)
            {
                foreach (var node in _network.InfraNodes)
                {
                    if ((link.FromNode.Coordinate == node.Coordinate))
                    {
                        link.FromNodeConnected = true;
                    }

                    if (link.ToNode.Coordinate == node.Coordinate)
                    {
                        link.ToNodeConnected = true;
                    }
                }

                if (link.FromNodeConnected && link.ToNodeConnected)
                {
                    link.LinkConnected = true;
                    connectedLinks.Add(link);
                }
            }

            if (connectedLinks.Count == _network.InfraLinks.Count)
            {
                _isGraphConnected = true;
            }
            else
            {
                _isGraphConnected = false;
            }

            return _isGraphConnected;
        }


        public int IsolatedNodeNumber
        {
            get { return isolatedNodes; }
            set { isolatedNodes = value; }
        }


    }
}
