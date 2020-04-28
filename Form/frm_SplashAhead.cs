using Common.Data.MySql;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using xxkUI.Bll;
using xxkUI.MyCls;
using xxkUI.Tool;

namespace xxkUI.Form
{
    public partial class frm_SplashAhead : XtraForm
    {
        private frm_Main MainFrm = null;
        //传递状态文本
        private string statusTxt = null;

        public frm_SplashAhead()
        {
            InitializeComponent();
            // MysqlEasy.ConnectionString = ConfigurationManager.ConnectionStrings["RemoteDbConnnect"].ConnectionString;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            this.panel.BackColor = Color.FromArgb(50, Color.White);

            DBInfo Dbinfo = DBInfoHelper.GetDbInfoFromFile(Application.StartupPath + "\\dbinfo.kdc");
            MysqlEasy.ConnectionString = "server=" + Dbinfo.server + ";port=" + Dbinfo.port + ";database=" + Dbinfo.database
              + ";uid=" + Dbinfo.uid + ";pwd=" + Dbinfo.pwd + ";Allow User Variables = True";

        }

        //启动分析应用系统
        private void StartDAS()
        {
            try
            {
                statusTxt = "正在初始化系统...";
                backgroundWorker.ReportProgress(10);

                statusTxt = "正在加载系统框架...";
                MainFrm = new frm_Main();
                backgroundWorker.ReportProgress(20);
                Thread.Sleep(100);
              
                MainFrm.InitForm();
                backgroundWorker.ReportProgress(50);
                Thread.Sleep(100);
            
                statusTxt = "正在加载信息树...";
                MainFrm.InitTree();
                backgroundWorker.ReportProgress(60);
                Thread.Sleep(100);

                statusTxt = "正在加载列表数据...";
                MainFrm.InitCombobox();
                backgroundWorker.ReportProgress(70);
                Thread.Sleep(100);

                statusTxt = "正在设置界面风格...";
                MainFrm.InitStyle();
                backgroundWorker.ReportProgress(80);
                Thread.Sleep(100);
              
                statusTxt = "系统初始化成功...";
                backgroundWorker.ReportProgress(100);
                Thread.Sleep(100);
               
                backgroundWorker_RunWorkerCompleted(null, null);
            }
            catch (Exception excep)
            {

                XtraMessageBox.Show("启动应用程序出错," + excep.Message + ",请与系统管理员联系!", "出错提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
             
                updateStatus("准备登录");
            }
        }


        /// <summary>                                
        /// 更新装载状态
        /// </summary>
        /// <param name="strText">状态文本</param>
        public void updateStatus(string strText)
        {
            lbl_Status.Text = "当前状态:" + strText;
            this.Refresh();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Hide();
            MainFrm.Show();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
          
            updateStatus(statusTxt);
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
           
            if (!MysqlEasy.IsCanConnected(MysqlEasy.ConnectionString))
            {
                XtraMessageBox.Show("数据库连接失败，请检查配置是否正确", "提示");
                return;
            }
       

            if (textUsername.Text == "" || textPsd.Text == "")
            {
                XtraMessageBox.Show("用户名和密码不能为空!", "提示");
                if (textUsername.Text == "")
                    textUsername.Focus();

                if (textUsername.Text != "" && textPsd.Text == "")
                    textPsd.Focus();

                return;
            }
            try
            {

                var userName = textUsername.Text;
                var password = textPsd.Text;


                UserInfoBean uiblogin = UserInfoBll.Instance.GetLogin(new UserInfoBean { UserName = userName, Password = UserInfoBll.Instance.encryptPWD(password) });
                if (uiblogin != null)
                {
                    if (uiblogin.Status == "0")//在审核状态
                    {
                        XtraMessageBox.Show("该用户正在审核中，请联系管理员!", "提示");
                        return;
                    }
                    else if (uiblogin.Status == "2")
                    {
                        XtraMessageBox.Show("该用户未通过审核，请联系管理员!", "提示");
                        return;
                    }
                    else
                    {
                        SystemInfo.CurrentUserInfo = uiblogin;
                        this.DialogResult = DialogResult.OK;
                        StartDAS();
                    }
                }
                else
                {
                    XtraMessageBox.Show("用户名或密码错误!", "提示");
                    return;
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }


        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
            Application.Exit();
        }


        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSignup_Click(object sender, EventArgs e)
        {
            frm_Sign sufrm = new frm_Sign();
            sufrm.ShowDialog(this);
        }

        #region 窗体可拖拽
        
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private void frm_SplashAhead_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion


        private void btnConnectTest_Click(object sender, EventArgs e)
        {
            frm_DatabaseConfig frmdb = new frm_DatabaseConfig();
            if(frmdb.ShowDialog(this)== DialogResult.OK)
            {
                DBInfo Dbinfo = DBInfoHelper.GetDbInfoFromFile(Application.StartupPath + "\\dbinfo.kdc");
                MysqlEasy.ConnectionString = "server=" + Dbinfo.server + ";port=" + Dbinfo.port + ";database=" + Dbinfo.database
                  + ";uid=" + Dbinfo.uid + ";pwd=" + Dbinfo.pwd + ";Allow User Variables = True";

            }
        }

        private void frm_SplashAhead_Load(object sender, EventArgs e)
        {
            Application.DoEvents();
            try
            {
                int lx = (this.Width - this.panel.Width) / 2;
                int ly = (this.Height - this.panel.Height) / 2;
                this.panel.Location = new Point(lx, ly);
                Application.DoEvents();
                int aboutx = (this.Width - this.panelAbout.Width) / 2;
                int abouty = this.Height - int.Parse(Math.Round((this.Height * 0.2), 0).ToString());
                this.panelAbout.Location = new Point(aboutx, abouty);
            }
            catch
            { }
        }


        private void frm_SplashAhead_Resize(object sender, EventArgs e)
        {
            Application.DoEvents();
            try
            {
                int lx = (this.Width - this.panel.Width) / 2;
                int ly = (this.Height - this.panel.Height) / 2;
                this.panel.Location = new Point(lx, ly);
                Application.DoEvents();
                int aboutx = (this.Width - this.panelAbout.Width) / 2;
                int abouty = this.Height - int.Parse(Math.Round((this.Height * 0.2), 0).ToString());
                this.panelAbout.Location = new Point(aboutx, abouty);
            }
            catch
            { }
        }
    }
}
