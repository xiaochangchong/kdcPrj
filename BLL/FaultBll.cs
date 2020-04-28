using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.DAL;
using System.Data;

namespace xxkUI.BLL
{
   public class FaultBll
    {
        public static FaultBll Instance
        {
            get { return SingletonProvider<FaultBll>.Instance; }
        }

        public int Add(FaultBean model)
        {
            return FaultDal.Instance.Insert(model);
        }

        public int Update(FaultBean model)
        {
            return FaultDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return FaultDal.Instance.Delete(keyid);
        }

        public FaultBean Get(int id)
        {
            return FaultDal.Instance.Get(id);
        }

        public IEnumerable<FaultBean> GetAll()
        {
            return FaultDal.Instance.GetAll();
        }

        public string GetNameByID(string getwhat, string idname, string idvalue)
        {
            return FaultDal.Instance.GetByID(getwhat, idname, idvalue).ToString();
        }

        public IEnumerable<FaultBean> GetList(string sql)
        {
            return FaultDal.Instance.GetList(sql);
        }

        public int GetRecordCount(string strWhere)
        {
            return FaultDal.Instance.CountWhere(strWhere);
        }

        public IEnumerable<FaultBean> GetPage(string strWhere,int page,int pagesize)
        {
            return FaultDal.Instance.GetPage(strWhere, page, pagesize);
        }

        public IEnumerable<FaultBean> GetListByPage(string strWhere, string orderby)
        {
            string sqlstr = @"SELECT t.* FROM t_fault t WHERE " + strWhere;
            return FaultDal.Instance.GetList(sqlstr);
        }

        public DataTable GetDataTable(string sql)
        {
            return FaultDal.Instance.GetDataTable(sql);
        }
    }
}
