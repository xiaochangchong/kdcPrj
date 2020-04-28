/***********************************************************/
//---模    块：测项合并对话框
//---功能描述：主要显示测量列表树
//---编码时间：2017-06-16
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/

using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
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
using xxkUI.Model;
using xxkUI.MyCls;
using xxkUI.Tool;

namespace xxkUI.Form
{
    public partial class frm_ObslineMerge : DevExpress.XtraEditors.XtraForm
    {
        public List<string> SelectedObsLineCode = new List<string>();//选中的测项编码
        public bool MoveToAverage = false;//是否移动到平均值处
        public string StartDateStr = "";
        public string EndDateStr = "";
        public frm_ObslineMerge()
        {
            InitializeComponent();
            InitTree();
        }



        /// <summary>
        /// 初始化测项树列表
        /// </summary>
        private void InitTree()
        {

            List<String> remoteExcelList = new List<string>();
            remoteExcelList = getFile(SystemInfo.DatabaseCache);

            List<TreeBean> tblist = new List<TreeBean>();
            foreach (string remoteLineCode in remoteExcelList)
            {
                try
                {
                    string subLineCode = remoteLineCode.Substring(0, remoteLineCode.Length - 4);
                    LineBean ol = LineBll.Instance.GetInfoByID(subLineCode);

                    TreeBean tb = new TreeBean();
                    tb.KeyFieldName = ol.OBSLINECODE;
                    tb.ParentFieldName = ol.SITECODE;
                    tb.Caption = ol.OBSLINENAME;
                    tb.Tag = ol;//lwl
                    tblist.Add(tb);
                }
                catch (Exception ex)
                {
                    continue;
                }

            }
                     this.treeListData.ClearNodes();
            this.treeListData.DataSource = tblist;
            //树列表显示
            this.treeListData.KeyFieldName = "KeyFieldName";          //这里绑定的ID的值必须是独一无二的
            this.treeListData.ParentFieldName = "ParentFieldName";  //表示使用parentID进行树形绑定
            this.treeListData.OptionsView.ShowCheckBoxes = true;
            this.treeListData.OptionsBehavior.AllowRecursiveNodeChecking = true;
            this.treeListData.OptionsBehavior.Editable = false;
            this.treeListData.ExpandAll();
     
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            frm_SelectPeriod frmSelectP = new frm_SelectPeriod();
            if (frmSelectP.ShowDialog() == DialogResult.Cancel)
                return;

            StartDateStr = frmSelectP.StartDateStr;
            EndDateStr = frmSelectP.EndDateStr;

            List<TreeBean> tblist = GetCheckedNode();
           
            if (tblist.Count==0)
            {
                XtraMessageBox.Show("未选中有效测项！", "提示");
                return;
            }

            try
            {

                foreach (TreeBean tb in tblist)
                {
                    SelectedObsLineCode.Add((tb.Tag as LineBean).OBSLINECODE);
                }

                this.MoveToAverage = (this.radioGroup.SelectedIndex == 0) ? true : false;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
                return;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



        private List<TreeBean> GetCheckedNode()
        {
            List<TreeBean> tblist = new List<TreeBean>();
            foreach (TreeListNode node in this.treeListData.Nodes)
            {
                if (node.CheckState == CheckState.Checked)
                {
                    TreeBean nodeInfo = node.TreeList.GetDataRecordByNode(node) as TreeBean;
                    tblist.Add(nodeInfo);
                }
            }

            return tblist;
        }



    }
}
