using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraBars;
using xxkUI.Form;
using GMap.NET;
using GMap.NET.WindowsForms;
using xxkUI.Bll;
using xxkUI.Model;
using xxkUI.Tool;
using System.IO;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using xxkUI.BLL;
using DevExpress.XtraEditors.Controls;
using xxkUI.MyCls;
using System.Text.RegularExpressions;
using DevExpress.XtraTab;
using xxkUI.Controls;
using xxkUI.GsProject;
using CefSharp.WinForms;
using System.Threading;
using DevExpress.XtraMap;

namespace xxkUI
{
    public partial class frm_Main : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private XTreeList xtl;
        private List<string> userAut = new List<string>();
        private TreeBean currentClickNodeInfo;// 当前点击的树节点信息
        private TreeList currentTree;//当前树
        private List<string> importDataFiles = new List<string>();// 导入数据的文件路径集

        private bool IsEqkShow = false;// 是否显示地震目录列表
        private bool IsFaultShow = false;//是否显示断层数据
        private int pagesize = 50;// 页行数
        private int pageIndex = 1;// 当前页
        private int pageCount;// 总页数

        private string XxkFile = Application.StartupPath + "\\远程信息库缓存";//信息库路径

        public frm_Main()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 初始化主框架
        /// </summary>
        public void InitForm()
        {
            this.WindowState = FormWindowState.Maximized;//默认最大化窗体

            /**********控件的隐藏与显示*************/

            //临时隐藏360功能
            btn360Map.Visibility = BarItemVisibility.Never;

            this.currentTask.Visibility = BarItemVisibility.Never;
            //只显示地图界面
            for (int i = 0; i < this.TabControl.TabPages.Count; i++)
                this.TabControl.TabPages[i].PageVisible = this.TabControl.TabPages[i].Name == "mapTabPage" ? true : false;

            if (SystemInfo.CurrentUserInfo.UserName == "superadmin")//远程库
            {
                btnUserManager.Visibility = BarItemVisibility.Always;
                btnAdmin.Visibility = BarItemVisibility.Always;
                btnImportObsline.Visibility = BarItemVisibility.Always;
                btnUpdateSiteInfo.Visibility = BarItemVisibility.Always;
                btnAddUnit.Visibility = BarItemVisibility.Always;
                btnAddSiteInfo.Visibility = BarItemVisibility.Always;
                rbpgUser.Visible = true;
            }
            else
            {
                btnUserManager.Visibility = BarItemVisibility.Never;
                btnAdmin.Visibility = BarItemVisibility.Never;
                btnImportObsline.Visibility = BarItemVisibility.Never;
                btnUpdateSiteInfo.Visibility = BarItemVisibility.Never;

                btnAddUnit.Visibility = BarItemVisibility.Never;//新增监测单位
                btnImportBaseInfo.Visibility = BarItemVisibility.Never;//导入基础信息库
                btnAddSiteInfo.Visibility = BarItemVisibility.Never;//新增场地
                btnUpdateSiteInfo.Visibility = BarItemVisibility.Never;//更新场地
                btnDeleteSitinfo.Visibility = BarItemVisibility.Never;//删除场地

                rbpgUser.Visible = false;
            }

            currentUserBar.Caption = "当前用户：" + UnitInfoBll.Instance.GetUnitNameBy(SystemInfo.CurrentUserInfo.UserUnit) + "_" + SystemInfo.CurrentUserInfo.UserName;


            this.zoomTrackBarControl.BackColor = Color.Transparent;//将Panel设为透明
            this.zoomTrackBarControl.Value = 500;//初始位置
            this.zoomTrackBarControl.Parent = this.gMapCtrl;//将panel父控件设为背景图片控件
            //this.panel1.BringToFront();//将panel放在前面
        }

        /// <summary>
        /// 初始化信息树
        /// </summary>
        public void InitTree()
        {
            xtl = new XTreeList(this.treeListData, this.treeListManipData);
            xtl.bSignDbTree();
            xtl.bSignInitManipdbTree();
             xtl.RefreshWorkspace(true);//初始化显示新测线名称
            panelContainer2.CustomHeaderButtons[0].Properties.ToolTip = "显示旧测线名";
        }

        /// <summary>
        /// 初始化断层数据列表
        /// </summary>
        public void InitCombobox()
        {
            try
            {
                {
                    //获取管理单位数据
                    List<UnitInfoBean> uiblist = UnitInfoBll.Instance.GetAll().ToList();
                    this.cmbManageUnit.Items.Clear();

                    CheckedListBoxItem[] itemListQuery = new CheckedListBoxItem[uiblist.Count];
                    int check = 0;
                    foreach (UnitInfoBean uib in uiblist)
                    {
                        itemListQuery[check] = new CheckedListBoxItem(uib.UnitCode, uib.UnitName, false);
                        check++;
                    }
                    this.cmbManageUnit.Items.AddRange(itemListQuery);
                    this.cmbManageUnit.AllowMultiSelect = true;
                    this.cmbManageUnit.SelectAllItemVisible = true;
                    this.cmbManageUnit.SelectAllItemCaption = "全选";
                }
                {

                    //场地类型
                    this.cmbSiteType.Items.Add("流动", false);
                    this.cmbSiteType.Items.Add("定点", false);
                    this.cmbSiteType.AllowMultiSelect = true;
                    this.cmbSiteType.SelectAllItemVisible = true;
                    this.cmbSiteType.SelectAllItemCaption = "全选";
                }
                {
                    //观测型
                    this.cmbObsType.Items.Add("水准", false);
                    this.cmbObsType.Items.Add("基线", false);
                    this.cmbObsType.Items.Add("综合", false);
                    this.cmbObsType.AllowMultiSelect = true;
                    this.cmbObsType.SelectAllItemVisible = true;
                    this.cmbObsType.SelectAllItemCaption = "全选";
                }
                {
                    //省级行政区
                    List<XzqBean> xzqlist = XzqBll.Instance.GetAll().ToList();
                    this.cmbXzqh.Items.Clear();

                    CheckedListBoxItem[] xzqitemList = new CheckedListBoxItem[xzqlist.Count];
                    int checkxzq = 0;
                    foreach (XzqBean uib in xzqlist)
                    {
                        xzqitemList[checkxzq] = new CheckedListBoxItem(uib.Xzqcode, uib.Xzqname, false);
                        checkxzq++;
                    }
                    this.cmbXzqh.Items.AddRange(xzqitemList);
                    this.cmbXzqh.AllowMultiSelect = true;
                    this.cmbXzqh.SelectAllItemVisible = true;
                    this.cmbXzqh.SelectAllItemCaption = "全选";
                }

                {
                    //场地运行状况
                    CheckedListBoxItem[] statusitemList = new CheckedListBoxItem[3];
                    statusitemList[0] = new CheckedListBoxItem("运行中", "运行中", false);
                    statusitemList[1] = new CheckedListBoxItem("停测", "停测", false);
                    statusitemList[2] = new CheckedListBoxItem("改造中", "改造中", false);
                    this.cmbSiteStatus.Items.AddRange(statusitemList);
                    this.cmbXzqh.AllowMultiSelect = true;
                    this.cmbSiteStatus.SelectAllItemVisible = true;
                    this.cmbSiteStatus.SelectAllItemCaption = "全选";
                }
                {

                    //活动性质
                    DataTable dt = FaultBll.Instance.GetDataTable("select activitynature from t_fault group by activitynature");
                    this.checkcmbActivitynature.Items.Clear();

                    List<CheckedListBoxItem> itemListQuery = new List<CheckedListBoxItem>();

                    foreach (DataRow uib in dt.Rows)
                    {
                        if (string.IsNullOrEmpty(uib[0].ToString()))
                            continue;
                        CheckedListBoxItem clbi = new CheckedListBoxItem(uib[0].ToString(), uib[0].ToString(), false);
                        if (itemListQuery.Find(n => n.Description == uib[0].ToString()) == null)
                            itemListQuery.Add(clbi);
                    }
                    this.checkcmbActivitynature.Items.AddRange(itemListQuery.ToArray());
                    this.checkcmbActivitynature.AllowMultiSelect = true;
                    this.checkcmbActivitynature.SelectAllItemVisible = true;
                    this.checkcmbActivitynature.SelectAllItemCaption = "全选";
                }
                {
                    //形成年代
                    DataTable dt = FaultBll.Instance.GetDataTable("select faultage from t_fault group by faultage");
                    this.checkcmbFaultage.Items.Clear();

                    List<CheckedListBoxItem> itemListQuery = new List<CheckedListBoxItem>();

                    foreach (DataRow uib in dt.Rows)
                    {
                        if (string.IsNullOrEmpty(uib[0].ToString()))
                            continue;
                        CheckedListBoxItem clbi = new CheckedListBoxItem(uib[0].ToString(), uib[0].ToString(), false);
                        if (itemListQuery.Find(n => n.Description == uib[0].ToString()) == null)
                            itemListQuery.Add(clbi);
                    }
                    this.checkcmbFaultage.Items.AddRange(itemListQuery.ToArray());
                    this.checkcmbFaultage.AllowMultiSelect = true;
                    this.checkcmbFaultage.SelectAllItemVisible = true;
                    this.checkcmbFaultage.SelectAllItemCaption = "全选";

                }
                {
                    //形成年代
                    DataTable dt = FaultBll.Instance.GetDataTable("select newestactivetime from t_fault group by newestactivetime");
                    this.checkcmbNewestactivetime.Items.Clear();

                    List<CheckedListBoxItem> itemListQuery = new List<CheckedListBoxItem>();

                    foreach (DataRow uib in dt.Rows)
                    {
                        if (string.IsNullOrEmpty(uib[0].ToString()))
                            continue;

                        //string newacttmname = "";
                        //if (uib[0].ToString() == "N") newacttmname = "晚第三纪";
                        //if (uib[0].ToString() == "Q") newacttmname = "第四纪";
                        //if (uib[0].ToString() == "Q1"|| uib[0].ToString() == "Q2"|| uib[0].ToString() == "Q3"|| uib[0].ToString() == "Q2-Q3") newacttmname = "更新世";
                        //if (uib[0].ToString() == "Q4" ||uib[0].ToString() == "Q3-Q4") newacttmname = "全新世";

                        CheckedListBoxItem clbi = new CheckedListBoxItem(uib[0].ToString(), uib[0].ToString(), false);
                        if (itemListQuery.Find(n => n.Description == uib[0].ToString()) == null)
                            itemListQuery.Add(clbi);
                    }
                    this.checkcmbNewestactivetime.Items.AddRange(itemListQuery.ToArray());
                    this.checkcmbNewestactivetime.AllowMultiSelect = true;
                    this.checkcmbNewestactivetime.SelectAllItemVisible = true;
                    this.checkcmbNewestactivetime.SelectAllItemCaption = "全选";
                }
                {
                    //历史地震
                    List<CheckedListBoxItem> itemListQuery = new List<CheckedListBoxItem>();
                    this.checkcmbHistoryEql.Items.Clear();
                    itemListQuery.Add(new CheckedListBoxItem("有", "有", false));
                    itemListQuery.Add(new CheckedListBoxItem("无", "无", false));
                    this.checkcmbHistoryEql.Items.AddRange(itemListQuery.ToArray());
                    this.checkcmbHistoryEql.AllowMultiSelect = true;
                    this.checkcmbHistoryEql.SelectAllItemVisible = true;
                    this.checkcmbHistoryEql.SelectAllItemCaption = "全选";
                }

            }
            catch (Exception ex)
            {

                XtraMessageBox.Show("初始化列表数据发生错误：" + ex.Message, "错误");
            }
        }

        /// <summary>
        /// 设置界面风格
        /// </summary>
        public void InitStyle()
        {
            defaultLookAndFeel.LookAndFeel.SkinName = "Office 2010 Blue";//蓝色风格
        }

        #region 地图事件 刘文龙


        #region 地图工具


        private void zoomTrackBarControl_EditValueChanged(object sender, EventArgs e)
        {
            gMapCtrl.Zoom = zoomTrackBarControl.Value / 100.0;
        }

