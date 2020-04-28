using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Common.Data.MySql;
using xxkUI.MyCls;
using System.Configuration;

namespace xxkUI.Form
{
    public partial class frm_DatabaseConfig : DevExpress.XtraEditors.XtraForm
    {
        public frm_DatabaseConfig()
        {
            InitializeComponent();

            DBInfo Dbinfo = DBInfoHelper.GetDbInfoFromFile(Application.StartupPath+ "\\dbinfo.kdc");
            txtServerIP.Text = Dbinfo.server;
            txtPort.Text = Dbinfo.port;
            txtDBName.Text = Dbinfo.database;
            txtUID.Text = Dbinfo.uid;
            txtPSW.Text = Dbinfo.pwd;
        }

        private void btnTestConn_Click(object sender, EventArgs e)
        {
            string connstr = "server=" + txtServerIP.Text + ";port=" + txtPort.Text + ";database=" + txtDBName.Text 
                + ";uid=" + txtUID.Text + ";pwd=" + txtPSW.Text + ";Allow User Variables = True";

            if (!MysqlEasy.IsCanConnected(connstr))
            {
                XtraMessageBox.Show("数据库连接失败，请检查配置是否正确", "提示");
            }
            else
            {
                XtraMessageBox.Show("数据库连接成功", "提示");
            }

        }

        private void btnSaveConfigInfo_Click(object sender, EventArgs e)
        {
            DBInfo Dbinfo = new DBInfo();
            Dbinfo.server = txtServerIP.Text;
            Dbinfo.port = txtPort.Text;
            Dbinfo.database = txtDBName.Text;
            Dbinfo.uid = txtUID.Text;
            Dbinfo.pwd = txtPSW.Text;

            if (DBInfoHelper.SaveDbInfo(Dbinfo, Application.StartupPath + "\\dbinfo.kdc"))
            {
                XtraMessageBox.Show("数据库信息配置成功", "提示");
                this.DialogResult = DialogResult.OK;
                this.Dispose();
                this.Close();
            }
            else
            {
                XtraMessageBox.Show("数据库信息配置失败，请检查数据库配置文件是否有读写权限", "提示");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
            this.Close();
        }
    }
}