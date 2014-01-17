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
    public partial class fmLink : DockContent
    {
        public fmLink()
        {
            InitializeComponent();
        }

        private void dataGridViewPersistent1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewPersistent1.SelectedRows.Count > 0)
            {
                int selectedIndex = dataGridViewPersistent1.SelectedRows[0].Index;
                DataGridViewHelper.CopySelectedRows(dataGridViewPersistent1, dataGridViewPersistent2);
                dataGridViewPersistent2.Refresh();
            }
        }
    }
}
