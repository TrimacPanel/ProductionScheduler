using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductionScheduler
{
    public partial class frmPrintSchedule : Form
    {
        private bool m_bCancel = false;
        private DateTime m_dSelectedDate;


        public bool Cancel
        {
            get
            {
                return m_bCancel;
            }
        }

        public DateTime SelectedDate
        {
            get
            {
                return m_dSelectedDate;
            }
        }

        public frmPrintSchedule()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void calMain_DateChanged(object sender, DateRangeEventArgs e)
        {
            m_dSelectedDate = calMain.SelectionStart;
        }

        private void frmPrintSchedule_Load(object sender, EventArgs e)
        {
            m_dSelectedDate = DateTime.Today;
            calMain.SelectionStart = DateTime.Today;
        }
    }
}
