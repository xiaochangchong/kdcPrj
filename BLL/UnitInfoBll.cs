﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using xxkUI.Dal;
using Common.Provider;
using System.Data;

namespace xxkUI.BLL
{
    class UnitInfoBll
    {
        public static UnitInfoBll Instance
        {
            get { return SingletonProvider<UnitInfoBll>.Instance; }
        }

        public int Add(UnitInfoBean model)
        {
        
            return UnitInfoDal.Instance.Insert(model);
        }

        public int Update(UnitInfoBean model)
        {
            return UnitInfoDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return UnitInfoDal.Instance.Delete(keyid);
        }

        public int Delete(object where)
        {
            return UnitInfoDal.Instance.DeleteWhere(where);
        }


        public UnitInfoBean Get(int id)
        {
            return UnitInfoDal.Instance.Get(id);
        }


        public IEnumerable<UnitInfoBean> GetAll()
        {
            return UnitInfoDal.Instance.GetAll();
        }

        public DataTable GetDataTable(string sql)
        {
            return UnitInfoDal.Instance.GetDataTable(sql);
        }


        //public List<string> GeSiteByUnit<UnitInfoBean>(string UnitCode)
        //{
        //    string authstr = UnitInfoDal.Instance.GetByID("SITECODE", "USERNAME", UnitCode).ToString();
        //    string[] auths = authstr.Split(';');
        //    return auths.ToList();
        //}

        public string GetUnitNameBy(string _unitcode)
        {
            try
            {
                IEnumerable<UnitInfoBean> uibEnum = UnitInfoDal.Instance.GetList("select unitname from t_unittb where unitcode=@Unitcode", new { Unitcode = _unitcode });
                return uibEnum.ToList()[0].UnitName;
            }
            catch (Exception ex)
            { return ""; }
        }

        public string GetUnitCodeBy(string _unitname)
        {
            try
            {
                IEnumerable<UnitInfoBean> uibEnum = UnitInfoDal.Instance.GetList("select unitcode from t_unittb where unitname=@Unitname", new { Unitname = _unitname });
                return uibEnum.ToList()[0].UnitCode;
            }
            catch (Exception ex)
            { return ""; }
        }

    }
}
