using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using xxkUI.Bll;
using System.Linq;

namespace xxkUI.GsProject
{
    public partial class InfoWindowContent : UserControl
    {
        public InfoWindowContent()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 填充信息框内功
        /// </summary>
        /// <param name="sitename">场地名</param>
        /// <param name="sitestatus">场地状态</param>
        /// <param name="sitetype">场地类型</param>
        /// <param name="obstype">观测类型</param>
        /// <param name="place">所在地</param>
        ///  <param name="place">所跨断裂</param>
        ///  <param name="obsunit">监测单位</param>
        /// <param name="sitesituation">场地状况</param>
        public void SetInfoWindContent(string sitecode, string sitename, string sitestatus, string sitetype, string obstype, string place, string fault,string obsunit, string sitesituation)
        {
            try
            {
                gbSite.Text = sitename + " 场地简介";
                lbSiteStatus.Text = sitestatus;
                lbSiteType.Text = sitetype;
                lbType.Text = obstype;
                lbObsUnit.Text = obsunit;
                lbPlace.Text = place;
                lbFault.Text = fault;
                lbSiteSituation.Text = sitesituation;

                //List<LayoutmapBean> rblist = LayoutmapBll.Instance.GetWhere(new { sitecode = sitecode }).ToList();
                //if (rblist.Count != 0)
                //    if (rblist[0].layoutmap != null && rblist[0].layoutmap.Length > 0)
                //    {
                //        MemoryStream ms = new MemoryStream(); //新建内存流
                //        ms.Write(rblist[0].layoutmap, 0, rblist[0].layoutmap.Length); //附值
                //        pictureBox.Image = Image.FromStream(ms); //读取流中内容
                //        this.Refresh();
                //    }

                this.gbSite.Visible = true;
                this.Height = this.gbSite.Height + 10;
                this.Width = this.gbSite.Width;
                this.gbSite.Dock = DockStyle.Fill;

            }
            catch { }
        }



    }
}
