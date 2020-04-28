﻿using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxkUI.Model;
using xxkUI.Tool;

namespace xxkUI.Form
{
    public partial class frm_RemoveStep : XtraForm
    {
        public DataProcessMethod dpm = DataProcessMethod.NoProg;

        private PriAlgorithmHelper pralgthHelper = new PriAlgorithmHelper();//数据操作类
        private DataTable datain = null;//全部数据
        private DataTable dataselect = null;//选中数据
        public DataTable dataoutsel = null;//处理完的选中数据
        public DataTable dataout = null;//处理完的数据
        private Left_Right lr;
        public double progreeValue = double.NaN;
        private int num_simple;
        public frm_RemoveStep(DataTable _datain, DataTable _dataselect)
        {
            InitializeComponent();

            datain = _datain;
            dataselect = _dataselect;
            lbDataCount.Text = datain.Rows.Count.ToString() + "个";
            this.radioGroup.SelectedIndex = 0;

            num_simple = int.Parse(this.spinEditAvgSimpling.Text);
            RefreshInformation();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.radioGroup.SelectedIndex == -1 || this.spinEditAvgSimpling.Text == "")
                return;
            string note = this.richTxtboxNote.Text;
            if (this.checkAddNote.CheckState == CheckState.Unchecked)
                dataout = pralgthHelper.RemoveStepJump(datain, dataselect, DataProcessMethod.RemoveStep, lr, false, out dataoutsel);
            else
                dataout = pralgthHelper.RemoveStepJump(datain, dataselect, DataProcessMethod.RemoveStep, lr, true, out dataoutsel);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void radioGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectexStr = this.radioGroup.Properties.Items[this.radioGroup.SelectedIndex].ToString();
            if (selectexStr == string.Empty)
                return;
            lr = Left_Right.left;
            if (selectexStr.Contains("左侧"))
                lr = Left_Right.left;
            else if (selectexStr.Contains("右侧"))
                lr = Left_Right.right;

            RefreshInformation();

        }

        private void spinEditAvgSimpling_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (e.NewValue != null && (e.NewValue.ToString().StartsWith("-"))|| (e.NewValue.ToString().StartsWith("0")))
            {
                e.Cancel = true;
            }
        }

        private void spinEditAvgSimpling_EditValueChanged(object sender, EventArgs e)
        {
            num_simple = int.Parse(this.spinEditAvgSimpling.Text);
            RefreshInformation();

        }

        private void RefreshInformation()
        {
            string selectexStr = this.radioGroup.Properties.Items[this.radioGroup.SelectedIndex].ToString();
            if (selectexStr == string.Empty)
                return;
            pralgthHelper = new PriAlgorithmHelper();
            int num_simple = int.Parse(this.spinEditAvgSimpling.Text);
            RemoveTipBean rtb = pralgthHelper.RemoveStepJumpTip(datain, dataselect, DataProcessMethod.RemoveStep, num_simple, selectexStr.Contains("消除左侧") ? Left_Right.left : Left_Right.right);

            this.richTxtboxNote.Text = rtb.Tip;
            this.lbExpectOffsetV.Text = rtb.Offset.ToString();
            this.radioGroup.Properties.Items[0].Description = rtb.LeftAve;
            this.radioGroup.Properties.Items[1].Description = rtb.RightAve;
            this.lbDataCount.Text = selectexStr.Contains("消除左侧") ? "左侧" + rtb.Totaleft.ToString() + "个" : "右侧" + rtb.Totalright.ToString() + "个";
        }

    }
}
