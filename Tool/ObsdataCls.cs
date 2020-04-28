/***********************************************************/
//---模    块：观测数据静态类
//---功能描述：数据缓存及数据操作
//---编码时间：2017-06-06
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using xxkUI.Bll;

namespace xxkUI.Tool
{
    /// <summary>
    /// 存储，一直放在缓存中
    /// </summary>
    public static class ObsdataCls
    {
        /// <summary>
        /// 恢复数据表
        /// </summary>
        public static DataTable UndoTable = null;
        /// <summary>
        /// 重做数据表
        /// </summary>
        public static DataTable RedoTable = null;


        public static Hashtable ObsdataHash = new Hashtable();

        /// <summary>
        /// 记录是否已经存在
        /// </summary>
        /// <param name="linecode"></param>
        /// <returns></returns>
        public static bool IsExisted(string linecode)
        {
            bool isexisted = false;
            foreach (string key in ObsdataCls.ObsdataHash.Keys)
            {
                if (key == linecode)
                    isexisted = true;
            }
            return isexisted;
        }

        /// <summary>
        /// 从缓存中获取观测数据
        /// </summary>
        /// <param name="linecode"></param>
        /// <returns></returns>
        public static DataTable GetObsdataFromHash(string linecode)
        {
            return ObsdataHash[linecode] as DataTable;
        }


        /// <summary>
        /// 重新加载数据
        /// </summary>
        /// <param name="obslinecodes"></param>
        /// <param name="cachepath"></param>
        public static void  LoadData(List<string> obslinecodes,string cachepath)
        {
            if (obslinecodes.Count == 0)
                return;

            ObsdataCls.ObsdataHash.Clear();

            for (int i = 0; i < obslinecodes.Count; i++)
            {
                DataTable dt = LineObsBll.Instance.GetDataTable(obslinecodes[i], cachepath);
                DataView dataViewselec = dt.DefaultView;
                dataViewselec.Sort = "obvdate asc";
                dt = dataViewselec.ToTable();
                ObsdataCls.ObsdataHash.Add(GetKeyStr(cachepath,obslinecodes[i]), dt);
            }
        }

        /// <summary>
        /// 添加单个数据
        /// </summary>
        /// <param name="DtKey"></param>
        /// <param name="excelPath"></param>
        /// <param name="linecode"></param>
        public static void AddData(string DtKey, string excelPath, string linecode)
        {
            DataTable dt = LineObsBll.Instance.GetDataTable(linecode, excelPath);
            DataView dataViewselec = dt.DefaultView;
            dataViewselec.Sort = "obvdate asc";
            dt = dataViewselec.ToTable();
            ObsdataCls.ObsdataHash.Add(DtKey, dt);

        }


        /// <summary>
        /// 获取哈希表的key值
        /// </summary>
        /// <param name="cachepath"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetKeyStr(string cachepath, string name)
        {
            //解析哈希表key
            string[] dirName = cachepath.Split('\\');
            string DtKey = dirName[dirName.Length - 1] + "," + name;

            return DtKey;
        }


    }
}
