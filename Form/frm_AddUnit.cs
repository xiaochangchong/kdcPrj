using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using xxkUI.BLL;
using System.Web.Security;
using xxkUI.Tool;
using xxkUI.Bll;

namespace xxkUI.Form
{
    public partial class frm_AddUnit : DevExpress.XtraEditors.XtraForm
    {
        public frm_AddUnit()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtUnitname.Text == "" || txtUnitcode.Text == "")
            {
                XtraMessageBox.Show("单位名称和单位编码不能为空!", "提示");
                return;
            }

            if (txtUnitcode.Text.Trim().Length != 6)
            {
                XtraMessageBox.Show("单位编码必须为六位数字!", "提示");
                return;
            }
            try
            {


                UnitInfoBean usermodel = new UnitInfoBean();
                usermodel.UnitName = txtUnitname.Text;
                usermodel.UnitCode = txtUnitcode.Text;

                if (UnitInfoBll.Instance.GetUnitNameBy(usermodel.UnitCode) != string.Empty)
                {
                    XtraMessageBox.Show("该单位已存在", "提示");
                    return;
                }
                else
                {
                    UnitInfoBll.Instance.Add(usermodel);

                    if (SystemInfo.CurrentUserInfo.UserAthrty.Substring(SystemInfo.CurrentUserInfo.UserAthrty.Length - 1, 1) == ";")
                        SystemInfo.CurrentUserInfo.UserAthrty += usermodel.UnitCode;
                    else
                        SystemInfo.CurrentUserInfo.UserAthrty +=";"+ usermodel.UnitCode;

                    UserInfoBll.Instance.UpdateWhatWhere(new { userathrty = SystemInfo.CurrentUserInfo.UserAthrty}, new { username = SystemInfo.CurrentUserInfo.UserName });


                    XtraMessageBox.Show("新增监测单位成功！", "提示");
                    this.Close();
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {

                XtraMessageBox.Show("新增监测单位失败，请联系管理员", "错误");

                this.DialogResult = System.Windows.Forms.DialogResult.No;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnUnitCode_Click(object sender, EventArgs e)
        {
            frm_UnitCodeHelp uch = new frm_UnitCodeHelp();
            uch.Show();
        }
    }
}