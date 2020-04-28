namespace xxkUI.Form
{
    partial class frm_DatabaseConfig
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
            this.txtServerIP = new DevExpress.XtraEditors.TextEdit();
            this.txtPort = new DevExpress.XtraEditors.TextEdit();
            this.txtDBName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtUID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.txtPSW = new DevExpress.XtraEditors.TextEdit();
            this.btnTestConn = new DevExpress.XtraEditors.SimpleButton();
            this.btnSaveConfigInfo = new DevExpress.XtraEditors.SimpleButton();
            this.btnExit = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerIP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDBName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPSW.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(95, 9);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(167, 20);
            this.txtServerIP.TabIndex = 0;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(95, 43);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(167, 20);
            this.txtPort.TabIndex = 1;
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(95, 77);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(167, 20);
            this.txtDBName.TabIndex = 2;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(26, 12);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(59, 14);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "服务器IP：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(29, 46);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(56, 14);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "端 口 号：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(25, 80);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "数据库名：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(29, 117);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(56, 14);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "登 录 ID：";
            // 
            // txtUID
            // 
            this.txtUID.Location = new System.Drawing.Point(95, 111);
            this.txtUID.Name = "txtUID";
            this.txtUID.Size = new System.Drawing.Size(167, 20);
            this.txtUID.TabIndex = 6;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(29, 148);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(56, 14);
            this.labelControl5.TabIndex = 9;
            this.labelControl5.Text = "密     码：";
            // 
            // txtPSW
            // 
            this.txtPSW.Location = new System.Drawing.Point(95, 145);
            this.txtPSW.Name = "txtPSW";
            this.txtPSW.Size = new System.Drawing.Size(167, 20);
            this.txtPSW.TabIndex = 8;
            // 
            // btnTestConn
            // 
            this.btnTestConn.Location = new System.Drawing.Point(25, 185);
            this.btnTestConn.Name = "btnTestConn";
            this.btnTestConn.Size = new System.Drawing.Size(75, 23);
            this.btnTestConn.TabIndex = 10;
            this.btnTestConn.Text = "测试连接";
            this.btnTestConn.Click += new System.EventHandler(this.btnTestConn_Click);
            // 
            // btnSaveConfigInfo
            // 
            this.btnSaveConfigInfo.Location = new System.Drawing.Point(106, 185);
            this.btnSaveConfigInfo.Name = "btnSaveConfigInfo";
            this.btnSaveConfigInfo.Size = new System.Drawing.Size(75, 23);
            this.btnSaveConfigInfo.TabIndex = 11;
            this.btnSaveConfigInfo.Text = "保存配置";
            this.btnSaveConfigInfo.Click += new System.EventHandler(this.btnSaveConfigInfo_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(187, 185);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 12;
            this.btnExit.Text = "退出";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // frm_DatabaseConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 236);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSaveConfigInfo);
            this.Controls.Add(this.btnTestConn);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.txtPSW);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtUID);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtDBName);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtServerIP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frm_DatabaseConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据库配置";
            ((System.ComponentModel.ISupportInitialize)(this.txtServerIP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDBName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPSW.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtServerIP;
        private DevExpress.XtraEditors.TextEdit txtPort;
        private DevExpress.XtraEditors.TextEdit txtDBName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtUID;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TextEdit txtPSW;
        private DevExpress.XtraEditors.SimpleButton btnTestConn;
        private DevExpress.XtraEditors.SimpleButton btnSaveConfigInfo;
        private DevExpress.XtraEditors.SimpleButton btnExit;
    }
}