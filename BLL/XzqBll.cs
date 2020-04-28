using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.DAL;

namespace xxkUI.BLL
{
   public class XzqBll
    {
        public static XzqBll Instance
        {
            get { return SingletonProvider<XzqBll>.Instance; }
        }

        public int Add(XzqBean model)
        {
            return XzqDal.Instance.Insert(model);
        }

        public int Update(XzqBean model)
        {
            return XzqDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return XzqDal.Instance.Delete(keyid);
        }

        public XzqBean Get(int id)
        {
            return XzqDal.Instance.Get(id);
        }

        public IEnumerable<XzqBean> GetAll()
        {
            return XzqDal.Instance.GetAll();
        }

 

        public IEnumerable<XzqBean> GetList(string sql)
        {
            return XzqDal.Instance.GetList(sql);
        }

        public int GetRecordCount(string strWhere)
        {
            return XzqDal.Instance.CountWhere(strWhere);
        }

        public IEnumerable<XzqBean> GetPage(string strWhere,int page,int pagesize)
        {
            return XzqDal.Instance.GetPage(strWhere, page, pagesize);
        }

        public IEnumerable<XzqBean> GetListByPage(string strWhere, string orderby)
        {
            string sqlstr = @"SELECT t.* FROM t_fault t WHERE " + strWhere;
            return XzqDal.Instance.GetList(sqlstr);
        }
    }
}
