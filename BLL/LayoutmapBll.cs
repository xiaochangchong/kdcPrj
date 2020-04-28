using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.Dal;
using System.Web.Security;

namespace xxkUI.Bll
{
    public class LayoutmapBll
    {
        public static LayoutmapBll Instance
        {
            get { return SingletonProvider<LayoutmapBll>.Instance; }
        }

        public int Add(LayoutmapBean model)
        {
            return LayoutmapDal.Instance.Insert(model);
        }

        public int Update(LayoutmapBean model)
        {
            return LayoutmapDal.Instance.Update(model);
        }

        public int UpdateWhatWhere(object what, object where)
        {
            return LayoutmapDal.Instance.UpdateWhatWhere(what, where);
        }


        public int Delete(int keyid)
        {
            return LayoutmapDal.Instance.Delete(keyid);
        }

        public int Delete(object where)
        {
            return LayoutmapDal.Instance.DeleteWhere(where);
        }



        public LayoutmapBean Get(int id)
        {
            return LayoutmapDal.Instance.Get(id);
        }

        public IEnumerable<LayoutmapBean> GetAll()
        {
            return LayoutmapDal.Instance.GetAll();
        }

        public List<LayoutmapBean> GetLayoutmapBy(object where)
        {
            return LayoutmapDal.Instance.GetWhere(where).ToList();
        }

        public List<LayoutmapBean> GetLayoutmapBy(string sql)
        {
            return LayoutmapDal.Instance.GetList(sql).ToList();
        }


        public IEnumerable<LayoutmapBean> GetLayoutmapByWhere(string where)
        {
            return LayoutmapDal.Instance.GetList(@"select * from t_layoutmap " + where);
        }


        public IEnumerable<LayoutmapBean> GetWhere(object where)
        {
            return LayoutmapDal.Instance.GetWhere(where);
        }


        public DataTable GetDataTable(string sql)
        {
            return LayoutmapDal.Instance.GetDataTable(sql);
        }



        /// <summary>
        /// 根据场地编码获取场地图编码
        /// </summary>
        /// <param name="sitename">场地编码</param>
        /// <returns>场地图编码</returns>
        public string GetLayoutMapCodeBySiteCode(string sitecode)
        {
            string layoutcode = LayoutmapDal.Instance.GetByID("LAYOUTMAPCODE", "SITECODE", sitecode).ToString();
            return layoutcode;
        }

        public string CreateLayoutmapCode(int count)
        {
            string layoutmapcodestr = "";

            int maxcode = count + 1;

            if (maxcode.ToString().Length == 1)
            {
                layoutmapcodestr = "PIC" + "00" + (maxcode).ToString();
            }
            if (maxcode.ToString().Length == 2)
            {
                layoutmapcodestr = "PIC" + "0" + (maxcode).ToString();
            }
            if (maxcode.ToString().Length >= 3)
            {
                layoutmapcodestr = "PIC" + (maxcode).ToString();
            }

            if (GetLayoutmapBy(new { LAYOUTMAPCODE = layoutmapcodestr }).Count > 0)
            {
                return CreateLayoutmapCode(maxcode + 1);
            }
            else
                return layoutmapcodestr;

        }





    }
}
