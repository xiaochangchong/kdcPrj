namespace xxkUI.Form
{
    partial class frm_About
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
            this.lbl_Name = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.lbl_Version = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_Name
            // 
            this.lbl_Name.Appearance.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_Name.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.lbl_Name.Location = new System.Drawing.Point(31, 12);
            this.lbl_Name.Name = "lbl_Name";
            this.lbl_Name.Size = new System.Drawing.Size(187, 16);
            this.lbl_Name.TabIndex = 10;
            this.lbl_Name.Text = "全国跨断层数据服务平台";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Blue;
            this.labelControl1.Location = new System.Drawing.Point(318, 20);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(0, 16);
            this.labelControl1.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelControl2);
            this.groupBox1.Controls.Add(this.labelControl4);
            this.groupBox1.Controls.Add(this.labelControl3);
            this.groupBox1.Controls.Add(this.labelControl10);
            this.groupBox1.Controls.Add(this.labelControl5);
            this.groupBox1.Location = new System.Drawing.Point(21, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(442, 177);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "版权信息";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.ForestGreen;
            this.labelControl2.Location = new System.Drawing.Point(15, 150);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(201, 14);
            this.labelControl2.TabIndex = 28;
            this.labelControl2.Text = "Email地址：liuwenlon1987@126.com\r\n";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.ForestGreen;
            this.labelControl4.Location = new System.Drawing.Point(15, 29);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(180, 14);
            this.labelControl4.TabIndex = 27;
            this.labelControl4.Text = "监制单位：中国地震应急搜救中心";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.ForestGreen;
            this.labelControl3.Location = new System.Drawing.Point(15, 91);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(141, 14);
            this.labelControl3.TabIndex = 26;
            this.labelControl3.Text = "联系电话：022-84942740";
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.ForeColor = System.Drawing.Color.ForestGreen;
            this.labelControl10.Location = new System.Drawing.Point(15, 122);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(93, 14);
            this.labelControl10.TabIndex = 24;
            this.labelControl10.Text = "QQ：414025855";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.ForestGreen;
            this.labelControl5.Location = new System.Drawing.Point(15, 59);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(192, 14);
            this.labelControl5.TabIndex = 20;
            this.labelControl5.Text = "技术支持：中国地震局第一监测中心";
            // 
            // lbl_Version
            // 
            this.lbl_Version.Location = new System.Drawing.Point(31, 45);
            this.lbl_Version.Name = "lbl_Version";
            this.lbl_Version.Size = new System.Drawing.Size(74, 14);
            this.lbl_Version.TabIndex = 17;
            this.lbl_Version.Text = "版 本 号：4.0";
            // 
            // frm_About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 282);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbl_Version);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.lbl_Name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_About";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "系统版权信息";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.LabelControl lbl_Name;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl lbl_Version;
    }
}