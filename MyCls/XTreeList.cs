using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxkUI.Bll;
using xxkUI.BLL;
using xxkUI.Model;
using xxkUI.Form;
using DevExpress.XtraTreeList.Nodes;
using System.IO;
using Common.Data.MySql;
using System.Configuration;
using xxkUI.Tool;
using System.Data;

namespace xxkUI.MyCls
{
   public class XTreeList
    {
        private DevExpress.XtraTreeList.TreeList treeListData;
        private DevExpress.XtraTreeList.TreeList treeListManipData;

         public XTreeList(DevExpress.XtraTreeList.TreeList _treeListData,DevExpress.XtraTreeList.TreeList _treeListManipData)
         {

            _treeListData.LookAndFeel.UseDefaultLookAndFeel = false;
            _treeListData.LookAndFeel.UseWindowsXPTheme = true;

            treeListData = _treeListData;
            treeListManipData = _treeListManipData;

         }

        public XTreeList()
        {

        }

        /// <summary>
        /// 加载远程库树列表
        /// </summary>
        /// <param name="gmmkks">GMAP控件</param>
        public void bSignDbTree()
        {
            try
            {
                this.treeListData.ClearNodes();
                this.treeListData.DataSource = null;
                List<TreeBean> treeData = new List<TreeBean>();

                DataTable ubEnumt = UnitInfoBll.Instance.GetDataTable("select UNITNAME,UNITCODE from t_unittb where HASSITES=1");

                foreach (DataRow dr in ubEnumt.Rows)
                {
                    TreeBean tb = new TreeBean();
                    tb.TreeName = treeListData.Name;
                    tb.KeyFieldName = dr["UNITCODE"].ToString();
                    tb.ParentFieldName = "0";
                    tb.Caption = dr["UNITNAME"].ToString();
                    tb.SiteType = "";
                    tb.LineStatus = "";
                    tb.Tag = new UnitInfoBean {  UnitName= dr["UNITNAME"].ToString() , UnitCode= dr["UNITCODE"].ToString() , HASSITES= 1};//lwl
                    treeData.Add(tb);

                }
               

                List<SiteBean> sbEnumt = new List<SiteBean>();
                DataTable sitedt = SiteBll.Instance.GetDataTable(@"select sitecode, unitcode,sitename from t_siteinfodb");
             
                foreach (DataRow sitedr in sitedt.Rows)
                {
                    SiteBean sgsb = new SiteBean();
                    sgsb.SiteCode = sitedr["sitecode"].ToString();
                    sgsb.SiteName = sitedr["sitename"].ToString();
                    sgsb.UnitCode = sitedr["unitcode"].ToString();
                    sbEnumt.Add(sgsb);
                }
                //场地列表显示
                List<string> olSiteCode = new List<string>();
                foreach (SiteBean sb in sbEnumt)
                {
                    olSiteCode.Add(sb.SiteCode);
                    TreeBean tb = new TreeBean();
                    tb.KeyFieldName = sb.SiteCode;
                    tb.ParentFieldName = sb.UnitCode;
                    tb.Caption = sb.SiteName;
                    tb.SiteType = sb.SiteType;
                    tb.Tag = sb.SiteCode;//lwl
                    treeData.Add(tb);
                }
           

                //树列表显示
                this.treeListData.KeyFieldName = "KeyFieldName";          //这里绑定的ID的值必须是独一无二的
                this.treeListData.ParentFieldName = "ParentFieldName";  //表示使用parentID进行树形绑定
                this.treeListData.DataSource = treeData;  //绑定数据源
                this.treeListData.OptionsView.ShowCheckBoxes = true;
                this.treeListData.OptionsBehavior.AllowRecursiveNodeChecking = true;
                this.treeListData.OptionsBehavior.Editable = false;

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }

        /// <summary>
        /// 刷新场地节点
        /// </summary>
        /// <param name="sitecode">场地编码</param>
        public void RefreshSiteNode(string sitecode)
        {
            SiteBean sb = SiteBll.Instance.GetWhere(new { SiteCode = sitecode }).ToList()[0];

            List<TreeBean> treeData = this.treeListData.DataSource as List<TreeBean>;

            TreeBean tb = treeData.Find(n => n.KeyFieldName == sitecode);
            tb.KeyFieldName = sitecode;
            tb.ParentFieldName = sb.UnitCode;
            tb.Caption = sb.SiteName;
            tb.SiteType = sb.SiteType;
            tb.Tag = sb.SiteCode;
            //treeData.Add(tb);
            treeListData.RefreshDataSource();

        }

        /// <summary>
        /// 加载测项合并数列表
        /// </summary>
        /// <returns></returns>
        public List<TreeBean> ObslineMergeTree()
        {
          
            List<TreeBean> tblist = new List<TreeBean>();
            try
            {
                IEnumerable<UnitInfoBean> ubEnumt = UnitInfoBll.Instance.GetAll();

                foreach (UnitInfoBean sb in ubEnumt)
                {
                    TreeBean tb = new TreeBean();
                    if (sb.UnitCode == "152002" || sb.UnitCode == "152003"
                        || sb.UnitCode == "152006" || sb.UnitCode == "152008"
                        || sb.UnitCode == "152009" || sb.UnitCode == "152010"
                        || sb.UnitCode == "152012" || sb.UnitCode == "152015"
                        || sb.UnitCode == "152022" || sb.UnitCode == "152023"
                        || sb.UnitCode == "152026" || sb.UnitCode == "152029"
                        || sb.UnitCode == "152032" || sb.UnitCode == "152034"
                        || sb.UnitCode == "152035" || sb.UnitCode == "152036"
                        || sb.UnitCode == "152039" || sb.UnitCode == "152040"
                        || sb.UnitCode == "152041" || sb.UnitCode == "152042"
                        || sb.UnitCode == "152043" || sb.UnitCode == "152044"
                        || sb.UnitCode == "152045" || sb.UnitCode == "152046"
                        || sb.UnitCode == "152001" || sb.UnitCode == "152047")
                    { continue; }

                    tb.KeyFieldName = sb.UnitCode;
                    tb.ParentFieldName = "0";
                    tb.Caption = sb.UnitName;
                    tb.SiteType = "";
                    tb.LineStatus = "";
                    tb.Tag = sb;//lwl
                    tblist.Add(tb);
                }

                List<SiteBean> sbEnumt = new List<SiteBean>();
                DataTable dt = SiteBll.Instance.GetDataTable(@"select sitecode, unitcode,sitename from t_siteinfodb");
               
                foreach (DataRow dr in dt.Rows)
                {
                    SiteBean sgsb = new SiteBean();
                    sgsb.SiteCode = dr["sitecode"].ToString();
                    sgsb.SiteName = dr["sitename"].ToString();
                    sgsb.UnitCode = dr["unitcode"].ToString();
                    sbEnumt.Add(sgsb);
                }


                //场地列表显示
                List<string> olSiteCode = new List<string>();
                foreach (SiteBean sb in sbEnumt)
                {
                    olSiteCode.Add(sb.SiteCode);
                    TreeBean tb = new TreeBean();
                    tb.KeyFieldName = sb.SiteCode;
                    tb.ParentFieldName = sb.UnitCode;
                    tb.Caption = sb.SiteName;
                    tb.SiteType = sb.SiteCode.Substring(0, 1) == "L" ? "流动" : "定点";
                    tb.Tag = sb.SiteCode;//lwl
                    tblist.Add(tb);
                }

                List<String> remoteExcelList = new List<string>();
                remoteExcelList = getFile(SystemInfo.DatabaseCache);

                foreach (string remoteLineCode in remoteExcelList)
                {
                    string subLineCode = remoteLineCode.Substring(0, remoteLineCode.Length - 4);
                    LineBean ol = LineBll.Instance.GetInfoByID(subLineCode);
                    if (olSiteCode.Contains(ol.SITECODE))
                    {
                        TreeBean tb = new TreeBean();
                        tb.KeyFieldName = ol.OBSLINECODE;
                        tb.ParentFieldName = ol.SITECODE;
                        tb.Caption = ol.OBSLINENAME;
                        tb.Tag = ol;//lwl
                        tblist.Add(tb);
                    }
                }

            }
            catch 
            {
                //throw new Exception(ex.Message);
            }

            return tblist;

        }

         /// <summary>
         /// 加载处理数据树列表
         /// </summary>
        public void bSignInitManipdbTree()
         {
             try
             {
                 this.treeListManipData.Nodes.Clear();
                 //处理数据库测线列表显示
                 List<String> manipExcelList = new List<string>();
                 string manipExcelPath = Application.StartupPath + "/处理数据缓存";
                 manipExcelList = getFile(manipExcelPath);
                 foreach (string manipLineName in manipExcelList)
                 {
                     string subLineName = manipLineName.Substring(0, manipLineName.Length - 4);
                     this.treeListManipData.BeginUnboundLoad();
                     TreeListNode newnode = this.treeListManipData.AppendNode(new object[] { subLineName, "" }, -1);
                     //newnode["OrgName"] = orgnamestr;    //重新赋值
                     //newnode["Id"] = newnodeid;             //重新赋值
                     this.treeListManipData.EndUnboundLoad();
                 }



                this.treeListManipData.OptionsView.ShowCheckBoxes = true;
                 this.treeListManipData.OptionsBehavior.AllowRecursiveNodeChecking = true;
                 this.treeListManipData.OptionsBehavior.Editable = false;
                 this.treeListManipData.Refresh();
             }
             catch (Exception ex)
             {
                 XtraMessageBox.Show(ex.Message, "错误");
             }
         }


        /// <summary> 
        /// 设置TreeList显示的图标 
        /// </summary> 
        /// <param name="tl">TreeList组件</param> 
        /// <param name="node">当前结点，从根结构递归时此值必须=null</param> 
        /// <param name="nodeIndex">根结点图标(无子结点)</param> 
        /// <param name="parentIndex">有子结点的图标</param> 
        private  void SetImageIndex(TreeList tl, TreeListNode node, int nodeIndex, int parentIndex)
        {
            if (node == null)
            {
                foreach (TreeListNode N in tl.Nodes)
                    SetImageIndex(tl, N, nodeIndex, parentIndex);
            }
            else
            {
                if (node.HasChildren || node.ParentNode == null)
                {
                    //node.SelectImageIndex = parentIndex; 
                    node.StateImageIndex = parentIndex;
                    node.ImageIndex = parentIndex;
                }
                else
                {
                    //node.SelectImageIndex = nodeIndex; 
                    node.StateImageIndex = nodeIndex;
                    node.ImageIndex = nodeIndex;
                }

                foreach (TreeListNode N in node.Nodes)
                {
                    SetImageIndex(tl, N, nodeIndex, parentIndex);
                }
            }
        }



        /// <summary>
        /// 刷新
        /// </summary>
        public void RefreshWorkspace(bool isnewlinename)
        {
            try
            {
                List<TreeBean> treebData = null;
                TreeList treelist = this.treeListData;
                // treelist.Cursor = Cursors.WaitCursor;
                treebData = treelist.DataSource as List<TreeBean>;

                List<String> excelList = new List<string>();

                excelList = getFile(SystemInfo.DatabaseCache);

                foreach (string lineCode in excelList)
                {
                    try
                    {
                        string subLineCode = lineCode.Substring(0, lineCode.Length - 4);

                        TreeBean tb = new TreeBean();
                        tb.KeyFieldName = subLineCode;

                       
                        string obslinename = isnewlinename ? "OBSLINENAME" : "NAMEBEFORE";

                        tb.Caption = LineBll.Instance.GetNameByID(obslinename, "OBSLINECODE", subLineCode);
                        string sitecode = LineBll.Instance.GetNameByID("SITECODE", "OBSLINECODE", subLineCode);
                        if (string.IsNullOrEmpty(sitecode))
                            continue;
                        tb.ParentFieldName = sitecode;
                        tb.Tag = LineBll.Instance.GetInfoByID(subLineCode);
                        if (treebData.Find(n => n.KeyFieldName == subLineCode) == null)
                            treebData.Add(tb);
                    }
                    catch
                    {
                        continue;
                    }
                }

                treelist.RefreshDataSource();
                treelist.Cursor = Cursors.Arrow;
            }
            catch { }

        }



        private void GetObsDataByUser(string username)
        {
            //1.根据username查询权限，并放入list中
            List<string> userlist = new List<string>();
            // userlist = ;
            //2.遍历list下载数据



        }

       ///<summary>
       ///遍历获取文件夹下的文件名
       ///<\summary>
       ///
        private List<string> getFile(string SourcePath)
        {
            List<String> list = new List<string>();
            //遍历文件夹
            DirectoryInfo theFolder = new DirectoryInfo(SourcePath);
            FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo NextFile in thefileInfo)  //遍历文件
                //list.Add(NextFile.FullName);
                list.Add(NextFile.Name);
                
            //遍历子文件夹
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                //list.Add(NextFolder.ToString());
                FileInfo[] fileInfo = NextFolder.GetFiles("*.*", SearchOption.AllDirectories);
                foreach (FileInfo NextFile in fileInfo)  //遍历文件
                    list.Add(NextFile.Name);
                    //list.Add(NextFile.FullName);
            }
            return list;
        }

