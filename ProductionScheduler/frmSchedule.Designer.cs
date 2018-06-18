namespace ProductionScheduler
{
    partial class frmSchedule
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
            this.splMain = new System.Windows.Forms.SplitContainer();
            this.grdOrders = new System.Windows.Forms.DataGridView();
            this.grdProd = new System.Windows.Forms.DataGridView();
            this.pnlMid = new System.Windows.Forms.Panel();
            this.btnSchedule = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).BeginInit();
            this.splMain.Panel1.SuspendLayout();
            this.splMain.Panel2.SuspendLayout();
            this.splMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdProd)).BeginInit();
            this.pnlMid.SuspendLayout();
            this.SuspendLayout();
            // 
            // splMain
            // 
            this.splMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splMain.Location = new System.Drawing.Point(0, 0);
            this.splMain.Name = "splMain";
            this.splMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splMain.Panel1
            // 
            this.splMain.Panel1.Controls.Add(this.grdOrders);
            // 
            // splMain.Panel2
            // 
            this.splMain.Panel2.Controls.Add(this.grdProd);
            this.splMain.Panel2.Controls.Add(this.pnlMid);
            this.splMain.Size = new System.Drawing.Size(1050, 544);
            this.splMain.SplitterDistance = 206;
            this.splMain.TabIndex = 5;
            // 
            // grdOrders
            // 
            this.grdOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdOrders.Location = new System.Drawing.Point(0, 0);
            this.grdOrders.Name = "grdOrders";
            this.grdOrders.Size = new System.Drawing.Size(1050, 206);
            this.grdOrders.TabIndex = 0;
            // 
            // grdProd
            // 
            this.grdProd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdProd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdProd.Location = new System.Drawing.Point(0, 34);
            this.grdProd.Name = "grdProd";
            this.grdProd.Size = new System.Drawing.Size(1050, 300);
            this.grdProd.TabIndex = 4;
            // 
            // pnlMid
            // 
            this.pnlMid.Controls.Add(this.btnSchedule);
            this.pnlMid.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMid.Location = new System.Drawing.Point(0, 0);
            this.pnlMid.Name = "pnlMid";
            this.pnlMid.Size = new System.Drawing.Size(1050, 34);
            this.pnlMid.TabIndex = 0;
            // 
            // btnSchedule
            // 
            this.btnSchedule.Location = new System.Drawing.Point(3, 5);
            this.btnSchedule.Name = "btnSchedule";
            this.btnSchedule.Size = new System.Drawing.Size(107, 23);
            this.btnSchedule.TabIndex = 0;
            this.btnSchedule.Text = "Add to Schedule";
            this.btnSchedule.UseVisualStyleBackColor = true;
            this.btnSchedule.Click += new System.EventHandler(this.btnSchedule_Click);
            // 
            // frmSchedule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 544);
            this.Controls.Add(this.splMain);
            this.Name = "frmSchedule";
            this.Text = "Trimac - Production Manager";
            this.splMain.Panel1.ResumeLayout(false);
            this.splMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splMain)).EndInit();
            this.splMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdProd)).EndInit();
            this.pnlMid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splMain;
        private System.Windows.Forms.DataGridView grdProd;
        private System.Windows.Forms.Panel pnlMid;
        private System.Windows.Forms.Button btnSchedule;
        private System.Windows.Forms.DataGridView grdOrders;
    }
}

