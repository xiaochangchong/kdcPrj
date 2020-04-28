using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Common.Data;

[TableName("t_fault")]
[Description("断层数据")]
public class FaultBean
{
    [Description("编码")]
    public int Faultcode
    {
        get { return faultcode; }
        set { faultcode = value; }
    }

    [Description("名称")]
    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }
    [Description("活动性质")]
    public string Activitynature
    {
        get
        {
            return activitynature;
        }

        set
        {
            activitynature = value;
        }
    }
    [Description("长度")]
    public string Length
    {
        get
        {
            return length;
        }

        set
        {
            length = value;
        }
    }
    [Description("形成时代")]
    public string Faultage
    {
        get
        {
            return faultage;
        }

        set
        {
            faultage = value;
        }
    }
    [Description("最新活动时代")]
    public string Newestactivetime
    {
        get
        {
            return newestactivetime;
        }

        set
        {
            newestactivetime = value;
        }
    }
    [Description("地震活动")]
    public string Seismicactivity
    {
        get
        {
            return seismicactivity;
        }

        set
        {
            seismicactivity = value;
        }
    }
    [Description("产状")]
    public string Faultoccurrences
    {
        get
        {
            return faultoccurrences;
        }

        set
        {
            faultoccurrences = value;
        }
    }
    [Description("水平断距")]
    public string Levelseparation
    {
        get
        {
            return levelseparation;
        }

        set
        {
            levelseparation = value;
        }
    }
    [Description("垂直断距")]
    public string Verticalseparation
    {
        get
        {
            return verticalseparation;
        }

        set
        {
            verticalseparation = value;
        }
    }
    [Description("走滑速率")]
    public string Strikesliprate
    {
        get
        {
            return strikesliprate;
        }

        set
        {
            strikesliprate = value;
        }
    }
    [Description("倾滑速率")]
    public string Dipsliprate
    {
        get
        {
            return dipsliprate;
        }

        set
        {
            dipsliprate = value;
        }
    }
    [Description("古地震_大_小_总数")]
    public string Ancientearthquake
    {
        get
        {
            return ancientearthquake;
        }

        set
        {
            ancientearthquake = value;
        }
    }
    [Description("断裂带特征_结构")]
    public string Faultzonechrtrstics_structure
    {
        get
        {
            return faultzonechrtrstics_structure;
        }

        set
        {
            faultzonechrtrstics_structure = value;
        }
    }
    [Description("地貌特征_断崖水系")]
    public string Geomorphicfeature_bluffdrainage
    {
        get
        {
            return geomorphicfeature_bluffdrainage;
        }

        set
        {
            geomorphicfeature_bluffdrainage = value;
        }
    }
    [Description("地球物理特征")]
    public string Geophysicalcharacteristics
    {
        get
        {
            return geophysicalcharacteristics;
        }

        set
        {
            geophysicalcharacteristics = value;
        }
    }
    [Description("火山喷发")]
    public string Volcaniceruption
    {
        get
        {
            return volcaniceruption;
        }

        set
        {
            volcaniceruption = value;
        }
    }
    [Description("岩浆侵入")]
    public string Magmaticintrusion
    {
        get
        {
            return magmaticintrusion;
        }

        set
        {
            magmaticintrusion = value;
        }
    }
    [Description("确定断层活动依据")]
    public string Determinethebasisoffaultactivity
    {
        get
        {
            return determinethebasisoffaultactivity;
        }

        set
        {
            determinethebasisoffaultactivity = value;
        }
    }
    [Description("坐标")]
    public string Locations
    {
        get
        {
            return locations;
        }

        set
        {
            locations = value;
        }
    }

    
    private int faultcode;
    private string name;
    private string activitynature;
    private string length;
    private string faultage;
    private string newestactivetime;
    private string seismicactivity;
    private string faultoccurrences;
    private string levelseparation;
    private string verticalseparation;
    private string strikesliprate;
    private string dipsliprate;
    private string ancientearthquake;
    private string faultzonechrtrstics_structure;
    private string geomorphicfeature_bluffdrainage;
    private string geophysicalcharacteristics;
    private string volcaniceruption;
    private string magmaticintrusion;
    private string determinethebasisoffaultactivity;
    private string locations;



}

