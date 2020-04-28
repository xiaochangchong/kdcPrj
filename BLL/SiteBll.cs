using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.Provider;
using xxkUI.Dal;
using xxkUI.BLL;
using System.IO;

namespace xxkUI.Bll
{
    public class SiteBll
    {
        public static SiteBll Instance
        {
            get { return SingletonProvider<SiteBll>.Instance; }
        }

        public int Add(SiteBean model)
        {
            //model = new SiteBean();

            return SiteDal.Instance.Insert(model);
        }

        public int Update(SiteBean model)
        {
            return SiteDal.Instance.Update(model);
        }

        public int Delete(int keyid)
        {
            return SiteDal.Instance.Delete(keyid);
        }
        public int Delete(object where)
        {
            return SiteDal.Instance.DeleteWhere(where);
        }

        public SiteBean Get(int id)
        {
            return SiteDal.Instance.Get(id);
        }

        public IEnumerable<SiteBean> GetAll()
        {
            return SiteDal.Instance.GetAll();
        }

        public byte[] GetBlob<SiteBean>(string idname, string idvalue, string blobfield)
        {
            return SiteDal.Instance.GetBlob(idname, idvalue, blobfield);
        }

        public IEnumerable<SiteBean> GetWhere(object where)
        {
            return SiteDal.Instance.GetWhere(where);
        }
        public string GetNameByID(string name, string ID, string Value)
        {
            string nameStr = SiteDal.Instance.GetByID(name, ID, Value).ToString();
            return nameStr;
        }

        public IEnumerable<SiteBean> GetSitesByAuth(string auths)
        {
            return SiteDal.Instance.GetList(@"select * where UNITCODE in " + auths);
        }

        public IEnumerable<SiteBean> GetSitesByWhere(string where)
        {
            return SiteDal.Instance.GetList(@"select * from t_siteinfodb " + where);
        }


        public DataTable GetDataTable(string sql)
        {
            return SiteDal.Instance.GetDataTable(sql);
        }


        /// <summary>
        /// ���ݳ��ر����ȡ��������
        /// </summary>
        /// <param name="sitecode">���ر���</param>
        /// <returns>��������</returns>
        public string GetSitenameByID(string sitecode)
        {
            string sitename = SiteDal.Instance.GetByID("SITENAME", "SITECODE", sitecode).ToString();
            return sitename;
        }

        /// <summary>
        /// ���ݳ������ƻ�ȡ���ر���
        /// </summary>
        /// <param name="sitename">��������</param>
        /// <returns>���ر���</returns>
        public string GetSiteCodeByName(string sitename)
        {
            string sitecode = SiteDal.Instance.GetByID("SITECODE", "SITENAME", sitename).ToString();
            return sitecode;
        }

        /// <summary>
        /// �����ĵ�
        /// </summary>
        /// <param name="idname"></param>
        /// <param name="idvalue"></param>
        /// <param name="blobfield"></param>
        /// <returns></returns>
        public string DownloadDoc(string idname, string idvalue, string blobfield)
        {
            string filename = string.Empty;
            try
            {
                filename = System.Windows.Forms.Application.StartupPath + "/�ĵ�����/" + idvalue + ".doc";
                if (!File.Exists(filename))
                {
                    byte[] blob = GetBlob<SiteBean>(idname, idvalue, blobfield);
                    if (blob == null)
                        return string.Empty;
                    if (blob.Length == 0)
                        return string.Empty;

                    File.WriteAllBytes(filename, blob);
                }

            }
            catch (Exception ex)
            {
                filename = string.Empty;
                throw new Exception(ex.Message);
            }

            return filename;
        }

        /// <summary>
        /// ����ͼƬ
        /// </summary>
        /// <param name="idname"></param>
        /// <param name="idvalue"></param>
        /// <param name="blobfield"></param>
        /// <returns></returns>
        public string DownloadPic(string idname, string idvalue, string blobfield)
        {
            string filename = string.Empty;
            try
            {
                filename = System.Windows.Forms.Application.StartupPath + "/ͼƬ����/" + idvalue + ".jpg";
                if (!File.Exists(filename))
                {
                    byte[] blob = GetBlob<SiteBean>(idname, idvalue, blobfield);
                    if (blob == null)
                        return string.Empty;
                    if (blob.Length == 0)
                        return string.Empty;

                    File.WriteAllBytes(filename, blob);
                }

            }
            catch (Exception ex)
            {
                filename = string.Empty;
                throw new Exception(ex.Message);
            }

            return filename;
        }

        /// <summary>
        /// ������Ϣ��
        /// </summary>
        /// <param name="sitecode">���ر���</param>
        /// <param name="filename">�ĵ�·��</param>
        /// <returns>Ӱ������</returns>
        public int UpdateBASEINFO(string sitecode, string filename)
        {
            byte[] blobData = File.ReadAllBytes(filename);
            string sql = "update t_siteinfodb set BASEINFO = @blobData where SiteCode ='" + sitecode + "'";

            return SiteDal.Instance.Writeblob(sql, "blobData", blobData);
        }

        public int UpdateWhatWhere(object what, object where)
        {
            return SiteDal.Instance.UpdateWhatWhere(what, where);
        }

        public string CreateNewSiteCode(string ldordd)
        {
            string sitecodestr = "";

            try
            {
                IEnumerable<SiteBean> sblist = GetAll();

                int maxcode = 0;

                foreach (SiteBean dr in sblist)
                {
                    string sitecode = dr.SiteCode;
                    int code = int.Parse(sitecode.Substring(2, 3));
                    if (code > maxcode)
                        maxcode = code;
                }

                if ((maxcode + 1).ToString().Length == 1)
                {

                    sitecodestr = ldordd + "00" + (maxcode + 1).ToString();
                }
                if ((maxcode + 1).ToString().Length == 2)
                {
                    sitecodestr = ldordd + "0" + (maxcode + 1).ToString();
                }
                if ((maxcode + 1).ToString().Length == 3)
                {
                    sitecodestr = ldordd + (maxcode + 1).ToString();
                }

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return sitecodestr;
        }
    }
}
