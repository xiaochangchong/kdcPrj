using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Common.Data;

[TableName("t_obsrvtntb")]
[Description("测线观测信息")]

public class LineObsBean
{
    private string _obslinecode;
   
     [Description("测线编码")]
    public string obslinecode
    {
        get { return _obslinecode; }
        set { _obslinecode = value; }
    }
    private DateTime _obvdate;
    [Description("观测日期")]
    public DateTime obvdate
    {
        get { return _obvdate; }
        set { _obvdate = value; }
    }
    private double _obvvalue;
    [Description("数据")]
    public double obvvalue
    {
        get { return _obvvalue; }
        set { _obvvalue = value; }
    }
  
    private string _note;
    [Description("备注")]
    public string note
    {
        get { return _note; }
        set { _note = value; }
    }


}

