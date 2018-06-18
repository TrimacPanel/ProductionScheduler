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
    public partial class frmPromptStages : Form
    {

        private DateTimePicker dtp;

        public frmPromptStages()
        {
            InitializeComponent();

            InitGrid();
        }

        private void InitGrid()
        {

            Dictionary<int, string> tasks = new Dictionary<int, string>();
            tasks.Add(1, "Laminate");
            tasks.Add(2, "Groove / Stripe");
            tasks.Add(3, "Punch");
            tasks.Add(4, "Saw");
            tasks.Add(5, "Edgeband");
            tasks.Add(6, "Brush & Pack");

            DataGridViewComboBoxColumn cboTasks = new DataGridViewComboBoxColumn();
            cboTasks.DataSource = tasks.ToList();
            cboTasks.Name = "Task";
            cboTasks.DataPropertyName = "byr_plnr";
            cboTasks.ValueMember = "Key";
            cboTasks.DisplayMember = "Value";
            cboTasks.FlatStyle = FlatStyle.Flat;

            grdMain.Columns.Add(cboTasks);

            Dictionary<int, string> lines = new Dictionary<int, string>();
            lines.Add(1, "Harlan 1");
            lines.Add(2, "Harlan 2");
            lines.Add(3, "Walco");
            lines.Add(4, "Dado");
            lines.Add(5, "Edgebander");
            lines.Add(6, "Groove/Stripe");
            lines.Add(7, "Punch Press");
            lines.Add(8, "Saw");
            lines.Add(9, "Brush & Pack");

            DataGridViewComboBoxColumn cboLines = new DataGridViewComboBoxColumn();
            cboLines.DataSource = lines.ToList();
            cboLines.Name = "Line";
            cboLines.DataPropertyName = "line";
            cboLines.ValueMember = "Key";
            cboLines.DisplayMember = "Value";
            cboLines.FlatStyle = FlatStyle.Flat;

            grdMain.Columns.Add(cboLines);

            grdMain.Columns.Add("date", "Date");
            grdMain.Columns.Add("note", "Note");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmPromptStages_Load(object sender, EventArgs e)
        {

        }

        private void grdMain_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            { 
                dtp = new DateTimePicker();
                grdMain.Controls.Add(dtp);
                dtp.Format = DateTimePickerFormat.Short;
                Rectangle Rectangle = grdMain.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
                dtp.Size = new Size(Rectangle.Width, Rectangle.Height);
                dtp.Location = new Point(Rectangle.X, Rectangle.Y);

                dtp.CloseUp += new EventHandler(dtp_CloseUp);
                dtp.TextChanged += new EventHandler(dtp_OnTextChange);

                dtp.Visible = true;
            }
        }

        private void dtp_OnTextChange(object sender, EventArgs e)
        {
   
            grdMain.CurrentCell.Value = dtp.Text.ToString();
        }

        void dtp_CloseUp(object sender, EventArgs e)
        {
            dtp.Visible = false;
        }
    
    }
}