        //private void treeListRemoteData_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        //{
        //    if (e.Column == treeListColumn1)
        //    {
        //        if (e.CellValue.ToString()!="")
        //        {
        //            e.Appearance.BackColor = Color.LightGray;
        //            e.Appearance.Options.UseBackColor = true;
        //        }
        //    }
        //}

        //private void treeListLocalData_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        //{
        //    if (e.Column == treeListColumn4)
        //    {

        //        if (e.CellValue.ToString() != "")
        //        {
        //            e.Appearance.BackColor = Color.LightGray;
        //            e.Appearance.Options.UseBackColor = true;
        //        }
        //    }
        //}


        /// <summary>
        /// 禁止操作节点CheckBox
        /// 说明
        /// 在BeforeCheckNode事件中使用
        /// </summary>
        /// <param name="tree">TreeListNode</param>
        /// <param name="conditionHanlder">委托</param>
        /// <param name="e">CheckNodeEventArgs</param>
        private void treeListLocalData_BeforeCheckNode(object sender, CheckNodeEventArgs e)
        {
            e.CanCheck = false;
            //if ((bool)sender)
            //{
            //    e.CanCheck = true;
            //}

        }

        private void treeListRemoteData_BeforeCheckNode_1(object sender, CheckNodeEventArgs e)
        {
            e.CanCheck = false;
            //if ((bool) sender)
            //{
            //    e.CanCheck = true;
            //}
        }

