namespace xxkUI.Form
{
    partial class frm_EqkSelect
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
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.radioGroup = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup.Properties)).BeginInit();
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
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(95, 92);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(14, 92);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 27;
            this.btnAdd.Text = "确定";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // radioGroup
            // 
            this.radioGroup.Location = new System.Drawing.Point(10, 6);
            this.radioGroup.Name = "radioGroup";
            this.radioGroup.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "数据库"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "外挂")});
            this.radioGroup.Size = new System.Drawing.Size(160, 71);
            this.radioGroup.TabIndex = 29;
            // 
            // EqkSelectFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(178, 127);
            this.Controls.Add(this.radioGroup);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EqkSelectFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "请选择震例数据源";
            ((System.ComponentModel.ISupportInitialize)(this.radioGroup.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.BarEditItem barFault;
        private DevExpress.XtraBars.BarEditItem barEditItem1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.RadioGroup radioGroup;
    }
}