        private void zoomTrackBarControl1_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            gMapCtrl.Zoom = zoomTrackBarControl.Value / 100.0;
        }

        //地图分辨率改变滑动条位置
        private void gMapCtrl_OnMapZoomChanged()
        {
            if (gMapCtrl.Zoom >= 3 && gMapCtrl.Zoom <= 18)
            {
                zoomTrackBarControl.Value = (int)(gMapCtrl.Zoom * 100.0);
            }
            if (gMapCtrl.Zoom < 3)
            {
                gMapCtrl.Zoom = 3;
            }
            if (gMapCtrl.Zoom > 18)
            {
                gMapCtrl.Zoom = 18;
            }
        }

        private void btnRoadMap_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => { MyGMap.InitMap(this.gMapCtrl, MapType.RoadMap, false); });
            thread.Start();
        }

        private void btnSatMap_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => { MyGMap.InitMap(this.gMapCtrl, MapType.SatelliteMap, false); });
            thread.Start();
        }

        #endregion


        /// <summary>
        /// 地图加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gMapCtrl_Load(object sender, EventArgs e)
        {
            if (MyGMap.InitMap(this.gMapCtrl, MapType.RoadMap, true))
            {
                mapBackgroundWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// 双击放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gMapCtrl_DoubleClick(object sender, EventArgs e)
        {
            MyGMap.Zoom(1, this.gMapCtrl);
        }

        /// <summary>
        /// 进入Marker
        /// </summary>
        /// <param name="item"></param>
        private void gMapCtrl_OnMarkerEnter(GMapMarker item)
        {
            try
            {
                if (item is GMapMarker)
                {
                    SiteBean sb = (SiteBean)item.Tag;
                    if (sb != null)
                    {
                        InfoWindow infowindow = new InfoWindow(item);
                        infowindow.Offset = new Point(-40, -40);
                        item.ToolTip = infowindow;
                    }
                }
            }
            catch (Exception ex)

            {
                XtraMessageBox.Show(ex.Message, "错误");
            }
        }

        /// <summary>
        /// 鼠标单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gMapCtrl_MouseDown(object sender, MouseEventArgs e)
        {

        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gMapCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            PointLatLng latLng = MyGMap.FromLocalToLatLng(e.X, e.Y, this.gMapCtrl);

            this.currentLocation.Caption = string.Format("经度：{0}, 纬度：{1} ", latLng.Lng, latLng.Lat);


        }

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gMapCtrl_MouseUp(object sender, MouseEventArgs e)
        {

        }




        #endregion


        #region 地震目录查询

        private void btnEventOnMap_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {

                case "btnEqkSearch":
                    {
                        try
                        {
                            this.currentTask.Visibility = BarItemVisibility.Always;
                            string sqlwhere = GetSqlWhere();
                            if (sqlwhere != string.Empty)
                            {
                                BindPageGridList(sqlwhere);
                            }

                            else
                                throw new Exception("不是有效的查询语句");
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("查询失败：" + ex.Message, "错误");
                        }
                        finally
                        {
                            this.currentTask.Visibility = BarItemVisibility.Never;
                        }
                    }
                    break;
                case "btnClearEqk":
                    {
                        this.gridControlEqklist.DataSource = null;
                        MyGMap.ClearAllEqkMarker(this.gMapCtrl);

                        IsEqkShow = false;
                        ChangePanelContainerItemVisible();
                    }
                    break;
                case "btnClearFault":
                    {
                        this.gridControlFaultlist.DataSource = null;
                        MyGMap.ClearFault(this.gMapCtrl);
                        IsFaultShow = false;
                        ChangePanelContainerItemVisible();
                    }
                    break;
                case "btnSearchSite":
                    {

                        this.currentTask.Visibility = BarItemVisibility.Always;
                        try
                        {
                            //管理单位
                            List<string> manageUnitSelected = new List<string>();
                            for (int i = 0; i < cmbManageUnit.Items.Count; i++)
                                if (cmbManageUnit.Items[i].CheckState == CheckState.Checked)
                                    manageUnitSelected.Add(cmbManageUnit.Items[i].Value.ToString());

                            //场地类型
                            List<string> siteTypeSelected = new List<string>();
                            for (int i = 0; i < cmbSiteType.Items.Count; i++)
                                if (cmbSiteType.Items[i].CheckState == CheckState.Checked)
                                    siteTypeSelected.Add(cmbSiteType.Items[i].Value.ToString());

                            //观测类型
                            List<string> obstypeSelected = new List<string>();
                            for (int i = 0; i < cmbObsType.Items.Count; i++)
                                if (cmbObsType.Items[i].CheckState == CheckState.Checked)
                                    obstypeSelected.Add(cmbObsType.Items[i].Value.ToString());



                            //所属断裂
                            List<string> faultSelected = new List<string>();
                            for (int i = 0; i < cmbBelongFault.Items.Count; i++)
                                if (cmbBelongFault.Items[i].CheckState == CheckState.Checked)
                                    faultSelected.Add(cmbBelongFault.Items[i].Value.ToString());


                            //场地状况
                            List<string> siteStausSelected = new List<string>();
                            for (int i = 0; i < cmbSiteStatus.Items.Count; i++)
                                if (cmbSiteStatus.Items[i].CheckState == CheckState.Checked)
                                    siteStausSelected.Add(cmbSiteStatus.Items[i].Value.ToString());

                            if (manageUnitSelected.Count == 0 && siteTypeSelected.Count == 0
                                && obstypeSelected.Count == 0 && faultSelected.Count == 0 && siteStausSelected.Count == 0)
                            {
                                MyGMap.ClearAllSiteMarker(this.gMapCtrl);
                                // MyGMap.AddAllSiteMarker(this.gMapCtrl);
                                this.mapBackgroundWorker.RunWorkerAsync();
                                return;
                            }
                            string finalwhere = "where 1=1 ";

                            string whereUnitcode = " and UNITCODE in (";
                            for (int i = 0; i < manageUnitSelected.Count; i++)
                                whereUnitcode += "'" + manageUnitSelected[i] + "',";
                            whereUnitcode = whereUnitcode.Substring(0, whereUnitcode.Length - 1) + ")";
                            if (manageUnitSelected.Count > 0)
                                finalwhere = finalwhere + whereUnitcode;

                            string whereSitetype = " and SITETYPE in (";
                            for (int i = 0; i < siteTypeSelected.Count; i++)
                            {
                                whereSitetype += "'" + siteTypeSelected[i] + "',";
                            }
                            whereSitetype = whereSitetype.Substring(0, whereSitetype.Length - 1) + ")";
                            if (siteTypeSelected.Count > 0)
                                finalwhere = finalwhere + whereSitetype;

                            string whereObstype = " and OBSTYPE in (";
                            for (int i = 0; i < obstypeSelected.Count; i++)
                            {
                                whereObstype += "'" + obstypeSelected[i] + "',";
                            }
                            whereObstype = whereObstype.Substring(0, whereObstype.Length - 1) + ")";

                            if (obstypeSelected.Count > 0)
                                finalwhere = finalwhere + whereObstype;

                            string whereStatus = " and SITESTATUS in (";
                            for (int i = 0; i < siteStausSelected.Count; i++)
                                whereStatus += "'" + siteStausSelected[i] + "',";
                            whereStatus = whereStatus.Substring(0, whereStatus.Length - 1) + ")";
                            if (siteStausSelected.Count > 0)
                                finalwhere = finalwhere + whereStatus;

                            MyGMap.ClearAllSiteMarker(this.gMapCtrl);



                            DataTable dt = SiteBll.Instance.GetDataTable("select sitecode,sitename,sitetype,latitude,longtitude,sitestatus,obstype, place,faultcode, obsunit, sitesituation  from t_siteinfodb " + finalwhere);
                            List<SiteBean> sbEnumt = new List<SiteBean>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                SiteBean sb = new SiteBean();
                                sb.SiteCode = dr["sitecode"].ToString();
                                sb.SiteName = dr["sitename"].ToString();
                                sb.ObsType = dr["obstype"].ToString();
                                sb.Latitude = double.Parse(dr["latitude"].ToString());
                                sb.Longtitude = double.Parse(dr["longtitude"].ToString());
                                sb.SiteType = dr["sitetype"].ToString();
                                sb.SiteSituation = dr["sitesituation"].ToString();
                                sb.Place = dr["place"].ToString();
                                sb.FaultCode = dr["faultcode"].ToString();
                                sb.ObsUnit = dr["obsunit"].ToString();
                                sb.SiteStatus = dr["sitestatus"].ToString();
                                sbEnumt.Add(sb);
                            }
                            MyGMap.AddSiteMarker(sbEnumt, this.gMapCtrl);

                            //this.mapBackgroundWorker.RunWorkerAsync();
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "错误");
                        }

                        finally
                        {
                            this.currentTask.Visibility = BarItemVisibility.Never;
                        }
                    }
                    break;
                case "btnSearchFault":
                    {

                        try
                        {

                            this.currentTask.Visibility = BarItemVisibility.Always;
                            //活动性质
                            List<string> cmbActivitynatureSelected = new List<string>();
                            for (int i = 0; i < checkcmbActivitynature.Items.Count; i++)
                                if (checkcmbActivitynature.Items[i].CheckState == CheckState.Checked)
                                    cmbActivitynatureSelected.Add(checkcmbActivitynature.Items[i].Value.ToString());

                            //形成年代
                            List<string> FaultageSelected = new List<string>();
                            for (int i = 0; i < checkcmbFaultage.Items.Count; i++)
                                if (checkcmbFaultage.Items[i].CheckState == CheckState.Checked)
                                    FaultageSelected.Add(checkcmbFaultage.Items[i].Value.ToString());

                            //观测类型
                            List<string> newestactivetimeSelected = new List<string>();
                            for (int i = 0; i < checkcmbNewestactivetime.Items.Count; i++)
                                if (checkcmbNewestactivetime.Items[i].CheckState == CheckState.Checked)
                                    newestactivetimeSelected.Add(checkcmbNewestactivetime.Items[i].Value.ToString());

                            //历史震例
                            List<string> hiseqkSelected = new List<string>();
                            for (int i = 0; i < checkcmbHistoryEql.Items.Count; i++)
                                if (checkcmbHistoryEql.Items[i].CheckState == CheckState.Checked)
                                    hiseqkSelected.Add(checkcmbHistoryEql.Items[i].Value.ToString());


                            if (cmbActivitynatureSelected.Count == 0 && FaultageSelected.Count == 0
                                    && newestactivetimeSelected.Count == 0 && hiseqkSelected.Count == 0)
                            {

                                List<FaultBean> ftlist = FaultBll.Instance.GetAll().ToList();

                                if (ftlist.Count() > 0)
                                {
                                    this.TabControl.SelectedTabPage = this.mapTabPage;
                                    IsFaultShow = true;
                                    ChangePanelContainerItemVisible();
                                    ModelHandler<FaultBean> mh = new ModelHandler<FaultBean>();

                                    DataTable faultShowData = mh.FillDataTable(ftlist);

                                    this.gridControlFaultlist.DataSource = faultShowData;
                                    this.gridControlFaultlist.Refresh();

                                    MyGMap.ClearFault(this.gMapCtrl);
                                    MyGMap.AddFaultMap(ftlist, this.gMapCtrl);
                                }
                                return;
                            }
                            string finalwhere = "where 1=1 ";

                            string whereactivitynature = " and activitynature in (";
                            for (int i = 0; i < cmbActivitynatureSelected.Count; i++)
                                whereactivitynature += "'" + cmbActivitynatureSelected[i] + "',";
                            whereactivitynature = whereactivitynature.Substring(0, whereactivitynature.Length - 1) + ")";
                            if (cmbActivitynatureSelected.Count > 0)
                                finalwhere = finalwhere + whereactivitynature;

                            string wherefaultage = " and faultage in (";
                            for (int i = 0; i < FaultageSelected.Count; i++)
                            {
                                wherefaultage += "'" + FaultageSelected[i] + "',";
                            }
                            wherefaultage = wherefaultage.Substring(0, wherefaultage.Length - 1) + ")";
                            if (FaultageSelected.Count > 0)
                                finalwhere = finalwhere + wherefaultage;

                            string wherenewestactivetime = " and newestactivetime in (";
                            for (int i = 0; i < newestactivetimeSelected.Count; i++)
                                wherenewestactivetime += "'" + newestactivetimeSelected[i] + "',";
                            wherenewestactivetime = wherenewestactivetime.Substring(0, wherenewestactivetime.Length - 1) + ")";
                            if (newestactivetimeSelected.Count > 0)
                                finalwhere = finalwhere + wherenewestactivetime;

                            string eqkactivity = " and SUBSTR(seismicactivity FROM 1)";
                            if (hiseqkSelected.Count == 2 || hiseqkSelected.Count == 0)
                                eqkactivity = "";
                            else if (hiseqkSelected.Count == 1)
                            {
                                if (hiseqkSelected[0].ToString() == "有")
                                    eqkactivity += "!=  ''";
                                else
                                    eqkactivity += "=  ''";

                                finalwhere = finalwhere + eqkactivity;
                            }

                            List<FaultBean> faultDataList = FaultBll.Instance.GetList("select * from t_fault " + finalwhere).ToList();

                            if (faultDataList.Count() > 0)
                            {
                                this.TabControl.SelectedTabPage = this.mapTabPage;
                                IsFaultShow = true;
                                ChangePanelContainerItemVisible();
                                this.gridControlFaultlist.DataSource = faultDataList;
                                this.gridControlFaultlist.Refresh();
                                MyGMap.ClearFault(this.gMapCtrl);
                                MyGMap.AddFaultMap(faultDataList, this.gMapCtrl);

                            }
                        }
                        catch (Exception ex)
                        {

                            XtraMessageBox.Show(ex.Message, "错误");

                        }
                        finally
                        {
                            this.currentTask.Visibility = BarItemVisibility.Never;
                        }

                    }
                    break;
            }
        }

        #endregion


        /// <summary>
        /// 控制dockPanel的显示
        /// </summary>
        /// <param name="controlname"></param>
        private void ChangePanelContainerItemVisible()
        {
            try
            {
                //if (this.TabControl.SelectedTabPage.Name == "chartTabPage")
                //{
                //    this.ribbon.SelectedPage = ribbonPageTchartTool;
                //}
                //else 
                if (this.TabControl.SelectedTabPage.Name == "mapTabPage")
                {
                    if (IsEqkShow)
                    {
                        if (this.dockPanelEqkCatalog.Visibility != DevExpress.XtraBars.Docking.DockVisibility.Visible)
                            this.dockPanelEqkCatalog.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                    }
                    else
                    {

                        if (this.dockPanelEqkCatalog.Visibility != DevExpress.XtraBars.Docking.DockVisibility.Hidden)
                            this.dockPanelEqkCatalog.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                    }

                    if (IsFaultShow)
                    {
                        if (this.dockPanelFaultCatalog.Visibility != DevExpress.XtraBars.Docking.DockVisibility.Visible)
                            this.dockPanelFaultCatalog.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Visible;
                    }
                    else
                    {
                        if (this.dockPanelFaultCatalog.Visibility != DevExpress.XtraBars.Docking.DockVisibility.Hidden)
                            this.dockPanelFaultCatalog.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                    }

                    this.ribbon.SelectedPage = ribbonPageMapTool;
                }
                else if (this.TabControl.SelectedTabPage.Name == "recycleTabPage")
                {
                    if (this.dockPanelEqkCatalog.Visibility != DevExpress.XtraBars.Docking.DockVisibility.Hidden)
                        this.dockPanelEqkCatalog.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;

                }
                else if (this.TabControl.SelectedTabPage.Name == "layoutmapTabpage")
                {
                    if (this.dockPanelEqkCatalog.Visibility != DevExpress.XtraBars.Docking.DockVisibility.Hidden)
                        this.dockPanelEqkCatalog.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;

                }

                this.ribbon.SelectedPage.Visible = true;


            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }

        /// <summary>
        /// 树列表右击菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tree_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                TreeList tree = sender as TreeList;
                currentTree = tree;
                if ((e.Button == MouseButtons.Right) && (ModifierKeys == Keys.None) && (tree.State == TreeListState.Regular))
                {
                    Point p = new Point(Cursor.Position.X, Cursor.Position.Y);
                    TreeListHitInfo hitInfo = tree.CalcHitInfo(e.Location);
                    if (hitInfo.HitInfoType == HitInfoType.Cell)
                    {
                        tree.SetFocusedNode(hitInfo.Node);

                        if (tree.Name == "treeListData")//信息库树
                        {
                            currentClickNodeInfo = tree.GetDataRecordByNode(hitInfo.Node) as TreeBean;
                            if (currentClickNodeInfo == null)
                            {
                                return;
                            }
                            if (hitInfo.Node.Level == 0)//省局节点
                            {
                                popupMenuZoomto.ShowPopup(p);
                            }
                            if (hitInfo.Node.Level == 1)
                            {
                                popRemoteSiteTree.ShowPopup(p);
                            }
                            else if (hitInfo.Node.Level == 2)
                            {
                                popRemoteLineTree.ShowPopup(p);
                            }
                        }
                        if (tree.Name == "treeListManipData")//处理数据
                        {

                            popupMenuHandleTree.ShowPopup(p);


                        }

                    }
                }


            }

            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");

            }

        }

        /// <summary>
        /// treelist双击事件，显示曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                TreeList tree = sender as TreeList;
                MouseEventArgs me = e as MouseEventArgs;

                TreeListHitInfo hitInfo = tree.CalcHitInfo(me.Location);
                if (hitInfo.HitInfoType == HitInfoType.Cell)
                {
                    tree.SetFocusedNode(hitInfo.Node);

                    if (tree.Name == "treeListData")//信息库树
                    {
                        currentClickNodeInfo = tree.GetDataRecordByNode(hitInfo.Node) as TreeBean;
                        if (currentClickNodeInfo == null)
                        {
                            return;
                        }
                        if (hitInfo.Node.Level == 1)//场地
                        {

                        }
                        if (hitInfo.Node.Level == 2)//测线
                        {
                            LineBean tag = currentClickNodeInfo.Tag as LineBean;

                            AddSeriesToChart(new List<LineBean>() { tag }, XxkFile);
                        }

                    }
                    if (tree.Name == "treeListManipData")//处理数据
                    {
                        AddSeriesToChart(new List<string>() { hitInfo.Node.GetDisplayText(0) }, SystemInfo.HandleDataCache);
                    }

                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

            ChangePanelContainerItemVisible();
        }

        private void AddSeriesToChart<T>(List<T> checkednode, string dfp)
        {
            frm_SplashScreen sdf = new frm_SplashScreen("提示", "正在加载......", "请耐心等候，正在加载数据！", checkednode.Count);
            try
            {
                Type t = typeof(T);
                if (t.Name == "LineBean")
                {
                    List<string> hashKeys = new List<string>();
                    List<string> obslinecodes = new List<string>();

                    int i = 0;
                    foreach (LineBean checkedLb in checkednode as List<LineBean>)
                    {
                        obslinecodes.Add(checkedLb.OBSLINECODE);
                        hashKeys.Add(ObsdataCls.GetKeyStr(dfp, checkedLb.OBSLINECODE));
                        i++;
                        sdf.SetCaption("执行进度（" + i.ToString() + "/" + checkednode.Count.ToString() + "）");

                    }
                    ObsdataCls.LoadData(obslinecodes, dfp);

                    this.tChartControl.AddSeries(hashKeys, dfp, false);
                }
                else if (t.Name == "String")
                {
                    List<string> hashKeys = new List<string>();

                    int i = 0;


                    foreach (string checkedLb in checkednode as List<string>)
                    {
                        //解析哈希表key
                        string[] dirName = dfp.Split('\\');
                        string DtKey = dirName[dirName.Length - 1] + "," + checkedLb.ToString();
                        hashKeys.Add(DtKey);
                        i++;
                        sdf.SetCaption("执行进度（" + i.ToString() + "/" + checkednode.Count.ToString() + "）");

                    }

                    this.tChartControl.AddSeries2(hashKeys, dfp);
                }

                this.chartTabPage.PageVisible = true;//曲线图页面可见
                this.TabControl.SelectedTabPage = this.chartTabPage;
                //跳转至菜单栏
                this.Ribbon.SelectedPage = ribbonPageTchartTool;


            }
            catch (Exception ex)
            { }

            sdf.Close();
        }


        /// <summary>
        /// 菜单项点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popMenuRemote_ItemClick(object sender, ItemClickEventArgs e)
        {

            switch (e.Item.Name)
            {
                case "btnChart"://趋势图
                    {
                        if (currentTree.Name == this.treeListData.Name)
                        {
                            AddSeriesToChart(xtl.GetCheckedLine(currentTree.Name), SystemInfo.DatabaseCache);
                        }
                        else if (currentTree.Name == this.treeListManipData.Name)
                        {
                            AddSeriesToChart(xtl.GetCheckedLineOnMuniTree(currentTree.Name), SystemInfo.HandleDataCache);
                        }
                    }
                    break;
                case "btnSiteLocation"://定位到地图
                    this.TabControl.SelectedTabPage = this.mapTabPage;
                    this.Ribbon.SelectedPage = ribbonPageMapTool;
                    MyGMap.ZoomToSite(SiteBll.Instance.GetWhere(new { SITECODE = currentClickNodeInfo.Tag.ToString() }).ToList()[0], this.gMapCtrl);
                    break;

                case "btn360Map"://全景地图
                    {
                        this.xtraTabPage360.Text = SiteBll.Instance.GetSitenameByID(currentClickNodeInfo.Tag.ToString()) + "场地全景图";
                        this.xtraTabPage360.PageVisible = true;

                        string sitecode = currentClickNodeInfo.Tag.ToString();
                        sitecode = "test";
                        string vrUrl = "http://127.0.0.1:8022/panoramaPage.html?key=" + sitecode;
                        ChromiumWebBrowser browser = new ChromiumWebBrowser(vrUrl);
                        Font font = new Font("微软雅黑", 10.5f);
                        this.xtraTabPage360.Controls.Add(browser);
                        browser.Font = font;
                        browser.Dock = DockStyle.Fill;
                        this.TabControl.SelectedTabPage = this.xtraTabPage360;
                    }
                    break;
                case "btnSiteInfo"://信息库
                    {
                        try
                        {
                            frm_SplashScreen sdf = new frm_SplashScreen("提示", "正在加载......", "请耐心等候，正在加载数据！", 1);
                            SiteBean sb = SiteBll.Instance.GetWhere(new { SITECODE = currentClickNodeInfo.Tag.ToString() }).ToList()[0];
                            //this.siteInfoDocCtrl1.LoadDocument(Application.StartupPath + "/文档缓存/信息库模板.doc");
                            //this.siteInfoDocCtrl1.FillBookMarkText(sb);
                            if (this.siteInfoPdfCtrl.SetPdfFilePath(Application.StartupPath + "\\文档缓存\\信息库模板.pdf"))
                            {
                                this.siteInfoPdfCtrl.WriteContent(sb);
                                this.siteInfoPdfCtrl.OpenFile();
                                this.siteInfoTabPage.PageVisible = true;
                                this.TabControl.SelectedTabPage = this.siteInfoTabPage;
                                sdf.SetCaption("执行进度（" + 1.ToString() + "/" + 1.ToString() + "）");
                            }
                            else
                            {
                                XtraMessageBox.Show("系统缺少 信息库木板.pdf 文件", "错误");
                            }
                            sdf.Close();

                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("发生错误：" + ex.Message, "错误");
                        }
                    }
                    break;

                case "btnUpdateSiteInfo"://更新信息库
                    {
                        string unitcode = currentClickNodeInfo.ParentFieldName;
                        if (!IsUserHasAth(unitcode))
                        {
                            XtraMessageBox.Show("当前用户无该场地权限，请向管理部门申请相应权限再执行该操作！", "提示");
                            return;
                        }

                        this.addXxkTabPage.PageVisible = true;
                        this.TabControl.SelectedTabPage = this.addXxkTabPage;
                        SetBaseinfoVGridControl();

                        SiteBean sb = new SiteBean();

                        sb = SiteBll.Instance.GetWhere(new { sitecode = currentClickNodeInfo.Tag.ToString() }).ToList()[0];

                        this.addXxkTabPage.Text = "更新信息库";
                        this.gCXXkAdd.Text = "更新信息库表单";
                        this.btnXxkAdd.Text = "更新至数据库";
                        this.btnXxkAdd.Enabled = true;
                        SetSiteValueVGridControl(sb, false);
                    }
                    break;
                case "btnDeleteSitinfo"://删除信息库
                    {

                        if (!(XtraMessageBox.Show("该操作将级联删除该场地基础信息库、场地图、卫星图、测线信息和观测数据等信息，确定执行该操作？", "提示") == DialogResult.OK))
                            return;

                        string unitcode = currentClickNodeInfo.ParentFieldName;
                        string sitecode = currentClickNodeInfo.Tag.ToString();

                        if (!IsUserHasAth(unitcode))
                        {
                            XtraMessageBox.Show("当前用户无该场地权限，请向管理部门申请相应权限再执行该操作！", "提示");
                            return;
                        }
                        try
                        {
                            SiteBll.Instance.Delete(new { sitecode = sitecode });

                            //级联删除
                            RemotemapBll.Instance.Delete(new { sitecode = sitecode });
                            LayoutmapBll.Instance.Delete(new { sitecode = sitecode });

                            List<LineBean> lblist = LineBll.Instance.GetWhere(new { sitecode = sitecode }).ToList();
                            if (lblist.Count > 0)
                            {
                                for (int i = 0; i < lblist.Count; i++)
                                {
                                    LineObsBll.Instance.Delete(new { obslinecode = lblist[i].OBSLINECODE });
                                }
                                LineBll.Instance.Delete(new { sitecode = sitecode });
                            }


                            XtraMessageBox.Show("删除成功", "成功");

                            xtl.bSignDbTree();

                             xtl.RefreshWorkspace(true);
                            MyGMap.ClearAllSiteMarker(this.gMapCtrl);
                            //MyGMap.AddAllSiteMarker(this.gMapCtrl);
                            this.mapBackgroundWorker.RunWorkerAsync();
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("删除失败：" + ex.Message, "错误");
                        }
                    }
                    break;
                case "btnDownloadSitinfo"://下载信息库
                    {
                        string unitcode = currentClickNodeInfo.ParentFieldName;

                        if (!IsUserHasAth(unitcode))
                        {
                            XtraMessageBox.Show("当前用户无该场地权限，请向管理部门申请相应权限再执行该操作！", "提示");
                            return;
                        }
                        try
                        {
                            frm_Download fd = new frm_Download(unitcode);
                            if (fd.ShowDialog() == DialogResult.OK)
                            {

                                 xtl.RefreshWorkspace(true);
                            }
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("下载失败：" + ex.Message, "错误");
                        }

                    }
                    break;
                case "btnImportObsline"://导入观测数据
                    {

                        try
                        {
                            OpenFileDialog ofd = new OpenFileDialog();
                            ofd.Multiselect = true;//可多选
                            ofd.Filter = "Excel文件|*.xls;*.xlsx;";
                            if (ofd.ShowDialog(this) == DialogResult.OK)
                            {
                                importDataFiles = ofd.FileNames.ToList();
                                frm_Progress ptPro = new frm_Progress();
                                ptPro.Show(this);
                                ptPro.progressWorker.DoWork += ImportData_DoWork;
                                ptPro.beginWorking();
                                ptPro.progressWorker.RunWorkerCompleted += ImportData_RunWorkerCompleted;
                            }
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("导入失败:" + ex.Message, "错误");
                        }

                    }
                    break;
                case "btnDownLoad"://加载测线节点
                    {
                        frm_SplashScreen sdf = null;
                        try
                        {
                            List<SiteBean> checkedNodes = xtl.GetCheckedSite(this.treeListData.Name);
                            sdf = new frm_SplashScreen("提示", "正在加载......", "请耐心等候，正在加载数据！", checkedNodes.Count);

                            int i = 0;
                            foreach (SiteBean checkedSb in checkedNodes)
                            {
                                List<string> aulist = SystemInfo.CurrentUserInfo.UserAthrty.Split(';').ToList();
                                if (!aulist.Contains(checkedSb.UnitCode))
                                {
                                    sdf.SetCaption("当前用户没有" + checkedSb.SiteName + "场地的下载权限");
                                    continue;
                                }

                                DataTable linecode = LineObsBll.Instance.GetDataTable("select obslinecode,obslinename from t_obslinetb where SITECODE = '" + checkedSb.SiteCode + "'");
                                foreach (DataRow row in linecode.Rows)
                                {
                                    string lCode = row[0].ToString();
                                    string lName = row[1].ToString();

                                    DataTable dt = LineObsBll.Instance.GetDataTable("select DATE_FORMAT(obvdate,'%Y-%m-%d') as obvdate,obvvalue,note from t_obsrvtntb where OBSLINECODE = '" + lCode + "'");
                                    if (dt.Rows.Count > 0)
                                    {
                                        NpoiCreator npcreator = new NpoiCreator();
                                        npcreator.TemplateFile = SystemInfo.DatabaseCache;
                                        npcreator.NpoiExcel(dt, SystemInfo.DatabaseCache + "/" + lCode + ".xls");
                                    }
                                }
                                i++;
                                sdf.SetCaption("执行进度（" + i.ToString() + "/" + checkedNodes.Count.ToString() + "）");
                            }

                             xtl.RefreshWorkspace(true);

                        }
                        catch (Exception ex)
                        {

                            XtraMessageBox.Show("下载数据失败：" + ex.Message, "错误");
                        }
                        finally
                        {
                            sdf.Close();
                        }


                    }
                    break;
                case "btnDeleteObsline"://删除测项
                    {
                        List<LineBean> lblist = xtl.GetCheckedLine(currentTree.Name);

                        frm_SplashScreen sdf = new frm_SplashScreen("提示", "正在删除......", "请耐心等候，正在删除数据！", lblist.Count);

                        try
                        {
                            PublicHelper php = new PublicHelper();
                            if (currentTree.Name == this.treeListData.Name)
                            {
                                int i = 0;
                                foreach (LineBean lb in lblist)
                                {
                                    string sourceFilePath = SystemInfo.DatabaseCache + "\\" + lb.OBSLINECODE + ".xls";
                                    File.Delete(sourceFilePath);
                                    i++;
                                    sdf.SetCaption("执行进度（" + i.ToString() + "/" + lblist.Count.ToString() + "）");
                                }

                                xtl.bSignDbTree();
                                 xtl.RefreshWorkspace(true);

                            }
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show(ex.Message, "错误");
                        }
                        finally
                        {
                            sdf.Close();
                        }
                    }

                    break;

                case "btnObslineExport"://观测数据导出至
                    {

                        FolderBrowserDialog fbd = new FolderBrowserDialog();

                        if (fbd.ShowDialog() == DialogResult.OK)
                        {
                            List<LineBean> lblist = xtl.GetCheckedLine(currentTree.Name);

                            frm_SplashScreen sdf = new frm_SplashScreen("提示", "正在导出数据......", "请耐心等候，正在导出数据！", lblist.Count);

                            int i = 0;
                            foreach (LineBean lb in lblist)
                            {
                                string sourceFilePath = SystemInfo.DatabaseCache + "\\" + lb.OBSLINECODE + ".xls";

                                string destFileName = fbd.SelectedPath + "\\" + lb.OBSLINENAME + ".xls";
                                File.Copy(sourceFilePath, destFileName);

                                i++;
                                sdf.SetCaption("执行进度（" + i.ToString() + "/" + lblist.Count.ToString() + "）");
                            }

                            sdf.Close();
                        }

                    }
                    break;

                case "btnObslineInfo"://查看测线基础信息
                    {
                        try
                        {
                            frm_SplashScreen sdf = new frm_SplashScreen("提示", "正在加载......", "请耐心等候，正在加载数据！", 1);
                            //this.siteInfoDocCtrl1.LoadDocument(Application.StartupPath + "/文档缓存/信息库模板.doc");
                            //this.siteInfoDocCtrl1.FillBookMarkText(sb);
                            if (this.lineInfoPdfCtrl.SetPdfFilePath(Application.StartupPath + "\\文档缓存\\测线信息模板.pdf"))
                            {
                                this.lineInfoPdfCtrl.WriteContent((LineBean)currentClickNodeInfo.Tag);
                                this.lineInfoPdfCtrl.OpenFile();
                                this.lineInfoTabPage.PageVisible = true;
                                this.TabControl.SelectedTabPage = this.lineInfoTabPage;
                                sdf.SetCaption("执行进度（" + 1.ToString() + "/" + 1.ToString() + "）");
                            }
                            else
                            {
                                XtraMessageBox.Show("系统缺少 信息库木板.pdf 文件", "错误");
                            }
                            sdf.Close();

                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("发生错误：" + ex.Message, "错误");
                        }

                    }
                    break;


                    #region 灰掉
                    //case "barButtonItem2"://二测布设图批量导入（用完删除）
                    //    {
                    //        try
                    //        {
                    //            FolderBrowserDialog dialog = new FolderBrowserDialog();
                    //            dialog.Description = "请选择布设图所在文件夹";
                    //            dialog.SelectedPath = @"D:\liuwlCloud\数据\安徽绑定材料";
                    //            if (dialog.ShowDialog() == DialogResult.OK)
                    //            {
                    //                if (string.IsNullOrEmpty(dialog.SelectedPath))
                    //                {
                    //                    return;
                    //                }
                    //                DirectoryInfo TheFolder = new DirectoryInfo(dialog.SelectedPath);

                    //                //遍历文件夹
                    //                foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
                    //                {
                    //                    LayoutmapBean lymapbean = new LayoutmapBean();
                    //                    lymapbean.layoutmapcode = LayoutmapBll.Instance.CreateLayoutmapCode(LayoutmapBll.Instance.GetAll().ToList().Count);
                    //                    lymapbean.sitecode = currentClickNodeInfo.Tag.ToString();
                    //                    foreach (FileInfo NextFile in NextFolder.GetFiles())
                    //                    {
                    //                        string linename = string.Empty;
                    //                        string linecode = string.Empty;

                    //                        if (NextFile.Extension == ".xls" || NextFile.Extension == ".xlsx")
                    //                        {
                    //                            linename = Path.GetFileNameWithoutExtension(NextFile.ToString());
                    //                            linecode = LineBll.Instance.GetIdByName(linename);
                    //                            lymapbean.Bindinglines += linecode + ",";
                    //                        }
                    //                        else
                    //                        {
                    //                            lymapbean.layoutmapname = Regex.Split(NextFile.Name, NextFile.Extension, RegexOptions.IgnoreCase)[0];
                    //                            lymapbean.layoutmap = File.ReadAllBytes(NextFile.FullName);
                    //                        }
                    //                    }
                    //                    lymapbean.Bindinglines = lymapbean.Bindinglines.Substring(0, lymapbean.Bindinglines.Length - 1);
                    //                    LayoutmapBll.Instance.Add(lymapbean);

                    //                }

                    //            }
                    //            MessageBox.Show("Done");
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            MessageBox.Show(ex.Message.ToString());
                    //        }
                    //    }
                    //    break;
                    #endregion


            }
        }


        private void popMenuZoomto_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "btnZoomto"://按管理单位定位
                    {
                        this.TabControl.SelectedTabPage = this.mapTabPage;
                        this.Ribbon.SelectedPage = ribbonPageMapTool;
                        MyGMap.ZoomToSiteByUnit((UnitInfoBean)currentClickNodeInfo.Tag, this.gMapCtrl);
                    }
                    break;

                case "btnAddSiteInfo"://新增信息库
                    {

                        this.addXxkTabPage.PageVisible = true;
                        this.TabControl.SelectedTabPage = this.addXxkTabPage;

                        this.addXxkTabPage.Text = "新增信息库";
                        this.gCXXkAdd.Text = "新增信息库表单";
                        this.btnXxkAdd.Text = "上传至数据库";
                        this.btnXxkAdd.Enabled = true;

                        SetBaseinfoVGridControl();
                        SetSiteValueVGridControl(null, true);

                    }
                    break;

                case "btnAddUnit"://添加监测单位
                    {
                        if (SystemInfo.CurrentUserInfo.UserName == "superadmin")
                        {
                            frm_AddUnit auf = new frm_AddUnit();

                            if (auf.ShowDialog() == DialogResult.OK)
                            {
                                if (xtl != null)
                                {
                                    xtl.bSignDbTree();
                                     xtl.RefreshWorkspace(true);
                                }
                                else
                                {
                                    InitTree();
                                }
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("非管理员无权进行该操作！", "提示");
                        }

                    }
                    break;

                case "btnDeleteUnit"://删除监测单位
                    {
                        if (SystemInfo.CurrentUserInfo.UserName == "superadmin")
                        {
                            if (XtraMessageBox.Show("确定删除该监测单位？", "提示") == DialogResult.OK)
                            {
                                try
                                {
                                    this.treeListData.Cursor = Cursors.WaitCursor;
                                    string unitcode = currentClickNodeInfo.KeyFieldName;
                                    UnitInfoBll.Instance.Delete(new { UNITCODE = unitcode });
                                    SiteBll.Instance.Delete(new { UNITCODE = unitcode });
                                    this.treeListData.Cursor = Cursors.Arrow;
                                }
                                catch (Exception ex)
                                {
                                    XtraMessageBox.Show("删除失败：" + ex.Message, "错误");
                                }
                                finally
                                {
                                    xtl.bSignDbTree();
                                     xtl.RefreshWorkspace(true);
                                }
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("非管理员无权进行该操作！", "提示");
                        }

                    }
                    break;

                case "btnImportBaseInfo"://批量导入基础信息（场地基础信息、测线基础信息、观测数据等）
                    {
                        if (SystemInfo.CurrentUserInfo.UserName == "superadmin")
                        {
                            FolderBrowserDialog fbd = new FolderBrowserDialog();
                            fbd.SelectedPath = @"D:\工作\跨断层信息库开发\数据";
                            //fbd.RootFolder = fbd.SelectedPath;
                            if (fbd.ShowDialog() == DialogResult.OK)
                            {

                                selectedBaseInfoPath = fbd.SelectedPath;
                                frm_Progress ptPro = new frm_Progress();
                                ptPro.Show(this);
                                ptPro.progressWorker.DoWork += ImportBaseInfo_DoWork;
                                ptPro.beginWorking();
                                ptPro.progressWorker.RunWorkerCompleted += ImportBaseInfo_RunWorkerCompleted;
                            }

                        }
                        else
                        {
                            XtraMessageBox.Show("非管理员无权进行该操作！", "提示");
                        }
                    }
                    break;

                case "btnDataStatistics"://资料完整性统计
                    {
                        string unitcode = currentClickNodeInfo.KeyFieldName;

                        frm_Progress ssPro = new frm_Progress();
                        ssPro.Show(this);
                        ssPro.progressWorker.DoWork += Statistics_DoWork;
                        ssPro.beginWorking();
                        ssPro.progressWorker.RunWorkerCompleted += Statistics_RunWorkerCompleted;
                    }
                    break;
            }
        }

        private void btnHandelData_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {

                case "btnHandelDataRename"://重命名处理数据
                    {
                        List<string> selectedlines = xtl.GetSelectedLineOnMuniTree(currentTree.Name);
                        frm_RenameManipLine renamefrm = new Form.frm_RenameManipLine(selectedlines[0]);
                        if (renamefrm.ShowDialog() == DialogResult.OK)
                        {
                            string sourceDriName = SystemInfo.HandleDataCache + "\\" + selectedlines[0] + ".xls";
                            string destDriName = SystemInfo.HandleDataCache + "\\" + renamefrm.targitFileName + ".xls";
                            File.Move(sourceDriName, destDriName);
                            xtl.bSignInitManipdbTree();
                        }
                    }
                    break;
                case "btnHandelDataDelete"://删除处理数据
                    {
                        List<string> checklines = xtl.GetCheckedLineOnMuniTree(currentTree.Name);
                        frm_SplashScreen sdf = new frm_SplashScreen("提示", "正在删除......", "请耐心等候，正在删除数据！", checklines.Count);
                        int i = 0;
                        foreach (string lb in checklines)
                        {
                            string sourceFilePath = SystemInfo.HandleDataCache + "\\" + lb + ".xls";
                            File.Delete(sourceFilePath);
                            i++;
                            sdf.SetCaption("执行进度（" + i.ToString() + "/" + checklines.Count.ToString() + "）");
                        }

                        xtl.bSignInitManipdbTree();
                        sdf.Close();
                    }

                    break;
            }
        }

        #region 导入观测数据

        private void ImportData_DoWork(object sender, DoWorkEventArgs e)
        {
            string sitecode = SiteBll.Instance.GetWhere(new { SITECODE = currentClickNodeInfo.Tag.ToString() }).ToList()[0].SiteCode;

            if (importDataFiles.Count == 0 || sitecode == string.Empty)
                return;

            MyBackgroundWorker worker = (MyBackgroundWorker)sender;
            e.Cancel = false;

            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            NpoiCreator npcreator = new NpoiCreator();
            ModelHandler<LineObsBean> mhd = new ModelHandler<LineObsBean>();

            int succedCount = 0;//入库的数量
            int faildCount = 0;//失败的数量
            foreach (string file in importDataFiles)
            {
                try
                {
                    string linename = Path.GetFileNameWithoutExtension(file);
                    string linecode = string.Empty;

                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "【4  开始提示】正在处理" + linename + "数据...");

                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, " 1.正在从数据库中获取测线信息...");
                    if (LineBll.Instance.IsExist(linename))
                    {
                        linecode = LineBll.Instance.GetIdByName(linename);
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "    测线已存在！");
                    }
                    else
                    {
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "    测线不存在,正在添加测线信息...");
                        /*提取测线信息入库*/
                        LineBean lb = new LineBean();
                        lb.SITECODE = sitecode;
                        lb.OBSLINENAME = linename;
                        lb.OBSLINECODE = LineBll.Instance.GenerateLineCode(sitecode);
                        LineBll.Instance.Add(lb);
                        linecode = lb.OBSLINECODE;
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    测线信息入库成功！");
                    }

                    if (linecode != string.Empty)
                    {
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, " 2.正在提取测线观测数据...");
                        /*提取测线观测信息入库*/
                        List<LineObsBean> lineobslist = mhd.FillObsLineModel(npcreator.ExcelToDataTable_LineObs(file, true));
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    测线观测数据提取成功！");
                        foreach (LineObsBean lob in lineobslist)
                        {
                            if (!LineObsBll.Instance.IsExist(lob.obvdate.ToShortDateString(), lob.obvvalue, linecode))
                            {
                                string dtstr = lob.obvdate.ToShortDateString();
                                if (dtstr == "0001/1/1")
                                    continue;
                                LineObsBll.Instance.Add(new LineObsBean() { obslinecode = linecode, obvdate = lob.obvdate, obvvalue = lob.obvvalue, note = lob.note });
                                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     观测时间：" + lob.obvdate.ToShortDateString() + "  观测值：" + lob.obvvalue + " 已入库！");
                            }
                            else
                            {
                                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "     观测时间：" + lob.obvdate.ToShortDateString() + "  观测值：" + lob.obvvalue + " 已存在！");
                            }

                            succedCount++;
                        }
                    }
                    else
                    {
                        /*获取测线编码失败*/
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "   获取测线编码失败！");
                        faildCount++;
                    }

                }
                catch (Exception ex)
                {
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "处理中发生错误:" + ex.Message);
                }
            }

            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "【导入完成提示】此任务处理了" + (succedCount + faildCount).ToString() + "条观测记录，其中成功入库" + succedCount.ToString() + "条，失败" + faildCount.ToString() + "条！");
        }

        private void ImportData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //xtl.bSignDbTree(DataFromPath.LocalDbPath);
        }

        #endregion

        #region 导入基础信息库

        string selectedBaseInfoPath = "";//存储选中的基础信息路径
        private void ImportBaseInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            MyBackgroundWorker worker = (MyBackgroundWorker)sender;
            e.Cancel = false;

            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

            int siteSuccedCount = 0, siteFaildCount = 0;
            int remotemapSuccedCount = 0, remotemapFaildCount = 0;
            int layoutSuccedCount = 0, layoutFaildCount = 0;
            int lineinfoSuccedCount = 0, lineinfoFaildCount = 0;
            int obslineSuccedCount = 0, obslineFaildCount = 0;

            //npoi对象实例化
            NpoiCreator npcreator = new NpoiCreator();

            #region 解析各种数据路径

            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "   1.正在解析数据路径...");

            /*场地基础信息路径*/
            string siteinfoFile = selectedBaseInfoPath + "\\场地基础信息.xlsx";
            /*卫星图所在文件夹*/
            string remoteMapPath = selectedBaseInfoPath + "\\卫星图";
            /*场地图所在文件夹*/
            string layoutMapPath = selectedBaseInfoPath + "\\场地图";
            /*测线信息路径*/
            string lineinfoFile = selectedBaseInfoPath + "\\测线信息\\测线基础信息\\测线基础信息.xlsx";
            /*测线信息变更文件*/
            string linenameChgedFile = selectedBaseInfoPath + "\\测项名称变更登记表.xlsx";
            /*观测数据所在文件夹*/
            string obsdataPath = selectedBaseInfoPath + "\\测线信息\\观测数据";

            if (!File.Exists(siteinfoFile))
            {
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "找不到场地基础信息数据，请将数据补充完整再执行该操作。");
                return;
            }

            if (!File.Exists(lineinfoFile))
            {
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "找不到测线基础信息文件，请将数据补充完整再执行该操作。");
                return;
            }
            if (Directory.GetFiles(obsdataPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".xlsx") || s.EndsWith(".xls")).Count() == 0)
            {
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "找不到观测数据，请将数据补充完整再执行该操作。");
                //return;
            }


            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    数据解析完成！");

            #endregion

            #region 场地基础信息入库



            SiteBean AddSiteinfo = new SiteBean();

            try
            {

                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "   2.正在解析场地基础信息...");
                DataTable siteinfoDt = npcreator.ExportToDataTable(siteinfoFile);


                string ldordd = siteinfoDt.Rows[0]["场地类型"].ToString();

                if (ldordd == "流动")
                {
                    ldordd = "LD";
                }
                else if (ldordd == "定点")
                {
                    ldordd = "DD";
                }
                else if (ldordd == string.Empty)
                {
                    XtraMessageBox.Show("场地类型不能为空", "提示");
                    return;
                }

                AddSiteinfo.UnitCode = currentClickNodeInfo.KeyFieldName;
                AddSiteinfo.SiteCode = SiteBll.Instance.CreateNewSiteCode(ldordd);

                AddSiteinfo.SiteName = siteinfoDt.Rows[0]["场地名称"].ToString();
                AddSiteinfo.SiteType = siteinfoDt.Rows[0]["场地类型"].ToString();
                AddSiteinfo.ObsType = siteinfoDt.Rows[0]["观测类型"].ToString();
                AddSiteinfo.Historysite = siteinfoDt.Rows[0]["历史场地"].ToString();

                double lat = double.NaN, ln = double.NaN;
                double.TryParse(siteinfoDt.Rows[0]["纬度(度)"].ToString(), out lat);
                double.TryParse(siteinfoDt.Rows[0]["经度(度)"].ToString(), out ln);
                AddSiteinfo.Latitude = lat;
                AddSiteinfo.Longtitude = ln;

                AddSiteinfo.Altitude = siteinfoDt.Rows[0]["海拔高程（m）"].ToString();
                AddSiteinfo.Place = siteinfoDt.Rows[0]["所在地"].ToString();
                AddSiteinfo.FaultCode = siteinfoDt.Rows[0]["所跨断裂断层"].ToString();
                AddSiteinfo.StartDate = siteinfoDt.Rows[0][9].ToString();// 起测时间
                AddSiteinfo.SiteStatus = siteinfoDt.Rows[0]["运行状况"].ToString();
                AddSiteinfo.MarkStoneType = siteinfoDt.Rows[0]["标石类型"].ToString();
                AddSiteinfo.BuildUnit = siteinfoDt.Rows[0]["建设单位"].ToString();
                AddSiteinfo.ObsUnit = siteinfoDt.Rows[0]["监测单位"].ToString();
                AddSiteinfo.SiteSituation = siteinfoDt.Rows[0]["场地概况"].ToString();
                AddSiteinfo.GeoSituation = siteinfoDt.Rows[0]["地质概况"].ToString();
                AddSiteinfo.OtherSituation = siteinfoDt.Rows[0]["其他情况"].ToString();
                AddSiteinfo.Datachg = siteinfoDt.Rows[0]["资料变更"].ToString();
                AddSiteinfo.ObsCyc = siteinfoDt.Rows[0]["观测周期"].ToString();
                AddSiteinfo.Note = siteinfoDt.Rows[0]["备注"].ToString();

                if (string.IsNullOrEmpty(AddSiteinfo.SiteName))
                {
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "    场地名称不能为空,进程已中断。");

                    return;
                }

                //if (string.IsNullOrEmpty(AddSiteinfo.ObsUnit))
                //{
                //    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "    监测单位不能为空,进程已中断。");

                //    return;
                //}

                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    场地基础信息解析完成。");

                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "   3.正在将" + AddSiteinfo.SiteName + "场地基础信息写入数据库...");

                List<SiteBean> sblist = SiteBll.Instance.GetWhere(new { sitename = AddSiteinfo.SiteName }).ToList();
                if (sblist.Count == 0)
                {
                    SiteBll.Instance.Add(AddSiteinfo);
                    siteSuccedCount++;
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    " + AddSiteinfo.SiteName + "场地基础信息入库成功！");
                }
                else
                {
                    AddSiteinfo = sblist[0];
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "    " + AddSiteinfo.SiteName + "场地基础信息已存在！");
                }
            }
            catch (Exception exp)
            {
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "    " + AddSiteinfo.SiteName + "场地基础信息入库过程中发生错误，进程已中断。");
                siteFaildCount++;
                return;
            }

            #endregion

            #region 卫星图和场地图入库

            if (!(Directory.GetFiles(remoteMapPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".jpg") || s.EndsWith(".png")).Count() == 0))
            {
                try
                {
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "   4.正在将" + AddSiteinfo.SiteName + "卫星图写入数据库...");
                    FileInfo remoteMapfile = new FileInfo(Directory.GetFiles(remoteMapPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".jpg") || s.EndsWith(".png")).ToList()[0]);
                    RemoteMapBean remapbean = new RemoteMapBean();

                    int rbcount = int.Parse(RemotemapBll.Instance.GetDataTable("select count(*) from t_remotemap").Rows[0][0].ToString());
                    remapbean.remotemapcode = RemotemapBll.Instance.CreateLayoutmapCode(rbcount);
                    remapbean.sitecode = AddSiteinfo.SiteCode;
                    remapbean.remotemapname = Regex.Split(remoteMapfile.Name, remoteMapfile.Extension, RegexOptions.IgnoreCase)[0];
                    remapbean.remotemap = File.ReadAllBytes(remoteMapfile.FullName);


                    List<RemoteMapBean> rblist = RemotemapBll.Instance.GetWhere(new { sitecode = AddSiteinfo.SiteCode }).ToList();
                    if (rblist.Count == 0)
                    {
                        RemotemapBll.Instance.Add(remapbean);
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    " + AddSiteinfo.SiteName + "卫星图入库成功！");
                        remotemapSuccedCount++;
                    }
                    else
                    {
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "    " + AddSiteinfo.SiteName + "卫星图已存在！");
                    }


                }
                catch (Exception ex)
                {
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "    " + AddSiteinfo.SiteName + "卫星图入库过程中发生错误，进程已中断。");
                    remotemapFaildCount++;
                    return;
                }

            }
            else
            {
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "   4." + AddSiteinfo.SiteName + "卫星图不存在，已跳过");
            }

            if (!(Directory.GetFiles(layoutMapPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".jpg") || s.EndsWith(".png")).Count() == 0))
            {

                try
                {
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "   5.正在将" + AddSiteinfo.SiteName + "场地图写入数据库...");
                    FileInfo layoutMapfile = new FileInfo(Directory.GetFiles(layoutMapPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".jpg") || s.EndsWith(".png")).ToList()[0]);
                    LayoutmapBean lymapbean = new LayoutmapBean();

                    int lbcount = int.Parse(LayoutmapBll.Instance.GetDataTable("select count(*) from t_layoutmap").Rows[0][0].ToString());

                    lymapbean.layoutmapcode = LayoutmapBll.Instance.CreateLayoutmapCode(lbcount);
                    lymapbean.sitecode = AddSiteinfo.SiteCode;
                    lymapbean.layoutmapname = Regex.Split(layoutMapfile.Name, layoutMapfile.Extension, RegexOptions.IgnoreCase)[0];
                    lymapbean.layoutmap = File.ReadAllBytes(layoutMapfile.FullName);
                    lymapbean.Sort = 0;
                    lymapbean.Bindinglines = "";

                    List<LayoutmapBean> lblist = LayoutmapBll.Instance.GetWhere(new { sitecode = AddSiteinfo.SiteCode }).ToList();
                    if (lblist.Count == 0)
                    {
                        LayoutmapBll.Instance.Add(lymapbean);
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "    " + AddSiteinfo.SiteName + "场地图入库成功！");
                        layoutSuccedCount++;
                    }
                    else
                    {
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "    " + AddSiteinfo.SiteName + "卫星图已存在！");
                    }


                }
                catch (Exception ex)
                {
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, "    " + AddSiteinfo.SiteName + "场地图入库过程中发生错误，进程已中断。");
                    layoutFaildCount++;
                    return;
                }
            }
            else
            {
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "   5." + AddSiteinfo.SiteName + "场地图不存在，已跳过。");
            }

            #endregion

            #region 测线基础信息入库

            try
            {
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "   6.正在将" + AddSiteinfo.SiteName + "测线数据写入数据库...");

                DataTable lineinfoDt = npcreator.ExportToDataTable(lineinfoFile);
                if (lineinfoDt.Rows.Count > 0)
                {
                    foreach (DataRow dr in lineinfoDt.Rows)
                    {
                        LineBean AddLineinfo = new LineBean();
                        AddLineinfo.SITECODE = AddSiteinfo.SiteCode;
                        AddLineinfo.OBSLINENAME = dr["测线名称"].ToString();

                        using (DataTable linenameChgDt = npcreator.ExportToDataTable(linenameChgedFile))
                            foreach (DataRow linenameDr in linenameChgDt.Rows)
                                if (linenameDr["变更后测项名称"].ToString() == dr["测线名称"].ToString())
                                {
                                    AddLineinfo.NAMEBEFORE = linenameDr["原始测项名称"].ToString();
                                    AddLineinfo.NOTE = linenameDr[3].ToString();
                                }

                        AddLineinfo.OBSLINECODE = LineBll.Instance.GenerateLineCode(AddSiteinfo.SiteCode);
                        AddLineinfo.BASEOBSTYPE = dr["基础测项"].ToString();
                        AddLineinfo.AIDSOBSTYPE = dr["辅助测项"].ToString();
                        AddLineinfo.OBSCYCLE = dr["观测周期"].ToString();
                        AddLineinfo.UP_BOT = dr["上盘-下盘"].ToString();
                        AddLineinfo.BUILDDATE = dr["建立时间"].ToString();
                        AddLineinfo.STARTDATE = dr["开测时间"].ToString();
                        AddLineinfo.ENDDATE = dr["停测时间"].ToString();
                        AddLineinfo.LENGTH = dr["测线长度"].ToString();
                        AddLineinfo.STATIONCOUNT = dr["测站数"].ToString();
                        AddLineinfo.FAULTZONE = dr["所属断层"].ToString();
                        AddLineinfo.FAULTSTRIKE = dr["断层走向"].ToString();
                        AddLineinfo.FAULTTENDENCY = dr["断层倾向"].ToString();
                        AddLineinfo.FAULTDIP = dr["断层倾角"].ToString();
                        AddLineinfo.LINE_FAULT_ANGLE = dr["夹角"].ToString();
                        AddLineinfo.PTROCK = dr["测点岩性"].ToString();
                        AddLineinfo.INSTRREPLACEDISCRIP = dr["仪器更换情况"].ToString();
                        AddLineinfo.STARTPOINTCODE = dr["起点"].ToString();
                        AddLineinfo.ENDPOINTCODE = dr["终点"].ToString();
                        AddLineinfo.LINESTATUS = dr["运行状况"].ToString();
                        AddLineinfo.NOTE += "  " + dr["备注"].ToString();

                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "       正在将" + AddLineinfo.OBSLINENAME + "测线基础信息写入数据库...");

                        List<LineBean> lblist = LineBll.Instance.GetWhere(new { obslinename = AddLineinfo.OBSLINENAME, sitecode = AddLineinfo.SITECODE }).ToList();
                        if (lblist.Count == 0)
                        {
                            LineBll.Instance.Add(AddLineinfo);
                            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     " + AddLineinfo.OBSLINENAME + "测线基础信息入库成功");
                            lineinfoSuccedCount++;
                        }
                        else
                        {

                            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "     " + AddLineinfo.OBSLINENAME + "测线基础信息已存在");
                        }

                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "       正在将" + AddLineinfo.OBSLINENAME + "测线观测数据写入数据库...");

                        bool isSearched = false;//记录是否遍历到了对应观测数据




                        foreach (string obslinefile in Directory.GetFiles(obsdataPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".xls") || s.EndsWith(".xlsx")))
                        {
                            FileInfo obslineFi = new FileInfo(obslinefile);
                            //找到对应观测文件
                            if (AddLineinfo.OBSLINENAME == obslineFi.Name.Split('.')[0])
                            {
                                DataTable obslineinfoDt = npcreator.ExportToObslineDataTable(obslineFi.FullName);
                                foreach (DataRow obslinedr in obslineinfoDt.Rows)
                                {
                                    LineObsBean lob = new LineObsBean();
                                    lob.obslinecode = AddLineinfo.OBSLINECODE;

                                    string obsdatastr = obslinedr[0].ToString();
                                    string obsvaluesetr = obslinedr[1].ToString();
                                    if (string.IsNullOrWhiteSpace(obsdatastr) || string.IsNullOrWhiteSpace(obslinedr[1].ToString()))
                                        continue;

                                    double obsvalue = double.NaN;
                                    if (!Double.TryParse(obsvaluesetr, out obsvalue))
                                        continue;

                                    try
                                    {
                                        if (obsdatastr.Contains("/"))// 1987-1-2格式
                                        {
                                            string[] datestrs = obsdatastr.Split('/');
                                            lob.obvdate = DateTime.Parse(datestrs[0] + "-" + datestrs[1] + "-" + datestrs[2]);
                                        }
                                        else if (obsdatastr.Contains("-"))// 1987-1-2格式
                                        {
                                            lob.obvdate = DateTime.Parse(obsdatastr);
                                        }
                                        else //19870102格式
                                        {
                                            string year = obsdatastr.Substring(0, 4);
                                            string month = obsdatastr.Substring(4, 2);
                                            string day = obsdatastr.Substring(6, 2);
                                            if (day == "00") day = "01";
                                            string datestr = year + "-" + month + "-" + day;
                                            lob.obvdate = DateTime.Parse(datestr);
                                        }


                                        lob.obvvalue = double.Parse(obslinedr[1].ToString());
                                        lob.note = obslinedr[2].ToString();
                                        LineObsBll.Instance.Add(lob);
                                        isSearched = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception(ex.Message);
                                    }



                                }
                            }

                        }

                        if (isSearched)
                        {
                            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "     " + AddLineinfo.OBSLINENAME + "测线观测数据入库成功");
                            obslineSuccedCount++;
                        }
                        else
                        {
                            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "     未找到" + AddLineinfo.OBSLINENAME + "测线观测数据。");
                            obslineFaildCount++;
                        }

                    }
                }
                else
                {
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Warning, "     未找到测线基础信息数据，已跳过。");
                }


            }
            catch (Exception ex)
            {
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Error, AddSiteinfo.SiteName + "测线入库过程中发生错误，进程已中断。" + "错误详情:" + ex.Message.ToString());
                return;
            }

            #endregion

            string taskSubster = "【导入完成提示】此任务处理了" + (siteFaildCount + siteSuccedCount).ToString() + "条场地基础信息，其中成功入库了" + siteSuccedCount.ToString() + "条，失败" + siteFaildCount.ToString() + "条。";
            taskSubster += " \r\n                 处理了" + (remotemapFaildCount + remotemapSuccedCount).ToString() + "条场地卫星图，其中成功入库了" + remotemapSuccedCount.ToString() + "条，失败" + remotemapFaildCount.ToString() + "条。";
            taskSubster += "\r\n                 处理了" + (layoutFaildCount + layoutSuccedCount).ToString() + "条场地图，其中成功入库了" + layoutSuccedCount.ToString() + "条，失败" + layoutFaildCount.ToString() + "条。";
            taskSubster += "\r\n                 处理了" + (lineinfoFaildCount + lineinfoSuccedCount).ToString() + "条测线信息，其中成功入库了" + lineinfoSuccedCount.ToString() + "条，失败" + lineinfoFaildCount.ToString() + "条。";
            taskSubster += "\r\n                 处理了" + (obslineFaildCount + obslineSuccedCount).ToString() + "条测线观测数据，其中成功入库了" + obslineSuccedCount.ToString() + "条，失败" + obslineFaildCount.ToString() + "条。";

            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, taskSubster);

        }

        private void ImportBaseInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            xtl.bSignDbTree();
             xtl.RefreshWorkspace(true);
            MyGMap.ClearAllSiteMarker(this.gMapCtrl);
            //MyGMap.AddAllSiteMarker(this.gMapCtrl);
            this.mapBackgroundWorker.RunWorkerAsync();
        }

        #endregion

        private void treeListOriData_CustomDrawNodeImages(object sender, CustomDrawNodeImagesEventArgs e)
        {

        }

        #region 系统菜单

        private void btnSys_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {

                case "btnSilveryStyle"://银色风格
                    {
                        defaultLookAndFeel.LookAndFeel.SkinName = "DevExpress Style";
                    }

                    break;
                case "btnBlueStyle"://蓝色风格
                    {
                        defaultLookAndFeel.LookAndFeel.SkinName = "Office 2010 Blue";
                    }
                    break;

                case "btnPrint"://打印
                    {
                        MyPrint mp = new MyPrint();
                        mp.Print(TabControl.SelectedTabPage);
                    }
                    break;
                case "btnSysAbout"://关于（版权信息等）
                    {
                        frm_About fa = new frm_About();
                        fa.ShowDialog();
                    }
                    break;
                case "btnHelper"://帮助
                    {
                        System.Diagnostics.Process.Start(Application.StartupPath.ToString() + "\\帮助.chm");
                    }
                    break;
                case "btnExit"://退出
                    {
                        this.Close();
                        this.Dispose();
                        Application.Exit();
                    }
                    break;

            }
        }

        #endregion

        /// <summary>
        /// Tchart操作工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChartTool_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.tChartControl.GetSeriesCont() != 1)
            {
                return;
            }
            switch (e.Item.Name)
            {

                case "btnHistoryEqk"://历史地震
                    {
                        this.tChartControl.GetEqkShowForm();
                    }
                    break;
                case "btnExportChart"://导出曲线图
                    {

                        this.tChartControl.ExportChart();
                    }
                    break;

                case "btnClearEqkAnnotation"://清除震例
                    {
                        this.tChartControl.DeleteEqkAnnotation();
                    }
                    break;

                case "btnFourCal"://加减乘除
                    {
                        this.tChartControl.PlusMinusMultiplyDivide();
                        this.btnUndo.Enabled = true;
                    }
                    break;
                case "btnRemoveStep"://消台阶
                    {
                        this.tChartControl.RemoStepOrJump(TChartEventType.RemoveStep);
                        this.btnUndo.Enabled = true;
                    }
                    break;
                case "btnRemoveJump"://消突跳
                    {
                        this.tChartControl.RemoStepOrJump(TChartEventType.RemoveJump);
                        this.btnUndo.Enabled = true;
                    }
                    break;
                case "btnLinesUnion"://测线合并
                    {
                        this.tChartControl.LinesUnion();
                        this.btnUndo.Enabled = true;
                    }
                    break;
                case "btnLinesBreak"://测线拆分
                    {
                        this.tChartControl.LinesBreak(TChartEventType.LineBreak);
                        this.btnUndo.Enabled = true;
                    }
                    break;
                case "barSaveToChuLi"://保存处理数据
                    {
                        this.tChartControl.SaveHandleData();
                        xtl.bSignInitManipdbTree();
                    }
                    break;

                case "btnInterval"://等间隔处理
                    {
                        this.tChartControl.IntervalPross();
                        this.btnUndo.Enabled = true;
                    }
                    break;
                case "btnExportToExcel"://输出为Excel
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Excel文件(*.xls)|*.xls";
                        if (sfd.ShowDialog(this) == DialogResult.OK)
                        {
                            this.tChartControl.ExportToExcel(sfd.FileName);
                        }
                    }
                    break;
                case "btnExportToTXT"://输出为TXT
                    {
                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "文本文件(*.txt)|*.txt";
                        if (sfd.ShowDialog(this) == DialogResult.OK)
                        {
                            this.tChartControl.ExportToTxt(sfd.FileName);
                        }
                    }
                    break;
                case "btnUndo"://撤销
                    {
                        this.tChartControl.Undo();
                        this.btnUndo.Enabled = false;
                        this.btnRedo.Enabled = true;
                    }
                    break;
                case "btnRedo"://恢复
                    {
                        this.tChartControl.Redo();
                        this.btnUndo.Enabled = true;
                        this.btnRedo.Enabled = false;
                    }
                    break;
            }


        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            ChangePanelContainerItemVisible();
        }

        /// <summary>
        /// 地震目录列表行双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlEqklist_MouseDown(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hInfo = this.gridViewEqklist.CalcHitInfo(new Point(e.X, e.Y));
            /*
             * 执行双击事件
             */
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //判断光标是否在行范围内 
                if (hInfo.InRow)
                {
                    try
                    {
                        if (TabControl.SelectedTabPage.Name != "mapTabPage")
                            TabControl.SelectedTabPage = mapTabPage;

                        DataRowView drv = (DataRowView)this.gridViewEqklist.GetRow(hInfo.RowHandle);
                        MyGMap.ZoomtoEqk(new EqkBean { Latitude = double.Parse(drv["Latitude"].ToString()), Longtitude = double.Parse(drv["Longtitude"].ToString()), Magntd = double.Parse(drv["Magntd"].ToString()) }, this.gMapCtrl);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "错误");
                    }

                }
            }
        }

        private void dockPanelEqkCatalog_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            IsEqkShow = false;
            this.gridControlEqklist.DataSource = null;
            this.gridControlEqklist.Refresh();

            IsEqkShow = false;
            ChangePanelContainerItemVisible();
            MyGMap.ClearAllEqkMarker(gMapCtrl);


        }


        /// <summary>  
        /// 绑定分页控件和GridControl数据  
        /// </summary>  
        /// <author>PengZhen</author>  
        /// <time>2013-11-5 14:22:22</time>  
        /// <param name="strWhere">查询条件</param>  
        public void BindPageGridList(string strWhere)
        {

            //记录获取开始数  
            int startIndex = (pageIndex - 1) * pagesize;
            //结束数  
            int endIndex = pageIndex * pagesize;

            //总行数  

            int row = EqkBll.Instance.GetRecordCount(Regex.Split(strWhere, "ORDER", RegexOptions.IgnoreCase)[0]);

            //获取总页数    
            if (row % pagesize > 0)
            {
                pageCount = row / pagesize + 1;
            }
            else
            {
                pageCount = row / pagesize;
            }

            if (pageIndex == 1)
            {
                dataNavigator.Buttons.First.Enabled = false;
                dataNavigator.Buttons.Prev.Enabled = false;
                dataNavigator.Buttons.Next.Enabled = true;
                dataNavigator.Buttons.Last.Enabled = true;
            }

            //最后页时获取真实记录数  
            if (pageCount == pageIndex)
            {
                endIndex = row;
                dataNavigator.Buttons.First.Enabled = true;
                dataNavigator.Buttons.Prev.Enabled = true;
                dataNavigator.Buttons.Next.Enabled = false;
                dataNavigator.Buttons.Last.Enabled = false;
            }

            List<EqkBean> eqkDataList = EqkBll.Instance.GetListByPage(strWhere, "").ToList();

            if (eqkDataList.Count() > 0)
            {
                this.TabControl.SelectedTabPage = this.mapTabPage;
                IsEqkShow = true;
                ChangePanelContainerItemVisible();
                ModelHandler<EqkBean> mh = new ModelHandler<EqkBean>();

                DataTable eqkShowData = mh.FillDataTable(eqkDataList);

                this.gridControlEqklist.DataSource = eqkShowData;
                this.gridControlEqklist.Refresh();
                dataNavigator.DataSource = eqkShowData;
                dataNavigator.TextStringFormat = string.Format("第 {0}页, 共 {1}页", pageIndex, pageCount);

                MyGMap.ClearAllEqkMarker(gMapCtrl);
                MyGMap.AnnotationEqkToMap(eqkDataList, gMapCtrl);

            }
            else
            {
                throw new Exception("没有相应震例");
            }
        }

        /// <summary>  
        /// 获取查询条件  
        /// </summary>  
        /// <author>PengZhen</author>  
        /// <time>2013-11-5 15:25:00</time>  
        /// <returns>返回查询条件</returns>  
        private string GetSqlWhere()
        {
            //查询条件  
            string strReturnWhere = " 1=1 ";


            float eqkMlMin = float.NaN;
            float eqkMlMax = float.NaN;
            try
            {
                eqkMlMin = float.Parse(this.beiEqkMinMtd.EditValue.ToString());
                eqkMlMax = float.Parse(this.beiEqkMaxMtd.EditValue.ToString());
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("不是有效的震级！", "提示");
                return string.Empty;
            }

            if (eqkMlMin > eqkMlMax)
            {
                XtraMessageBox.Show("最大震级应大于最小震级，重新输入！", "提示");
                this.beiEqkMinMtd.EditValue = "";
                this.beiEqkMaxMtd.EditValue = "";
                return string.Empty;
            }


            float eqkDepthMin = float.NaN;
            float eqkDepthMax = float.NaN;
            try
            {
                eqkDepthMin = float.Parse(this.beiEqkMinDepth.EditValue.ToString());
                eqkDepthMax = float.Parse(this.beiEqkMaxDepth.EditValue.ToString());
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("不是有效的震源深度值！", "提示");
                return string.Empty;
            }

            if (eqkDepthMin > eqkDepthMax)
            {
                XtraMessageBox.Show("最大深度应大于最小深度，重新输入！", "提示");
                this.beiEqkMinDepth.EditValue = "";
                this.beiEqkMinDepth.EditValue = "";
                return string.Empty;
            }



            //string timeStStr = this.beiEqkStartTime.EditValue.ToString(); 

            //       DateTime timeStc = Convert.ToDateTime(timeStStr);
            //DateTime timeSt = Convert.ToDateTime(timeStc).Date;
            //string timeEdStr = this.beiEqkEndTime.EditValue.ToString();
            //DateTime timeEdc = Convert.ToDateTime(timeEdStr);
            //DateTime timeEd = Convert.ToDateTime(timeEdc).Date;



            DateTime timeSt = (DateTime)this.beiEqkStartTime.EditValue;


            DateTime timeEd = (DateTime)this.beiEqkEndTime.EditValue;


            if (DateTime.Compare(timeSt, timeEd) > 0)
            {
                XtraMessageBox.Show("结束时间应在开始时间之后！", "提示");
                this.beiEqkStartTime.EditValue = "";
                this.beiEqkEndTime.EditValue = "";
                return string.Empty;
            }


            if (eqkMlMin == eqkMlMax) strReturnWhere += " and MAGNTD = " + eqkMlMin;
            else
                strReturnWhere += " and MAGNTD >= " + eqkMlMin + " and MAGNTD <=" + eqkMlMax;

            if (eqkDepthMin == eqkDepthMax)
                strReturnWhere += " and DEPTH =" + eqkDepthMin;
            else
                strReturnWhere += " and DEPTH >=" + eqkDepthMin + " and DEPTH <=" + eqkDepthMax;

            if (DateTime.Compare(timeSt, timeEd) == 0)
                strReturnWhere += " and EAKDATE =" + "'" + timeSt.ToString() + "'";
            else
                strReturnWhere += " and EAKDATE between '" + timeSt.ToString() + "' and '" + timeEd.ToString() + "'";

            return strReturnWhere += " ORDER BY t.EQKCODE limit " + pageIndex.ToString() + "," + pagesize.ToString() + "";
        }

        private void dataNavigator_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            string type = e.Button.Tag.ToString();
            if (type == "首页")
            {
                pageIndex = 1;
            }

            if (type == "下一页")
            {
                pageIndex++;
            }

            if (type == "末页")
            {
                pageIndex = pageCount;
            }

            if (type == "上一页")
            {
                pageIndex--;
            }

            //绑定分页控件和GridControl数据  
            try
            {
                string sqlwhere = GetSqlWhere();
                if (sqlwhere != string.Empty)
                    BindPageGridList(sqlwhere);
                else
                    throw new Exception("不是有效的查询语句");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("查询失败：" + ex.Message, "错误");
            }

        }

        private void btnXxkAdd_Click(object sender, EventArgs e)
        {
            vGridControlSiteInfo.UpdateFocusedRecord();
            try
            {
                SiteBean AddSiteinfo = new SiteBean();
                string ldordd = GetValue(1);

                if (ldordd == "流动")
                {
                    ldordd = "LD";
                }
                else if (ldordd == "定点")
                {
                    ldordd = "DD";
                }
                else if (ldordd == string.Empty)
                {
                    XtraMessageBox.Show("场地类型不能为空", "提示");
                    return;
                }

                if (this.btnXxkAdd.Text == "更新至数据库")
                {
                    AddSiteinfo.UnitCode = currentClickNodeInfo.ParentFieldName;
                    AddSiteinfo.SiteCode = currentClickNodeInfo.KeyFieldName;
                }
                else if (this.btnXxkAdd.Text == "上传至数据库")
                {
                    AddSiteinfo.UnitCode = currentClickNodeInfo.KeyFieldName;
                    AddSiteinfo.SiteCode = SiteBll.Instance.CreateNewSiteCode(ldordd);
                }

                AddSiteinfo.SiteName = GetValue(0);
                AddSiteinfo.SiteType = GetValue(1);
                AddSiteinfo.ObsType = GetValue(2);
                AddSiteinfo.Historysite = GetValue(3);

                double lat = double.NaN, ln = double.NaN;
                double.TryParse(GetValue(4).ToString(), out ln);
                double.TryParse(GetValue(5).ToString(), out lat);
                AddSiteinfo.Latitude = lat;
                AddSiteinfo.Longtitude = ln;

                AddSiteinfo.Altitude = GetValue(6);
                AddSiteinfo.Place = GetValue(7);
                AddSiteinfo.FaultCode = GetValue(8);
                AddSiteinfo.StartDate = GetValue(9);
                AddSiteinfo.SiteStatus = GetValue(10);
                AddSiteinfo.MarkStoneType = GetValue(11);
                AddSiteinfo.BuildUnit = GetValue(12);
                AddSiteinfo.ObsUnit = GetValue(13);
                AddSiteinfo.SiteSituation = GetValue(14);
                AddSiteinfo.GeoSituation = GetValue(15);
                AddSiteinfo.OtherSituation = GetValue(16);
                AddSiteinfo.Datachg = GetValue(17);
                AddSiteinfo.ObsCyc = GetValue(18);
                AddSiteinfo.Note = GetValue(21);


                if (string.IsNullOrEmpty(AddSiteinfo.SiteName))
                {
                    XtraMessageBox.Show("场地名称不能为空!", "提示");
                    return;
                }

                if (string.IsNullOrEmpty(AddSiteinfo.ObsUnit))
                {
                    XtraMessageBox.Show("监测单位不能为空!", "提示");
                    return;
                }

                try
                {
                    if (this.btnXxkAdd.Text == "更新至数据库")
                    {
                        SiteBll.Instance.UpdateWhatWhere(
                        new
                        {
                            sitename = AddSiteinfo.SiteName,
                            sitetype = AddSiteinfo.SiteType,
                            obstype = AddSiteinfo.ObsType,
                            historysite = AddSiteinfo.Historysite,
                            longtitude = AddSiteinfo.Longtitude,
                            latitude = AddSiteinfo.Latitude,
                            altitude = AddSiteinfo.Altitude,
                            place = AddSiteinfo.Place,
                            faultcode = AddSiteinfo.FaultCode,
                            startdate = AddSiteinfo.StartDate,
                            sitestatus = AddSiteinfo.SiteStatus,
                            markstonetype = AddSiteinfo.MarkStoneType,
                            buildunit = AddSiteinfo.BuildUnit,
                            obsunit = AddSiteinfo.ObsUnit,
                            sitesituation = AddSiteinfo.SiteSituation,
                            geosituation = AddSiteinfo.GeoSituation,
                            othersituation = AddSiteinfo.OtherSituation,
                            datachg = AddSiteinfo.Datachg,
                            obscyc = AddSiteinfo.ObsCyc,
                            note = AddSiteinfo.Note,
                            unitcode = AddSiteinfo.UnitCode
                        },
                        new { sitecode = AddSiteinfo.SiteCode }
                        );

                        string rfstr = GetValue(19);
                        if (!string.IsNullOrEmpty(rfstr))
                        {
                            FileInfo remotemapfile = new FileInfo(GetValue(19));
                            if (File.Exists(remotemapfile.Name))
                            {
                                RemoteMapBean rmb = RemotemapBll.Instance.GetWhere(new { sitecode = AddSiteinfo.SiteCode }).ToList()[0];
                                rmb.remotemapname = Regex.Split(remotemapfile.Name, remotemapfile.Extension, RegexOptions.IgnoreCase)[0];
                                rmb.remotemap = GetPicStream(19);
                                RemotemapBll.Instance.UpdateWhatWhere(
                                    new
                                    {
                                        remotemapname = rmb.remotemapname,
                                        remotemap = rmb.remotemap
                                    },
                                    new { sitecode = AddSiteinfo.SiteCode, remotemapcode = rmb.remotemapcode, }
                                    );
                            }
                        }

                        string lfstr = GetValue(20);
                        if (!string.IsNullOrEmpty(lfstr))
                        {

                            FileInfo layoutmapfile = new FileInfo(GetValue(20));
                            if (File.Exists(layoutmapfile.Name))
                            {
                                LayoutmapBean lmb = LayoutmapBll.Instance.GetWhere(new { sitecode = AddSiteinfo.SiteCode }).ToList()[0];
                                lmb.layoutmapname = Regex.Split(layoutmapfile.Name, layoutmapfile.Extension, RegexOptions.IgnoreCase)[0];
                                lmb.layoutmap = GetPicStream(20);
                                LayoutmapBll.Instance.UpdateWhatWhere(
                                    new
                                    {
                                        layoutmapname = lmb.layoutmapname,
                                        layoutmap = lmb.layoutmap

                                    },
                                    new { sitecode = AddSiteinfo.SiteCode, layoutmapcode = lmb.layoutmapcode, }
                                    );
                            }

                        }
                    }
                    else
                    {
                        SiteBll.Instance.Add(AddSiteinfo);
                        string rfstr = GetValue(19);
                        if (!string.IsNullOrEmpty(rfstr))
                        {
                            FileInfo remotemapfile = new FileInfo(rfstr);
                            if (File.Exists(remotemapfile.Name))
                            {
                                RemoteMapBean rmb = new RemoteMapBean();
                                rmb.sitecode = AddSiteinfo.SiteCode;
                                rmb.remotemapcode = RemotemapBll.Instance.CreateLayoutmapCode(RemotemapBll.Instance.GetAll().ToList().Count);
                                rmb.remotemapname = Regex.Split(remotemapfile.Name, remotemapfile.Extension, RegexOptions.IgnoreCase)[0];
                                rmb.remotemap = GetPicStream(19);
                                RemotemapBll.Instance.Add(rmb);
                            }
                        }

                        string lfstr = GetValue(20);
                        if (!string.IsNullOrEmpty(lfstr))
                        {
                            FileInfo layoutmapfile = new FileInfo(GetValue(20));
                            if (File.Exists(layoutmapfile.Name))
                            {
                                LayoutmapBean lmb = new LayoutmapBean();
                                lmb.sitecode = AddSiteinfo.SiteCode;
                                lmb.layoutmapcode = LayoutmapBll.Instance.CreateLayoutmapCode(LayoutmapBll.Instance.GetAll().ToList().Count);

                                lmb.layoutmapname = Regex.Split(layoutmapfile.Name, layoutmapfile.Extension, RegexOptions.IgnoreCase)[0];
                                lmb.layoutmap = GetPicStream(20);
                                LayoutmapBll.Instance.Add(lmb);
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("执行数据库操作时发生错误：" + ex.Message, "错误");
                }
                finally
                {

                    xtl.bSignDbTree();

                     xtl.RefreshWorkspace(true);
                    MyGMap.ClearAllSiteMarker(this.gMapCtrl);
                    //  MyGMap.AddAllSiteMarker(this.gMapCtrl);

                    this.mapBackgroundWorker.RunWorkerAsync();

                    //重置表格
                    for (int i = 0; i < vGridControlSiteInfo.Rows.Count; i++)
                    {
                        vGridControlSiteInfo.Rows[i].Properties.Value = "";
                    }
                    xtl.RefreshSiteNode(AddSiteinfo.SiteCode);
                    this.addXxkTabPage.PageVisible = false;
                }



            }
            catch (Exception excep)
            {
                XtraMessageBox.Show("上传编辑后的值失败，" + excep.Message, "错误提示");
                return;
            }
            btnXxkAdd.Enabled = false;


            //刷新树
            // InitTree();
        }

        /// 设置是VGridControl行列样式
        /// </summary>
        /// 设置是VGridControl行列样式
        /// </summary>
        private void SetBaseinfoVGridControl()
        {
            try
            {
                // gCXXkAdd.Location= new Point(this.addXxkTabP)
                PublicHelper ph = new PublicHelper();
                int cHeight = vGridControlSiteInfo.Height;

                //RepositoryItemButtonEdit wxtFileBtnEdit = new RepositoryItemButtonEdit();
                DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit wxtFileBtnEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();//Button按钮
                wxtFileBtnEdit2.ButtonClick += WxtFileBtnEdit2_ButtonClick;
                wxtFileBtnEdit2.Buttons[0].Caption = "下载"; //按钮上的文字
                DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit layoutMapFileBtnEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();//Button按钮
                layoutMapFileBtnEdit2.ButtonClick += LayoutMapFileBtnEdit2_ButtonClick;
                layoutMapFileBtnEdit2.Buttons[0].Caption = "下载"; //按钮上的文字

                for (int i = 0; i < vGridControlSiteInfo.Rows.Count; i++)
                {
                    vGridControlSiteInfo.Rows[i].Properties.ReadOnly = false;
                    vGridControlSiteInfo.Rows[i].Properties.UnboundType = DevExpress.Data.UnboundColumnType.String;

                    vGridControlSiteInfo.Rows[i].Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    vGridControlSiteInfo.Rows[i].Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;

                    //if (i == 10)//运行状况
                    //{
                    //    vGridControlSiteInfo.Rows[i].Properties.RowEdit = ph.CreateLookUpEdit(new string[] { "正常", "停测", "改造中" });
                    //}
                    if (i == 1)//场地类型
                    {
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = ph.CreateLookUpEdit(new string[] { "定点", "流动" });
                    }
                    if (i == 2)//观测类型
                    {

                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = ph.CreateLookUpEdit(new string[] { "基线", "水准", "综合" });
                    }

                    //if (i == 12)
                    //{
                    //    List<UnitInfoBean> ublist = UnitInfoBll.Instance.GetAll().ToList();
                    //    string[] units = new string[ublist.Count];
                    //    for (int j = 0; j < units.Length; j++)
                    //    {
                    //        units[j] = ublist[j].UnitName;
                    //    }
                    //    vGridControlSiteInfo.Rows[i].Properties.RowEdit = ph.CreateLookUpEdit(units);//建设单位
                    //    vGridControlSiteInfo.Rows[i + 1].Properties.RowEdit = ph.CreateLookUpEdit(units);//监测单位
                    //}
                    if (i == 19)
                    {
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = wxtFileBtnEdit2;
                    }
                    if (i == 20)
                    {
                        vGridControlSiteInfo.Rows[i].Properties.RowEdit = layoutMapFileBtnEdit2;
                    }

                    //if (i == vGridControlSiteInfo.Rows.Count - 2)
                    //{
                    //    vGridControlSiteInfo.Rows[i].Height = (cHeight) / vGridControlSiteInfo.Rows.Count * 3;
                    //    vGridControlSiteInfo.Rows[i].Properties.RowEdit = memoEdit;
                    //}
                    //else
                    vGridControlSiteInfo.Rows[i].Height = (cHeight - 15) / vGridControlSiteInfo.Rows.Count;
                }

                vGridControlSiteInfo.RowHeaderWidth = vGridControlSiteInfo.Width / 3;
                vGridControlSiteInfo.RecordWidth = vGridControlSiteInfo.Width / 3 * 2 - 20;
                //vGridControlSiteInfo.Rows[0].Height = vGridControlSiteInfo.Width / 3 * 2 - 10;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }

        private void WxtFileBtnEdit2_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(*.jpg,*.png,*.jpeg,*.bmp,*.gif)|*.jgp;*.png;*.jpeg;*.bmp;*.gif|All files(*.*)|*.*";
            if (ofd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[19], 0, ofd.FileName);
        }

        private void LayoutMapFileBtnEdit2_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(*.jpg,*.png,*.jpeg,*.bmp,*.gif)|*.jgp;*.png;*.jpeg;*.bmp;*.gif|All files(*.*)|*.*";
            if (ofd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[20], 0, ofd.FileName);
        }


        /// <summary>
        /// 设置VGrid行值
        /// </summary>
        /// <param name="sb"></param>
        private void SetSiteValueVGridControl(SiteBean sb, bool NewSite)
        {
            if (NewSite)
            {
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[0], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[1], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[2], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[3], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[4], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[5], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[6], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[7], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[8], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[9], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[10], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[11], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[12], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[13], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[14], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[15], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[16], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[17], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[20], 0, "");
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[21], 0, "");
            }
            else
            {
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[0], 0, sb.SiteName);//场地名称
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[1], 0, sb.SiteType);//场地类型
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[2], 0, sb.ObsType);//观测类型
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[3], 0, sb.Historysite);//历史场地
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[4], 0, sb.Longtitude);//经度
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[5], 0, sb.Latitude);//纬度
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[6], 0, sb.Altitude);//高程
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[7], 0, sb.Place);//所在地
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[8], 0, sb.FaultCode);//所跨断裂
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[9], 0, sb.StartDate);//起测试间
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[10], 0, sb.SiteStatus);//运行状况
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[11], 0, sb.MarkStoneType);//标石类型
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[12], 0, sb.BuildUnit);//建设单位
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[13], 0, sb.ObsUnit);//监测单位
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[14], 0, sb.SiteSituation);//场地概况
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[15], 0, sb.GeoSituation);//地质概况
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[16], 0, sb.OtherSituation);//其他情况
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[17], 0, sb.Datachg);//资料变更
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[18], 0, sb.ObsCyc);//观测周期
                //vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[19], 0, sb.LayoutMap);//卫星图
                //vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[20], 0, sb.Note);//场地图
                vGridControlSiteInfo.SetCellValue(vGridControlSiteInfo.Rows[21], 0, sb.Note);//备注
            }
        }

        private string GetValue(int row)
        {
            int iRecordIndex = 0;
            Type type = vGridControlSiteInfo.Rows[row].Properties.RowType;

            object value = vGridControlSiteInfo.GetCellValue(vGridControlSiteInfo.Rows[row], iRecordIndex);

            if (type.FullName == "System.Int32")
            {
                value = (value == DBNull.Value || value == null) ? "null" : value;
            }
            else if (type.FullName == "System.Double")
            {
                value = (value == DBNull.Value || value == null) ? "null" : value;
            }
            else if (type.FullName == "System.DataTime")
            {
                value = (value == DBNull.Value || value == null) ? "null" : value;
            }
            else if (type.FullName == "System.Decimal")
            {
                value = (value == DBNull.Value || value == null) ? "null" : value;
            }

            if (value == null)
                return string.Empty;
            else
                return value.ToString();
        }

        private byte[] GetPicStream(int row)
        {
            try
            {
                int iRecordIndex = 0;
                Type type = vGridControlSiteInfo.Rows[row].Properties.RowType;

                object value = vGridControlSiteInfo.GetCellValue(vGridControlSiteInfo.Rows[row], iRecordIndex);

                byte[] img = File.ReadAllBytes(value.ToString());

                if (value.ToString() != "")
                {
                    return img;
                }
                else
                {
                    return new byte[0];
                }
            }
            catch (Exception ex)
            {
                return new byte[0];
            }

        }

        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs EArg = (DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs)e;
            string name = EArg.Page.Text;//得到关闭的选项卡的text  
            foreach (XtraTabPage page in TabControl.TabPages)//遍历得到和关闭的选项卡一样的Text  
            {
                if (page.Text == name)
                {
                    //xtraTabControl1.TabPages.Remove(page);
                    //page.Dispose();
                    page.PageVisible = false;
                    return;
                }
            }
        }


        /// <summary>
        /// 用户操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "btnUserManager"://用户管理
                    {

                        try
                        {
                            frm_UserManage usfrm = new frm_UserManage();
                            usfrm.ShowDialog(this);
                        }
                        catch (Exception ex)
                        {


                        }

                    }
                    break;
            }
        }

        /// <summary>
        /// 数据提交与审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDataUpload_ItemClick(object sender, ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "btnDataCommit"://提交
                    {

                        frm_DataCommit dctFrm = new frm_DataCommit();
                        dctFrm.ShowDialog(this);

                    }
                    break;
                case "btnAdmin"://审核
                    {

                        if (SystemInfo.CurrentUserInfo.UserName == "superadmin")
                        {
                            frm_DataAdmin danFrm = new frm_DataAdmin();
                            danFrm.ShowDialog(this);
                        }

                    }
                    break;
            }

        }


        #region 断层数据入库(已废弃)

        //private string midFile = @"D:\liuwlCloud\开发资料\中国活动断裂\mif\中国活动断裂.MID";
        //private string mifFile = @"D:\liuwlCloud\开发资料\中国活动断裂\mif\中国活动断裂.MIF";
        //private System.Collections.ArrayList mid_Sentence;
        //private System.Collections.ArrayList mif_Sentence;
        //private StreamReader m_fReader;

        //private void InsertFaultDataToDb()
        //{
        //    try
        //        {
        //        mid_Sentence = OpenFile(midFile);
        //        mif_Sentence = OpenFile(mifFile);

        //        //int index = 1;
        //        List<string> linestrList = new List<string>();
        //        for (int i = 26; i < mif_Sentence.Count; i++)
        //        {
        //            if (mif_Sentence[i].ToString().Contains("Pline Multiple"))//一条记录有多条线的情况
        //            {
        //                int linecount = int.Parse(mif_Sentence[i].ToString().Split(' ')[2]);//线段数量
        //                string linestr = "";
        //                for (int j = i + 1; j <= i + linecount; j++)
        //                {
        //                    int pointcount = int.MinValue;
        //                    try
        //                    {
        //                        pointcount = Convert.ToInt32(mif_Sentence[j].ToString().Trim()); //线段点数量
        //                    }
        //                    catch
        //                    {
        //                        continue;
        //                    }
        //                    if (pointcount > 0)
        //                    {
        //                        string ptsStr = "";
        //                        for (int n = j + 1; n <= j + pointcount; n++)
        //                        {
        //                            ptsStr += mif_Sentence[n].ToString() + ",";
        //                        }

        //                        linestr += ptsStr.Substring(0, ptsStr.Length - 1) + ";";
        //                    }

        //                }

        //                linestrList.Add(linestr);
        //            }
        //            else if (!mif_Sentence[i].ToString().Contains("Pline Multiple") && mif_Sentence[i].ToString().Contains("Pline"))//一条记录只有一条线的情况
        //            {
        //                int pointcount = int.Parse(mif_Sentence[i].ToString().Split(' ')[1]);//线段点数量
        //                string ptsStr = "";
        //                for (int n = 1; n <= pointcount; n++)
        //                {
        //                    ptsStr += mif_Sentence[i + n].ToString() + ",";
        //                }

        //                linestrList.Add(ptsStr.Substring(0, ptsStr.Length - 1) + ";");

        //            }
        //            else //if (mif_Sentence[i].ToString().Contains("Pen"))//一条记录结束的标志
        //            {
        //                continue;
        //            }
        //        }

        //        #region

        //        if (mid_Sentence.Count > 0)
        //        {
        //            foreach (string ecstr in mid_Sentence)
        //            {

        //                string[] strs = ecstr.Split('"');
        //                List<string> datalist = new List<string>();
        //                for (int i = 0; i < strs.Length; i++)
        //                {
        //                    if (strs[i] != ",")
        //                        datalist.Add(strs[i]);
        //                }

        //                FaultBean fb = new FaultBean();
        //                if (datalist[0].Contains(","))
        //                {
        //                    string ss = datalist[0].Replace(",", "");
        //                    fb.Faultcode = int.Parse(ss);
        //                }
        //                fb.Name = datalist[1];
        //                fb.Activitynature = datalist[2];
        //                fb.Length = datalist[3];
        //                fb.Faultage = datalist[4];
        //                fb.Newestactivetime = datalist[5];
        //                fb.Seismicactivity = datalist[6];
        //                fb.Faultoccurrences = datalist[7];
        //                fb.Levelseparation = datalist[8];
        //                fb.Verticalseparation = datalist[9];
        //                fb.Strikesliprate = datalist[10];
        //                fb.Dipsliprate = datalist[11];
        //                fb.Ancientearthquake = datalist[12];
        //                fb.Faultzonechrtrstics_structure = datalist[13];
        //                fb.Geomorphicfeature_bluffdrainage = datalist[14];
        //                fb.Geophysicalcharacteristics = datalist[15];
        //                fb.Volcaniceruption = datalist[16];
        //                fb.Magmaticintrusion = datalist[17];
        //                fb.Determinethebasisoffaultactivity = datalist[18];
        //                fb.Locations = linestrList[fb.Faultcode - 1];

        //                FaultBll.Instance.Add(fb);

        //            }
        //        }

        //        #endregion





        //    }
        //        catch (Exception ex)
        //        {
        //        MessageBox.Show("错误", ex.Message);
        //    }
        //}
        //private System.Collections.ArrayList OpenFile(string file)
        //{
        //    System.Collections.ArrayList arraylist;
        //    try
        //    {
        //        if (file == null)
        //            throw new ArgumentNullException();
        //        if (!File.Exists(file))
        //            throw new ArgumentException("Invalid file name" + file);
        //        if (Path.GetExtension(file) != null && (file.EndsWith(".mif") || file.EndsWith(".mif")))
        //            throw new Exception("Invalid file type!");
        //    }
        //    //捕获异常
        //    catch (ArgumentNullException ane)
        //    {

        //    }
        //    catch (ArgumentException ae)
        //    {

        //    }
        //    catch (Exception e)
        //    {

        //    }

        //    FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
        //    m_fReader = new StreamReader(fs, Encoding.Default);

        //    string text_line;
        //    arraylist = new System.Collections.ArrayList();   //初始化arraylist对象
        //    while ((text_line = m_fReader.ReadLine()) != null) //只要文件未结束
        //    {
        //        if (text_line.Length == 0)   //空行不读
        //            continue;
        //        arraylist.Add(text_line); //将读到的行添加到arraylist对象中
        //    }

        //    return arraylist;
        //}

        #endregion

        private void RibbonForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            Application.Exit();
        }

        /// <summary>
        /// 判断用户是否拥有单位权限
        /// </summary>
        /// <param name="unitcode">单位编码</param>
        /// <returns></returns>
        private bool IsUserHasAth(string unitcode)
        {
            bool res = false;

            if (SystemInfo.CurrentUserInfo.UserAthrty.Split(';').ToList() != null)
                if (SystemInfo.CurrentUserInfo.UserAthrty.Split(';').ToList().Contains(unitcode))
                    res = true;
                else
                    res = false;

            return res;
        }

        #region 资料完整性统计

        private void Statistics_DoWork(object sender, DoWorkEventArgs e)
        {
            MyBackgroundWorker worker = (MyBackgroundWorker)sender;
            e.Cancel = false;

            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            NpoiCreator npcreator = new NpoiCreator();

            string unitcode = currentClickNodeInfo.KeyFieldName;
            string unitname = currentClickNodeInfo.Caption;
            int m = 0;

            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "【统计开始提示】...");

            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "    正在统计" + unitname + "数据...");

            List<SiteBean> sblist = SiteBll.Instance.GetWhere(new { unitcode = unitcode }).ToList();
            m += sblist.Count;
            int n = 1;
            double unitrate = 0;
            foreach (SiteBean sb in sblist)
            {
                BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "    " + n.ToString() + " .正在统计" + sb.SiteName + "信息...");

                List<string> missFields = new List<string>();//记录缺失的字段

                if (string.IsNullOrEmpty(sb.SiteName)) missFields.Add("场地名称");
                if (string.IsNullOrEmpty(sb.SiteType)) missFields.Add("场地类型");
                if (string.IsNullOrEmpty(sb.ObsType)) missFields.Add("观测类型");
                if (string.IsNullOrEmpty(sb.Historysite)) missFields.Add("历史迁移");
                if (double.IsNaN(sb.Longtitude)) missFields.Add("经度");
                if (double.IsNaN(sb.Latitude)) missFields.Add("纬度");
                if (string.IsNullOrEmpty(sb.Altitude)) missFields.Add("高程");
                if (string.IsNullOrEmpty(sb.Place)) missFields.Add("所在地");
                if (string.IsNullOrEmpty(sb.FaultCode)) missFields.Add("所跨断层");
                if (string.IsNullOrEmpty(sb.StartDate)) missFields.Add("起测试间");
                if (string.IsNullOrEmpty(sb.SiteStatus)) missFields.Add("运行状况");
                if (string.IsNullOrEmpty(sb.MarkStoneType)) missFields.Add("标石类型");
                if (string.IsNullOrEmpty(sb.BuildUnit)) missFields.Add("建设单位");
                if (string.IsNullOrEmpty(sb.ObsUnit)) missFields.Add("监测单位");
                if (string.IsNullOrEmpty(sb.SiteSituation)) missFields.Add("场地概况");
                if (string.IsNullOrEmpty(sb.GeoSituation)) missFields.Add("地质概况");
                if (string.IsNullOrEmpty(sb.Note)) missFields.Add("备注");
                if (string.IsNullOrEmpty(sb.OtherSituation)) missFields.Add("其他情况");
                if (string.IsNullOrEmpty(sb.Datachg)) missFields.Add("资料变更");
                if (string.IsNullOrEmpty(sb.ObsCyc)) missFields.Add("观测周期");

                List<LayoutmapBean> lbs = LayoutmapBll.Instance.GetLayoutmapBy(new { sitecode = sb.SiteCode });
                if (lbs.Count == 0) missFields.Add("场地图");
                else if (lbs[0].layoutmap == null || lbs[0].layoutmap.Length == 0) missFields.Add("场地图");

                List<RemoteMapBean> rbs = RemotemapBll.Instance.GetRemotemapBy(new { sitecode = sb.SiteCode });
                if (rbs.Count == 0) missFields.Add("卫星图");
                else if (rbs[0].remotemap == null || rbs[0].remotemap.Length == 0) missFields.Add("卫星图读");


                double rate = 100 - Math.Round(double.Parse(missFields.Count.ToString()) / 22 * 100, 2);
                unitrate += rate;
                if (rate > 0)
                {
                    string resstr = "      " + sb.SiteName + "统计结果：资料完整率" + (rate).ToString() + "%。其中";

                    for (int i = 0; i < missFields.Count; i++)
                    {
                        resstr += "'" + missFields[i] + "' ";
                    }
                    resstr += " 数据缺少信息。";

                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, resstr);
                }
                else
                {
                    string resstr = "      " + sb.SiteName + "统计结果：资料完整率100%。";
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, resstr);
                }
                n++;



                List<LineBean> lblist = LineBll.Instance.GetWhere(new { sitecode = sb.SiteCode }).ToList();
                foreach (LineBean lb in lblist)
                {
                    BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "       正在统计" + lb.OBSLINENAME + "测线信息...");

                    List<string> missLineFields = new List<string>();//记录缺失的字段

                    if (string.IsNullOrEmpty(lb.OBSLINENAME)) missLineFields.Add("测线名称");
                    if (string.IsNullOrEmpty(lb.BASEOBSTYPE)) missLineFields.Add("基础测项");
                    if (string.IsNullOrEmpty(lb.AIDSOBSTYPE)) missLineFields.Add("辅助测项");
                    if (string.IsNullOrEmpty(lb.OBSCYCLE)) missLineFields.Add("观测周期");
                    if (string.IsNullOrEmpty(lb.UP_BOT)) missLineFields.Add("上盘-下盘");
                    if (string.IsNullOrEmpty(lb.BUILDDATE)) missLineFields.Add("建立时间");
                    if (string.IsNullOrEmpty(lb.STARTDATE)) missLineFields.Add("开测时间");
                    if (string.IsNullOrEmpty(lb.ENDDATE)) missLineFields.Add("停测时间");
                    if (string.IsNullOrEmpty(lb.LENGTH)) missLineFields.Add("测线长度");
                    if (string.IsNullOrEmpty(lb.STATIONCOUNT)) missLineFields.Add("测站数");
                    if (string.IsNullOrEmpty(lb.FAULTZONE)) missLineFields.Add("所属断层");
                    if (string.IsNullOrEmpty(lb.FAULTSTRIKE)) missLineFields.Add("断层走向");
                    if (string.IsNullOrEmpty(lb.FAULTTENDENCY)) missLineFields.Add("断层倾向");
                    if (string.IsNullOrEmpty(lb.FAULTDIP)) missLineFields.Add("断层倾角");
                    if (string.IsNullOrEmpty(lb.LINE_FAULT_ANGLE)) missLineFields.Add("夹角");
                    if (string.IsNullOrEmpty(lb.PTROCK)) missLineFields.Add("测点岩性");
                    if (string.IsNullOrEmpty(lb.INSTRREPLACEDISCRIP)) missLineFields.Add("仪器更换情况");
                    if (string.IsNullOrEmpty(lb.STARTPOINTCODE)) missLineFields.Add("起点");
                    if (string.IsNullOrEmpty(lb.ENDPOINTCODE)) missLineFields.Add("终点");
                    if (string.IsNullOrEmpty(lb.LINESTATUS)) missLineFields.Add("运行状况");
                    if (string.IsNullOrEmpty(lb.NOTE)) missLineFields.Add("备注");

                    DataTable obslineDt = LineObsBll.Instance.GetDataTable("select count(*) from t_obsrvtntb where obslinecode ='" + lb.OBSLINECODE + "'");
                    if (obslineDt.Rows[0][0].ToString() == "0") missLineFields.Add("观测数据");

                    double linerate = 100 - Math.Round(double.Parse(missFields.Count.ToString()) / 22 * 100, 2);

                    if (linerate > 0)
                    {
                        string resstr = "      " + lb.OBSLINENAME + "测线统计结果：资料完整率" + (linerate).ToString() + "%。其中";

                        for (int i = 0; i < missLineFields.Count; i++)
                        {
                            resstr += "'" + missLineFields[i] + "' ";
                        }
                        resstr += " 数据缺少信息。";

                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, resstr);
                    }
                    else
                    {
                        string resstr = "      " + lb.OBSLINENAME + "测线统计结果：资料完整率100%。";
                        BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, resstr);
                    }




                }



            }

            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Right, "      " + unitname + "资料平均完整率为" + Math.Round((unitrate / sblist.Count), 2) + "%");
            BackgroundWorkerHelper.outputWorkerLog(worker, LogType.Common, "【统计完成提示】此任务处理了" + m.ToString() + "条记录");
        }

        private void Statistics_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }




        private Color GetRandomColor()
        {
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            int R = ran.Next(255);
            int G = ran.Next(255);
            int B = ran.Next(255);
            B = (R + G > 400) ? R + G - 400 : B;//0 : 380 - R - G;
            B = (B > 255) ? 255 : B;
            return Color.FromArgb(R, G, B);
        }


        #endregion

        private void recycleControl_RefreshTree(string dbpath)
        {
            xtl.bSignDbTree();
             xtl.RefreshWorkspace(true);
            xtl.bSignInitManipdbTree();
        }

        #region 场地标注右击菜单

        private GMapMarker clickedMarker = null;
        private void popupMenuSiteRightClick_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (clickedMarker == null)
                return;

            if (clickedMarker is GMapMarker)
            {
                if (clickedMarker.Overlay.Id == "sitemarkers")//场地标签图层
                {
                    try
                    {
                        switch (e.Item.Name)
                        {
                            case "btnHide"://隐藏场地图标
                                {
                                    MyGMap.ClearSiteByCode(clickedMarker.Tag.ToString(), this.gMapCtrl);
                                }
                                break;
                            case "barSiteCondition"://场地基本情况
                                {
                                    frm_SplashScreen sdf = new frm_SplashScreen("提示", "正在加载......", "请耐心等候，正在加载数据！", 1);
                                    if (this.siteInfoPdfCtrl.SetPdfFilePath(Application.StartupPath + "\\文档缓存\\信息库模板.pdf"))
                                    {
                                        SiteBean sb = SiteBll.Instance.GetWhere(new { SITECODE = clickedMarker.Tag.ToString() }).ToList()[0];
                                        this.siteInfoPdfCtrl.WriteContent(sb);
                                        this.siteInfoPdfCtrl.OpenFile();
                                        this.siteInfoTabPage.PageVisible = true;
                                        this.TabControl.SelectedTabPage = this.siteInfoTabPage;
                                        sdf.SetCaption("执行进度（" + 1.ToString() + "/" + 1.ToString() + "）");
                                    }
                                    else
                                    {
                                        XtraMessageBox.Show("系统缺少 信息库木板.pdf 文件", "错误");
                                    }
                                    sdf.Close();
                                }
                                break;
                        }
                    }
                    catch { }
                }
            }
        }





        #endregion

        #region 断裂带检索


        /// <summary>
        /// 地震目录列表行双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridControlFaultlist_MouseDown(object sender, MouseEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hInfo = this.girdViewFaultlist.CalcHitInfo(new Point(e.X, e.Y));
            /*
             * 执行双击事件
             */
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                //判断光标是否在行范围内 
                if (hInfo.InRow)
                {
                    try
                    {
                        if (TabControl.SelectedTabPage.Name != "mapTabPage")
                            TabControl.SelectedTabPage = mapTabPage;

                        DataRowView drv = (DataRowView)this.girdViewFaultlist.GetRow(hInfo.RowHandle);

                        string locations = drv[19].ToString();
                        //this.gMapCtrl.Position = new PointLatLng(double.Parse(drv["Latitude"].ToString()), double.Parse(drv["Longtitude"].ToString()));
                        MyGMap.ZoomtoFault(locations, this.gMapCtrl);

                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "错误");
                    }

                }
            }
        }


        private void girdViewFaultlist_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            if (e.Info.IsRowIndicator)
            {
                if (e.RowHandle >= 0)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                }
                else if (e.RowHandle < 0 && e.RowHandle > -1000)
                {
                    e.Info.Appearance.BackColor = System.Drawing.Color.AntiqueWhite;
                    e.Info.DisplayText = "G" + e.RowHandle.ToString();
                }
            }
        }

        private void dockPanelFaultCatalog_ClosedPanel(object sender, DevExpress.XtraBars.Docking.DockPanelEventArgs e)
        {
            IsFaultShow = false;
            this.gridControlFaultlist.DataSource = null;
            this.gridControlFaultlist.Refresh();

            IsFaultShow = false;
            ChangePanelContainerItemVisible();
            MyGMap.ClearFault(gMapCtrl);

        }


        #endregion


        #region 异步加载Marker

        List<SiteBean> pSitelist = new List<SiteBean>();
        private void mapBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {


            DataTable dt = SiteBll.Instance.GetDataTable("select sitecode,sitename,sitetype,latitude,longtitude,sitestatus,obstype, place,faultcode, obsunit, sitesituation  from t_siteinfodb ");
            rpitProgressBar.Maximum = 100;

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                SiteBean sb = new SiteBean();
                sb.SiteCode = dr["sitecode"].ToString();
                sb.SiteName = dr["sitename"].ToString();
                sb.ObsType = dr["obstype"].ToString();
                sb.Latitude = double.Parse(dr["latitude"].ToString());
                sb.Longtitude = double.Parse(dr["longtitude"].ToString());
                sb.SiteType = dr["sitetype"].ToString();
                sb.SiteSituation = dr["sitesituation"].ToString();
                sb.Place = dr["place"].ToString();
                sb.FaultCode = dr["faultcode"].ToString();
                sb.ObsUnit = dr["obsunit"].ToString();
                sb.SiteStatus = dr["sitestatus"].ToString();

                pSitelist.Add(sb);

                int percent = (int)((i + 1) / dt.Rows.Count);
                mapBackgroundWorker.ReportProgress(percent);

                i++;
            }

        }

        private void mapBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {


        }

        private void mapBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MyGMap.AddSiteMarker(pSitelist, gMapCtrl);

            this.currentTask.Visibility = BarItemVisibility.Never;

        }





        #endregion

        private void panelContainer2_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (e.Button == panelContainer2.CustomHeaderButtons[0])
            {
                xtl.bSignDbTree();
                if (e.Button.Properties.ToolTip == "显示新测线名")
                {
                    xtl.RefreshWorkspace(true);
                    e.Button.Properties.ToolTip = "显示旧测线名";
                }
                else
                {
                    xtl.RefreshWorkspace(false );
                    e.Button.Properties.ToolTip = "显示新测线名";
                }
            }
            else
            {
                xtl.bSignDbTree();
                xtl.RefreshWorkspace(true);
            }

         

        }


    }
}