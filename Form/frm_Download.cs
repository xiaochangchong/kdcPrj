using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxkUI.Bll;
using xxkUI.Tool;

namespace xxkUI.Form
{
    public partial class frm_Download : XtraForm
    {
        private string unitcode = string.Empty;
        public frm_Download(string _unitcode)
        {
            if (_unitcode == "")
                return;
            unitcode = _unitcode;

            InitializeComponent();
            InitProgressbar();
            InitSites();
        }


        private void InitSites()
        {
            DataTable dt = SiteBll.Instance.GetDataTable("select sitecode,sitename from t_siteinfodb where unitcode = '" + unitcode + "'");
            this.comboBoxEdit.Properties.Items.Clear();
            CheckedListBoxItem[] itemListQuery = new CheckedListBoxItem[dt.Rows.Count];

            int check = 0;
            foreach (DataRow dr in dt.Rows)
            {
                itemListQuery[check] = new CheckedListBoxItem(dr["sitecode"].ToString(), dr["sitename"].ToString());
                check++;

            }
            this.comboBoxEdit.Properties.Items.AddRange(itemListQuery);

            this.comboBoxEdit.RefreshEditValue();
            this.comboBoxEdit.SelectedIndex = -1;
        }

        private void InitObslines(string sitecode)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable datasource = new DataTable();
            datasource.Columns.Add("linename");
            datasource.Columns.Add("startdate", typeof(DateTime));
            datasource.Columns.Add("enddate", typeof(DateTime));

            DataTable dt = LineBll.Instance.GetDataTable("select OBSLINENAME,OBSLINECODE from t_obslinetb where sitecode ='" + sitecode + "'");
            foreach (DataRow dr in dt.Rows)
            {
                string linecode = dr["OBSLINECODE"].ToString();
                DataTable dtmaxmindata = LineObsBll.Instance.GetDataTable("select max(OBVDATE) as maxdate,min(OBVDATE) as mindate from t_obsrvtntb where OBSLINECODE='" + linecode + "'");

                DateTime datetimemax = Convert.ToDateTime(Convert.ToDateTime(dtmaxmindata.Rows[0]["maxdate"]).ToString("yyyy-MM-dd HH:mm:ss"));
                DateTime datetimemin = Convert.ToDateTime(Convert.ToDateTime(dtmaxmindata.Rows[0]["mindate"]).ToString("yyyy-MM-dd HH:mm:ss"));

                DataRow newdr = datasource.NewRow();
                newdr["linename"] = dr["OBSLINENAME"].ToString();
                newdr["startdate"] = datetimemin;
                newdr["enddate"] = datetimemax;
                datasource.Rows.Add(newdr);
            }

            this.gridControl.DataSource = datasource;

            this.Cursor = Cursors.Arrow;
        }

        private void InitProgressbar()
        {
            progressBarControl.Visible = true;

            //是否显示进度数据  
            progressBarControl.Properties.ShowTitle = true;
            //是否显示百分比  
            progressBarControl.Properties.PercentView = true;
            //设置进度条的样式  
            progressBarControl.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;

        }


