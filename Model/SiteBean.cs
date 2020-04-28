using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Common.Data;

[TableName("t_siteinfodb")]
[Description("场地信息")]
public class SiteBean
{
    [Description("场地编码")]
    private string _SiteCode;
    public string SiteCode
    {
        get { return _SiteCode; }
        set { _SiteCode = value; }
    }
    [Description("场地名")]
    private string _SiteName;
    public string SiteName
    {
        get { return _SiteName; }
        set { _SiteName = value; }
    }

    [Description("场地类型")]
    private string _SiteType;
    public string SiteType
    {
        get { return _SiteType; }
        set { _SiteType = value; }
    }


    [Description("观测类型")]
    private string _ObsType;
    public string ObsType
    {
        get { return _ObsType; }
        set { _ObsType = value; }
    }

    [Description("历史迁移")]
    private string _Historysite;
    public string Historysite
    {
        get { return _Historysite; }
        set { _Historysite = value; }
    }


    [Description("经度")]
    private double _Longtitude;
    public double Longtitude
    {
        get { return _Longtitude; }
        set { _Longtitude = value; }
    }
    [Description("纬度")]
    private double _Latitude;
    public double Latitude
    {
        get { return _Latitude; }
        set { _Latitude = value; }
    }

    [Description("高程")]
    private string _Altitude;
    public string Altitude
    {
        get { return _Altitude; }
        set { _Altitude = value; }
    }

    [Description("所在地")]
    private string _Place;
    public string Place
    {
        get { return _Place; }
        set { _Place = value; }
    }

    [Description("所跨断层")]
    private string _FaultCode;
    public string FaultCode
    {
        get { return _FaultCode; }
        set { _FaultCode = value; }
    }

    [Description("起测试间")]
    private string _StartDate;
    public string StartDate
    {
        get { return _StartDate; }
        set { _StartDate = value; }
    }

    [Description("运行状况")]
    private string _SiteStatus;
    public string SiteStatus
    {
        get { return _SiteStatus; }
        set { _SiteStatus = value; }
    }


    [Description("单位代码")]
    private string _UnitCode;
    public string UnitCode
    {
        get { return _UnitCode; }
        set { _UnitCode = value; }
    }

    [Description("标石类型")]
    private string _MarkStoneType;
    public string MarkStoneType
    {
        get { return _MarkStoneType; }
        set { _MarkStoneType = value; }
    }

    [Description("建设单位")]
    private string _BuildUnit;
    public string BuildUnit
    {
        get { return _BuildUnit; }
        set { _BuildUnit = value; }
    }

    [Description("监测单位")]
    private string _ObsUnit;
    public string ObsUnit
    {
        get { return _ObsUnit; }
        set { _ObsUnit = value; }
    }

    [Description("场地概况")]
    private string _SiteSituation;
    public string SiteSituation
    {
        get { return _SiteSituation; }
        set { _SiteSituation = value; }
    }

    [Description("地质概况")]
    private string _GeoSituation;
    public string GeoSituation
    {
        get { return _GeoSituation; }
        set { _GeoSituation = value; }
    }


    [Description("备注")]
    private string _Note;
    public string Note
    {
        get { return _Note; }
        set { _Note = value; }
    }

    [Description("其他情况")]
    private string _OtherSituation;
    public string OtherSituation
    {
        get { return _OtherSituation; }
        set { _OtherSituation = value; }
    }

    [Description("资料变更")]
    private string _Datachg;
    public string Datachg
    {
        get { return _Datachg; }
        set { _Datachg = value; }
    }

     [Description("行政区编码")]
    private string _Xzqcode;
    public string Xzqcode
    {
        get { return _Xzqcode; }
        set { _Xzqcode = value; }
    }

    [Description("观测周期")]
    private string _ObsCyc;
    public string ObsCyc
    {
        get { return _ObsCyc; }
        set { _ObsCyc = value; }
    }



}
