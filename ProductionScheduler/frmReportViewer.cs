using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using System.Drawing.Printing;

namespace ProductionScheduler
{
    public partial class frmReportViewer : Form
    {

        private string m_sConnection = "Server=macola.tmp.trimacpanel;Database=data_01;Trusted_Connection=True;";
        private DateTime m_dSelectedDate;

        public DateTime SelectedDate
        {
            get
            {
                return m_dSelectedDate;
            }

            set
            {
                m_dSelectedDate = value;
            }
        }

        public void LoadReport()
        {

            Cursor.Current = Cursors.WaitCursor;

            rptMain.SetDisplayMode(DisplayMode.PrintLayout);

            PageSettings ps = new PageSettings();

            ps.Landscape = true;
            ps.Margins = new Margins(25, 25, 25, 25);

            rptMain.SetPageSettings(ps);

            rptMain.Width = this.ClientRectangle.Width;
            rptMain.Height = this.ClientRectangle.Height;

            rptMain.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;

            LocalReport rpt = rptMain.LocalReport;

            rpt.ReportPath = @".\Day Schedule.rdl";

            DataSet ds = new DataSet("DataSet1");

            rpt.DataSources.Clear();

            GetReportData(ref ds);

            DataTable dt = ds.Tables[0];
            ReportDataSource rds = new ReportDataSource("DataSet1", dt);

            rpt.DataSources.Add(rds);

            rptMain.RefreshReport();

            Cursor.Current = Cursors.Default;
        }


        public frmReportViewer()
        {
            InitializeComponent();
        }

        private void frmReportViewer_Load(object sender, EventArgs e)
        {


        }

        private void GetReportData(ref DataSet ds)
        {
            string sql =  "select s.*, o.cus_name from tps_prod_schedule s "
                        + "left outer join ppordfil_sql o on s.ord_no = ltrim(o.ord_no) "
                        + "where status <> 'Complete' and schedule_date = '" + m_dSelectedDate.ToShortDateString() + "' order by sequence";

            using (SqlConnection cn = new SqlConnection(m_sConnection))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);

                    adp.Fill(ds, "Schedule");
                }
            }

               
        }

        private void frmReportViewer_Resize(object sender, EventArgs e)
        {
            rptMain.Width = this.ClientRectangle.Width;
            rptMain.Height = this.ClientRectangle.Height;
        }
    }
}
