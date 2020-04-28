using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Common.Data;

[TableName("t_remotemap")]
[Description("卫星图")]

public class RemoteMapBean
{
    private string _remotemapcode;
    [Description("卫星图编码")]
    public string remotemapcode
    {
        get { return _remotemapcode; }
        set { _remotemapcode = value; }
    }

    private string _sitecode;
    [Description("场地编码")]
    public string sitecode
    {
        get { return _sitecode; }
        set { _sitecode = value; }
    }

    private string _remotemapname;
    [Description("卫星图名称")]
    public string remotemapname
    {
        get { return _remotemapname; }
        set { _remotemapname = value; }
    }

    private byte[] _remotemap;
    [Description("布设图")]
    public byte[] remotemap
    {
        get { return _remotemap; }
        set { _remotemap = value; }
    }
}

