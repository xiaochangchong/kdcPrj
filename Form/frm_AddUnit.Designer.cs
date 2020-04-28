namespace xxkUI.Form
{
    partial class frm_AddUnit
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
            this.barFault = new DevExpress.XtraBars.BarEditItem();
            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
            this.txtUnitcode = new DevExpress.XtraEditors.TextEdit();
            this.btnUnitCode = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.txtUnitname = new DevExpress.XtraEditors.TextEdit();
            this.labelUserName = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnitcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnitname.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // barFault
            // 
            this.barFault.Caption = "断层数据：";
            this.barFault.Edit = null;
            this.barFault.EditWidth = 120;
            this.barFault.Id = 51;
            this.barFault.Name = "barFault";
            // 
            // barEditItem1
            // 
            this.barEditItem1.Caption = "断层数据：";
            this.barEditItem1.Edit = null;
            this.barEditItem1.EditWidth = 120;
            this.barEditItem1.Id = 51;
            this.barEditItem1.Name = "barEditItem1";
            // 
            // txtUnitcode
            // 
            this.txtUnitcode.EditValue = "";
            this.txtUnitcode.Location = new System.Drawing.Point(84, 38);
            this.txtUnitcode.Name = "txtUnitcode";
            this.txtUnitcode.Size = new System.Drawing.Size(86, 20);
            this.txtUnitcode.TabIndex = 31;
            // 
            // btnUnitCode
            // 
            this.btnUnitCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnitCode.Location = new System.Drawing.Point(175, 36);
            this.btnUnitCode.Name = "btnUnitCode";
            this.btnUnitCode.Size = new System.Drawing.Size(95, 23);
            this.btnUnitCode.TabIndex = 30;
            this.btnUnitCode.Text = "查看编码规则";
            this.btnUnitCode.Click += new System.EventHandler(this.btnUnitCode_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(11, 38);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(67, 14);
            this.labelControl1.TabIndex = 29;
            this.labelControl1.Text = "*单位编码：";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(195, 73);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(114, 73);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 27;
            this.btnAdd.Text = "新增";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtUnitname
            // 
            this.txtUnitname.EditValue = "";
            this.txtUnitname.Location = new System.Drawing.Point(84, 9);
            this.txtUnitname.Name = "txtUnitname";
            this.txtUnitname.Size = new System.Drawing.Size(187, 20);
            this.txtUnitname.TabIndex = 26;
            // 
            // labelUserName
            // 
            this.labelUserName.Location = new System.Drawing.Point(12, 12);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(67, 14);
            this.labelUserName.TabIndex = 25;
            this.labelUserName.Text = "*单位名称：";
            // 
            // AddUnitFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(286, 108);
            this.Controls.Add(this.txtUnitcode);
            this.Controls.Add(this.btnUnitCode);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtUnitname);
            this.Controls.Add(this.labelUserName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddUnitFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新增监测单位";
            ((System.ComponentModel.ISupportInitialize)(this.txtUnitcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUnitname.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.BarEditItem barFault;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.TextEdit txtUnitcode;
        private DevExpress.XtraEditors.SimpleButton btnUnitCode;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.TextEdit txtUnitname;
        private DevExpress.XtraEditors.LabelControl labelUserName;
    }
}