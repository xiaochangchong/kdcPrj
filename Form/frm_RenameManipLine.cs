using DevExpress.XtraEditors;
/***********************************************************/
//---模    块：保存到数据处理文件
//---功能描述：保存到数据处理文件窗体
//---编码时间：2017-5-23
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xxkUI.Form
{
    public partial class frm_RenameManipLine : DevExpress.XtraEditors.XtraForm
    {
        public string targitFileName = string.Empty;
    
        public frm_RenameManipLine(string lntitle)
        {
            InitializeComponent();
            this.txtFileReName.Text = lntitle;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtFileReName.Text == string.Empty)
            {
                XtraMessageBox.Show("请输入文件名！", "提示");
                this.txtFileReName.Focus();
                return;
            }
            this.targitFileName = this.txtFileReName.Text;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
