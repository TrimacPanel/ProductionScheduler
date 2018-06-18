using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ProductionScheduler
{
    public partial class frmSchedule : Form
    {
        private string m_sConnection = "Server=macola;Database=data_02;Trusted_Connection=True;";

        public frmSchedule()
        {
            InitializeComponent();

            InitializeForm();

            GetOrderData();

            //GetProdData();


        }


        private void InitializeForm()
        {
            
            // Init pending production datagrid
            grdOrders.AutoGenerateColumns = false;
            grdOrders.DefaultCellStyle.Font = new Font("Calibri", 12, GraphicsUnit.Point);

            grdOrders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grdOrders.MultiSelect = false;
            grdOrders.RowPrePaint += new DataGridViewRowPrePaintEventHandler(grdOrders_RowPrePaint);

            grdOrders.Columns.Add("ord_no", "TMV #");
            grdOrders.Columns[0].DataPropertyName = "ord_no";
            grdOrders.Columns[0].Width = 70;

            grdOrders.Columns.Add("cus_name", "Reference");
            grdOrders.Columns[1].DataPropertyName = "cus_name";
            grdOrders.Columns[1].Width = 350;
            grdOrders.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            grdOrders.Columns.Add("item_no", "SKU");
            grdOrders.Columns[2].DataPropertyName = "item_no";
            grdOrders.Columns[2].Width = 140;

            grdOrders.Columns.Add("overlay_sku", "Overlay");
            grdOrders.Columns[3].DataPropertyName = "overlay_sku";
            grdOrders.Columns[3].Width = 140;

            grdOrders.Columns.Add("substrate_sku", "Substrate");
            grdOrders.Columns[4].DataPropertyName = "substrate_sku";
            grdOrders.Columns[4].Width = 150;

            grdOrders.Columns.Add("item_desc_1", "Description");
            grdOrders.Columns[5].DataPropertyName = "item_desc_1";
            grdOrders.Columns[5].Width = 350;

            grdOrders.Columns.Add("ord_qty", "Qty.");
            grdOrders.Columns[6].DataPropertyName = "ord_qty";
            grdOrders.Columns[6].Width = 70;
            grdOrders.Columns[6].DefaultCellStyle.Format = "N0";
            grdOrders.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            grdOrders.Columns.Add("due_dt", "Due Date");
            grdOrders.Columns[7].DataPropertyName = "due_dt";
            grdOrders.Columns[7].Width = 100;
            grdOrders.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            Dictionary<int, string> salespeople = new Dictionary<int, string>();
            salespeople.Add(17, "Dick");
            salespeople.Add(22, "Dave");
            salespeople.Add(20, "Vic");
            salespeople.Add(24, "Vince");
            salespeople.Add(14, "Tim");
            salespeople.Add(16, "Default");
            salespeople.Add(0, "Unknown(0)");
            salespeople.Add(6, "Unknown(6)");

            DataGridViewComboBoxColumn cbo = new DataGridViewComboBoxColumn();
            cbo.DataSource = salespeople.ToList();
            cbo.Name = "Salesperson";
            cbo.DataPropertyName = "byr_plnr";            
            cbo.ValueMember = "Key";
            cbo.DisplayMember = "Value";
            cbo.Width = 100;
            cbo.FlatStyle = FlatStyle.Flat;
            grdOrders.Columns.Add(cbo);


            // Init prod grid

            //grdProd.ColumnCount = 12;

            /*
            grdProd.Columns[0].HeaderText = "Line";
            grdProd.Columns[0].DataPropertyName = "line";
            grdProd.Columns[0].Width = 60;

            grdProd.Columns[1].HeaderText = "Stage";
            grdProd.Columns[1].DataPropertyName = "stage";
            grdProd.Columns[1].Width = 60;

            grdProd.Columns[2].HeaderText = "Date";
            grdProd.Columns[2].DataPropertyName = "schedule_date";
            grdProd.Columns[2].Width = 200;

            grdProd.Columns[3].HeaderText = "TMV #";
            grdProd.Columns[3].DataPropertyName = "ord_no";
            grdProd.Columns[3].Width = 120;

            grdProd.Columns[4].HeaderText = "Notes";
            grdProd.Columns[4].DataPropertyName = "notes";
            grdProd.Columns[4].Width = 120;

            grdProd.Columns[5].HeaderText = "SKU";
            grdProd.Columns[5].DataPropertyName = "item_no";
            grdProd.Columns[5].Width = 120;

            grdProd.Columns[6].HeaderText = "Film";
            grdProd.Columns[6].DataPropertyName = "overlay_sku";
            grdProd.Columns[6].Width = 200;

            grdProd.Columns[7].HeaderText = "Substrate";
            grdProd.Columns[7].DataPropertyName = "substrate_sku";
            grdProd.Columns[7].Width = 60;
            grdProd.Columns[7].DefaultCellStyle.Format = "N0";

            grdProd.Columns[8].HeaderText = "Description";
            grdProd.Columns[8].DataPropertyName = "item_desc_1";
            grdProd.Columns[8].Width = 80;

            grdProd.Columns[9].HeaderText = "Qty.";
            grdProd.Columns[9].DataPropertyName = "ord_qty";
            grdProd.Columns[9].Width = 100;

            grdProd.Columns[10].HeaderText = "Reference";
            grdProd.Columns[10].DataPropertyName = "cus_name";
            grdProd.Columns[10].Width = 100;

            grdProd.Columns[11].HeaderText = "Due";
            grdProd.Columns[11].DataPropertyName = "due_dt";
            grdProd.Columns[11].Width = 100;

            */

        }


        private void GetOrderData()
        {
            string sql = "select ltrim(ord_no) as ord_no, rtrim(cus_name) as cus_name, rtrim(item_no) as item_no, " +
                         "rtrim((select comp_item_no from bmprdstr_sql bm inner join imitmidx_sql ix on bm.comp_item_no = ix.item_no where bm.item_no = ppordfil_sql.item_no and ix.prod_cat not like '9__' and ix.prod_cat in ('810', '820', '825'))) as overlay_sku, " +
                         "rtrim((select comp_item_no from bmprdstr_sql bm inner join imitmidx_sql ix on bm.comp_item_no = ix.item_no where bm.item_no = ppordfil_sql.item_no and ix.prod_cat not like '9__' and ix.prod_cat not in ('810', '820', '825', '975'))) as substrate_sku, " +
                         "rtrim(item_desc_1) as item_desc_1, ord_qty, due_dt, byr_plnr from PPORDFIL_SQL " +
                         "where loc = 'TMV' and ord_status = 'P' order by due_dt asc;";

            SqlDataAdapter adp = new SqlDataAdapter(sql, m_sConnection);

            SqlCommandBuilder bld = new SqlCommandBuilder(adp);

            DataTable tbl = new DataTable();
            adp.Fill(tbl);

            grdOrders.DataSource = tbl;

        }


        private void GetProdData()
        {

            string sql = "select s.ord_no, s.status, s.stage, s.line, s.schedule_date, s.sequence, s.notes, p.item_no, p.ord_qty, " +
                         "s.substrate_sku, s.overlay_sku, p.item_desc_1, p.cus_name, p.due_dt " +
                         "FROM tmc_prod_schedule AS s INNER JOIN ppordfil_sql AS p ON s.ord_no = ltrim(p.ord_no) " +
                         "inner join imitmidx_sql x on p.item_no = x.item_no";

            SqlDataAdapter adp = new SqlDataAdapter(sql, m_sConnection);

            SqlCommandBuilder bld = new SqlCommandBuilder(adp);

            DataTable tbl = new DataTable();
            adp.Fill(tbl);

            grdProd.DataSource = tbl;

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void splMain_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSchedule_Click(object sender, EventArgs e)
        {
            frmPromptStages frm = new frmPromptStages();
            frm.Visible = true;
        }

        private void grdOrders_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            e.PaintParts &= ~DataGridViewPaintParts.Focus;
        }

        private void vDataGridView1_Click(object sender, EventArgs e)
        {

        }
    }
}
