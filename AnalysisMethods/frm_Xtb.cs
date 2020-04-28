/***********************************************************/
//---模    块：协调比计算
//---功能描述：载入数据，设置参数
//---编码时间：2018-10-22
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxkUI.Tool;

namespace xxkUI.AnalysisMethods
{
    public partial class frm_Xtb : DevExpress.XtraEditors.XtraForm
    {
        private DataTable jx1Dt_orgn;//原始基线1表
        private DataTable sz1Dt_orgn;//原始水准1表
        private double jj1;//测线1夹角
        private DataTable jx2Dt_orgn;//原始基线2表
        private DataTable sz2Dt_orgn;//原始水准2表
        private double jj2;//测线2夹角

        private DataTable jx1Dt_jzn;//基准年基线1表
        private DataTable sz1Dt_jzn;//基准年水准1表
        private DataTable jx2Dt_jzn;//基准年基线2表
        private DataTable sz2Dt_jzn;//基准年水准2表

        private DataTable jx1Dt_bhl;//变化量基线1表
        private DataTable sz1Dt_bhl;//变化量水准1表
        private DataTable jx2Dt_bhl;//变化量基线2表
        private DataTable sz2Dt_bhl;//变化量水准2表


        private DataTable db;//
        private DataTable dc;//
        private DataTable bc;//

        public List<DataTable> resDb = null;
      

        public frm_Xtb()
        {
            InitializeComponent();
          
        }


        private void btnFile_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;//可多选
            ofd.Filter = "Excel文件|*.xls;*.xlsx;";
            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;

            SimpleButton btn = sender as SimpleButton;
            switch (btn.Name)
            {
                case "btnFileJx1":
                    { txtJx1.Text = ofd.FileName; }
                    break;
                case "btnFileSz1":
                    { txtSz1.Text = ofd.FileName; }
                    break;
                case "btnFileJx2":
                    { txtJx2.Text = ofd.FileName; }
                    break;
                case "btnFileSz2":
                    { txtSz2.Text = ofd.FileName; }
                    break;
            }
           
           
           

        }

