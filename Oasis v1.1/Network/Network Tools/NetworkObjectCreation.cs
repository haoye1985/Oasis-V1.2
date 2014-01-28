using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using SharpMap.Forms;
using GeoAPI.Geometries;
using Pan.Diagram.Basic;
using Pan.Diagram;

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

            //TODO 
            // define the shape convert to a new class

            BasicNode _Shape = new BasicNode();
            _Shape.CenterX = (float)point.X;
            _Shape.CenterY = (float)point.Y;
            _Shape.ComponentName = "Node";
            _Shape.AlwaysScaleOnPageChange = true;
            ParentNode node1 = new ParentNode();
            node1.X = (float)point.X;
            node1.Y = (float)point.Y;
            _Shape.ParentNode = node1;

            Forms.dockDesignDiagram.diagramControl1.diagramPicture1.AddObject(_Shape);
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
