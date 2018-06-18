using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TenTec.Windows.iGridLib;

namespace ProductionScheduler
{
    public partial class frmShowHideColumns : Form
    {

        private iGrid m_grdActive;

        public frmShowHideColumns()
        {
            InitializeComponent();

            grdMain.Cols[0].CellStyle.Type = TenTec.Windows.iGridLib.iGCellType.Check;
        }

        public iGrid ActiveGrid
        {
            get { return m_grdActive;  }
            set
            {
                m_grdActive = value;
                grdMain.Rows.Clear();

                iGRow r;

                foreach (iGCol c in m_grdActive.Cols)
                {
                    r = grdMain.Rows.Add();
                    r.Cells[0].Value = c.Visible;
                    r.Cells[1].Value = c.Text;
                }
            }
        }

        private void frmShowHideColumns_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            for (int i = 0;i < grdMain.Rows.Count; i++)
            {
                m_grdActive.Cols[i].Visible = Convert.ToBoolean(grdMain.Rows[i].Cells[0].Value);
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
