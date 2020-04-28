﻿namespace xxkUI.Form
{
    partial class frm_UserManage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_UserManage));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gridControl = new DevExpress.XtraGrid.GridControl();
            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.GCUserName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GCUserUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GCUserAth = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GCUserStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rICbExamine = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.GCEdit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rIBtnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.GCDelete = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rIbtnDelte = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.rILookUpEditExamine = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.myBackgroundWorker = new xxkUI.Controls.MyBackgroundWorker();
            this.progressShow = new DevExpress.XtraEditors.ProgressBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rICbExamine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rIBtnEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rIbtnDelte)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rILookUpEditExamine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressShow.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl
            // 
            this.gridControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControl.Location = new System.Drawing.Point(0, 0);
            this.gridControl.MainView = this.gridView;
            this.gridControl.Name = "gridControl";
            this.gridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rIBtnEdit,
            this.rIbtnDelte,
            this.rICbExamine,
            this.rILookUpEditExamine});
            this.gridControl.Size = new System.Drawing.Size(581, 291);
            this.gridControl.TabIndex = 0;
            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView});
            // 
            // gridView
            // 
            this.gridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.GCUserName,
            this.GCUserUnit,
            this.GCUserAth,
            this.GCUserStatus,
            this.GCEdit,
            this.GCDelete});
            this.gridView.GridControl = this.gridControl;
            this.gridView.Name = "gridView";
            this.gridView.OptionsView.ShowGroupPanel = false;
            // 
            // GCUserName
            // 
            this.GCUserName.Caption = "用户名";
            this.GCUserName.FieldName = "username";
            this.GCUserName.Name = "GCUserName";
            this.GCUserName.Visible = true;
            this.GCUserName.VisibleIndex = 0;
            // 
            // GCUserUnit
            // 
            this.GCUserUnit.Caption = "用户单位";
            this.GCUserUnit.FieldName = "userunit";
            this.GCUserUnit.Name = "GCUserUnit";
            this.GCUserUnit.Visible = true;
            this.GCUserUnit.VisibleIndex = 1;
            // 
            // GCUserAth
            // 
            this.GCUserAth.Caption = "用户权限";
            this.GCUserAth.FieldName = "userathrty";
            this.GCUserAth.Name = "GCUserAth";
            this.GCUserAth.Visible = true;
            this.GCUserAth.VisibleIndex = 2;
            // 
            // GCUserStatus
            // 
            this.GCUserStatus.Caption = "用户状态";
            this.GCUserStatus.ColumnEdit = this.rICbExamine;
            this.GCUserStatus.FieldName = "status";
            this.GCUserStatus.Name = "GCUserStatus";
            this.GCUserStatus.Visible = true;
            this.GCUserStatus.VisibleIndex = 3;
            this.GCUserStatus.Width = 93;
            // 
            // rICbExamine
            // 
            this.rICbExamine.AutoHeight = false;
            this.rICbExamine.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rICbExamine.Name = "rICbExamine";
            // 
            // GCEdit
            // 
            this.GCEdit.Caption = "编辑";
            this.GCEdit.ColumnEdit = this.rIBtnEdit;
            this.GCEdit.Name = "GCEdit";
            this.GCEdit.Visible = true;
            this.GCEdit.VisibleIndex = 4;
            this.GCEdit.Width = 40;
            // 
            // rIBtnEdit
            // 
            this.rIBtnEdit.AutoHeight = false;
            this.rIBtnEdit.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.rIBtnEdit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("rIBtnEdit.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.rIBtnEdit.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.rIBtnEdit.Name = "rIBtnEdit";
            this.rIBtnEdit.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // GCDelete
            // 
            this.GCDelete.Caption = "删除";
            this.GCDelete.ColumnEdit = this.rIbtnDelte;
            this.GCDelete.Name = "GCDelete";
            this.GCDelete.OptionsEditForm.Caption = "删除";
            this.GCDelete.Visible = true;
            this.GCDelete.VisibleIndex = 5;
            this.GCDelete.Width = 40;
            // 
            // rIbtnDelte
            // 
            this.rIbtnDelte.AutoHeight = false;
            this.rIbtnDelte.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, ((System.Drawing.Image)(resources.GetObject("rIbtnDelte.Buttons"))), new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, null, true)});
            this.rIbtnDelte.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.rIbtnDelte.Name = "rIbtnDelte";
            this.rIbtnDelte.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // rILookUpEditExamine
            // 
            this.rILookUpEditExamine.AutoHeight = false;
            this.rILookUpEditExamine.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rILookUpEditExamine.Name = "rILookUpEditExamine";
            // 
            // myBackgroundWorker
            // 
            this.myBackgroundWorker.IsExitWithException = false;
            this.myBackgroundWorker.StatusText = null;
            this.myBackgroundWorker.WorkerReportsProgress = true;
            this.myBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.myBackgroundWorker_DoWork);
            this.myBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.myBackgroundWorker_ProgressChanged);
            this.myBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.myBackgroundWorker_RunWorkerCompleted);
            // 
            // progressShow
            // 
            this.progressShow.EditValue = 1;
            this.progressShow.Location = new System.Drawing.Point(0, 297);
            this.progressShow.Name = "progressShow";
            this.progressShow.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.progressShow.Properties.Appearance.ForeColor = System.Drawing.Color.Black;
            this.progressShow.Properties.EndColor = System.Drawing.Color.Empty;
            this.progressShow.Properties.LookAndFeel.SkinName = "Blue";
            this.progressShow.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.progressShow.Properties.LookAndFeel.UseWindowsXPTheme = true;
            this.progressShow.Properties.ReadOnly = true;
            this.progressShow.Properties.ShowTitle = true;
            this.progressShow.Properties.StartColor = System.Drawing.Color.Empty;
            this.progressShow.Properties.Step = 1;
            this.progressShow.Size = new System.Drawing.Size(581, 32);
            this.progressShow.TabIndex = 5;
            // 
            // frm_UserManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 326);
            this.Controls.Add(this.progressShow);
            this.Controls.Add(this.gridControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frm_UserManage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "用户管理";
            this.Load += new System.EventHandler(this.UserManage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rICbExamine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rIBtnEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rIbtnDelte)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rILookUpEditExamine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressShow.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
        private DevExpress.XtraGrid.Columns.GridColumn GCUserName;
        private DevExpress.XtraGrid.Columns.GridColumn GCUserUnit;
        private DevExpress.XtraGrid.Columns.GridColumn GCUserAth;
        private DevExpress.XtraGrid.Columns.GridColumn GCUserStatus;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox rICbExamine;
        private DevExpress.XtraGrid.Columns.GridColumn GCEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit rIBtnEdit;
        private DevExpress.XtraGrid.Columns.GridColumn GCDelete;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit rIbtnDelte;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rILookUpEditExamine;
        private Controls.MyBackgroundWorker myBackgroundWorker;
        private DevExpress.XtraEditors.ProgressBarControl progressShow;
    }
}