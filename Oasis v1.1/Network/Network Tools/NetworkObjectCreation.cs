using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using SharpMap.Forms;
using GeoAPI.Geometries;

namespace Oasis_v1._1
{
    public static class ObjectCreation
    {


        public static void AddNode(MapBox mapbox, Coordinate coordinate, System.Drawing.Point point)
        {
            mapbox.Cursor = Cursors.Cross;
            InfraNode node = new InfraNode(coordinate);
            States.Network.AddNode(node);
            Forms.dockDiagram.pDiagram1.AddRectangle(point.X, point.Y);
            ControlHelper.AddNetworkNode(Forms.dockNode.dataGridViewPersistent1, node);
            mapbox.Invalidate();
        }

        public static void AddLink(MapBox mapbox, Coordinate start, Coordinate end)
        {
            InfraLink link = new InfraLink(start, end);
            States.Network.AddLink(link);
            PointF centroid = mapbox.Map.WorldToImage(link.Line.Centroid.Coordinate);
            Forms.dockDiagram.pDiagram1.AddRectangle((int)centroid.X, (int)centroid.Y);
            ControlHelper.AddNetworkLink(Forms.dockLink.dataGridViewPersistent1, link);
            mapbox.Invalidate();
        }

    }
}
