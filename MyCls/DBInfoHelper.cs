using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace xxkUI.MyCls
{
    public class DBInfoHelper
    {
        public static DBInfo GetDbInfoFromFile(string infofile)
        {
            DBInfo dbif = new DBInfo();

            try
            {
                using (StreamReader sr = new StreamReader(infofile))
                {
                    string line;

                    int i = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        //取前四行未数据库连接信息
                        if (i == 0) dbif.server = line.Split('=')[1];
                        if (i == 1) dbif.port = line.Split('=')[1];
                        if (i == 2) dbif.database = line.Split('=')[1];
                        if (i == 3) dbif.uid = line.Split('=')[1];
                        if (i == 4) dbif.pwd = line.Split('=')[1];
                        i++;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }


            return dbif;
        }


        public static bool SaveDbInfo(DBInfo di, string infofile)
        {
            bool ok = false;
            using (StreamWriter sw = new StreamWriter(infofile))
            {
                sw.WriteLine("server="+di.server);
                sw.WriteLine("port=" + di.port);
                sw.WriteLine("database=" + di.database);
                sw.WriteLine("uid=" + di.uid);
                sw.WriteLine("pwd=" + di.pwd);

                ok = true;
            }

            return ok;
        }

    }


    /// <summary>
    /// 数据库信息结构体
    /// </summary>
    public struct DBInfo
    {
        public string server;
        public string port;
        public string database;
        public string uid;
        public string pwd;

        //"server=10.6.201.5;port=3306;database=kdcdb;uid=kdcadmin;pwd=jced123;Allow User Variables = True
    }
}
