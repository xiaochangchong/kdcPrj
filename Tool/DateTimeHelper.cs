using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xxkUI.Tool
{
   public static class DateTimeHelper
    {

        /// <summary>
        /// 字符串转为时间格式
        /// </summary>
        /// <param name="datestr">输入字符串</param>
        /// <returns></returns>
        public static DateTime ConvertToDateTime(string datestr)
        {
            DateTime dt = new DateTime();
            try
            {
                string sf = string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2}", Regex.Matches(datestr, "\\d+").Cast<Match>().Select(x => (object)int.Parse(x.Value)).ToArray());
                dt = Convert.ToDateTime(sf);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return dt;


        }
    }
}