        private void wizardControl_SelectedPageChanged(object sender, DevExpress.XtraWizard.WizardPageChangedEventArgs e)
        {
            //装载原始数据
            if (e.Page.Name == "calculateSHPage")
            {
                if (txtJx1.Text == "" || btnFileSz1.Text == "" || btnFileJx2.Text == "" || btnFileSz2.Text == "" || txtJj1.Text == "" || txtJj2.Text == "")
                {
                    XtraMessageBox.Show("请填写完整信息再执行下一步", "提示");
                    return;
                }
                else
                {

                    jj1 = double.Parse(txtJj1.Text) * Math.PI / 180;//转化为弧度值
                    jj2 = double.Parse(txtJj2.Text) * Math.PI / 180;//转化为弧度值
                    NpoiCreator npcreator = new NpoiCreator();

                    jx1Dt_orgn = npcreator.ExportToDataTable(txtJx1.Text);
                    sz1Dt_orgn = npcreator.ExportToDataTable(txtSz1.Text);
                    jx2Dt_orgn = npcreator.ExportToDataTable(txtJx2.Text);
                    sz2Dt_orgn = npcreator.ExportToDataTable(txtSz2.Text);

                    //计算基准年数据
                    jx1Dt_jzn = GetJznDataTable(jx1Dt_orgn);
                    sz1Dt_jzn = GetJznDataTable(sz1Dt_orgn); 
                    jx2Dt_jzn = GetJznDataTable(jx2Dt_orgn); 
                    sz2Dt_jzn = GetJznDataTable(sz2Dt_orgn);

                    //计算基准值
                    //计算ΔS1、ΔS2、ΔH1、ΔH2（基线1变化量用ΔS1表示，基线2变化量用ΔS2表示，水准1变化量用ΔH1表示，水准2变化量用ΔH2表示）
                    jx1Dt_bhl = GetBhlDataTable(jx1Dt_orgn, jx1Dt_jzn);
                    sz1Dt_bhl = GetBhlDataTable(sz1Dt_orgn, sz1Dt_jzn);
                    jx2Dt_bhl = GetBhlDataTable(jx2Dt_orgn, jx2Dt_jzn);
                    sz2Dt_bhl = GetBhlDataTable(sz2Dt_orgn, sz2Dt_jzn);


                    gridControlS1.DataSource = jx1Dt_bhl;
                    gridControlS2.DataSource = jx2Dt_bhl;
                    gridControlH1.DataSource = sz1Dt_bhl;
                    gridControlH2.DataSource = sz2Dt_bhl;


                }
            }
            else if(e.Page.Name == "calculateBCDPage")
            {

                db = jx1Dt_bhl.Clone();
                dc = jx1Dt_bhl.Clone();
                bc = jx1Dt_bhl.Clone();
                for (int i = 0; i < jx1Dt_bhl.Rows.Count; i++)
                {
                    double s1_v = double.Parse(jx1Dt_bhl.Rows[i][1].ToString());
                    double s2_v = double.Parse(jx2Dt_bhl.Rows[i][1].ToString());

                    double h1_v = double.Parse(sz1Dt_bhl.Rows[i][1].ToString());
                    double h2_v = double.Parse(sz2Dt_bhl.Rows[i][1].ToString());

                    double d_v = (s1_v * Math.Sin(jj2) - s2_v * Math.Sin(jj1)) / (Math.Cos(jj1)*Math.Sin(jj2)-Math.Sin(jj1)*Math.Cos(jj2));
                    double b_v = (s1_v - d_v * Math.Cos(jj1)) / Math.Sin(jj1);
                    double c_v = (h1_v + h2_v) / 2;

                    double db_v = d_v / b_v;
                    double dc_v = d_v / c_v;
                    double bc_v = b_v / c_v;

                    string date = jx1Dt_bhl.Rows[i][0].ToString();
                    DateTime datetime = DateTime.ParseExact(date, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

                    datetime.AddYears(int.Parse(date.Substring(0, 4)));

                    DataRow db_dr = db.NewRow();
                    db_dr[0] = datetime.ToShortDateString();
                    db_dr[1] = Math.Round(db_v,4);
                    db.Rows.Add(db_dr);

                    DataRow dc_dr = dc.NewRow();
                    dc_dr[0] = datetime.ToShortDateString();
                    dc_dr[1] = Math.Round(dc_v,4);
                    dc.Rows.Add(dc_dr);

                    DataRow bc_dr = bc.NewRow();
                    bc_dr[0] = datetime.ToShortDateString();
                    bc_dr[1] = Math.Round(bc_v,4);
                    bc.Rows.Add(bc_dr);

                }

                gridControldb.DataSource = db;
                gridControldc.DataSource = dc;
                gridControlbc.DataSource = bc;

            }

    
            
         

        }


        /// <summary>
        /// 抽取基准年数据
        /// </summary>
        /// <param name="dt">原始数据表</param>
        /// <returns>基准年数据表</returns>
        private DataTable GetJznDataTable(DataTable origdt)
        {
            DataTable jzndt = origdt.Clone();

            string firstyear = origdt.Rows[0][0].ToString().Substring(0, 4);
            foreach (DataRow dr in origdt.Rows)
            {
                string datestr = dr[0].ToString();
                if (datestr.Substring(0, 4) == firstyear)
                {
                    DataRow newdr = jzndt.NewRow();
                    newdr[0] = datestr;
                    newdr[1] = dr[1];
                    jzndt.Rows.Add(newdr);
                }
            }
            return jzndt;
        }

        /// <summary>
        /// 计算变化量
        /// </summary>
        /// <param name="origdt">原始数据表</param>
        /// <param name="jzndt">基准年数据表</param>
        /// <returns>变化量数据表</returns>
        private DataTable GetBhlDataTable(DataTable origdt, DataTable jzndt)
        {
            DataTable bhldt = origdt.Clone();
            try
            {
                int jznyear = int.Parse(jzndt.Rows[0][0].ToString().Substring(0, 4));//基准年份
                foreach (DataRow dr in origdt.Rows)
                {
                    string obvdate = dr[0].ToString();
                    int thisyear = int.Parse(obvdate.Substring(0, 4));
                    if (thisyear == jznyear)
                        continue;

                    int thismonth = int.Parse(obvdate.Substring(4, 2));

                    foreach (DataRow jzndr in jzndt.Rows)
                    {
                        string jzdate = jzndr[0].ToString();
                        int jznmonth = int.Parse(jzdate.Substring(4, 2));

                        if (jznmonth == thismonth)
                        {
                            double bhl = double.Parse(dr[1].ToString()) - double.Parse(jzndr[1].ToString());

                            DataRow newdr = bhldt.NewRow();
                            newdr[0] = dr[0].ToString();
                            newdr[1] = bhl;
                            bhldt.Rows.Add(newdr);
                        }
                    }

                }
            }
            catch (Exception ex)
            { }
            return bhldt;
        }

        private void wizardControl_FinishClick(object sender, CancelEventArgs e)
        {
            resDb = new List<DataTable>();
            resDb.Add(db);
            resDb.Add(dc);
            resDb.Add(bc);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
