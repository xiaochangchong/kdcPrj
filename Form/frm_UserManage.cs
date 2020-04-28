using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using xxkUI.Bll;
using xxkUI.BLL;
using xxkUI.Tool;

namespace xxkUI.Form
{
    public partial class frm_UserManage : DevExpress.XtraEditors.XtraForm
    {

        private DataTable userdt = null;
        private List<UserInfoBean> userlist = null;
        public frm_UserManage()
        {
            InitializeComponent();
            userdt = new DataTable("userinfo");
            userdt.Columns.Add("username", System.Type.GetType("System.String"));
            userdt.Columns.Add("userunit", System.Type.GetType("System.String"));
            userdt.Columns.Add("userathrty", System.Type.GetType("System.String"));
            userdt.Columns.Add("status", System.Type.GetType("System.String"));
        }

        private void UserManage_Load(object sender, EventArgs e)
        {
            try
            {
                rICbExamine.Items.Add(new CboItemEntity() { Text = "待审核", Value = 0 });
                rICbExamine.Items.Add(new CboItemEntity() { Text = "通过审核", Value = 1 });
                rICbExamine.Items.Add(new CboItemEntity() { Text = "未通过审核", Value = 2 });

                rICbExamine.SelectedIndexChanged += rICbExamine_SelectedIndexChanged;
                rIBtnEdit.ButtonClick += rIBtnEdit_ButtonClick;
                rIbtnDelte.ButtonClick += rIbtnDelte_ButtonClick;
                //4.解决IConvertible问题
                rICbExamine.ParseEditValue += new ConvertEditValueEventHandler(rICbExamine_ParseEditValue);

                BeginInit();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString(),"错误");
            }
            
        }

        void rIbtnDelte_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                DataRow dr = this.gridView.GetDataRow(this.gridView.FocusedRowHandle);
                string UserName = dr[0].ToString();

                UserInfoBll.Instance.DeleteWhere(new { username = UserName });

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("删除数据失败！", "错误");
            }
            finally
            {
                BeginInit();
            }
           

        }

        void rIBtnEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                DataRow dr = this.gridView.GetDataRow(this.gridView.FocusedRowHandle);
                UserInfoBean UIf = UserInfoBll.Instance.GetUserBy(dr[0].ToString());
                frm_Sign signupForm = new frm_Sign(UIf);

                if (signupForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    BeginInit();
                }
               
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("编辑数据失败！", "错误");
            }
            
 
        }

        void rICbExamine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CboItemEntity item = (CboItemEntity)(sender as ComboBoxEdit).SelectedItem;
                string text = item.Text.ToString();
                int value = (int)item.Value;
                DataRow dr = this.gridView.GetDataRow(this.gridView.FocusedRowHandle);
                string UserName = dr[0].ToString();

                UserInfoBll.Instance.UpdateWhatWhere(new { status = value.ToString() }, new { username = UserName });
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show("修改用户状态失败：" + ex.Message, "错误");
            }

            BeginInit();
        }

        void rICbExamine_ParseEditValue(object sender, ConvertEditValueEventArgs e)
        {
            e.Value = e.Value.ToString(); e.Handled = true;
        }

        private void myBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                for (int i = 0; i < userlist.Count; i++)
                {
                    DataRow dr = userdt.NewRow();
                    dr["username"] = userlist[i].UserName;
                    dr["userunit"] = UnitInfoBll.Instance.GetUnitNameBy(userlist[i].UserUnit);
                    string userAthstr = "";
                    if (userlist[i].UserAthrty != null)
                    {
                        string[] unitcodes = userlist[i].UserAthrty.Split(';');
                        if (unitcodes.Length > 0)
                            foreach (string uc in unitcodes)
                            {
                                string unitname = UnitInfoBll.Instance.GetUnitNameBy(uc);
                                if (unitname != string.Empty)
                                    userAthstr += unitname + ";";
                            }
                    }
                    dr["userathrty"] = userAthstr;

                    dr["status"] = new PublicHelper().GetUserStatusDiscription(userlist[i].Status);

                    userdt.Rows.Add(dr);

                    myBackgroundWorker.SetStatusText((i + 1).ToString());
                }
            
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("加载用户数据失败:" + ex.Message, "错误");
            }
        }

        private void myBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressShow.PerformStep();
        }

        private void myBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.gridControl.DataSource = userdt;
            this.progressShow.Visible = false;
            this.gridControl.Height = 329;
        }


        private void BeginInit()
        {
            this.progressShow.Visible = true;
            this.gridControl.Height = 291;
            userlist = UserInfoBll.Instance.GetAll().ToList();
            myBackgroundWorker.RunWorkerAsync();
            progressShow.Properties.Minimum = 1;
            progressShow.Properties.Maximum = userlist.Count;
            progressShow.Properties.Step = 1;
            progressShow.PerformStep();

        }
    }
}