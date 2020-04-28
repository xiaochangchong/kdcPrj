/***********************************************************/
//---模    块：断层速率合成指标
//---功能描述：载入数据，设置参数
//---编码时间：2018-11-01
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using xxkUI.Tool;

namespace xxkUI.AnalysisMethods
{
    public class Slhczb
    {
        private int m_Interval = 0;//观测周期
        struct UnionRate
        {
            public int rateDate;
            public double rateValue;
            public int rateCount;
        }

        public DataTable ConvertExcelToDbs(string excelpath)
        {
            if (string.IsNullOrEmpty(excelpath))
                return null;

            NpoiCreator nct = new NpoiCreator();
            DirectoryInfo mydir = new DirectoryInfo(excelpath);
            if (mydir.GetFiles().Length > 0)
            {
                List<DataTable> dblist = new List<DataTable>();
                foreach (FileSystemInfo fsi in mydir.GetFileSystemInfos())
                {
                    if (fsi is FileInfo)//
                    {
                        FileInfo fi = (FileInfo)fsi;
                        if (fi.Extension.ToUpper() == ".XLS")//
                        {
                            dblist.Add(nct.ExportToDataTable(fi.FullName));
                        }
                    }
                }
                return ExportResult(dblist);
            }
            else
            {
                return null;
            }

        }


        /// <summary>
        /// 将XML格式的字符串转化为DataTable
        /// </summary>
        /// <param name="xmlStr">XML格式的字符串</param>
        /// <returns>返回：DataTable</returns>
        public DataTable XmlStrToData(String xmlStr)
        {
            XmlTextReader reader = null;
            try
            {
                xmlStr = xmlStr.Replace("换行符", "\r\n").Replace("~", "/");
                DataSet xmlds = new DataSet();
                StringReader stream = new StringReader(xmlStr);
                reader = new XmlTextReader(stream);
                xmlds.ReadXml(reader);

                if (reader != null)
                {
                    reader.Close();
                }

                return xmlds.Tables[0];
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                return null;
            }
        }


        /// <summary>
        /// 输出计算结果
        /// </summary>
        /// <param name="inputDbs">输入数据集</param>
        /// <returns>计算结果</returns>
        public DataTable ExportResult(List<DataTable> inputDbs)
        {
            DataTable resDb = null;

            List<DataTable> rateDbs = new List<DataTable>();

            foreach (DataTable dt in inputDbs)
            {
                rateDbs.Add(CalculateRateDb(dt));
            }
            resDb = UnionRateDbs(rateDbs);

            return resDb;
        }


