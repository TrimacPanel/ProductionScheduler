namespace ProductionScheduler
{
    partial class frmShowHideColumns
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TenTec.Windows.iGridLib.iGColPattern iGColPattern1 = new TenTec.Windows.iGridLib.iGColPattern();
            TenTec.Windows.iGridLib.iGColPattern iGColPattern2 = new TenTec.Windows.iGridLib.iGColPattern();
            this.iGCellStyleDesign1 = new TenTec.Windows.iGridLib.iGCellStyleDesign();
            this.grdMainCol4CellStyle = new TenTec.Windows.iGridLib.iGCellStyle(true);
            this.grdMainCol4ColHdrStyle = new TenTec.Windows.iGridLib.iGColHdrStyle(true);
            this.grdMain = new TenTec.Windows.iGridLib.iGrid();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).BeginInit();
            this.SuspendLayout();
            // 
            // iGCellStyleDesign1
            // 
            this.iGCellStyleDesign1.ReadOnly = TenTec.Windows.iGridLib.iGBool.False;
            // 
            // grdMain
            // 
            this.grdMain.BorderStyle = TenTec.Windows.iGridLib.iGBorderStyle.Standard;
            iGColPattern1.AllowGrouping = false;
            iGColPattern1.AllowMoving = false;
            iGColPattern1.AllowSizing = false;
            iGColPattern1.CellStyle = this.iGCellStyleDesign1;
            iGColPattern1.Key = "checked";
            iGColPattern1.SortType = TenTec.Windows.iGridLib.iGSortType.None;
            iGColPattern1.Width = 20;
            iGColPattern2.AllowGrouping = false;
            iGColPattern2.AllowMoving = false;
            iGColPattern2.AllowSizing = false;
            iGColPattern2.CellStyle = this.grdMainCol4CellStyle;
            iGColPattern2.ColHdrStyle = this.grdMainCol4ColHdrStyle;
            iGColPattern2.IncludeInSelect = false;
            iGColPattern2.Key = "col_name";
            iGColPattern2.Text = "Column Name";
            iGColPattern2.Width = 220;
            this.grdMain.Cols.AddRange(new TenTec.Windows.iGridLib.iGColPattern[] {
            iGColPattern1,
            iGColPattern2});
            this.grdMain.DefaultAutoGroupRow.Height = 16;
            this.grdMain.DefaultRow.Height = 16;
            this.grdMain.DefaultRow.NormalCellHeight = 16;
            this.grdMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdMain.GridLines.Horizontal = new TenTec.Windows.iGridLib.iGPenStyle(System.Drawing.SystemColors.ControlDark, 0, System.Drawing.Drawing2D.DashStyle.Dash);
            this.grdMain.GridLines.Mode = TenTec.Windows.iGridLib.iGGridLinesMode.None;
            this.grdMain.GridLines.Vertical = new TenTec.Windows.iGridLib.iGPenStyle(System.Drawing.SystemColors.ControlDark, 0, System.Drawing.Drawing2D.DashStyle.Solid);
            this.grdMain.Header.Height = 20;
            this.grdMain.Location = new System.Drawing.Point(9, 9);
            this.grdMain.Margin = new System.Windows.Forms.Padding(0);
            this.grdMain.Name = "grdMain";
            this.grdMain.RowMode = true;
            this.grdMain.SingleClickEdit = true;
            this.grdMain.Size = new System.Drawing.Size(249, 297);
            this.grdMain.StaySorted = true;
            this.grdMain.TabIndex = 17;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(71, 318);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 28);
            this.btnOK.TabIndex = 47;
            this.btnOK.Text = "O&K";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(167, 318);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 28);
            this.btnCancel.TabIndex = 48;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmShowHideColumns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(265, 358);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grdMain);
            this.Name = "frmShowHideColumns";
            this.Text = "Show/Hide Columns";
            this.Load += new System.EventHandler(this.frmShowHideColumns_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TenTec.Windows.iGridLib.iGrid grdMain;
        private TenTec.Windows.iGridLib.iGCellStyleDesign iGCellStyleDesign1;
        private TenTec.Windows.iGridLib.iGCellStyle grdMainCol4CellStyle;
        private TenTec.Windows.iGridLib.iGColHdrStyle grdMainCol4ColHdrStyle;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}