        /// <summary>
        /// 获取选中的测线节点tag lwl
        /// </summary>
        /// <param name="treeType"></param>
        /// <returns></returns>
        public List<LineBean> GetCheckedLine(string treeType)
        {
            TreeList tree = null;
            if (treeType == "treeListData")
                tree = this.treeListData;
            else
                return null;

            List<LineBean> lblist = new List<LineBean>();
            try
            {
                foreach (TreeListNode dn in tree.Nodes)
                {
                    GetCheckedNode(dn, ref lblist);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lblist;
        }

        /// <summary>
        /// 获取处理数据树选中的节点
        /// </summary>
        /// <param name="treeType"></param>
        /// <returns></returns>
        public List<string> GetCheckedLineOnMuniTree(string treeType)
        {
            TreeList tree = null;
            if (treeType == "treeListManipData")
                tree = this.treeListManipData;

            List<string> lblist = new List<string>();
            try
            {
                foreach (TreeListNode dn in tree.Nodes)
                {
                    if (dn.CheckState == CheckState.Checked)
                    {
                        lblist.Add(dn.GetDisplayText(0));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lblist;

        }


        public List<string> GetSelectedLineOnMuniTree(string treeType)
        {
            TreeList tree = null;
            if (treeType == "treeListManipData")
                tree = this.treeListManipData;

            List<string> lblist = new List<string>();
            try
            {
                foreach (TreeListNode dn in tree.Nodes)
                {
                    if (dn.Selected)
                    {
                        lblist.Add(dn.GetDisplayText(0));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lblist;




        }

        public List<TreeListNode> GetNodesByKey(string treeType,string keyfieldname)
        {
            TreeList tree = null;
            if (treeType == "treeListData")
                tree = this.treeListData;

            List<TreeListNode> lblist = new List<TreeListNode>();
            try
            {
                foreach (TreeListNode dn in tree.Nodes)
                {
                    GetNodesRec(dn, keyfieldname, ref lblist);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lblist;

        }
        /// <summary>
        /// 获取测线选择状态的数据主键ID集合  lwl
        /// </summary>
        /// <param name="parentNode">父级节点</param>
        private void GetCheckedNode(TreeListNode parentNode, ref List<LineBean> lblist)
        {
            if (parentNode.Nodes.Count == 0)
            {
                return;//递归终止
            }

            foreach (TreeListNode node in parentNode.Nodes)
            {

                if (node.CheckState == CheckState.Checked)
                {
                    TreeBean nodeInfo = node.TreeList.GetDataRecordByNode(node) as TreeBean;
                    LineBean tag = nodeInfo.Tag as LineBean;
                    if (tag != null)
                    {
                        lblist.Add(tag);
                    }
                }
                GetCheckedNode(node, ref lblist);

            }

        }


        private void GetNodesRec(TreeListNode parentNode, string keyfieldname,ref List<TreeListNode> lblist)
        {
            if (parentNode.Nodes.Count == 0)
            {
                return;//递归终止
            }

            foreach (TreeListNode node in parentNode.Nodes)
            {
                TreeBean nodeInfo = node.TreeList.GetDataRecordByNode(node) as TreeBean;

                if (nodeInfo.KeyFieldName == keyfieldname)
                    lblist.Add(node);

                GetNodesRec(node, keyfieldname, ref lblist);

            }

        }

        /// <summary>
        /// 获取选中的场地节点tag lwl
        /// </summary>
        /// <param name="treeType"></param>
        /// <returns></returns>
        public List<SiteBean> GetCheckedSite(string treeType)
        {
            TreeList tree = null;
            if (treeType == "treeListData")
                tree = this.treeListData;

            List<SiteBean> lblist = new List<SiteBean>();
            try
            {
                foreach (TreeListNode dn in tree.Nodes)
                {
                    GetSiteCheckedNode(dn, ref lblist);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return lblist;

        }
        /// <summary>
        /// 获取场地选择状态的数据主键ID集合  lwl
        /// </summary>
        /// <param name="parentNode">父级节点</param>
        private void GetSiteCheckedNode(TreeListNode parentNode, ref List<SiteBean> lblist)
        {
            if (parentNode.Nodes.Count == 0)
            {
                return;//递归终止
            }

            foreach (TreeListNode node in parentNode.Nodes)
            {

                if (node.CheckState == CheckState.Checked)
                {
                    TreeBean nodeInfo = node.TreeList.GetDataRecordByNode(node) as TreeBean;
                    LineBean lb = nodeInfo.Tag as LineBean;
                    if (lb == null)
                    {
                        SiteBean tag = SiteBll.Instance.GetWhere(new { SITECODE = nodeInfo.Tag.ToString() }).ToList()[0];
                        if (tag != null)
                        {
                            lblist.Add(tag);
                        }
                    }
                }
                GetSiteCheckedNode(node, ref lblist);

            }

        }
        private void GetSiteNodesRec(TreeListNode parentNode, string keyfieldname, ref List<TreeListNode> lblist)
        {
            if (parentNode.Nodes.Count == 0)
            {
                return;//递归终止
            }

            foreach (TreeListNode node in parentNode.Nodes)
            {
                TreeBean nodeInfo = node.TreeList.GetDataRecordByNode(node) as TreeBean;

                if (nodeInfo.KeyFieldName == keyfieldname)
                    lblist.Add(node);

                GetSiteNodesRec(node, keyfieldname, ref lblist);
            }

        }

        public void ClearTreelistNodes()
        {
            this.treeListData.ClearNodes();
        }
    }
}
