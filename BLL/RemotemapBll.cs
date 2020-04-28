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
    public class RemotemapBll
    {
        public static RemotemapBll Instance
        {
            get { return SingletonProvider<RemotemapBll>.Instance; }
        }

        public int Add(RemoteMapBean model)
        {
            return RemotemapDal.Instance.Insert(model);
        }

        public int Update(RemoteMapBean model)
        {
            return RemotemapDal.Instance.Update(model);
        }

        public int UpdateWhatWhere(object what, object where)
        {
            return RemotemapDal.Instance.UpdateWhatWhere(what, where);
        }


        public int Delete(int keyid)
        {
            return RemotemapDal.Instance.Delete(keyid);
        }


        public int Delete(object where)
        {
            return RemotemapDal.Instance.DeleteWhere(where);
        }


        public RemoteMapBean Get(int id)
        {
            return RemotemapDal.Instance.Get(id);
        }

        public IEnumerable<RemoteMapBean> GetAll()
        {
            return RemotemapDal.Instance.GetAll();
        }


        public DataTable GetDataTable(string sql)
        {
            return RemotemapDal.Instance.GetDataTable(sql);
        }



        public List<RemoteMapBean> GetRemotemapBy(object where)
        {
            return RemotemapDal.Instance.GetWhere(where).ToList();
        }


        public IEnumerable<RemoteMapBean> GetWhere(object where)
        {
            return RemotemapDal.Instance.GetWhere(where);
        }

   


        /// <summary>
        /// ∏˘æ›≥°µÿ±‡¬ÎªÒ»°Œ¿–«Õº±‡¬Î
        /// </summary>
        /// <param name="sitename">≥°µÿ±‡¬Î</param>
        /// <returns>Œ¿–«Õº±‡¬Î</returns>
        public string GetRemoteMapCodeBySiteCode(string sitecode)
        {
            string remotecode = RemotemapDal.Instance.GetByID("REMOTEMAPCODE", "SITECODE", sitecode).ToString();
            return remotecode;
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

            if (GetRemotemapBy(new { REMOTEMAPCODE = layoutmapcodestr }).Count > 0)
            {
                return CreateLayoutmapCode(maxcode + 1);
            }
            else
                return layoutmapcodestr;

        }





    }
}
