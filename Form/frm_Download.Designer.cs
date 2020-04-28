namespace xxkUI.Form
{
    partial class frm_Download
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.progressBar = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
            this.btnDown = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.progressBarControl = new DevExpress.XtraEditors.ProgressBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl
            // 
            this.gridControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            gridLevelNode2.RelationName = "Level1";
            this.gridControl.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.gridControl.Location = new System.Drawing.Point(12, 38);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.progressBar});
            this.gridControl.Size = new System.Drawing.Size(503, 284);
            this.gridControl.TabIndex = 1;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsSelection.MultiSelect = true;
            this.gridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridView.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "测项";
            this.gridColumn1.FieldName = "linename";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "起始时间";
            this.gridColumn2.FieldName = "startdate";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "终止时间";
            this.gridColumn3.FieldName = "enddate";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point(359, 328);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(75, 23);
            this.btnDown.TabIndex = 2;
            this.btnDown.Text = "下载";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(440, 328);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "场地：";
            // 
            // comboBoxEdit
            // 
            this.comboBoxEdit.EditValue = "";
            this.comboBoxEdit.Location = new System.Drawing.Point(54, 12);
            this.comboBoxEdit.Name = "comboBoxEdit";
            this.comboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit.Size = new System.Drawing.Size(461, 20);
            this.comboBoxEdit.TabIndex = 0;
            this.comboBoxEdit.SelectedIndexChanged += new System.EventHandler(this.comboBoxEdit_SelectedIndexChanged);
            // 
            // progressBarControl
            // 
            this.progressBarControl.Location = new System.Drawing.Point(12, 333);
            this.progressBarControl.Name = "progressBarControl";
            this.progressBarControl.Size = new System.Drawing.Size(325, 18);
            this.progressBarControl.TabIndex = 5;
            // 
            // frm_Download
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 363);
            this.Controls.Add(this.progressBarControl);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.gridControl);
            this.Controls.Add(this.comboBoxEdit);
            this.Name = "frm_Download";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "下载信息库及测项";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraEditors.SimpleButton btnDown;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.Repository.RepositoryItemProgressBar progressBar;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl;
    }
}