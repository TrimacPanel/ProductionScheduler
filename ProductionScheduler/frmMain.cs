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
using TenTec.Windows.iGridLib;
using Microsoft.Win32;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.Reporting.WinForms;

namespace ProductionScheduler
    {
    public partial class frmMain : Form
    {

        private string m_sConnection = "Server=macola.tmp.trimacpanel;Database=data_01;Trusted_Connection=True;";
        private ContextMenu m_mnuSchedule;
        private ContextMenu m_mnuUnscheduled;

        private bool m_bFullScreen;
        private bool m_bMouseDown;
        private int m_iDragRow;

        private DropdownCalendar m_ctlCalendar;

        public frmMain()
        {

            try
            {

                InitializeComponent();

                m_bMouseDown = false;
                m_iDragRow = -1;

                this.FormClosing += frmMain_FormClosing;

                // Context menu - Schedule grid 
                m_mnuSchedule = new ContextMenu();

                m_mnuSchedule.MenuItems.Add("Move Up", new EventHandler(MenuMoveUp_Click));
                m_mnuSchedule.MenuItems.Add("Move Down", new EventHandler(MenuMoveDown_Click));
                m_mnuSchedule.MenuItems.Add("Unschedule Order", new EventHandler(MenuUnschedule_Click));
                m_mnuSchedule.MenuItems.Add("Edit Scheduled Order", new EventHandler(MenuEditScheduledOrder_Click));
                m_mnuSchedule.MenuItems.Add("Mark Complete", new EventHandler(MenuMarkComplete_Click));
                m_mnuSchedule.MenuItems.Add("Load Data", new EventHandler(MenuLoadData_Click));
                m_mnuSchedule.MenuItems.Add("Save Data", new EventHandler(MenuSaveData_Click));
                m_mnuSchedule.MenuItems.Add("Refresh All", new EventHandler(MenuRefreshAll_Click));

                grdMain.ContextMenu = m_mnuSchedule;

                // Context menu - Unscheduled
                m_mnuUnscheduled = new ContextMenu();
                m_mnuUnscheduled.MenuItems.Add("Schedule Order", new EventHandler(MenuScheduleOrder_Click));
                grdUnscheduled.ContextMenu = m_mnuUnscheduled;

                // Setup dropdown calendar on schedule grid
                m_ctlCalendar = new DropdownCalendar();

                grdMain.Cols["schedule_date"].CellStyle.DropDownControl = m_ctlCalendar;
                grdMain.Cols["schedule_date"].CellStyle.TypeFlags = iGCellTypeFlags.NoTextEdit | iGCellTypeFlags.ComboPreferValue;


                LoadAppSettings();


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Source + ":" + e.Message);
            }

        }

        public void UpdateToolbar()
        {
            bool bMainSel = ((grdScheduled.SelectedRows.Count > 0) ||
                    (
                        (grdMain.SelectedRows.Count > 0) &&
                        (grdMain.SelectedRows[0].Type == iGRowType.Normal)
                    )
                );

            tbScheduleOrder.Enabled = (grdUnscheduled.SelectedRows.Count > 0);
            tbEditOrder.Enabled = bMainSel;
            tbUnscheduleOrder.Enabled = bMainSel;
            tbMoveUp.Enabled = bMainSel;

        }

        public void MoveSelectedUp()
        {
            if (grdMain.SelectedRows.Count > 0)
            {

                iGRow r = grdMain.SelectedRows[0];

                if (r.Index > 0)
                {

                    if (r.Parent == null)
                    {
                        grdMain.SelectedRows[0].Move(grdMain.SelectedRows[0].Index - 1);
                    }
                    else
                    {
                        if (r.Parent.Index < r.Index - 1)
                        {
                            grdMain.SelectedRows[0].Move(grdMain.SelectedRows[0].Index - 1);
                        }
                    }

                }

            }
        }

        public void MoveSelectedDown()
        {

            if (grdMain.SelectedRows.Count > 0)
            {

                iGRow r = grdMain.SelectedRows[0];

                if (r.Parent != null)
                {
                    if (grdMain.Rows[r.Index + 1].Parent != null)
                    {
                        if (r.Parent.Index == grdMain.Rows[r.Index + 1].Parent.Index)
                        {
                            grdMain.SelectedRows[0].Move(grdMain.SelectedRows[0].Index + 2);
                        }
                    }
                }
                else
                {
                    if (r.Index < grdMain.Rows.Count - 1)
                    {
                        r.Index = r.Index + 1;
                    }
                }

            }

        }

        private void LoadUnscheduledOrders()
        {

            Cursor.Current = Cursors.WaitCursor;
            
            // Cache cell selection and scroll position
            int iSelIndex = -1;
            int iScrollPos = -1;

            if (grdUnscheduled.SelectedRows.Count > 0)
            {
                iSelIndex = grdUnscheduled.SelectedRows[0].Index;
                iScrollPos = grdUnscheduled.VScrollBar.Value;
            }   

            grdUnscheduled.BeginUpdate();

            grdUnscheduled.Rows.Clear();

            string sql = "select ltrim(o.ord_no) as ord_no, rtrim(o.item_no) as item_no, idx.prod_cat, si.sub_item as substrate_sku, oi.sub_item as overlay_sku, " +
                            "(o.item_desc_1) as item_desc_1, o.ord_qty, o.due_dt, rtrim(o.cus_name) as cus_name, rtrim(o.user_def_fld_1) as user_def_fld_1, " +
                            "s.description as salesperson " +
                            "from ppordfil_sql o " +
                            "left join tps_salesperson s on o.byr_plnr = s.id " +
                            "inner join vPROD_Overlay oi on o.ord_no = oi.ord_no and  o.item_no = oi.item_no " +
                            "inner join vPROD_Substrate si on o.ord_no = si.ord_no and o.item_no = si.item_no " +
                            "inner join imitmidx_sql idx on o.item_no = idx.item_no " +
                            "where ord_status = 'P' and ltrim(o.ord_no) not in (select ord_no from tps_prod_schedule)";

            string cat = "";

            using (SqlConnection cn = new SqlConnection(m_sConnection))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {

                    SqlDataReader rdr = cmd.ExecuteReader();

                    iGRow row;

                    while (rdr.Read())
                    {
                        row = grdUnscheduled.Rows.Add();

                        row.Cells[0].Value = rdr.GetString(rdr.GetOrdinal("ord_no"));
                        row.Cells[1].Value = rdr.GetString(rdr.GetOrdinal("item_no"));
                        row.Cells[2].Value = rdr.GetString(rdr.GetOrdinal("substrate_sku"));
                        row.Cells[3].Value = rdr.GetString(rdr.GetOrdinal("overlay_sku"));
                        row.Cells[4].Value = rdr.GetString(rdr.GetOrdinal("item_desc_1"));
                        row.Cells[5].Value = rdr.GetDecimal(rdr.GetOrdinal("ord_qty")).ToString("0");
                        row.Cells[6].Value = rdr.GetDateTime(rdr.GetOrdinal("due_dt"));
                        row.Cells[7].Value = rdr.GetString(rdr.GetOrdinal("cus_name"));
                        row.Cells[8].Value = rdr.GetString(rdr.GetOrdinal("user_def_fld_1"));
                        row.Cells[9].Value = rdr.GetString(rdr.GetOrdinal("salesperson"));

                        cat = rdr.GetString(rdr.GetOrdinal("prod_cat"));                        

                        if (cat == "210") //paper laminated
                        {
                            row.Cells[10].Value = "Paper Laminated";
                            row.Cells[10].ImageIndex = 0;
                        }
                        else if (cat == "220") //vinyl laminated
                        {
                            row.Cells[10].Value = "Vinyl Laminated";
                            row.Cells[10].ImageIndex = 2;
                        }
                        else if (cat.Substring(0, 1) == "1") //paneling
                        {
                            row.Cells[10].Value = "Paneling";
                            row.Cells[10].ImageIndex = 7;
                        }
                        else if (cat.Substring(0, 1) == "4") //components
                        {
                            row.Cells[10].Value = "Components";
                            row.Cells[10].ImageIndex = 3;
                        }
                        else
                        {
                            row.Cells[10].Value = "Other";
                            row.Cells[10].ImageIndex = 8;
                        }

                    }

                }

                cn.Close();
            }

            grdUnscheduled.Sort();

            grdUnscheduled.EndUpdate();

            // Restore selection and scroll position
            if (iSelIndex > -1)
            {
                if (grdUnscheduled.Rows.Count >= iSelIndex)
                {
                    grdUnscheduled.Rows[iSelIndex].Selected = true;
                }
                
                grdUnscheduled.VScrollBar.Value = iScrollPos;
            }

            Cursor.Current = Cursors.Default;
        }

        private void LoadScheduledOrders()
        {

            Cursor.Current = Cursors.WaitCursor;

            grdScheduled.BeginUpdate();

            // Cache cell selection and scroll position
            int iSelIndex = -1;
            int iScrollPos = -1;

            if (grdScheduled.SelectedRows.Count > 0)
            {
                iSelIndex = grdScheduled.SelectedRows[0].Index;
                iScrollPos = grdScheduled.VScrollBar.Value;
            }

            grdScheduled.Rows.Clear();

            string sql = "select ltrim(o.ord_no) as ord_no, rtrim(o.item_no) as item_no, idx.prod_cat, " +
                        "(select top 1 comp_item_no as substrate_sku from bmprdstr_sql bom inner join imitmidx_sql idx on bom.comp_item_no = idx.item_no where bom.item_no like o.item_no and idx.prod_cat not in ('810', '820', '825', '975')) as substrate_sku, " +
                        "(select top 1 comp_item_no as overlay_sku from bmprdstr_sql bom inner join imitmidx_sql idx on bom.comp_item_no = idx.item_no where bom.item_no like o.item_no and idx.prod_cat in ('810', '820', '825', '975')) as overlay_sku, " +
                        "rtrim(o.item_desc_1) as item_desc_1, o.ord_qty, o.due_dt, rtrim(o.cus_name) as cus_name, rtrim(o.user_def_fld_1) as user_def_fld_1, " +
                        "s.description as salesperson " +
                        "from ppordfil_sql o left join tps_salesperson s on o.byr_plnr = s.id " +
                        "inner join imitmidx_sql idx on o.item_no = idx.item_no " +
                        "where ltrim(o.ord_no) in (select ord_no from tps_prod_schedule where status <> 'Complete')";

            string cat = "";

            using (SqlConnection cn = new SqlConnection(m_sConnection))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {

                    SqlDataReader rdr = cmd.ExecuteReader();

                    iGRow row;

                    while (rdr.Read())
                    {
                        row = grdScheduled.Rows.Add();

                        row.Cells[0].Value = rdr.GetString(rdr.GetOrdinal("ord_no"));
                        row.Cells[1].Value = rdr.GetString(rdr.GetOrdinal("item_no"));
                        row.Cells[2].Value = rdr.GetString(rdr.GetOrdinal("substrate_sku"));
                        row.Cells[3].Value = rdr.GetString(rdr.GetOrdinal("overlay_sku"));
                        row.Cells[4].Value = rdr.GetString(rdr.GetOrdinal("item_desc_1"));
                        row.Cells[5].Value = rdr.GetDecimal(rdr.GetOrdinal("ord_qty")).ToString("0");
                        row.Cells[6].Value = rdr.GetDateTime(rdr.GetOrdinal("due_dt"));
                        row.Cells[7].Value = rdr.GetString(rdr.GetOrdinal("cus_name"));
                        row.Cells[8].Value = rdr.GetString(rdr.GetOrdinal("user_def_fld_1"));
                        row.Cells[9].Value = rdr.GetString(rdr.GetOrdinal("salesperson"));

                        cat = rdr.GetString(rdr.GetOrdinal("prod_cat"));

                        if (cat == "210") //paper laminated
                        {
                            row.Cells[10].Value = "Paper Laminated";
                            row.Cells[10].ImageIndex = 0;
                        }
                        else if (cat == "220") //vinyl laminated
                        {
                            row.Cells[10].Value = "Vinyl Laminated";
                            row.Cells[10].ImageIndex = 2;
                        }
                        else if (cat.Substring(0, 1) == "1") //paneling
                        {
                            row.Cells[10].Value = "Paneling";
                            row.Cells[10].ImageIndex = 7;
                        }
                        else if (cat.Substring(0, 1) == "4") //components
                        {
                            row.Cells[10].Value = "Components";
                            row.Cells[10].ImageIndex = 3;
                        }
                        else
                        {
                            row.Cells[10].Value = "Other";
                            row.Cells[10].ImageIndex = 8;
                        }
                    }

                }

                cn.Close();
            }

            grdScheduled.EndUpdate();

            // Restore selection and scroll position
            if (iSelIndex > -1)
            {
                if (grdScheduled.Rows.Count >= iSelIndex)
                {
                    grdScheduled.Rows[iSelIndex].Selected = true;
                }

                grdScheduled.VScrollBar.Value = iScrollPos;
            }

            Cursor.Current = Cursors.Default;
        }

        private void LoadScheduleData()
        {

            Cursor.Current = Cursors.WaitCursor;

            grdMain.BeginUpdate();

            // Cache cell selection and scroll position
            int iSelIndex = -1;
            int iScrollPos = -1;

            if (grdMain.SelectedRows.Count > 0)
            {
                iSelIndex = grdMain.SelectedRows[0].Index;
                iScrollPos = grdMain.VScrollBar.Value;
            }

            string layout = grdMain.LayoutObject.Text;

            int[] groups = grdMain.GroupObject.ColArray;

            grdMain.Rows.Clear();

            string sql = "select s.id, s.ord_no, s.status, s.stage, s.line, s.schedule_date, s.due_dt, s.notes, p.item_no, p.ord_qty, " +
                         "s.substrate_sku, s.overlay_sku, s.wo_type, p.item_desc_1, p.cus_name, p.due_dt, s.wo_type, s.byr_plnr, " +
                         "s.created_by, s.created_at, s.modified_by, s.modified_at " +
                         "FROM tps_prod_schedule AS s INNER JOIN ppordfil_sql AS p ON s.ord_no = ltrim(p.ord_no) " +
                         "inner join imitmidx_sql x on p.item_no = x.item_no ";

            if (!tbShowComplete.Checked)
            {
                sql += " where s.Status <> 'Complete'";
            }

            sql += " order by s.sequence";

            using (SqlConnection cn = new SqlConnection(m_sConnection))
            {

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {

                    cn.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    iGRow row;

                    while (rdr.Read())
                    {
                        row = grdMain.Rows.Add();

                        row.Cells["ord_no"].Value = rdr.GetString(rdr.GetOrdinal("ord_no"));
                        row.Cells["status"].Value = rdr.GetString(rdr.GetOrdinal("status"));
                        row.Cells["stage"].Value = rdr.GetString(rdr.GetOrdinal("stage"));
                        row.Cells["line"].Value = rdr.GetString(rdr.GetOrdinal("line"));
                        row.Cells["schedule_date"].Value = rdr.GetDateTime(rdr.GetOrdinal("schedule_date"));
                        row.Cells["due_dt"].Value = rdr.GetDateTime(rdr.GetOrdinal("due_dt"));
                        row.Cells["notes"].Value = rdr.GetString(rdr.GetOrdinal("notes"));
                        row.Cells["sku"].Value = rdr.GetString(rdr.GetOrdinal("item_no"));
                        row.Cells["ord_qty"].Value = rdr.GetDecimal(rdr.GetOrdinal("ord_qty")).ToString("0");
                        row.Cells["substrate_sku"].Value = rdr.GetString(rdr.GetOrdinal("substrate_sku"));
                        row.Cells["overlay_sku"].Value = rdr.GetString(rdr.GetOrdinal("overlay_sku"));
                        row.Cells["wo_type"].Value = rdr.GetString(rdr.GetOrdinal("wo_type"));
                        row.Cells["item_desc_1"].Value = rdr.GetString(rdr.GetOrdinal("item_desc_1"));
                        row.Cells["cus_name"].Value = rdr.GetString(rdr.GetOrdinal("cus_name"));
                        row.Cells["wo_type"].Value = rdr.GetString(rdr.GetOrdinal("wo_type"));
                        row.Cells["byr_plnr"].Value = rdr.GetString(rdr.GetOrdinal("byr_plnr"));
                        row.Cells["created_by"].Value = rdr.GetString(rdr.GetOrdinal("created_by"));
                        row.Cells["created_at"].Value = rdr.GetDateTime(rdr.GetOrdinal("created_at"));
                        row.Cells["modified_by"].Value = rdr.GetString(rdr.GetOrdinal("modified_by"));
                        row.Cells["modified_at"].Value = rdr.GetDateTime(rdr.GetOrdinal("modified_at"));

                    }

                    cn.Close();
                }
            }

            grdMain.GroupObject.Clear();

            for (int i = 0; i < groups.Length; i++)
            {
                grdMain.GroupObject.Add(groups[i]);
            }

            grdMain.LayoutObject.Text = layout;

            grdMain.Group();

            grdMain.EndUpdate();

            // Restore selection and scroll position
            if (iSelIndex > -1)
            {
                if (grdMain.Rows.Count >= iSelIndex)
                {
                    grdMain.Rows[iSelIndex].Selected = true;
                }

                grdMain.VScrollBar.Value = iScrollPos;
            }

            Cursor.Current = Cursors.Default;

        }

        private void SaveScheduleData()
        {

            Cursor.Current = Cursors.WaitCursor;

            using (SqlConnection cn = new SqlConnection(m_sConnection))
            {
                cn.Open();

                SqlTransaction trn = cn.BeginTransaction();

                string sql = "delete from tps_prod_schedule";

                using (SqlCommand cmd = new SqlCommand(sql, cn, trn))
                {
                    cmd.ExecuteNonQuery();
                }

                sql = "insert into tps_prod_schedule (ord_no, status, stage, line, schedule_date, due_dt, sku, substrate_sku, overlay_sku, wo_type,item_desc_1, " +
                    "notes, ord_qty, byr_plnr, created_by, created_at, modified_by, modified_at, sequence) values (@ord_no, @status, @stage, @line, @schedule_date, @due_dt, " +
                    "@sku, @substrate_sku, @overlay_sku, @wo_type, @item_desc_1, @notes, @ord_qty, @byr_plnr, @created_by, @created_at, @modified_by, @modified_at, @sequence)";

                using (SqlCommand cmd = new SqlCommand(sql, cn, trn))
                {

                    SqlParameter[] prm = new SqlParameter[]
                    {
                        new SqlParameter("@ord_no", SqlDbType.VarChar),
                        new SqlParameter("@status", SqlDbType.VarChar),
                        new SqlParameter("@stage", SqlDbType.VarChar),
                        new SqlParameter("@line", SqlDbType.VarChar),
                        new SqlParameter("@schedule_date", SqlDbType.DateTime),
                        new SqlParameter("@due_dt", SqlDbType.DateTime),
                        new SqlParameter("@sku", SqlDbType.VarChar),
                        new SqlParameter("@substrate_sku", SqlDbType.VarChar),
                        new SqlParameter("@overlay_sku", SqlDbType.VarChar),
                        new SqlParameter("@wo_type", SqlDbType.VarChar),
                        new SqlParameter("@item_desc_1", SqlDbType.VarChar),
                        new SqlParameter("@notes", SqlDbType.VarChar),
                        new SqlParameter("@ord_qty", SqlDbType.Decimal),
                        new SqlParameter("@byr_plnr", SqlDbType.VarChar),
                        new SqlParameter("@created_by", SqlDbType.VarChar),
                        new SqlParameter("@created_at", SqlDbType.DateTime),
                        new SqlParameter("@modified_by", SqlDbType.VarChar),
                        new SqlParameter("@modified_at", SqlDbType.DateTime),
                        new SqlParameter("@sequence", SqlDbType.Int)
                    };

                    cmd.Parameters.AddRange(prm);

                    int seq = 0;

                    foreach (iGRow r in grdMain.Rows)
                    {
                        if (r.Type == iGRowType.Normal)
                        {
                            cmd.Parameters["@ord_no"].Value = r.Cells["ord_no"].Value;
                            cmd.Parameters["@status"].Value = r.Cells["status"].Value;
                            cmd.Parameters["@stage"].Value = r.Cells["stage"].Value;
                            cmd.Parameters["@line"].Value = r.Cells["line"].Value;
                            cmd.Parameters["@schedule_date"].Value = Convert.ToDateTime(r.Cells["schedule_date"].Value);
                            cmd.Parameters["@due_dt"].Value = Convert.ToDateTime(r.Cells["due_dt"].Value);
                            cmd.Parameters["@sku"].Value = r.Cells["sku"].Value;
                            cmd.Parameters["@substrate_sku"].Value = r.Cells["substrate_sku"].Value;
                            cmd.Parameters["@overlay_sku"].Value = r.Cells["overlay_sku"].Value;
                            cmd.Parameters["@wo_type"].Value = r.Cells["wo_type"].Value;
                            cmd.Parameters["@item_desc_1"].Value = r.Cells["item_desc_1"].Value;
                            cmd.Parameters["@notes"].Value = (r.Cells["notes"].Value == null ? "" : r.Cells["notes"].Value);
                            cmd.Parameters["@ord_qty"].Value = Convert.ToDecimal(r.Cells["ord_qty"].Value);
                            cmd.Parameters["@byr_plnr"].Value = r.Cells["byr_plnr"].Value;
                            cmd.Parameters["@created_by"].Value = r.Cells["created_by"].Value;
                            cmd.Parameters["@created_at"].Value = Convert.ToDateTime(r.Cells["created_at"].Value);
                            cmd.Parameters["@modified_by"].Value = r.Cells["modified_by"].Value;
                            cmd.Parameters["@modified_at"].Value = Convert.ToDateTime(r.Cells["modified_at"].Value);
                            cmd.Parameters["@sequence"].Value = seq;

                            cmd.ExecuteNonQuery();
                        }

                        seq++;
                    }

                }

                trn.Commit();

                cn.Close();

            }

            Cursor.Current = Cursors.Default;

        }

        private bool ScheduleOrder()
        {
            if (grdUnscheduled.SelectedRows.Count > 0)
            {

                iGRow r = grdUnscheduled.SelectedRows[0];

                frmScheduleOrder f = new frmScheduleOrder();

                f.OrderNo = r.Cells[0].Value.ToString();
                f.ItemNo = r.Cells[1].Value.ToString();
                f.Overlay = r.Cells[3].Value.ToString();
                f.Substrate = r.Cells[2].Value.ToString();
                f.OrderType= r.Cells[8].Value.ToString();
                f.Description = r.Cells[4].Value.ToString();
                f.Quantity = r.Cells[5].Value.ToString();
                f.DueDate = r.Cells[6].Value.ToString();
                f.Customer = r.Cells[7].Value.ToString();
                f.Saleperson = r.Cells[9].Value.ToString();

                f.ShowDialog();

                if (!f.Cancel)
                {
                    foreach (iGRow rs in f.ScheduleRows)
                    {
                        r = grdMain.Rows.Add();
                        
                        r.Cells["ord_no"].Value = f.OrderNo;
                        r.Cells["status"].Value = "Scheduled";
                        r.Cells["stage"].Value = rs.Cells[0].Value;
                        r.Cells["line"].Value = rs.Cells[1].Value;
                        r.Cells["schedule_date"].Value = rs.Cells[2].Value;
                        r.Cells["notes"].Value = (rs.Cells[3].Value == null ? "" : rs.Cells[3].Value);
                        r.Cells["sku"].Value = f.ItemNo;
                        r.Cells["ord_qty"].Value = f.Quantity;
                        r.Cells["substrate_sku"].Value = f.Substrate;
                        r.Cells["overlay_sku"].Value = f.Overlay;
                        r.Cells["item_desc_1"].Value = f.Description;
                        r.Cells["cus_name"].Value = f.Customer;
                        r.Cells["wo_type"].Value = f.OrderType;
                        r.Cells["byr_plnr"].Value = f.Saleperson;
                        r.Cells["due_dt"].Value = f.DueDate;
                        r.Cells["created_by"].Value = Environment.UserName;
                        r.Cells["created_at"].Value = DateTime.Now;
                        r.Cells["modified_by"].Value = Environment.UserName;
                        r.Cells["modified_at"].Value = r.Cells["created_at"].Value;
                    }

                    //Save and refresh schedule data (lazy grouping!)
                    SaveScheduleData();
                    LoadScheduleData();

                    // Refresh scheduled/unscheduled grids
                    LoadScheduledOrders();
                    LoadUnscheduledOrders();

                }

            }

            return true;
        }

        private bool UnscheduleOrder(string OrderNo)
        {
            iGRow r;

            for (int i = grdMain.Rows.Count - 1;i >= 0;i--)
            {
                r = grdMain.Rows[i];
                Debug.WriteLine("Looking at row:" + r.Index.ToString());

                if (r.Cells["ord_no"].Value != null)
                {
                    if (r.Cells["ord_no"].Value.ToString() == OrderNo)
                    {
                        r.Selected = false;
                        grdMain.Rows.RemoveAt(r.Index);
                        Debug.WriteLine("Deleting: " + r.Index.ToString());
                    }
                }

            }

            // Save and reload schedule data, lazy way to cleanup unneeded group headers
            SaveScheduleData();
            LoadScheduleData();

            // Refresh work order grids
            LoadScheduledOrders();
            LoadUnscheduledOrders();

            return true;
        }

        private bool EditScheduledOrder()
        {
            if (grdMain.SelectedRows.Count > 0)
            {

                string sOrderNo = grdMain.SelectedRows[0].Cells["ord_no"].Value.ToString();

                iGRow r = grdMain.SelectedRows[0];

                frmScheduleOrder f = new frmScheduleOrder();

                f.OrderNo = r.Cells["ord_no"].Value.ToString();
                f.ItemNo = r.Cells["sku"].Value.ToString();
                f.Overlay = r.Cells["overlay_sku"].Value.ToString();
                f.Substrate = r.Cells["substrate_sku"].Value.ToString();
                f.Description = r.Cells["item_desc_1"].Value.ToString();
                f.OrderType = r.Cells["wo_type"].Value.ToString();
                f.Quantity = r.Cells["ord_qty"].Value.ToString();
                f.DueDate = r.Cells["due_dt"].Value.ToString();
                f.Customer = r.Cells["cus_name"].Value.ToString();
                f.Saleperson = r.Cells["byr_plnr"].Value.ToString();
                f.CreatedBy = r.Cells["created_by"].Value.ToString();
                f.CreatedAt = Convert.ToDateTime(r.Cells["created_at"].Value);
                f.ModifiedBy = r.Cells["modified_by"].Value.ToString();
                f.ModifiedAt = Convert.ToDateTime(r.Cells["modified_at"].Value);

                // Iterate and populate scheduled tasks
                foreach (iGRow r2 in grdMain.Rows)
                {
                    if (r2.Type == iGRowType.Normal)
                    {
                        if (r2.Cells["ord_no"].Value.ToString() == sOrderNo)
                        {
                            r = f.ScheduleRows.Add();

                            r.Cells["stage"].Value = r2.Cells["stage"].Value.ToString();
                            r.Cells["line"].Value = r2.Cells["line"].Value.ToString();
                            r.Cells["schedule_date"].Value = r2.Cells["schedule_date"].Value.ToString();
                            r.Cells["notes"].Value = r2.Cells["notes"].Value.ToString();
                            r.Cells["status"].Value = r2.Cells["status"].Value.ToString();
                        }
                    }
                }

                f.ShowDialog();

                if (!f.Cancel)
                {
                    // Unschedule to clear previous rows...
                    UnscheduleOrder(sOrderNo);

                    foreach (iGRow rs in f.ScheduleRows)
                    {
                        r = grdMain.Rows.Add();

                        r.Cells["ord_no"].Value = f.OrderNo;
                        r.Cells["status"].Value = "Pending";
                        r.Cells["stage"].Value = rs.Cells[0].Value;
                        r.Cells["line"].Value = rs.Cells[1].Value;
                        r.Cells["schedule_date"].Value = rs.Cells[2].Value;
                        r.Cells["notes"].Value = (rs.Cells[3].Value == null ? "" : rs.Cells[3].Value);
                        r.Cells["sku"].Value = f.ItemNo;
                        r.Cells["ord_qty"].Value = f.Quantity;
                        r.Cells["substrate_sku"].Value = f.Substrate;
                        r.Cells["overlay_sku"].Value = f.Overlay;
                        r.Cells["item_desc_1"].Value = f.Description;
                        r.Cells["cus_name"].Value = f.Customer;
                        r.Cells["due_dt"].Value = f.DueDate;
                        r.Cells["wo_type"].Value = f.OrderType;
                        r.Cells["byr_plnr"].Value = f.Saleperson;
                        r.Cells["created_at"].Value = f.CreatedAt;
                        r.Cells["created_by"].Value = f.CreatedBy;
                        r.Cells["modified_by"].Value = Environment.UserName;
                        r.Cells["modified_at"].Value = DateTime.Now;
                    }

                    // Save and refresh schedule data
                    SaveScheduleData();
                    LoadScheduleData();

                    // Refresh scheduled/unscheduled grids
                    this.LoadScheduledOrders();
                    this.LoadUnscheduledOrders();

                }

            }

            return true;
        }

        private void MarkComplete()
        {
            if (grdMain.SelectedRows.Count > 0)
            { 
                string sOrderNo = grdMain.SelectedRows[0].Cells["ord_no"].Value.ToString();

                foreach (iGRow r in grdMain.Rows)
                {
                    if (r.Type == iGRowType.Normal)
                    {
                        if (r.Cells["ord_no"].Value.ToString() == sOrderNo)
                        {
                            r.Cells["status"].Value = "Complete";
                        }
                    }
                }
            }

            SaveScheduleData();
            LoadScheduleData();
        }

        private void SaveAppSettings()
        {

            // Main form window
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "frmMain.WindowState", (int)this.WindowState);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "frmMain.Left", this.Left);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "frmMain.Top", this.Top);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "frmMain.Height", this.Height);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "frmMain.Width", this.Width);


            // Grid layouts
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Unscheduled.Grid.Layout", grdUnscheduled.LayoutObject.Text);

            if (grdUnscheduled.SortObject.Count > 0)
            { 
                if (grdUnscheduled.SortObject.Count == 0)
                {
                    Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Unscheduled.Grid.SortColumn", "");
                    Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Unscheduled.Grid.SortOrder", "");
                }
                else
                {
                    Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Unscheduled.Grid.SortColumn", grdUnscheduled.SortObject[0].ColIndex);
                    Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Unscheduled.Grid.SortOrder", grdUnscheduled.SortObject[0].SortOrder);
                }
            }

            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Scheduled.Grid.Layout", grdScheduled.LayoutObject.Text);

            if (grdScheduled.SortObject.Count > 0)
            {
                if (grdScheduled.SortObject.Count == 0)
                {
                    Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Scheduled.Grid.SortColumn", "");
                    Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Scheduled.Grid.SortOrder", "");
                }
                else
                {
                    Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Scheduled.Grid.SortColumn", grdScheduled.SortObject[0].ColIndex);
                    Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Scheduled.Grid.SortOrder", grdScheduled.SortObject[0].SortOrder);
                }                
            }

            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Schedule.Grid.Layout", grdMain.LayoutObject.Text);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Schedule.Grid.Groups", JsonConvert.SerializeObject(grdMain.GroupObject.ColArray));

            // Splitter positions
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Split.Vertical", splVertical.SplitterDistance);
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Split.Horizontal", splHorizontal.SplitterDistance);

            // Show Complete toolbutton
            Registry.SetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Schedule.Grid.ShowComplete", tbShowComplete.Checked.ToString());
            
        }

        private void LoadAppSettings()
        {

            try
            {
                string s = "";

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "frmMain.WindowState", "").ToString();
                if (s != "")
                {
                    this.WindowState = (FormWindowState)Convert.ToInt16(s);
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "frmMain.Left", "").ToString();
                if (s != "")
                {
                    this.Left = Convert.ToInt16(s);
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "frmMain.Top", "").ToString();
                if (s != "")
                {
                    this.Top = Convert.ToInt16(s);
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "frmMain.Height", "").ToString();
                if (s != "")
                {
                    this.Height = Convert.ToInt16(s);
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "frmMain.Width", "").ToString();
                if (s != "")
                {
                    this.Width = Convert.ToInt16(s);
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Unscheduled.Grid.Layout", "").ToString();
                if (s != "")
                {
                    grdUnscheduled.LayoutObject.Text = s;
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Unscheduled.Grid.SortColumn", "").ToString();
                if (s != "")
                {
                    grdUnscheduled.SortObject.Add(Convert.ToInt16(s));
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Unscheduled.Grid.SortOrder", "").ToString();
                if (s != "")
                {
                    grdUnscheduled.SortObject[0].SortType = (iGSortType)Convert.ToInt16(s);
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Scheduled.Grid.Layout", "").ToString();
                if (s != "")
                {
                    grdScheduled.LayoutObject.Text = s;
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Scheduled.Grid.SortColumn", "").ToString();
                if (s != "")
                {
                    grdScheduled.SortObject.Add(Convert.ToInt16(s));
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Scheduled.Grid.SortOrder", "").ToString();
                if (s != "")
                {
                    grdScheduled.SortObject[0].SortType = (iGSortType)Convert.ToInt16(s);
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Schedule.Grid.Layout", "").ToString();
                if (s != "")
                {
                    grdMain.LayoutObject.Text = s;
                }

                int[] grp = JsonConvert.DeserializeObject<int[]>(Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Schedule.Grid.Groups", "").ToString());
                for (int i = 0; i < grp.Length; i++)
                {
                    grdMain.GroupObject.Add(grp[i]);
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Split.Vertical", "").ToString();
                if (s != "")
                {
                    splVertical.SplitterDistance = Convert.ToInt16(s);
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Split.Horizontal", "").ToString();
                if (s != "")
                {
                    splHorizontal.SplitterDistance = Convert.ToInt16(s);
                }

                s = Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Trimac\ProductionPro", "Schedule.Grid.ShowComplete", "").ToString();
                if (s != "")
                {
                    tbShowComplete.Checked = Convert.ToBoolean(s);
                }
            }
            catch(Exception e)
            {
                //hehehe
            }
        }

        private void ApplyRowFormat(iGRow r)
        {
            int days = (int)(DateTime.Today - (DateTime)r.Cells["due_dt"].Value).TotalDays;

            if (days > 7)
            {
                r.CellStyle.BackColor = Color.White;
            }
            else if (days > 3)
            {
                r.CellStyle.BackColor = Color.Yellow;
            }
            else if (days == 1)
            {
                r.CellStyle.BackColor = Color.Orange;
            }
            else
            {
                r.CellStyle.BackColor = Color.LightPink;
            }
        }

        private void MenuMarkComplete_Click(object sender, EventArgs e)
        {
            MarkComplete();
        }

        private void MenuEditScheduledOrder_Click(object sender, EventArgs e)
        {
            EditScheduledOrder();
        }

        private void MenuScheduleOrder_Click(object sender, EventArgs e)
        {
            ScheduleOrder();
        }

        private void MenuLoadData_Click(object sender, EventArgs e)
        {
            LoadScheduleData();
        }

        private void MenuRefreshAll_Click(object sender, EventArgs e)
        {
            LoadUnscheduledOrders();

            LoadScheduledOrders();

            LoadScheduleData();
        }

        private void MenuSaveData_Click(object sender, EventArgs e)
        {
            SaveScheduleData();
        }

        private void MenuMoveUp_Click(object sender, EventArgs e)
        {
            MoveSelectedUp();
        }

        private void MenuMoveDown_Click(object sender, EventArgs e)
        {
            MoveSelectedDown();
        }

        private void MenuUnschedule_Click(object sender, EventArgs e)
        {

            if (grdMain.SelectedRows.Count > 0)
            {
                if (grdMain.SelectedRows[0].Type == iGRowType.Normal)
                {
                    string sOrderNo = grdMain.SelectedRows[0].Cells["ord_no"].Value.ToString();
                    if (MessageBox.Show("Are you sure you wish to unschedule order " + sOrderNo + "?", "Unschedule Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        UnscheduleOrder(sOrderNo);
                    }
                }
            }

        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            LoadScheduleData();

            LoadUnscheduledOrders();

            LoadScheduledOrders();

        }

        private void mnuMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void grdMain_Click(object sender, EventArgs e)
        {

        }

        private void grdUnscheduled_Click(object sender, EventArgs e)
        {

        }

        private void frmMain_FormClosing(Object sender, FormClosingEventArgs e)
        {
            SaveAppSettings();
        }

        private void grdScheduled_SelectionChanged(object sender, EventArgs e)
        {
            UpdateToolbar();

            if (grdScheduled.SelectedRows.Count > 0)
            {

                string sOrderNo = grdScheduled.SelectedRows[0].Cells[0].Value.ToString();

                foreach (iGRow r in grdMain.Rows)
                {
                    if (r.Type == iGRowType.Normal)
                    {
                        if (r.Cells["ord_no"].Value.ToString() == sOrderNo)
                        {
                            r.CellStyle.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            r.CellStyle.BackColor = Color.White;
                        }
                    }
                }

                grdMain.Refresh();
            }
        }

        private void unscheduledOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowHideColumns f = new frmShowHideColumns();
            f.ActiveGrid = grdUnscheduled;

            f.ShowDialog();
        }

        private void scheduledOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowHideColumns f = new frmShowHideColumns();
            f.ActiveGrid = grdScheduled;

            f.ShowDialog();
        }

        private void productionScheduleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowHideColumns f = new frmShowHideColumns();
            f.ActiveGrid = grdMain;

            f.ShowDialog();
        }

        private void grdMain_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void grdMain_StartDragCell(object sender, iGStartDragCellEventArgs e)
        {
        }

        private void grdMain_CellMouseDown(object sender, iGCellMouseDownEventArgs e)
        {
            if (grdMain.Rows[e.RowIndex].Type == iGRowType.Normal)
            {
                if (e.Button == MouseButtons.Left)
                {
                    m_bMouseDown = true;
                }
            }
        }

        private void grdMain_CellMouseMove(object sender, iGCellMouseMoveEventArgs e)
        {
            iGrid grid = sender as iGrid;
            iGRow row = grid.Rows[e.RowIndex];

            if (m_bMouseDown)
            {                
                // We're out of row Y-bounds, begin drag...
                if ((e.MousePos.Y < row.Y) || (e.MousePos.Y > row.Y + row.Height))
                {
                    Debug.WriteLine("Drag start - row: " + row.Index);
                    m_iDragRow = row.Index;
                    grdMain.DoDragDrop(row.Cells[0].Value.ToString(), DragDropEffects.Copy);
                }
            }
        }

        private void grdMain_CellMouseUp(object sender, iGCellMouseUpEventArgs e)
        {
            if (m_bMouseDown)
            {
                m_bMouseDown = false;
                Debug.WriteLine("CELLMOUSEUP");
            }
        }

        private void grdMain_CellMouseLeave(object sender, iGCellMouseEnterLeaveEventArgs e)
        {
            if (m_bMouseDown)
            { 
                m_bMouseDown = false;
            }
        }

        private void grdMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (m_bMouseDown)
            {
                m_bMouseDown = false;

            }
        }

        private void grdMain_DragDrop(object sender, DragEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            p = grdMain.PointToClient(p);

            iGCell c = grdMain.Cells.FromPoint(p.X, p.Y);

            // Do we have a valid drop target cell?
            if (c != null)
            {

                // Get drop source and destination
                iGRow src = grdMain.Rows[m_iDragRow];
                iGRow dest = c.Row;

                // Are source and destination in the same group?
                // If so, just move sequence
                //if (src.Parent == dest.Parent)
                if (((src.Parent != null) && (dest.Parent != null)) && (src.Parent.Index == dest.Parent.Index))
                {
                    src.Move(dest.Index);
                }
                else
                {

                    // Iterate group array, look for a date group
                    for (int i = 0;i <  grdMain.GroupObject.Count;i++)
                    {
                        iGCol col = grdMain.Cols[grdMain.GroupObject[i].ColIndex];
                        if (col.Key == "schedule_date")
                        {
                            // We found a date group - modify source's date value, refresh grid
                            // src.Cells["schedule_date"]
                            if (dest.Type == iGRowType.Normal)
                            {
                                src.Cells["schedule_date"].Value = dest.Cells["schedule_date"].Value;
                                SaveScheduleData();
                                LoadScheduleData();
                            }
                            else
                            {
                                // Look for first normal row occuring after our destination
                                iGRow child;
                                int ic = dest.Index + 1;

                                child = grdMain.Rows[ic];

                                while (child.Type != iGRowType.Normal)
                                {
                                    ic++;
                                    child = grdMain.Rows[ic];
                                }

                                src.Cells["schedule_date"].Value = child.Cells["schedule_date"].Value;
                                //SaveScheduleData();
                                //LoadScheduleData();

                            }
                            
                        }
                    }

                    //// If we dropped to a normal row, look at move to parent
                    //if (dest.Type == iGRowType.Normal)
                    //{
                    //    // Get parent row's column 
                    //    iGCol pCol = grdMain.Cols[grdMain.GroupObject[dest.Parent.Level].ColIndex];

                    //    //! validate
                    //    switch (pCol.Key)
                    //    {
                    //        case "schedule_date":
                    //            src.Cells["schedule_date"].Value = dest.Cells["schedule_date"].Value;
                    //            src.Move(dest.Index);
                    //            break;
                    //        default:
                    //            MessageBox.Show("Haven't handled dropping here yet...");
                    //            break;
                    //    }
                    //}
                  
                    

                }

                SaveScheduleData();
                LoadScheduleData();

            }

           // Debug.WriteLine("DRAGDROP! - row " + m_iDragRow.ToString() + " to " + c.Row.Index.ToString());

            m_bMouseDown = false;
        }

        private void grdMain_DragOver(object sender, DragEventArgs e)
        {
            // Debug.WriteLine("DRAGOVER");
            Point p = new Point(e.X, e.Y);
            p = grdMain.PointToClient(p);

            iGCell c = grdMain.Cells.FromPoint(p.X, p.Y);

            e.Effect = DragDropEffects.Copy;
        }

        private void grdMain_DragLeave(object sender, EventArgs e)
        {

            Debug.WriteLine("DRAGLEAVE");

        }

        private void chkComplete_CheckedChanged(object sender, EventArgs e)
        {
            LoadScheduleData();
        }

        private void tbScheduleOrder_Click(object sender, EventArgs e)
        {
            ScheduleOrder();
        }

        private void tbEditOrder_Click(object sender, EventArgs e)
        {
            EditScheduledOrder();
        }

        private void tbUnscheduleOrder_Click(object sender, EventArgs e)
        {

            if (grdMain.SelectedRows.Count > 0)
            {
                if (grdMain.SelectedRows[0].Type == iGRowType.Normal)
                {
                    string sOrderNo = grdMain.SelectedRows[0].Cells["ord_no"].Value.ToString();
                    if (MessageBox.Show("Are you sure you wish to unschedule order " + sOrderNo + "?", "Unschedule Order", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        UnscheduleOrder(sOrderNo);
                    }
                }
            }

        }

        private void tbMoveUp_Click(object sender, EventArgs e)
        {
            MoveSelectedUp();
        }

        private void tbMarkComplete_Click(object sender, EventArgs e)
        {

            if (grdMain.SelectedRows.Count > 0)
            {
                string sOrderNo = grdMain.SelectedRows[0].Cells["ord_no"].Value.ToString();

                if (MessageBox.Show("Are you sure you wish to mark order '" + sOrderNo + "' as complete?", "Mark Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MarkComplete();
                }
            }

        }

        private void grdUnscheduled_SelectionChanged(object sender, EventArgs e)
        {
            UpdateToolbar();
        }

        private void grdMain_SelectionChanged(object sender, EventArgs e)
        {
            UpdateToolbar();
        }

        private void tbMoveDown_Click(object sender, EventArgs e)
        {
            MoveSelectedDown();
        }

        private void tbShowComplete_Click(object sender, EventArgs e)
        {
            LoadScheduleData();
        }

        private void mnuMain_KeyUp(object sender, KeyEventArgs e)
        {


        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                if (m_bFullScreen)
                {
                    mnuMain.Visible = true;
                    toolStripContainer1.Visible = true;
                    splHorizontal.Panel1.Show();
                    splHorizontal.Top = 27;
                    splHorizontal.Height = this.ClientSize.Height - 27;
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    mnuMain.Visible = false;
                    
                    toolStripContainer1.Visible = false;
                    splHorizontal.Panel1.Hide();
                    splHorizontal.SplitterDistance = 0;
                    splHorizontal.Top = 0;
                    splHorizontal.Height = this.ClientSize.Height;
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                }
                m_bFullScreen = !m_bFullScreen;
            }
        }

        private void grdMain_CellMouseEnter(object sender, iGCellMouseEnterLeaveEventArgs e)
        {
            
            if (e.ColIndex == 7)
            {

                string sku = grdMain.Rows[e.RowIndex].Cells["sku"].Value.ToString();

                string sql = "select qty_on_hand, qty_allocated, qty_bkord, qty_on_ord from iminvloc_sql where loc = 'TMV' and item_no = '" + sku + "'";

                decimal onhand = 0m;
                decimal allocated = 0m;
                decimal bo = 0m;
                decimal onorder = 0m;

                using (SqlConnection cn = new SqlConnection(m_sConnection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, cn))
                    {
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            onhand = rdr.GetDecimal(0);
                            allocated = rdr.GetDecimal(1);
                            bo = rdr.GetDecimal(2);
                            onorder = rdr.GetDecimal(3);
                        }
                    }

                    cn.Close();

                }

                lblPopSKU.Text = sku;
                lblOnHand.Text = onhand.ToString("0");
                lblAllocated.Text = allocated.ToString("0");
                lblBackordered.Text = bo.ToString("0");
                lblOnOrder.Text = onorder.ToString("0");

                pnlQtyPop.Left = e.Bounds.Left + grdMain.Left + (e.Bounds.Width / 2) - (pnlQtyPop.Width / 2);
                pnlQtyPop.Top = e.Bounds.Top + grdMain.Top + (e.Bounds.Height / 2) - (pnlQtyPop.Height / 2);
                pnlQtyPop.Visible = true;
            }
            else
            {
                pnlQtyPop.Visible = false;
            }
        }

        private void tbPrintSchedule_Click(object sender, EventArgs e)
        {

            frmPrintSchedule f = new frmPrintSchedule();

            if (f.ShowDialog(this) == DialogResult.Cancel)
            {
                return;
            }

            if (!f.Cancel)
            {
                DateTime d = f.SelectedDate;
                
                frmReportViewer fViewer = new frmReportViewer();

                fViewer.SelectedDate = d;
                fViewer.LoadReport();

                fViewer.ShowDialog();
            }

        }

        private void grdMain_CellDoubleClick(object sender, iGCellDoubleClickEventArgs e)
        {
            EditScheduledOrder();
        }

        private void grdUnscheduled_CellDoubleClick(object sender, iGCellDoubleClickEventArgs e)
        {
            ScheduleOrder();
        }

        private void grdScheduled_Click(object sender, EventArgs e)
        {

        }

        private void grdUnscheduled_BeforeContentsSorted(object sender, EventArgs e)
        {
            if (grdUnscheduled.SortObject.ColArray.Length > 0)
            {
                if (grdUnscheduled.SortObject.ColArray[0] != 0)
                {
                    grdUnscheduled.SortObject.Add(0);
                    grdUnscheduled.SortObject[1].SortOrder = iGSortOrder.Ascending;
                }
            }
        }

        private void grdScheduled_BeforeContentsSorted(object sender, EventArgs e)
        {
            if (grdScheduled.SortObject.ColArray.Length > 0)
            {
                if (grdScheduled.SortObject.ColArray[0] != 0)
                {
                    grdScheduled.SortObject.Add(0);
                    grdScheduled.SortObject[1].SortOrder = iGSortOrder.Ascending;
                }
            }
        }

        private void iGAutoFilterManager1_FilterApplied(object sender, TenTec.Windows.iGridLib.Filtering.iGFilterAppliedEventArgs e)
        {

        }
    }
}
