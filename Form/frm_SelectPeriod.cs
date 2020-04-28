using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;


namespace xxkUI.Form
{
    public partial class frm_SelectPeriod : DevExpress.XtraEditors.XtraForm
    {
        public string StartDateStr = "";
        public string EndDateStr = "";
        public frm_SelectPeriod()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            StartDateStr = this.dateEditStartDate.DateTime.ToShortDateString();
            EndDateStr = this.dateEditEndDate.DateTime.ToShortDateString();

            if (StartDateStr == "" || EndDateStr == "")
            {
                XtraMessageBox.Show("起始日期和终止日期不能为空！", "提示");
                return;
            }
            this.DialogResult = DialogResult.OK;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}