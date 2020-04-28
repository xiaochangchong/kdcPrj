﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace xxkUI.Tool
{
    public class PublicHelper
    {
        /// <summary>
        /// 数据树结点信息
        /// </summary>
        public struct TreeListItemInfo
        {
            /// <summary>
            /// 结点名称
            /// </summary>
            public string itemName;
            /// <summary>
            /// 结点对应数据的类型,可为：table, comtable,folder等
            /// </summary>
            public string destType;
            /// <summary>
            /// 结点显示名称
            /// </summary>
            public string itemText;
            /// <summary>
            /// item的层次(在第几层结点)
            /// </summary>
            public int itemLevel;
            /// <summary>
            /// 正常图标索引
            /// </summary>
            public int imageIndex;
            /// <summary>
            /// 错误图标索引
            /// </summary>
            public int errorImageIndex;
            /// <summary>
            /// 数据信息表，刘文龙
            /// </summary>
            public string infoTblName;
        }

        public DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit CreateLookUpEdit(string[] values)
        {
            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rEdit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();

            DataTable dtTmp = new DataTable();
            dtTmp.Columns.Add("请选择");

            for (int i = 0; i < values.Length; i++)
            {
                DataRow drTmp1 = dtTmp.NewRow();
                drTmp1[0] = values[i];
                dtTmp.Rows.Add(drTmp1);
            }

            rEdit.DataSource = dtTmp;

            rEdit.ValueMember = "请选择";
            rEdit.DisplayMember = "请选择";
            rEdit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFit;
            rEdit.ShowFooter = false;
            rEdit.ShowHeader = false;
            return rEdit;
        }


        /// <summary>
        /// 创建当前时间字符串(构建删除文件名用)
        /// </summary>
        /// <returns></returns>
        public string CreateTimeStr()
        {
            string zipname = "";

            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string second = DateTime.Now.Second.ToString();

            if (month.Length < 2) month = "0" + month;
            if (day.Length < 2) day = "0" + day;
            if (hour.Length < 2) hour = "0" + hour;
            if (minute.Length < 2) minute = "0" + minute;
            if (second.Length < 2) second = "0" + second;

            zipname = year + month + day + hour + minute + second;

            return zipname;
        }

        /// <summary>
        /// 获取用户状态描述
        /// </summary>
        /// <param name="userstatuscode"></param>
        /// <returns></returns>
        public string GetUserStatusDiscription(string userstatuscode)
        {
            string discription = "";
            if (userstatuscode == "0")
                discription = "待审核";
            else if (userstatuscode == "1")
                discription = "通过审核";
            else if (userstatuscode == "2")
                discription = "未通过审核";
            else
                discription = "";
            return discription;
        }

        /// <summary>
        /// 获取用户状态值
        /// </summary>
        /// <param name="userstatusdiscripton"></param>
        /// <returns></returns>
        public string GetUserStatusCode(string userstatusdiscripton)
        {
            string discription = "";
            if (userstatusdiscripton == "待审核")
                discription = "0";
            else if (userstatusdiscripton == "通过审核")
                discription = "1";
            else if (userstatusdiscripton == "未通过审核")
                discription = "2";
            else
                discription = "";

            return discription;
        }



    }

    /// <summary>
    /// 数据操作分类
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 新增
        /// </summary>
        Add = 0,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 1,
        /// <summary>
        /// 修改
        /// </summary>
        Modify = 2,
        /// <summary>
        /// 无动做
        /// </summary>
        NoAction = 3
    }

    /// <summary>
    /// Series类型分类
    /// </summary>
    public enum SereisType
    {
        /// <summary>
        /// 曲线类型
        /// </summary>
        LineSeries = 0,
        /// <summary>
        /// 散点类型
        /// </summary>
        PointsSeries = 1,
        /// <summary>
        /// 未知类型
        /// </summary>
        UnknownSeris = 3
    }

    /// <summary>
    /// 导出的图片格式分类
    /// </summary>
    public enum ChartToImgType
    {
        jpg = 0,
        bmp = 1,
        png = 2
    }

    public enum LogType
    {
        Common, Right, Warning, Error
    }


    /// <summary>
    /// 数据源分类：原始信息库、本地信息库、处理数据
    /// </summary>
    public enum DataFromType
    {
        RemoteDb,  HandleData, Nothing
    }

    #region 数据处理
    /// <summary>
    /// 数据处理方法分类
    /// </summary>
    public enum DataProcessMethod
    {
        /// <summary>
        /// 加
        /// </summary>
        Plus,
        /// <summary>
        /// 减
        /// </summary>
        Minus,
        /// <summary>
        /// 乘
        /// </summary>
        Multiply,
        /// <summary>
        /// 除
        /// </summary>
        Divide,
        /// <summary>
        /// 消台阶
        /// </summary>
        RemoveStep,
        /// <summary>
        /// 消突跳
        /// </summary>
        RemoveJump,
        /// <summary>
        /// 无操作
        /// </summary>
        NoProg
    }

    public enum Left_Right
    {
        left, right, both
    }

    public enum MergenceSeparation
    {
        Mergence, Separation
    }

    /// <summary>
    /// 记录Tchart事件类型
    /// </summary>
    public enum TChartEventType
    {
        /// <summary>
        /// 热线
        /// </summary>
        Hotline,
        /// <summary>
        /// 消台阶
        /// </summary>
        RemoveStep,
        /// <summary>
        /// 消突跳
        /// </summary>
        RemoveJump,
        /// <summary>
        /// 测项拆分
        /// </summary>
        LineBreak,
        /// <summary>
        /// 显示差值
        /// </summary>
        DValue,
        /// <summary>
        /// 无操作
        /// </summary>
        NoProg
    }
    /// <summary>
    /// 观测值结构体
    /// x:观测时间
    /// y:观测值
    /// </summary>
    public struct ObsPoint
    {
        public double x;
        public double y;
    }

    #endregion

    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        /// 待审核
        /// </summary>
        Examining = 0,
        /// <summary>
        /// 通过审核
        /// </summary>
        ExamineOK = 1,
        /// <summary>
        /// 未通过审核
        /// </summary>
        ExamineReject = 2

    }

    public static class UserDicCls
    {
        public static Dictionary<UserStatus, string> UserDictionary = new Dictionary<UserStatus, string> { { UserStatus.Examining, "0" }, { UserStatus.ExamineOK, "1" }, { UserStatus.ExamineReject, "2" } };
    }


    /// <summary>
    /// 地震目录标注的结构
    /// </summary>
    public struct EqkAnotationStc
    {
        public DateTime dateTime;
        public double value;
        public string text;
    }

    /// <summary>
    /// 地图类型，标准地图和卫星地图
    /// </summary>
    public enum MapType
    {
        RoadMap, SatelliteMap
    }


    /// <summary>
    /// 系统配置信息
    /// </summary>
    public static class SystemInfo
    {
        public static string DatabaseCache= System.Windows.Forms.Application.StartupPath + "\\远程信息库缓存";//数据库缓存
        public static string HandleDataCache = System.Windows.Forms.Application.StartupPath + "\\处理数据缓存";//处理库缓存
        public static string PicturesCach= System.Windows.Forms.Application.StartupPath + "\\图片缓存";//图片缓存
        public static string DocCache= System.Windows.Forms.Application.StartupPath + "\\文档缓存";//文档缓存
        public static UserInfoBean CurrentUserInfo = null;//当前用户
    }



}
