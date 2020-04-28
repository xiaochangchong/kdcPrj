namespace xxkUI.Form
{
    partial class frm_LayoutMap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_LayoutMap));
            this.pictureEdit = new DevExpress.XtraEditors.PictureEdit();
            this.btnZoomin = new DevExpress.XtraEditors.SimpleButton();
            this.btnZoomout = new DevExpress.XtraEditors.SimpleButton();
            this.btnExit = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureEdit
            // 
            this.pictureEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit.Location = new System.Drawing.Point(0, 0);
            this.pictureEdit.Name = "pictureEdit";
            this.pictureEdit.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.pictureEdit.Properties.ZoomAccelerationFactor = 1D;
            this.pictureEdit.Size = new System.Drawing.Size(284, 264);
            this.pictureEdit.TabIndex = 2;
            this.pictureEdit.EditValueChanged += new System.EventHandler(this.pictureEdit_EditValueChanged);
            this.pictureEdit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LayoutMapFrm_MouseDown);
            this.pictureEdit.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LayoutMapFrm_MouseMove);
            this.pictureEdit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LayoutMapFrm_MouseUp);
            // 
            // btnZoomin
            // 
            this.btnZoomin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomin.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnZoomin.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomin.Image")));
            this.btnZoomin.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.btnZoomin.Location = new System.Drawing.Point(207, 3);
            this.btnZoomin.Name = "btnZoomin";
            this.btnZoomin.Size = new System.Drawing.Size(22, 20);
            this.btnZoomin.TabIndex = 3;
            this.btnZoomin.Text = "放大";
            this.btnZoomin.Click += new System.EventHandler(this.btnZoomin_Click);
            // 
            // btnZoomout
            // 
            this.btnZoomout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomout.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnZoomout.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomout.Image")));
            this.btnZoomout.Location = new System.Drawing.Point(233, 3);
            this.btnZoomout.Name = "btnZoomout";
            this.btnZoomout.Size = new System.Drawing.Size(22, 20);
            this.btnZoomout.TabIndex = 4;
            this.btnZoomout.Text = "simpleButton2";
            this.btnZoomout.Click += new System.EventHandler(this.btnZoomout_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.Location = new System.Drawing.Point(259, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(22, 20);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "simpleButton3";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // LayoutMapFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnZoomout);
            this.Controls.Add(this.btnZoomin);
            this.Controls.Add(this.pictureEdit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LayoutMapFrm";
            this.Text = "场地布设图";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LayoutMapFrm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.LayoutMapFrm_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LayoutMapFrm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LayoutMapFrm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LayoutMapFrm_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PictureEdit pictureEdit;
        private DevExpress.XtraEditors.SimpleButton btnZoomin;
        private DevExpress.XtraEditors.SimpleButton btnZoomout;
        private DevExpress.XtraEditors.SimpleButton btnExit;
    }
}