        /// <summary>
        /// 计算单个测项速率
        /// </summary>
        /// <param name="inputdb">测项数据</param>
        /// <returns>速率结果</returns>
        private DataTable CalculateRateDb(DataTable inputdb)
        {
            DataTable rateDb = new DataTable("rate");
            DataColumn dc1 = new DataColumn("ratedate", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("ratevalue", Type.GetType("System.Double"));
            rateDb.Columns.Add(dc1);
            rateDb.Columns.Add(dc2);

            try
            {

                DateTime firstDt = DateTime.ParseExact(inputdb.Rows[0][0].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);//获取第一行时间列
                int startyear = firstDt.Year;//起始年份
                int startmonth = firstDt.Month;//起始月份

                DateTime secondDt = DateTime.ParseExact(inputdb.Rows[1][0].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);//获取第二行时间列

                m_Interval = ObvCycle(inputdb);

                for (int i = 0; i < inputdb.Rows.Count; i++)
                {
                    if (i + m_Interval >= inputdb.Rows.Count)//遍历到尽头
                        break;

                    DataRow dr = inputdb.Rows[i];
                    DataRow dr2 = inputdb.Rows[i + m_Interval];

                    //构建时间字符串
                    DateTime obvdate2 = DateTime.ParseExact(dr2[0].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);//获取下一年对应数据
                    string monthstr2 = obvdate2.Month.ToString();
                    if (monthstr2.Length == 1)
                        monthstr2 = "0" + monthstr2;

                    //填充数据
                    DataRow ratedr = rateDb.NewRow();
                    ratedr["ratedate"] = obvdate2.Year.ToString() + monthstr2;//速率时间统一用后一个时间
                    ratedr["ratevalue"] = double.Parse(dr2[1].ToString()) - double.Parse(dr[1].ToString());
                    rateDb.Rows.Add(ratedr);
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.ToString());
            }

            return rateDb;
        }


        /// <summary>
        /// 计算测项观测周期
        /// </summary>
        /// <param name="inputdb">输入数据集</param>
        /// <returns></returns>
        private int ObvCycle(DataTable inputdb)
        {
            int obvcycle = 0;

            List<int> obvcs = new List<int>();

            try
            {
                DateTime firstDt = DateTime.ParseExact(inputdb.Rows[0][0].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);//获取第一行时间列
                int startyear = firstDt.Year;//起始年份

                int n = 0;//计数器
                for (int i = 0; i < inputdb.Rows.Count; i++)
                {
                    DateTime daterow = DateTime.ParseExact(inputdb.Rows[i][0].ToString(), "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    int year = daterow.Year;
                    if (year == startyear)
                        n++;
                    else
                    {
                        obvcs.Add(n);
                        n = 0;
                        startyear = year;
                        n++;
                    }
                }

                int total = 0;
                for (int i = 0; i < obvcs.Count; i++)
                {
                    total += obvcs[i];
                }
                double aver = total / obvcs.Count;
                obvcycle = int.Parse(Math.Ceiling(aver).ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }

            return obvcycle;
        }

        /// <summary>
        /// 速率集合成
        /// </summary>
        /// <param name="ratedbs">速率集</param>
        /// <returns>合成数据</returns>
        private DataTable UnionRateDbs(List<DataTable> ratedbs)
        {
            DataTable unionDb = new DataTable("union");
            DataColumn dc1 = new DataColumn("uniondate", Type.GetType("System.String"));
            DataColumn dc2 = new DataColumn("unionvalue", Type.GetType("System.Double"));
            unionDb.Columns.Add(dc1);
            unionDb.Columns.Add(dc2);

            //获取最大时间和最小时间（用于确定遍历范围）
            int[] max_min = GetMaxMinDate(ratedbs);
            int min = max_min[1];
            int max = max_min[0];

            List<UnionRate> unionratelist = new List<UnionRate>();
            for (int i = min; i <= max; i++)
            {
                string istr = i.ToString();
                int month = int.Parse(istr.Substring(istr.Length - 2, 2));
                if (month > 12 || month == 0)//计数器超过12个月份
                    continue;
                UnionRate ur = new UnionRate();
                ur.rateDate = i;
                foreach (DataTable dt in ratedbs)//遍历每个速率表
                {
                    var var_dtTable = dt.AsEnumerable().Where<DataRow>(W => W["ratedate"].ToString() == i.ToString());
                    foreach (DataRow dr in var_dtTable)
                    {

                        ur.rateValue += double.Parse(dr[1].ToString());
                        ur.rateCount++;
                    }
                }

                if (ur.rateCount == 0)
                    ur.rateValue = 0;
                else
                    ur.rateValue = ur.rateValue / ur.rateCount;//同一期数据速率平均值

                unionratelist.Add(ur);
            }

            foreach (UnionRate ur in unionratelist)
            {
                if (ur.rateDate.ToString() != "0")
                {
                    DataRow addedDr = unionDb.NewRow();
                    addedDr[0] = ur.rateDate;
                    addedDr[1] = ur.rateValue;
                    unionDb.Rows.Add(addedDr);
                }
            }
            return unionDb;
        }

        /// <summary>
        /// 获取数据集最大最小时间
        /// </summary>
        /// <param name="ratedbs">速率集合</param>
        /// <returns>最大最小时间数组</returns>
        private int[] GetMaxMinDate(List<DataTable> ratedbs)
        {
            int[] maxmin = new int[2];

            int max = int.MinValue, min = int.MaxValue;

            for (int i = 0; i < ratedbs.Count; i++)
            {
                for (int j = 0; j < ratedbs[i].Rows.Count; j++)
                {
                    int date = int.Parse(ratedbs[i].Rows[j][0].ToString());
                    if (date > max)
                        max = date;
                    if (date < min)
                        min = date;
                }
            }

            maxmin[0] = max;
            maxmin[1] = min;
            return maxmin;
        }
    }
}
