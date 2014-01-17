using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pan.Utilities;

namespace Oasis_v1._1
{
    public class ControlHelper
    {
        public static void AddNetworkNode(DataGridViewPersistent dgvp, InfraNode node)
        {
            if (dgvp != null&&node!=null)
            {
                dgvp.Rows.Add(node.ID, node.NodeName, node.IsSelected, node.Point.OgcGeometryType, node.Point.SRID, node.Point.X, node.Point.Y, node.Point.SRID);
            }
 
        }

        public static void AddNetworkLink(DataGridViewPersistent dgvp, InfraLink link)
        {
            if (dgvp != null && link != null)
            {
                dgvp.Rows.Add(link.ID, link.StartPoint.Coordinate.X, link.EndPoint.Coordinate.Y, link.Line.Centroid.Coordinate.X, link.Line.Centroid.Coordinate.Y);
            }
        }


        public static void AddNetworkNodeColumn(DataGridViewPersistent dgvp)
        {
            if (dgvp != null)
            {
                dgvp.Columns.Add("Node_ID", "nodeid");
                dgvp.Columns.Add("Node_Name", "nodename");
                dgvp.Columns.Add("Network_Name", "netname");
                dgvp.Columns.Add("Network_ID", "netid");
                dgvp.Columns.Add("IGC_Type", "gtype");
                dgvp.Columns.Add("Coordinate_X", "Latitude");
                dgvp.Columns.Add("Coordinate_Y", "longitude");
                dgvp.Columns.Add("Datum", "SRID");
                dgvp.Columns.Add("Precision_Model", "Precision");
            }
            else
            {
                throw new Exception("the datagridview is not exit or not correct version! ");
            }
        }

        public static void AddNetworkLinkColumn(DataGridViewPersistent dgvp)
        {
            if (dgvp != null)
            {
                dgvp.Columns.Add("Link_ID", "linkid");
                dgvp.Columns.Add("Link_Name", "linkname");
                dgvp.Columns.Add("Network_Name", "netname");
                dgvp.Columns.Add("Network_ID", "netid");
                dgvp.Columns.Add("IGC_Type", "gtype");
                dgvp.Columns.Add("Coordinate_X", "Latitude");
                dgvp.Columns.Add("Coordinate_Y", "longitude");
                dgvp.Columns.Add("Datum", "SRID");
                dgvp.Columns.Add("Precision_Model", "Precision");
            }
            else
            {
                throw new Exception("the datagridview is not exit or not correct version! ");
            }
        }




        public static void Clear(DataGridViewPersistent dgvp)
        {
            if (dgvp != null)
            {
                dgvp.Columns.Clear();
                dgvp.DataSource = null;
            }
        }

    }
}
