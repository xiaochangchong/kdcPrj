using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Common.Data;

[TableName("t_xzq")]
[Description("行政区表")]
public class XzqBean
{
    
    
    private string xzqcode;
    private string xzqname;
    private string xqzlocations;
    [Description("行政区编码")]
    public string Xzqcode
    {
        get
        {
            return xzqcode;
        }

        set
        {
            xzqcode = value;
        }
    }
    [Description("行政区名称")]
    public string Xzqname
    {
        get
        {
            return xzqname;
        }

        set
        {
            xzqname = value;
        }
    }
    [Description("行政区坐标")]
    public string Xqzlocations
    {
        get
        {
            return xqzlocations;
        }

        set
        {
            xqzlocations = value;
        }
    }
}

