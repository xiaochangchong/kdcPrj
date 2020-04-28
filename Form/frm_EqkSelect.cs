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
    public partial class frm_EqkSelect : DevExpress.XtraEditors.XtraForm
    {
        public bool isSelectedDb = false;
        public frm_EqkSelect()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (radioGroup.SelectedIndex == 0)
                isSelectedDb = true;
            else
                isSelectedDb = false;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}