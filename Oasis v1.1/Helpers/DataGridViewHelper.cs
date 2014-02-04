using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pan.Utilities;
using System.Windows.Forms;

namespace Oasis_v1._1
{
    public static class DataGridViewHelper
    {

        public static void CopySelectedRow(DataGridViewPersistent dgv_org, DataGridViewRow row)
        {
            DataGridViewPersistent dgv_copy = new DataGridViewPersistent();

            try
            {
                if (dgv_copy.Columns.Count == 0)
                {
                    foreach (DataGridViewColumn dgvc in dgv_org.Columns)
                    {
                        dgv_copy.Columns.Add(dgvc.Clone() as DataGridViewColumn);
                    }
                }

                row = (DataGridViewRow)row.Clone();
                row.Cells[0].Value = row.Cells[0].Value;
                dgv_copy.Rows.Add(row);

                dgv_copy.AllowUserToAddRows = false;
                dgv_copy.Refresh();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void CopySelectedRows(DataGridViewPersistent CopyFrom, DataGridViewPersistent CopyTo)
        {
            CopyTo.Rows.Clear();

            if (CopyTo.Columns.Count == 0)
            {
                foreach (DataGridViewColumn c in CopyFrom.Columns)
                {
                    CopyTo.Columns.Add(c.Clone() as DataGridViewColumn);
                }
            }

            foreach (DataGridViewRow r in CopyFrom.SelectedRows)
            {
                int index = CopyTo.Rows.Add(r.Clone() as DataGridViewRow);
                foreach (DataGridViewCell o in r.Cells)
                {
                    CopyTo.Rows[index].Cells[o.ColumnIndex].Value = o.Value;
                }
            }
        }

        public static DataGridViewPersistent CopySelectedDataView(DataGridViewPersistent dgv_org)
        {
            DataGridViewPersistent dgv_copy = new DataGridViewPersistent();

            try
            {
                if (dgv_copy.Columns.Count == 0)
                {
                    foreach (DataGridViewColumn dgvc in dgv_org.Columns)
                    {
                        dgv_copy.Columns.Add(dgvc.Clone() as DataGridViewColumn);
                    }
                }

                DataGridViewRow row = new DataGridViewRow();

                for (int i = 0; i < dgv_org.SelectedRows.Count; i++)
                {
                    row = (DataGridViewRow)dgv_org.Rows[i].Clone();
                    int intColIndex = 0;
                    foreach (DataGridViewCell cell in dgv_org.SelectedRows[i].Cells)
                    {
                        row.Cells[intColIndex].Value = cell.Value;
                        intColIndex++;
                    }
                    dgv_copy.Rows.Add(row);
                }
                dgv_copy.AllowUserToAddRows = false;
                dgv_copy.Refresh();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return dgv_copy;
        }





    }
}
