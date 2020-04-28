using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.Dal;
using System.IO;
using xxkUI.Tool;
using System.Drawing;

namespace xxkUI.Bll
{
    public class LineObsBll
    {
        public static LineObsBll Instance
        {
            get { return SingletonProvider<LineObsBll>.Instance; }
        }

        public int Add(LineObsBean model)
        {
           return LineObsDal.Instance.Insert(model);
        }

        public int Update(LineObsBean model)
        {
            return LineObsDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return LineObsDal.Instance.Delete(keyid);
        }

        public int Delete(object where)
        {
            return LineObsDal.Instance.DeleteWhere(where);
        }



        public LineObsBean Get(int id)
        {
            return LineObsDal.Instance.Get(id);
        }

        public DataTable GetDataTable(string sql)
        {
            return LineObsDal.Instance.GetDataTable(sql);
        }
        public DataTable GetDataTable(string linecode, string path)
        {
            DataTable dt = null;

            string filename = path + "\\" + linecode + ".xls";
            if (File.Exists(filename))
            {
                NpoiCreator npcreator = new NpoiCreator();
                dt = npcreator.ExcelToDataTable_LineObs(filename, true);
                //dt = npcreator.ExportToObslineDataTable(filename);
            }
            else
            {
                dt = LineObsBll.Instance.GetDataTable("select DATE_FORMAT(obvdate,'%Y-%m-%d') as obvdate,obvvalue,note from t_obsrvtntb where OBSLINECODE = '" + linecode + "'");
                if (dt.Rows.Count > 0)
                {
                    NpoiCreator npcreator = new NpoiCreator();
                    npcreator.TemplateFile = path;
                    npcreator.NpoiExcel(dt, path + "/" + linecode + ".xls");
                }

            }

            return dt;
        }

        public IEnumerable<LineObsBean> GetAll()
        {
            return LineObsDal.Instance.GetAll();
        }

        /// <summary>
        /// 是否存在相同记录
        /// </summary>
        /// <param name="obvdate"></param>
        /// <param name="obslinecode"></param>
        /// <returns></returns>
        public bool IsExist(string obvdate,double obsvalue, string obslinecode)
        {
            bool isexist = false;

            string sql = "select * from t_obsrvtntb where obvdate = '" + obvdate + "' and obslinecode='" + obslinecode + "' and obvvalue = " + obsvalue;

            DataTable dt = null;

            try
            {
                dt = LineObsDal.Instance.GetDataTable(sql);

                if (dt.Rows.Count > 0)
                    isexist = true;
                else
                    isexist = false;
            }
            catch
            {

            }
            

            return isexist;
           
        }

        internal List<string> GetNameByID(string p1, string p2, string lineCode)
        {
            throw new NotImplementedException();
        }
    }
}
