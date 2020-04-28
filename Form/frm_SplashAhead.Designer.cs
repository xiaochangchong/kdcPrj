namespace xxkUI.Form
{
    partial class frm_SplashAhead
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_SplashAhead));
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSignup = new DevExpress.XtraEditors.SimpleButton();
            this.lbl_Status = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExit = new DevExpress.XtraEditors.SimpleButton();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.textPsd = new DevExpress.XtraEditors.TextEdit();
            this.textUsername = new DevExpress.XtraEditors.TextEdit();
            this.btnConnectTest = new DevExpress.XtraEditors.SimpleButton();
            this.panelAbout = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textPsd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textUsername.Properties)).BeginInit();
            this.panelAbout.SuspendLayout();
            this.SuspendLayout();
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(78, 74);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 36;
            this.pictureBox1.TabStop = false;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.panel1);
            this.panel.Location = new System.Drawing.Point(401, 191);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(678, 383);
            this.panel.TabIndex = 41;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnSignup);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.lbl_Status);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnLogin);
            this.panel1.Controls.Add(this.textPsd);
            this.panel1.Controls.Add(this.textUsername);
            this.panel1.Controls.Add(this.btnConnectTest);
            this.panel1.Location = new System.Drawing.Point(18, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(638, 340);
            this.panel1.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("微软雅黑", 25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label4.Location = new System.Drawing.Point(87, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(548, 59);
            this.label4.TabIndex = 51;
            this.label4.Text = "全国跨断层数据服务平台（4.0版）";
            // 
            // btnSignup
            // 
            this.btnSignup.Appearance.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnSignup.Appearance.ForeColor = System.Drawing.Color.White;
            this.btnSignup.Appearance.Options.UseBackColor = true;
            this.btnSignup.Appearance.Options.UseForeColor = true;
            this.btnSignup.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnSignup.Location = new System.Drawing.Point(332, 250);
            this.btnSignup.Name = "btnSignup";
            this.btnSignup.Size = new System.Drawing.Size(71, 25);
            this.btnSignup.TabIndex = 50;
            this.btnSignup.Text = "注册";
            this.btnSignup.Click += new System.EventHandler(this.btnSignup_Click);
            // 
            // lbl_Status
            // 
            this.lbl_Status.AutoSize = true;
            this.lbl_Status.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Status.ForeColor = System.Drawing.Color.DimGray;
            this.lbl_Status.Location = new System.Drawing.Point(447, 311);
            this.lbl_Status.Name = "lbl_Status";
            this.lbl_Status.Size = new System.Drawing.Size(59, 15);
            this.lbl_Status.TabIndex = 47;
            this.lbl_Status.Text = "正在加载";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F);
            this.label1.ForeColor = System.Drawing.Color.MediumBlue;
            this.label1.Location = new System.Drawing.Point(145, 135);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 19);
            this.label1.TabIndex = 43;
            this.label1.Text = "用户名：";
            // 
            // btnExit
            // 
            this.btnExit.Appearance.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnExit.Appearance.ForeColor = System.Drawing.Color.White;
            this.btnExit.Appearance.Options.UseBackColor = true;
            this.btnExit.Appearance.Options.UseForeColor = true;
            this.btnExit.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(426, 250);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(71, 25);
            this.btnExit.TabIndex = 49;
            this.btnExit.Text = "退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F);
            this.label2.ForeColor = System.Drawing.Color.MediumBlue;
            this.label2.Location = new System.Drawing.Point(145, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 19);
            this.label2.TabIndex = 44;
            this.label2.Text = "密   码：";
            // 
            // btnLogin
            // 
            this.btnLogin.Appearance.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnLogin.Appearance.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Appearance.Options.UseBackColor = true;
            this.btnLogin.Appearance.Options.UseForeColor = true;
            this.btnLogin.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnLogin.Location = new System.Drawing.Point(237, 250);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(71, 25);
            this.btnLogin.TabIndex = 48;
            this.btnLogin.Text = "登录";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // textPsd
            // 
            this.textPsd.EditValue = "";
            this.textPsd.Location = new System.Drawing.Point(224, 171);
            this.textPsd.Name = "textPsd";
            this.textPsd.Properties.AutoHeight = false;
            this.textPsd.Properties.UseSystemPasswordChar = true;
            this.textPsd.Size = new System.Drawing.Size(199, 35);
            this.textPsd.TabIndex = 42;
            // 
            // textUsername
            // 
            this.textUsername.EditValue = "";
            this.textUsername.Location = new System.Drawing.Point(224, 129);
            this.textUsername.Name = "textUsername";
            this.textUsername.Properties.AutoHeight = false;
            this.textUsername.Size = new System.Drawing.Size(199, 35);
            this.textUsername.TabIndex = 41;
            // 
            // btnConnectTest
            // 
            this.btnConnectTest.Appearance.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnConnectTest.Appearance.ForeColor = System.Drawing.Color.White;
            this.btnConnectTest.Appearance.Options.UseBackColor = true;
            this.btnConnectTest.Appearance.Options.UseForeColor = true;
            this.btnConnectTest.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.btnConnectTest.Location = new System.Drawing.Point(149, 250);
            this.btnConnectTest.Name = "btnConnectTest";
            this.btnConnectTest.Size = new System.Drawing.Size(71, 25);
            this.btnConnectTest.TabIndex = 45;
            this.btnConnectTest.Text = "数据库配置";
            this.btnConnectTest.Click += new System.EventHandler(this.btnConnectTest_Click);
            // 
            // panelAbout
            // 
            this.panelAbout.Controls.Add(this.label6);
            this.panelAbout.Controls.Add(this.label5);
            this.panelAbout.Location = new System.Drawing.Point(401, 585);
            this.panelAbout.Name = "panelAbout";
            this.panelAbout.Size = new System.Drawing.Size(678, 100);
            this.panelAbout.TabIndex = 53;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("黑体", 10F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(18, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(619, 43);
            this.label6.TabIndex = 54;
            this.label6.Text = "联系电话：022-84942740";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(17, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(636, 43);
            this.label5.TabIndex = 53;
            this.label5.Text = "监制：中国地震应急搜救中心   研制：中国地震局第一监测中心";
            // 
            // frm_SplashAhead
            // 
            this.AcceptButton = this.btnLogin;
            this.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayoutStore = System.Windows.Forms.ImageLayout.Stretch;
            this.BackgroundImageStore = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImageStore")));
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(1354, 715);
            this.Controls.Add(this.panelAbout);
            this.Controls.Add(this.panel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Office 2013";
            this.Name = "frm_SplashAhead";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "全国跨断层数据服务平台（4.0版）";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_SplashAhead_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frm_SplashAhead_MouseDown);
            this.Resize += new System.EventHandler(this.frm_SplashAhead_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textPsd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textUsername.Properties)).EndInit();
            this.panelAbout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel;
        private DevExpress.XtraEditors.SimpleButton btnSignup;
        private System.Windows.Forms.Label lbl_Status;
        private DevExpress.XtraEditors.SimpleButton btnExit;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SimpleButton btnConnectTest;
        private DevExpress.XtraEditors.TextEdit textUsername;
        private DevExpress.XtraEditors.TextEdit textPsd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelAbout;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
    }
}