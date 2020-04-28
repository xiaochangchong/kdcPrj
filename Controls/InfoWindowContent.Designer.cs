namespace xxkUI.GsProject
{
    partial class InfoWindowContent
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gbSite = new System.Windows.Forms.GroupBox();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.lbObsUnit = new DevExpress.XtraEditors.LabelControl();
            this.lbSiteSituation = new DevExpress.XtraEditors.LabelControl();
            this.lbPlace = new DevExpress.XtraEditors.LabelControl();
            this.lbType = new DevExpress.XtraEditors.LabelControl();
            this.lbSiteType = new DevExpress.XtraEditors.LabelControl();
            this.lbSiteStatus = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.lbFault = new DevExpress.XtraEditors.LabelControl();
            this.myBackgroundWorker = new xxkUI.Controls.MyBackgroundWorker();
            this.gbSite.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSite
            // 
            this.gbSite.Controls.Add(this.labelControl4);
            this.gbSite.Controls.Add(this.lbFault);
            this.gbSite.Controls.Add(this.labelControl3);
            this.gbSite.Controls.Add(this.labelControl5);
            this.gbSite.Controls.Add(this.labelControl6);
            this.gbSite.Controls.Add(this.labelControl7);
            this.gbSite.Controls.Add(this.labelControl2);
            this.gbSite.Controls.Add(this.labelControl1);
            this.gbSite.Controls.Add(this.lbObsUnit);
            this.gbSite.Controls.Add(this.lbSiteSituation);
            this.gbSite.Controls.Add(this.lbPlace);
            this.gbSite.Controls.Add(this.lbType);
            this.gbSite.Controls.Add(this.lbSiteType);
            this.gbSite.Controls.Add(this.lbSiteStatus);
            this.gbSite.Location = new System.Drawing.Point(6, 3);
            this.gbSite.Name = "gbSite";
            this.gbSite.Size = new System.Drawing.Size(323, 242);
            this.gbSite.TabIndex = 26;
            this.gbSite.TabStop = false;
            this.gbSite.UseWaitCursor = true;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelControl3.Location = new System.Drawing.Point(161, 61);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 41;
            this.labelControl3.Text = "监测单位：";
            this.labelControl3.UseWaitCursor = true;
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelControl5.Location = new System.Drawing.Point(8, 59);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 14);
            this.labelControl5.TabIndex = 39;
            this.labelControl5.Text = "观测类型：";
            this.labelControl5.UseWaitCursor = true;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelControl6.Location = new System.Drawing.Point(161, 21);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 14);
            this.labelControl6.TabIndex = 38;
            this.labelControl6.Text = "场地类型：";
            this.labelControl6.UseWaitCursor = true;
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelControl7.Location = new System.Drawing.Point(8, 22);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(60, 14);
            this.labelControl7.TabIndex = 37;
            this.labelControl7.Text = "运行状况：";
            this.labelControl7.UseWaitCursor = true;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelControl2.Location = new System.Drawing.Point(8, 97);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 36;
            this.labelControl2.Text = "所  在 地：";
            this.labelControl2.UseWaitCursor = true;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelControl1.Location = new System.Drawing.Point(8, 163);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 35;
            this.labelControl1.Text = "场地概况：";
            this.labelControl1.UseWaitCursor = true;
            // 
            // lbObsUnit
            // 
            this.lbObsUnit.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lbObsUnit.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lbObsUnit.Location = new System.Drawing.Point(227, 61);
            this.lbObsUnit.Name = "lbObsUnit";
            this.lbObsUnit.Size = new System.Drawing.Size(60, 14);
            this.lbObsUnit.TabIndex = 34;
            this.lbObsUnit.Text = "监测单位：";
            this.lbObsUnit.UseWaitCursor = true;
            // 
            // lbSiteSituation
            // 
            this.lbSiteSituation.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lbSiteSituation.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lbSiteSituation.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.lbSiteSituation.Location = new System.Drawing.Point(71, 163);
            this.lbSiteSituation.Name = "lbSiteSituation";
            this.lbSiteSituation.Size = new System.Drawing.Size(242, 0);
            this.lbSiteSituation.TabIndex = 32;
            this.lbSiteSituation.UseWaitCursor = true;
            // 
            // lbPlace
            // 
            this.lbPlace.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lbPlace.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lbPlace.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.lbPlace.Location = new System.Drawing.Point(71, 97);
            this.lbPlace.Name = "lbPlace";
            this.lbPlace.Size = new System.Drawing.Size(242, 0);
            this.lbPlace.TabIndex = 33;
            this.lbPlace.UseWaitCursor = true;
            // 
            // lbType
            // 
            this.lbType.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lbType.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lbType.Location = new System.Drawing.Point(71, 61);
            this.lbType.Name = "lbType";
            this.lbType.Size = new System.Drawing.Size(60, 14);
            this.lbType.TabIndex = 30;
            this.lbType.Text = "观测类型：";
            this.lbType.UseWaitCursor = true;
            // 
            // lbSiteType
            // 
            this.lbSiteType.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lbSiteType.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lbSiteType.Location = new System.Drawing.Point(227, 21);
            this.lbSiteType.Name = "lbSiteType";
            this.lbSiteType.Size = new System.Drawing.Size(60, 14);
            this.lbSiteType.TabIndex = 29;
            this.lbSiteType.Text = "场地类型：";
            this.lbSiteType.UseWaitCursor = true;
            // 
            // lbSiteStatus
            // 
            this.lbSiteStatus.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lbSiteStatus.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lbSiteStatus.Location = new System.Drawing.Point(71, 21);
            this.lbSiteStatus.Name = "lbSiteStatus";
            this.lbSiteStatus.Size = new System.Drawing.Size(60, 14);
            this.lbSiteStatus.TabIndex = 28;
            this.lbSiteStatus.Text = "运行状况：";
            this.lbSiteStatus.UseWaitCursor = true;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.labelControl4.Location = new System.Drawing.Point(8, 129);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(60, 14);
            this.labelControl4.TabIndex = 43;
            this.labelControl4.Text = "所跨断裂：";
            this.labelControl4.UseWaitCursor = true;
            // 
            // lbFault
            // 
            this.lbFault.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.lbFault.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.lbFault.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.lbFault.Location = new System.Drawing.Point(71, 129);
            this.lbFault.Name = "lbFault";
            this.lbFault.Size = new System.Drawing.Size(242, 0);
            this.lbFault.TabIndex = 42;
            this.lbFault.UseWaitCursor = true;
            // 
            // myBackgroundWorker
            // 
            this.myBackgroundWorker.IsExitWithException = false;
            this.myBackgroundWorker.StatusText = null;
            // 
            // InfoWindowContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.gbSite);
            this.Name = "InfoWindowContent";
            this.Size = new System.Drawing.Size(332, 248);
            this.UseWaitCursor = true;
            this.gbSite.ResumeLayout(false);
            this.gbSite.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSite;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl lbObsUnit;
        private DevExpress.XtraEditors.LabelControl lbSiteSituation;
        private DevExpress.XtraEditors.LabelControl lbPlace;
        private DevExpress.XtraEditors.LabelControl lbType;
        private DevExpress.XtraEditors.LabelControl lbSiteType;
        private DevExpress.XtraEditors.LabelControl lbSiteStatus;
        private Controls.MyBackgroundWorker myBackgroundWorker;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl lbFault;
    }
}