        private void btnDown_Click(object sender, EventArgs e)
        {
            if (this.gridView.GetSelectedRows().Length == 0)
            {
                XtraMessageBox.Show("未选中任何记录", "提示");
                return;
            }
            try
            {
                FolderBrowserDialog sfd = new FolderBrowserDialog();
                //sfd.Title = "请选择信息库下载路径";
                if (sfd.ShowDialog() == DialogResult.OK)
                {


                    string sitecode = (comboBoxEdit.SelectedItem as CheckedListBoxItem).Value.ToString();
                    DataTable dt = SiteBll.Instance.GetDataTable("select * from t_siteinfodb where sitecode='" + sitecode + "'");

                    #region 创建信息库路径
                    string baseinfoPath = sfd.SelectedPath + "\\" + dt.Rows[0]["sitename"].ToString();
                    if (!Directory.Exists(baseinfoPath))
                    {
                        Directory.CreateDirectory(baseinfoPath);
                    }

                    string laytoutmapPath = baseinfoPath + "\\场地图";
                    if (!Directory.Exists(laytoutmapPath))
                    {
                        Directory.CreateDirectory(laytoutmapPath);
                    }

                    string remotemapPath = baseinfoPath + "\\卫星图";
                    if (!Directory.Exists(remotemapPath))
                    {
                        Directory.CreateDirectory(remotemapPath);
                    }


                    string linePath = baseinfoPath + "\\测线信息";
                    if (!Directory.Exists(linePath))
                    {
                        Directory.CreateDirectory(linePath);
                    }

                    string lineinfoPath = baseinfoPath + "\\测线信息\\测线基础信息";
                    if (!Directory.Exists(lineinfoPath))
                    {
                        Directory.CreateDirectory(lineinfoPath);
                    }

                    string lineobsPath = baseinfoPath + "\\测线信息\\观测数据";
                    if (!Directory.Exists(lineobsPath))
                    {
                        Directory.CreateDirectory(lineobsPath);
                    }

                    #endregion

                    #region 生成场地基础信息表

                    if (dt.Rows.Count > 0)
                    {
                        NpoiCreator npcreator = new NpoiCreator();
                        npcreator.TemplateFile = SystemInfo.DatabaseCache;
                        npcreator.NpoiExcel(dt, baseinfoPath + "\\场地基础信息.xls");
                    }
                    #endregion

                    #region 生成卫星图
                    List<RemoteMapBean> rblist = RemotemapBll.Instance.GetWhere(new { sitecode = sitecode }).ToList();
                    if (rblist.Count != 0)
                    {
                        for (int i = 0; i < rblist.Count; i++)
                        {
                            if (rblist[i].remotemap != null && rblist[i].remotemap.Length > 0)
                            {
                                MemoryStream stream = new MemoryStream();
                                stream.Write(rblist[i].remotemap, 0, rblist[i].remotemap.Length);

                                Bitmap bitMap = new Bitmap(stream);
                                bitMap.Save(remotemapPath + "\\" + rblist[i].remotemapname + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                    }

                    #endregion

                    #region 生成场地图

                    List<LayoutmapBean> lblist = LayoutmapBll.Instance.GetWhere(new { sitecode = sitecode }).ToList();
                    if (lblist.Count != 0)
                    {
                        for (int i = 0; i < lblist.Count; i++)
                        {
                            if (lblist[i].layoutmap != null && lblist[i].layoutmap.Length > 0)
                            {
                                MemoryStream stream = new MemoryStream();
                                stream.Write(lblist[i].layoutmap, 0, lblist[i].layoutmap.Length);

                                Bitmap bitMap = new Bitmap(stream);
                                bitMap.Save(laytoutmapPath + "\\" + lblist[i].layoutmapname + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                    }

                    #endregion


                    #region 生成测线信息表


                   

                    DataTable dtline = LineBll.Instance.GetDataTable("select * from t_obslinetb where sitecode ='" + sitecode + "'");


                    foreach (DataRow dr in dt.Rows)
                    {
                        string linecode = dr["OBSLINECODE"].ToString();
                        DataTable dtmaxmindata = LineObsBll.Instance.GetDataTable("select max(OBVDATE) as maxdate,min(OBVDATE) as mindate from t_obsrvtntb where OBSLINECODE='" + linecode + "'");

                      
                      
                    }

                    


                    #endregion


                }
                else
                {
                    return;
                }

                //设置一个最小值  
                progressBarControl.Properties.Minimum = 0;
                //设置一个最大值  
                progressBarControl.Properties.Maximum = this.gridView.GetSelectedRows().Length;
                //设置步长，即每次增加的数  
                progressBarControl.Properties.Step = 1;

                //当前值  
                progressBarControl.Position = 0;


                for (int i = 0; i < this.gridView.GetSelectedRows().Length; i++)
                {
                    int rowindex = this.gridView.GetSelectedRows()[i];
                    DataRow row = gridView.GetDataRow(rowindex);
                    string linecode = LineBll.Instance.GetIdByName(row[0].ToString());
                    string startdate = row[1].ToString();
                    string enddate = row[2].ToString();


                    DataTable dt = LineObsBll.Instance.GetDataTable("select obvdate,obvvalue,note from t_obsrvtntb where unix_timestamp(OBVDATE) between unix_timestamp('" + startdate + "') and unix_timestamp('" + enddate + "' ) and OBSLINECODE = '" + linecode + "'");
                    if (dt.Rows.Count > 0)
                    {
                        NpoiCreator npcreator = new NpoiCreator();
                        npcreator.TemplateFile = SystemInfo.DatabaseCache;
                        npcreator.NpoiExcel(dt, SystemInfo.DatabaseCache + "/" + linecode + ".xls");
                    }
                    Application.DoEvents();
                    progressBarControl.Position += 1;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                this.DialogResult = DialogResult.No;
                XtraMessageBox.Show("下载失败：" + ex.Message, "错误");
            }

        }

        private void comboBoxEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEdit.SelectedIndex == -1)
                return;
            InitObslines((comboBoxEdit.SelectedItem as CheckedListBoxItem).Value.ToString());
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
