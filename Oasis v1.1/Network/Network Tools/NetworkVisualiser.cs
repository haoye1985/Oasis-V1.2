using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpMap.Forms;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using System.Drawing;
using NetTopologySuite.Geometries;
using SharpMap;
using BruTile.Web;

namespace Oasis_v1._1
{
    public class NetworkVisualiser
    {
        public static void DrawNetwork(Graphics p, Map map, InfraNetwork network)
        {
            if (network.InfraNodes.Count != 0)
            {
                GeometryFeatureProvider gfp = new GeometryFeatureProvider(network.NodeGeometry);
                VectorLayer layer = new VectorLayer("NetworkNode");
                layer.DataSource = gfp;
                layer.IsQueryEnabled = true;
                layer.Render(p, map);
            }

            if (network.LineGeometry.Count != 0)
            {
                GeometryFeatureProvider gf3 = new GeometryFeatureProvider(network.LineGeometry);
                VectorLayer layer2 = new VectorLayer("NetworkLink", gf3);
                layer2.IsQueryEnabled = true;
                layer2.Style.Line = new System.Drawing.Pen(Color.Black, 2F);
                layer2.Render(p, map);
            }

            if (network.Centriods.Count != 0)
            {
                GeometryFeatureProvider gf3 = new GeometryFeatureProvider(network.Centriods);
                VectorLayer layer2 = new VectorLayer("NetworkLink", gf3);
                layer2.IsQueryEnabled = true;

                var pps = SharpMap.Rendering.Symbolizer.PathPointSymbolizer.CreateSquare(new System.Drawing.Pen(System.Drawing.Color.Blue, 4),new System.Drawing.SolidBrush(System.Drawing.Color.DodgerBlue), 5);
                layer2.Style.PointSymbolizer = pps;
                //layer2.Style.Line = new System.Drawing.Pen(Color.Black, 2F);
                layer2.Render(p, map);

            }
        }





    }
}
