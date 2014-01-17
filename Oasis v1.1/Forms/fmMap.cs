using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Oasis_v1._1
{
    public partial class fmMap : DockContent
    {
        public fmMap()
        {
            InitializeComponent();
        }

        private void mapBox1_MouseMove(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
            this.stripCoordinate.Text = " Lat: " + worldPos.Y.ToString("N2") + " Lon: " + worldPos.X.ToString("N2");
        }



    }
}
