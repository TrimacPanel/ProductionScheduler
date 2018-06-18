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
    public partial class frmScheduleOrder : Form
    {

        MonthCalendar cal = new MonthCalendar();

        private bool m_bCancel;
        private string m_sCreatedBy;
        private DateTime m_dCreatedAt;
        private string m_sModifiedBy;
        private DateTime m_dModifiedAt;

        private DropdownCalendar m_ctlCalendar;

        public frmScheduleOrder()
        {
            InitializeComponent();

            m_ctlCalendar = new DropdownCalendar();

            grdMain.Cols["schedule_date"].CellStyle.DropDownControl = m_ctlCalendar;

            grdMain.Cols["schedule_date"].CellStyle.TypeFlags = iGCellTypeFlags.NoTextEdit | iGCellTypeFlags.ComboPreferValue;

            m_bCancel = true;

        }

        private bool AddTask(string Line, string Task)
        {
            if (TaskExists(Task))
            {
                MessageBox.Show("Task already exists and will not be added!", "Add Task", MessageBoxButtons.OK);
                return false;
            }

            iGRow r = grdMain.Rows.Add();
            r.Cells[0].Value = Task;
            r.Cells[1].Value = Line;
            r.Cells[2].Value = DateTime.Now.AddDays(1).ToString("d");

            return true;
        }

        private bool TaskExists(string Task)
        {
            bool b = false;

            foreach(iGRow r in grdMain.Rows)
            {
                if (r.Cells[0].Text == Task)
                {
                    b = true;
                    break;
                }
            }

            return b;
        }

        private void frmScheduleOrder_Load(object sender, EventArgs e)
        {

        }

        public bool Cancel
        {
            get { return m_bCancel; }
        }

        public string CreatedBy
        {
            get { return m_sCreatedBy; }
            set { m_sCreatedBy = value; }
        }

        public DateTime CreatedAt
        {
            get { return m_dCreatedAt; }
            set { m_dCreatedAt = value; }
        }

        public string ModifiedBy
        {
            get { return m_sModifiedBy; }
            set { m_sModifiedBy = value; }
        }

        public DateTime ModifiedAt
        {
            get { return m_dModifiedAt; }
            set { m_dModifiedAt = value; }
        }

        public iGRowCollection ScheduleRows
        {
            get { return grdMain.Rows; }
        }

        public string OrderNo
        {
            get { return txtOrderNo.Text; }
            set { txtOrderNo.Text = value; }            
        }

        public string ItemNo
        {
            get { return txtItemNo.Text;  }
            set { txtItemNo.Text = value; }
        }

        public string Overlay
        {
            get { return txtOverlay.Text; }
            set { txtOverlay.Text = value; }
        }

        public string Substrate
        {
            get { return txtSubstrate.Text; }
            set { txtSubstrate.Text = value; }
        }

        public string OrderType
        {
            get { return txtOrderType.Text; }
            set { txtOrderType.Text = value; }
        }

        public string Description
        {
            get { return txtDescription.Text;  }
            set { txtDescription.Text = value; }
        }

        public string Quantity
        {
            get { return txtQuantity.Text;  }
            set { txtQuantity.Text = value; }
        }

        public string DueDate
        {
            get { return txtDueDate.Text; }
            set { txtDueDate.Text = value; }
        }

        public string Customer
        {
            get { return txtCustomer.Text; }
            set { txtCustomer.Text = value; }
        }

        public string Saleperson
        {
            get { return txtSalesperson.Text;  }
            set { txtSalesperson.Text = value; }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AddTask("Dado Line", "Dado");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTask("Harlan 1", "Laminate");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            iGRow r = grdMain.Rows.Add();
            r.Cells[0].Value = "Groove/Stripe";
            r.Cells[1].Value = "Harlan 1";
            r.Cells[2].Value = DateTime.Now.AddDays(1).ToString("d");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddTask("Harlan 1", "Brush/Pack");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddTask("Harlan 2", "Laminate");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            iGRow r = grdMain.Rows.Add();
            r.Cells[0].Value = "Groove/Stripe";
            r.Cells[1].Value = "Harlan 2";
            r.Cells[2].Value = DateTime.Now.AddDays(1).ToString("d");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddTask("Harlan 2", "Brush/Pack");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AddTask("Walco", "Laminate");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AddTask("Walco", "Brush/Pack");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AddTask("Edgebander", "Edgeband");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AddTask("Saw", "Saw");
        }

        private void grdMain_RequestEdit(object sender, iGRequestEditEventArgs e)
        {
            
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            //TODO: Date handling
            if (grdMain.SelectedRows.Count > 0)
            {
                if (grdMain.SelectedRows[0].Index > 0)
                {
                    grdMain.SelectedRows[0].Index = 0;
                }
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (grdMain.SelectedRows.Count > 0)
            {
                if (grdMain.SelectedRows[0].Index > 0)
                {
                    grdMain.SelectedRows[0].Index = grdMain.SelectedRows[0].Index - 1;
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (grdMain.SelectedRows.Count > 0)
            {
                if (grdMain.SelectedRows[0].Index < grdMain.Rows.Count - 1)
                {
                    grdMain.SelectedRows[0].Index = grdMain.SelectedRows[0].Index + 1;
                }
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (grdMain.SelectedRows.Count > 0)
            {
                if (grdMain.SelectedRows[0].Index < grdMain.Rows.Count - 1)
                {
                    grdMain.SelectedRows[0].Index = grdMain.Rows.Count - 1;
                }
            }
        }

        private void grdMain_BeforeContentsSorted(object sender, EventArgs e)
        {
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (grdMain.Rows.Count == 0)
            {
                if (MessageBox.Show("No tasks have been scheduled for this job.  Do you wish to cancel now?", "Schedule Job", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }
            }

            m_bCancel = false;

            this.Visible = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            m_bCancel = true;

            this.Visible = false;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            AddTask("Punch Press", "Punch");
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            AddTask("Groover", "Groove/Stripe");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AddTask("Groover", "Brush/Pack");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdMain.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Are you sure you wise to delete the selected task?", "Delete Task", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    grdMain.Rows.RemoveAt(grdMain.SelectedRows[0].Index);
                }
            }
        }
    }
}
