using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.Dal;

namespace xxkUI.Bll
{
    public class LineBll
    {
        public static LineBll Instance
        {
            get { return SingletonProvider<LineBll>.Instance; }
        }

        public int Add(LineBean model)
        {
                       
            return LineDal.Instance.Insert(model);
        }


        public int Update(LineBean model)
        {
            return LineDal.Instance.Update(model);
        }

        public int UpdateWhatWhere(object what,object where)
        {
            return LineDal.Instance.UpdateWhatWhere(what,where);
        }

        public int Delete(int keyid)
        {
            return LineDal.Instance.Delete(keyid);
        }


        public int Delete(object where)
        {
            return LineDal.Instance.DeleteWhere(where);
        }

        public LineBean Get(int id)
        {
            return LineDal.Instance.Get(id);
            
        }

        public DataTable GetDataTable(string sql)
        {
            return LineDal.Instance.GetDataTable(sql);
        }



        public IEnumerable<LineBean> GetWhere(object where)
        {
            return LineDal.Instance.GetWhere(where);
        }
        public string GetNameByID(string getwhat, string idname, string idvalue)
        {
            return LineDal.Instance.GetByID(getwhat, idname, idvalue).ToString();
        }

        /// <summary>
        /// ���ݲ��߱����ȡ���ر���
        /// </summary>
        /// <param name="linecode"></param>
        /// <returns></returns>
        public string GetSitecodeByLinecode(string linecode)
        {
            try
            {
                List<LineBean> lblist = LineDal.Instance.GetAll().ToList();

                if (lblist != null)
                {
                    if (lblist.Count > 0)
                        return lblist.Find(n => n.OBSLINECODE == linecode).SITECODE;
                    else
                        throw new Exception("�Ҳ������߶�Ӧ�ĳ���");
                }
                else
                    throw new Exception("�Ҳ������߶�Ӧ�ĳ���");

            }
            catch (Exception e)
            {
                throw new Exception("�Ҳ������߶�Ӧ�ĳ���");
            }
        }

        /// <summary>
        /// ���ݲ��߱����ȡ������Ϣ
        /// </summary>
        /// <param name="idvalue">���߱���</param>
        /// <returns></returns>
        public LineBean GetInfoByID(string idvalue)
        {
            return LineDal.Instance.GetList("select * from t_obslinetb where OBSLINECODE = '" + idvalue + "'").ToList()[0];
        }

        /// <summary>
        /// ���ݳ��ر����ȡ���߼���
        /// </summary>
        /// <param name="sitecode">���ر���</param>
        /// <returns></returns>
        public IEnumerable<LineBean> GetBySitecode(string sitecode)
        {
            return LineDal.Instance.GetList("select * from t_obslinetb where SITECODE = '" + sitecode + "'").ToList();
        }

        public IEnumerable<LineBean> GetAll()
        {
            return LineDal.Instance.GetAll();
        }

        /// <summary>
        /// �����Ƿ����
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public bool IsExist(string id)
        {
            bool existed = false;

            if (LineDal.Instance.CountWhere(new { OBSLINENAME = id }) > 0)
                existed = true;
            else
                existed = false;

            return existed;
        }
        /// <summary>
        /// ���㳡�ز�����Ŀ
        /// </summary>
        /// <param name="sitecode">���ر���</param>
        /// <returns>������Ŀ</returns>
        public int GetCountBySiteCode(string sitecode)
        {
            object where = new { SiteCode = sitecode };
            return LineDal.Instance.CountWhere(where);
        }

        /// <summary>
        /// ͨ���������ƻ�ȡ���߱���
        /// </summary>
        /// <param name="linename">��������</param>
        /// <returns>���߱���</returns>
        public string GetIdByName(string linename)
        {
            return LineDal.Instance.GetWhere(new { OBSLINENAME = linename }).ToList()[0].OBSLINECODE;
        }

        /// <summary>
        /// ���ɲ��߱��� ������
        /// </summary>
        /// <param name="sitecode">���ر���</param>
        /// <returns>���߱���</returns>
        public string GenerateLineCode(string sitecode)
        {
            string linecode = string.Empty;
            try
            {
               int count= GetCountBySiteCode(sitecode)+1;
                if (count < 10)
                    linecode = sitecode + "00" + count.ToString();
                else if (count >= 10 && count < 100)
                    linecode = sitecode + "0" + count.ToString();
                else if (count >= 100)
                    linecode = sitecode + count.ToString();
            }
            catch(Exception ex) {
                throw new Exception(ex.Message);
            }

            return linecode;
        }
    }
}
