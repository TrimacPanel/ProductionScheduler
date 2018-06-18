namespace ProductionScheduler
{
    partial class frmMarkComplete
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
            TenTec.Windows.iGridLib.iGColPattern iGColPattern6 = new TenTec.Windows.iGridLib.iGColPattern();
            TenTec.Windows.iGridLib.iGColPattern iGColPattern7 = new TenTec.Windows.iGridLib.iGColPattern();
            TenTec.Windows.iGridLib.iGColPattern iGColPattern8 = new TenTec.Windows.iGridLib.iGColPattern();
            TenTec.Windows.iGridLib.iGColPattern iGColPattern9 = new TenTec.Windows.iGridLib.iGColPattern();
            TenTec.Windows.iGridLib.iGColPattern iGColPattern10 = new TenTec.Windows.iGridLib.iGColPattern();
            this.grdMain = new TenTec.Windows.iGridLib.iGrid();
            this.grdMainCol5CellStyle = new TenTec.Windows.iGridLib.iGCellStyle(true);
            this.grdMainCol5ColHdrStyle = new TenTec.Windows.iGridLib.iGColHdrStyle(true);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).BeginInit();
            this.SuspendLayout();
            // 
            // grdMain
            // 
            iGColPattern6.AllowGrouping = false;
            iGColPattern6.AllowMoving = false;
            iGColPattern6.AllowSizing = false;
            iGColPattern6.CellStyle = this.grdMainCol5CellStyle;
            iGColPattern6.ColHdrStyle = this.grdMainCol5ColHdrStyle;
            iGColPattern6.Key = "checked";
            iGColPattern7.AllowGrouping = false;
            iGColPattern7.AllowMoving = false;
            iGColPattern7.AllowSizing = false;
            iGColPattern7.CellStyle = this.grdMainCol5CellStyle;
            iGColPattern7.ColHdrStyle = this.grdMainCol5ColHdrStyle;
            iGColPattern7.Key = "stage";
            iGColPattern7.Text = "Stage";
            iGColPattern8.AllowGrouping = false;
            iGColPattern8.AllowMoving = false;
            iGColPattern8.AllowSizing = false;
            iGColPattern8.CellStyle = this.grdMainCol5CellStyle;
            iGColPattern8.ColHdrStyle = this.grdMainCol5ColHdrStyle;
            iGColPattern8.Key = "line";
            iGColPattern8.Text = "Line";
            iGColPattern9.AllowGrouping = false;
            iGColPattern9.AllowMoving = false;
            iGColPattern9.AllowSizing = false;
            iGColPattern9.Key = "date";
            iGColPattern9.Text = "Date";
            iGColPattern10.AllowGrouping = false;
            iGColPattern10.AllowMoving = false;
            iGColPattern10.AllowSizing = false;
            iGColPattern10.Key = "notes";
            iGColPattern10.SortType = TenTec.Windows.iGridLib.iGSortType.None;
            iGColPattern10.Text = "Notes";
            iGColPattern10.Width = 233;
            this.grdMain.Cols.AddRange(new TenTec.Windows.iGridLib.iGColPattern[] {
            iGColPattern6,
            iGColPattern7,
            iGColPattern8,
            iGColPattern9,
            iGColPattern10});
            this.grdMain.DefaultAutoGroupRow.Height = 20;
            this.grdMain.DefaultRow.Height = 20;
            this.grdMain.DefaultRow.NormalCellHeight = 20;
            this.grdMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grdMain.Header.Height = 23;
            this.grdMain.Location = new System.Drawing.Point(9, 66);
            this.grdMain.Margin = new System.Windows.Forms.Padding(0);
            this.grdMain.Name = "grdMain";
            this.grdMain.RowMode = true;
            this.grdMain.Size = new System.Drawing.Size(563, 232);
            this.grdMain.TabIndex = 17;
            // 
            // grdMainCol5CellStyle
            // 
            this.grdMainCol5CellStyle.Type = TenTec.Windows.iGridLib.iGCellType.Check;
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(481, 311);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 28);
            this.btnCancel.TabIndex = 50;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(385, 311);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 28);
            this.btnOK.TabIndex = 49;
            this.btnOK.Text = "O&K";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // frmMarkComplete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 385);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grdMain);
            this.Name = "frmMarkComplete";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMarkComplete";
            this.Load += new System.EventHandler(this.frmMarkComplete_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TenTec.Windows.iGridLib.iGrid grdMain;
        private TenTec.Windows.iGridLib.iGCellStyle grdMainCol5CellStyle;
        private TenTec.Windows.iGridLib.iGColHdrStyle grdMainCol5ColHdrStyle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}