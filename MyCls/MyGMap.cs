/***********************************************************/
//---模    块：GMap地图操作类
//---功能描述：地图初始化、重载、缩放、全图、场地地震等要素加载和清除
//---编码时间：2017-5-22
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GsProject;
using xxkUI.GsProject;
using System.Threading;
using xxkUI.Bll;
using System.Drawing.Drawing2D;
using xxkUI.Tool;
using xxkUI.Form;
using System.Data;
using System.Runtime.Serialization;
using GMap.NET.ObjectModel;

namespace xxkUI.MyCls
{
    public enum MapToolType
    {
        None,
        Marker,
        AlarmMarker,
        Rectangle,
        Circle,
        Polygon,
        DownRectangle,
        Chart,
        Route
    }


    public static class MyGMap {

        /// <summary>
        /// 地图中心点
        /// </summary>
        private static readonly PointLatLng chinaCenter = new PointLatLng(35, 107.5);

        /// <summary>
        /// 初始化地图
        /// </summary>
        /// <param name="gMapCtrl">GMap控件</param>
        /// <param name="maptype">地图类型</param>
        /// <param name="isFocus">是否定位到地图中心</param>
        /// <returns></returns>
        public static bool InitMap(GMapControl gMapCtrl, MapType maptype,bool isFocus)
        {
            bool isOk = false;
            try
            {
                //在线模式
                gMapCtrl.Manager.Mode = AccessMode.ServerOnly;
               
                if (maptype == MapType.RoadMap)
                {
                    //设置控件显示的地图来源  
                    gMapCtrl.MapProvider = GMapProviders.GoogleChinaMap;
                    
                    //gmdb离线地图数据
                    //string mapfile = System.Windows.Forms.Application.StartupPath + "\\GMap\\RoadMap\\Data.gmdb";

                    //载入离线地图
                    MapManagerLoader.Instance.Load("http://10.7.201.2:8090/offlineMap/googlemaps/roadmap/");
                }
                else if (maptype == MapType.SatelliteMap)
                {
                    //设置控件显示的地图来源  
                    gMapCtrl.MapProvider = GMapProviders.GoogleChinaSatelliteMap;
                    //gmdb离线地图数据
                    //string mapfile = System.Windows.Forms.Application.StartupPath + "\\GMap\\SatelliteMap\\Data.gmdb";
                    //载入离线地图
                    MapManagerLoader.Instance.Load("http://10.7.201.2:8090/offlineMap/googlemaps/satellite/");
                }
                if (isFocus)
                {
                    //不显示中心十字点  
                    gMapCtrl.ShowCenter = false;
                    //右键拖拽地图  
                    gMapCtrl.DragButton = System.Windows.Forms.MouseButtons.Right;
                   
                    //设置控件显示的当前中心位置
                    gMapCtrl.Position = chinaCenter;
                    //设置控件最大的缩放比例
                    gMapCtrl.MaxZoom = 18;
                    //设置控件最小的缩放比例
                    gMapCtrl.MinZoom = 3;
                    //设置控件当前的缩放比例
                    gMapCtrl.Zoom = 5;
                }
                
                isOk = true;
            }
            catch (Exception ex)
            {
                isOk = false;
                throw;
            }

            return isOk;
        }

        /// <summary>
        /// 重载地图
        /// </summary>
        public static void ReloadMap(GMapControl gMapCtrl)
        {
            gMapCtrl.ReloadMap();
        }

        /// <summary>
        /// 地图缩放
        /// </summary>
        /// <param name="scale">缩放级别</param>
        public static void Zoom(int scale, GMapControl gMapCtrl)
        {
            gMapCtrl.Zoom += scale;
        }

        /// <summary>
        /// 全图
        /// </summary>
        public static void Full(GMapControl gMapCtrl)
        {
            gMapCtrl.Position = chinaCenter;
            //设置控件当前的缩放比例  
            gMapCtrl.Zoom = 4;
        }

        /// <summary>
        /// 坐标转经纬度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static PointLatLng FromLocalToLatLng(int x, int y, GMapControl gMapCtrl)
        {
            return gMapCtrl.FromLocalToLatLng(x, y);
        }

        /// <summary>
        /// 加载场地标记
        /// </summary>
        /// <param name="sblist"></param>
        public static void AddSiteMarker(IEnumerable<SiteBean> sblist, GMapControl gMapCtrl)
        {

            GMapOverlay SiteOverlay = IsLayerExisted("sitemarkers", gMapCtrl) ? GetOverlay("sitemarkers", gMapCtrl) : new GMapOverlay("sitemarkers");
            if (!IsLayerExisted("sitemarkers", gMapCtrl))
                gMapCtrl.Overlays.Add(SiteOverlay);


          
            int i = 0;

            foreach (SiteBean sb in sblist)
            {
                GMapMarker marker = null;

                if (sb.SiteCode.Substring(0, 1) == "L")//流动
                {
                    string picPath = System.Windows.Forms.Application.StartupPath + "//图片缓存//场地类型图片//";

                    if (sb.ObsType == "水准")
                    {
                        picPath += "ldsz.png";
                    }
                    else if (sb.ObsType == "基线")
                    {
                        picPath += "ldjx.png";
                    }
                    else if (sb.ObsType == "综合")
                    {
                        picPath += "ldzh.png";
                    }
                    else
                    {
                        picPath += "ldsz.png";
                    }
                    Bitmap markerPic = new Bitmap(picPath);
                   
                    //double[] correctLocation = GpsCorrect.transform(sb.Latitude, sb.Longtitude);
                    marker = new GMarkerGoogle(new PointLatLng(sb.Latitude, sb.Longtitude), markerPic);
                }
                else//定点
                {
                    string picPath = System.Windows.Forms.Application.StartupPath + "//图片缓存//场地类型图片//";

                    if (sb.ObsType == "水准")
                    {
                        picPath += "ddsz.png";
                    }
                    else if (sb.ObsType == "基线")
                    {
                        picPath += "ddjx.png";
                    }
                    else if (sb.ObsType == "综合")
                    {
                        picPath += "ddzh.png";
                    }
                    else
                    {
                        picPath += "ddsz.png";
                    }
                    Bitmap markerPic = new Bitmap(picPath);
                    marker = new GMarkerGoogle(new PointLatLng(sb.Latitude, sb.Longtitude), markerPic);
                }
                //marker.Tag = sb.SiteCode;
                marker.Tag = sb;

                marker.ToolTipText = sb.SiteName;
                marker.ToolTip.Offset = new Point(10, -20);

                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
             
                SiteOverlay.Markers.Add(marker);
                i++;
            }
           

        }

        /// <summary>
        /// 加载单个场地标记
        /// </summary>
        /// <param name="sblist"></param>
        /// <param name="gMapCtrl"></param>
        public static void AddOneSiteMarker(SiteBean sb, GMapControl gMapCtrl)
        {

            GMapOverlay SiteOverlay = IsLayerExisted("sitemarkers", gMapCtrl) ? GetOverlay("sitemarkers", gMapCtrl) : new GMapOverlay("sitemarkers");
            if (!IsLayerExisted("sitemarkers", gMapCtrl))
                gMapCtrl.Overlays.Add(SiteOverlay);

            GMapMarker marker = null;

            if (sb.SiteCode.Substring(0, 1) == "L")//流动
            {
                string picPath = System.Windows.Forms.Application.StartupPath + "//图片缓存//场地类型图片//";

                if (sb.ObsType == "水准")
                {
                    picPath += "ldsz.png";
                }
                else if (sb.ObsType == "基线")
                {
                    picPath += "ldjx.png";
                }
                else if (sb.ObsType == "综合")
                {
                    picPath += "ldzh.png";
                }
                else
                {
                    picPath += "ldsz.png";
                }
                Bitmap markerPic = new Bitmap(picPath);

                //double[] correctLocation = GpsCorrect.transform(sb.Latitude, sb.Longtitude);
                marker = new GMarkerGoogle(new PointLatLng(sb.Latitude, sb.Longtitude), markerPic);
            }
            else//定点
            {
                string picPath = System.Windows.Forms.Application.StartupPath + "//图片缓存//场地类型图片//";

                if (sb.ObsType == "水准")
                {
                    picPath += "ddsz.png";
                }
                else if (sb.ObsType == "基线")
                {
                    picPath += "ddjx.png";
                }
                else if (sb.ObsType == "综合")
                {
                    picPath += "ddzh.png";
                }
                else
                {
                    picPath += "ddsz.png";
                }
                Bitmap markerPic = new Bitmap(picPath);
                marker = new GMarkerGoogle(new PointLatLng(sb.Latitude, sb.Longtitude), markerPic);
            }
            //marker.Tag = sb.SiteCode;
            marker.Tag = sb;

            marker.ToolTipText = sb.SiteName;
            marker.ToolTip.Offset = new Point(10, -20);

            marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

            SiteOverlay.Markers.Add(marker);

        }

        /// <summary>
        /// 清除所有场地
        /// </summary>
        public static void ClearAllSiteMarker(GMapControl gMapCtrl)
        {
            GMapOverlay gmo = GetOverlay("sitemarkers", gMapCtrl);
            if (gmo != null)
                gmo.Markers.Clear();

        }

        /// <summary>
        /// 通过编码删除场地
        /// </summary>
        /// <param name="sitecode">场地编码</param>
        public static void ClearSiteByCode(string sitecode, GMapControl gMapCtrl)
        {
            GMapOverlay gmo = GetOverlay("sitemarkers", gMapCtrl);
            if (gmo != null)
            {
                for (int i = 0; i < gmo.Markers.Count; i++)
                {
                    if (sitecode == gmo.Markers[i].Tag.ToString())
                    {
                        gmo.Markers.RemoveAt(i);
                    }
                }
                //gmo.Markers.Clear();

            }
        }

        /// <summary>
        /// 定位到场地
        /// </summary>
        /// <param name="sb"></param>
        public static void ZoomToSite(SiteBean sb, GMapControl gMapCtrl)
        {
            PointLatLng sitepoint = new PointLatLng(sb.Latitude, sb.Longtitude);
            gMapCtrl.Position = sitepoint;
            gMapCtrl.Zoom = 10;
        }

        /// <summary>
        /// 按单位加载场地
        /// </summary>
        /// <param name="ub"></param>
        /// <param name="gMapCtrl"></param>
        public static void ZoomToSiteByUnit(UnitInfoBean ub, GMapControl gMapCtrl)
        {
            try
            {
                ClearAllSiteMarker(gMapCtrl);
                if (IsLayerExisted("unitlayer", gMapCtrl))
                    GetOverlay("unitlayer", gMapCtrl).Polygons.Clear();

                List<SiteBean> sblist = SiteBll.Instance.GetWhere(new { UNITCODE = ub.UnitCode }).ToList();//获取数据

                //DataTable sitedb = SiteBll.Instance.GetDataTable("select UNITCODE,SITECODE,SITENAME,SITETYPE,HISTORYSITE," +
                //    "LONGTITUDE,LATITUDE,ALTITUDE,PLACE,FAULTCODE,STARTDATE,SITESTATUS,MARKSTONETYPE,BUILDUNIT,OBSUNIT," +
                //    "SITESITUATION,GEOSITUATION,NOTE,LOCATIONS,OTHERSITUATION,DATACHG,XZQCODE,OBSCYC from t_siteinfodb where UNITCODE='" + ub.UnitCode + "'");

                //ModelHandler<SiteBean> mh = new ModelHandler<SiteBean>();

                //foreach (DataRow dr in sitedb.Rows)
                //{
                //    sblist.Add(mh.FillModel(dr));
                //}
                if (sblist == null)
                    return;
                if (sblist.Count == 0)
                    return;

                List<PointLatLng> plllist = new List<PointLatLng>();

                #region 获取最大最小角点坐标

                double minlat = double.MaxValue;
                double minlgt = double.MaxValue;
                double maxlat = double.MinValue;
                double maxlgt = double.MinValue;

                foreach (SiteBean sb in sblist)
                {
                    if (sb.Latitude == 0 || sb.Longtitude == 0)
                        continue;
                    if (sb.Latitude < minlat) minlat = sb.Latitude;
                    if (sb.Longtitude < minlgt) minlgt = sb.Longtitude;
                    if (sb.Latitude > maxlat) maxlat = sb.Latitude;
                    if (sb.Longtitude > maxlgt) maxlgt = sb.Longtitude;
                }
                minlat = minlat - 0.2;
                minlgt = minlgt - 0.2;
                maxlat = maxlat + 0.2;
                maxlgt = maxlgt + 0.2;

                #endregion

                plllist.Add(new PointLatLng(minlat, minlgt));
                plllist.Add(new PointLatLng(maxlat, minlgt));
                plllist.Add(new PointLatLng(maxlat, maxlgt));
                plllist.Add(new PointLatLng(minlat, maxlgt));

                // GMapPolygon gmp = new GMapPolygon(plllist, ub.UnitName);
                //GMapOverlay unitOverlay = IsLayerExisted("unitlayer", gMapCtrl) ? GetOverlay("unitlayer", gMapCtrl) : new GMapOverlay("unitlayer");

                //Pen pen = new Pen(Color.Red, 2);
                //pen.DashStyle = DashStyle.DashDot;
                //pen.DashPattern = new float[] { 1f, 1f };

                //gmp.Stroke = pen;
                //gmp.Fill = new SolidBrush(Color.FromArgb(50, Color.LightBlue));

                //if (!IsLayerExisted("unitlayer", gMapCtrl))
                //    gMapCtrl.Overlays.Add(unitOverlay);

                // unitOverlay.Polygons.Add(gmp);


                AddSiteMarker(sblist, gMapCtrl);

                gMapCtrl.SetZoomToFitRect(new RectLatLng(maxlat, minlgt, maxlgt - minlgt, maxlat - minlat));
            }
            catch (Exception ex)
            {

            }
          

        }

        /// <summary>
        /// 清除单位矩形
        /// </summary>
        /// <param name="gMapCtrl"></param>
        public static void ClearUnitRect(GMapControl gMapCtrl)
        {
            GMapOverlay gmo = GetOverlay("unitlayer", gMapCtrl);
            if (gmo != null)
                gmo.Polygons.Clear();
          
        }

        public static void AddXzq(List<XzqBean> xblist, GMapControl gMapCtrl)
        {
            GMapOverlay xzqOverlay = IsLayerExisted("xzqlayer", gMapCtrl) ? GetOverlay("xzqlayer", gMapCtrl) : new GMapOverlay("xzqlayer");
            if (!IsLayerExisted("xzqlayer", gMapCtrl))
                gMapCtrl.Overlays.Add(xzqOverlay);

            foreach (XzqBean xb in xblist)
            {
                if (xb.Xzqname == "台湾省")
                    continue;

                List<PointLatLng> plllist = new List<PointLatLng>();

                string[] pointsStr = xb.Xqzlocations.Split(';');
                for (int i = 0; i < pointsStr.Length; i++)
                {
                    string[] locationStr = pointsStr[i].Split(',');
                    double lgt = Convert.ToDouble(locationStr[0]);
                    double lat = Convert.ToDouble(locationStr[1]);
                    plllist.Add(new PointLatLng(lat, lgt));
                }

                GMapPolygon gmp = new GMapPolygon(plllist, xb.Xzqname);

                Pen pen = new Pen(Color.Blue, 2);
                pen.DashStyle = DashStyle.DashDot;

                gmp.Stroke = pen;
                gmp.Fill = new SolidBrush(Color.FromArgb(50, Color.LightBlue));
                if (!IsLayerExisted("xzqlayer", gMapCtrl))
                    gMapCtrl.Overlays.Add(xzqOverlay);
                xzqOverlay.Polygons.Add(gmp);
            }
           
        }


        /// <summary>
        /// 地图添加polygon
        /// </summary>
        /// <param name="plllist">点集</param>
        /// <param name="gMapCtrl">gmap</param>
        public static void AddPolygon(List<PointLatLng> plllist,GMapControl gMapCtrl)
        {
            GMapOverlay toollayer = IsLayerExisted("toollayer", gMapCtrl) ? GetOverlay("toollayer", gMapCtrl) : new GMapOverlay("toollayer");
            if (!IsLayerExisted("toollayer", gMapCtrl))
                gMapCtrl.Overlays.Add(toollayer);

            toollayer.Clear();

            GMapPolygon gmp = new GMapPolygon(plllist,"");

            Pen pen = new Pen(Color.Blue, 2);
            pen.DashStyle = DashStyle.DashDot;

            gmp.Stroke = pen;
            gmp.Fill = new SolidBrush(Color.FromArgb(50, Color.LightBlue));
          
            toollayer.Polygons.Add(gmp);

        }

        /// <summary>
        /// 清除单位矩形
        /// </summary>
        /// <param name="gMapCtrl"></param>
        public static void ClearXzq(GMapControl gMapCtrl)
        {
            GMapOverlay gmo = GetOverlay("xzqlayer", gMapCtrl);
            if (gmo != null)
                gmo.Polygons.Clear();
        }

        /// <summary>
        /// 显示断层图层
        /// </summary>
        /// <param name="ftlist">数据列表</param>
        /// <param name="gMapCtrl">地图控件</param>
        public static void AddFaultMap(IEnumerable<FaultBean> ftlist, GMapControl gMapCtrl)
        {
            GMapOverlay faultOverlay = IsLayerExisted("faultlayer", gMapCtrl) ? GetOverlay("faultlayer", gMapCtrl) : new GMapOverlay("faultlayer");
            if (!IsLayerExisted("faultlayer", gMapCtrl))
                gMapCtrl.Overlays.Add(faultOverlay);

            //ObservableCollectionThreadSafe<GMapRoute> routelist = new ObservableCollectionThreadSafe<GMapRoute>();
            foreach (FaultBean fb in ftlist)
            {
                string[] lines = fb.Locations.Split(';');
                foreach (string line in lines)
                {
                    List<PointLatLng> ptEum = new List<PointLatLng>();
                    string[] points = line.Split(',');
                    foreach (string point in points)
                    {
                        string[] lalg = point.Split(' ');

                        try
                        {
                           double[] correctlocation= GpsCorrect.transform(double.Parse(lalg[1]), double.Parse(lalg[0]));
                            PointLatLng pt = new PointLatLng(correctlocation[0], correctlocation[1]);
                            ptEum.Add(pt);
                        }
                        catch {
                            //continue;
                        }
                    }
                    if (fb.Newestactivetime == "N")
                    {
                        GMapRoute route = new GMapRoute(ptEum, fb.Name);
                        route.Stroke = new Pen(Color.LightGreen, 2);
                        route.IsHitTestVisible = true;
                       
                        faultOverlay.Routes.Add(route);
                      
                    }
                    if (fb.Newestactivetime == "Q")
                    {
                        GMapRoute route = new GMapRoute(ptEum, fb.Name);
                        route.Stroke = new Pen(Color.Blue, 2);
                        route.IsHitTestVisible = true;
                        faultOverlay.Routes.Add(route);
                      
                    }
                    if (fb.Newestactivetime == "Q1" || fb.Newestactivetime == "Q2" || fb.Newestactivetime == "Q3" || fb.Newestactivetime == "Q2-Q3")
                    {
                        GMapRoute route = new GMapRoute(ptEum, fb.Name);
                        route.Stroke = new Pen(Color.Orange, 2);
                        route.IsHitTestVisible = true;
                        faultOverlay.Routes.Add(route);
                       
                    }
                    if (fb.Newestactivetime == "Q4")
                    {
                        GMapRoute route = new GMapRoute(ptEum, fb.Name);
                        route.Stroke = new Pen(Color.Red, 2);
                        route.IsHitTestVisible = true;
                        faultOverlay.Routes.Add(route);
                    }
                }
                
            }
          
        }

        /// <summary>
        /// 定位到断层
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="gMapCtrl"></param>
        public static void ZoomtoFault(string locations, GMapControl gMapCtrl)
        {
            GMapOverlay faultOverlay = IsLayerExisted("locationFaultlayer", gMapCtrl) ? GetOverlay("locationFaultlayer", gMapCtrl) : new GMapOverlay("locationFaultlayer");
            if (!IsLayerExisted("locationFaultlayer", gMapCtrl))
                gMapCtrl.Overlays.Add(faultOverlay);

            faultOverlay.Clear();

            string[] lines = locations.Split(';');
            double lamin = double.MaxValue;
            double lamax = double.MinValue;
            double lgmin = double.MaxValue;
            double lgmax = double.MinValue;
            foreach (string line in lines)
            {
                List<PointLatLng> ptEum = new List<PointLatLng>();
                string[] points = line.Split(',');
                foreach (string point in points)
                {
                    string[] lalg = point.Split(' ');

                    try
                    {
                        if (lamin > double.Parse(lalg[1])) lamin = double.Parse(lalg[1]);
                        if (lamax < double.Parse(lalg[1])) lamax = double.Parse(lalg[1]);
                        if (lgmin > double.Parse(lalg[0])) lgmin = double.Parse(lalg[0]);
                        if (lgmax < double.Parse(lalg[0])) lgmax = double.Parse(lalg[0]);

                        PointLatLng pt = new PointLatLng(double.Parse(lalg[1]), double.Parse(lalg[0]));
                        ptEum.Add(pt);
                    }
                    catch
                    {
                        //continue;
                    }
                }
                GMapRoute route = new GMapRoute(ptEum, "");
                route.Stroke = new Pen(Color.Cyan, 2);
                route.IsHitTestVisible = true;

                faultOverlay.Routes.Add(route);
            }

            gMapCtrl.Position = new PointLatLng((lamin + lamax) / 2, (lgmax + lgmin) / 2);
            gMapCtrl.Zoom = 6;

        }

        /// <summary>
        /// 定位到选中的地震
        /// </summary>
        /// <param name="eb"></param>
        /// <param name="gMapCtrl"></param>
        public static void ZoomtoEqk(EqkBean eb, GMapControl gMapCtrl)
        {

            GMapOverlay locationEqklayer = IsLayerExisted("locationEqklayer", gMapCtrl) ? GetOverlay("locationEqklayer", gMapCtrl) : new GMapOverlay("locationEqklayer");
            if (!IsLayerExisted("locationEqklayer", gMapCtrl))
                gMapCtrl.Overlays.Add(locationEqklayer);

            locationEqklayer.Clear();

            string picName = "";
            switch ((int)eb.Magntd)
            {
                case 0:
                    picName = "2.png";
                    break;
                case 1:
                    picName = "2.png";
                    break;
                case 2:
                    picName = "2.png";
                    break;
                case 3:
                    picName = "3.png";
                    break;
                case 4:
                    picName = "4.png";
                    break;
                case 5:
                    picName = "5.png";
                    break;
                case 6:
                    picName = "6.png";
                    break;
                case 7:
                    picName = "7.png";
                    break;
                case 8:
                    picName = "8.png";
                    break;
                case 9:
                    picName = "9.png";
                    break;

            }
            string picPath = System.Windows.Forms.Application.StartupPath + "//图片缓存//地震标注图片//seletedEqk//" + picName;
            Bitmap eqkDotPic = new Bitmap(picPath);
            GMapMarker marker = new GMarkerGoogle(new PointLatLng(eb.Latitude, eb.Longtitude), eqkDotPic);
            //marker.Tag = eqkList[i];

            // marker.ToolTipText = "震级：" + eqkList[i].Magntd + "\r\n时间：" + eqkList[i].EakDate.ToString() + "\r\n地点：" + eqkList[i].Place;

           // marker.ToolTipMode = MarkerTooltipMode.Never;
         
            locationEqklayer.Markers.Add(marker);

            gMapCtrl.Position = new PointLatLng(eb.Latitude, eb.Longtitude);
            gMapCtrl.Zoom = 6;

        }

        /// <summary>
        /// 清除断层数据
        /// </summary>
        public static void ClearFault(GMapControl gMapCtrl)
        {
            GMapOverlay gmo = GetOverlay("faultlayer", gMapCtrl);
            if (gmo != null)
                gmo.Routes.Clear();

            GMapOverlay locationFaultlayer = GetOverlay("locationFaultlayer", gMapCtrl);
            if (locationFaultlayer != null)
                locationFaultlayer.Routes.Clear();
        }

        /// <summary>
        /// 清除所有地震标注
        /// </summary>
        /// <param name="gMapCtrl"></param>
        public static void ClearAllEqkMarker(GMapControl gMapCtrl)
        {
            GMapOverlay gmo1 = GetOverlay("eqkmarkers", gMapCtrl);
            if (gmo1 != null)
                gmo1.Markers.Clear();

            GMapOverlay gmo2 = GetOverlay("circleOverlay", gMapCtrl);
            if (gmo2 != null)
                gmo2.Markers.Clear();

            GMapOverlay gmo3 = GetOverlay("locationEqklayer", gMapCtrl);
            if (gmo3 != null)
                gmo3.Markers.Clear();
        }

        /// <summary>
        /// 画圆
        /// </summary>
        /// <param name="circleCenter">圆心</param>
        /// <param name="r">半径</param>
        public static void CreateCircle(PointLatLng circleCenter, double radius1, double radius2, GMapControl gMapCtrl)
        {
            radius1 = radius1 * 1000;
            radius2 = radius2 * 1000;
              GSCoordConvertionClass_Xian80 cc = new GSCoordConvertionClass_Xian80();
            cc.IsBigNumber = true;
            cc.Strip = EnumStrip.Strip3;
            cc.L0 = decimal.Parse(circleCenter.Lng.ToString());
            decimal x = decimal.MinValue, y = decimal.MinValue;
            cc.GetXYFromBL(decimal.Parse(circleCenter.Lat.ToString()), decimal.Parse(circleCenter.Lng.ToString()), ref x, ref y);

            GPoint gp = gMapCtrl.FromLatLngToLocal(circleCenter);

            List<PointLatLng> gpollist = new List<PointLatLng>();

            double seg = Math.PI * 2 / 100;

            for (int i = 0; i < 100; i++)
            {
                double theta = seg * i;
                decimal a = decimal.Parse((double.Parse(x.ToString()) + Math.Cos(theta) * radius1).ToString());
                decimal b = decimal.Parse((double.Parse(y.ToString()) + Math.Sin(theta) * radius1).ToString());

                decimal B = decimal.MinValue, L = decimal.MinValue;
                cc.GetBLFromXY(a, b, ref B, ref L);

                PointLatLng gpoi = new PointLatLng(double.Parse(B.ToString()), double.Parse(L.ToString()));
                gpollist.Add(gpoi);
            }

            GMapMarkerCircle gpol = new GMapMarkerCircle(circleCenter, (int)radius1, (int)radius2);
           // GMapPolygon gpol = new GMapPolygon(gpollist, "circlePolygon");

            //gpol.Fill = new SolidBrush(Color.FromArgb(50, 0, 155, 255));
            //gpol.Stroke = new Pen(Color.FromArgb(50, 0, 155, 255), 0);

            GMapOverlay CircleOverlay = new GMapOverlay("circleOverlay");
            CircleOverlay.Markers.Add(gpol);
            gMapCtrl.Overlays.Add(CircleOverlay);

        }

        /// <summary>
        /// Map标注地震
        /// </summary>
        public static void AnnotationEqkToMap(List<EqkBean> eqkList, GMapControl gMapCtrl)
        {
            GMapOverlay EqkOverlay = IsLayerExisted("eqkmarkers", gMapCtrl) ? GetOverlay("eqkmarkers", gMapCtrl) : new GMapOverlay("eqkmarkers");

            if (!IsLayerExisted("eqkmarkers", gMapCtrl))
                gMapCtrl.Overlays.Add(EqkOverlay);

            GMapMarker marker = null;
            string picName = "";
            for (int i = 0; i < eqkList.Count; i++)
            {
                switch ((int)eqkList[i].Magntd)
                {
                    case 0: picName = "2.png";
                        break;
                    case 1: picName = "2.png";
                        break;
                    case 2: picName = "2.png";
                        break;
                    case 3: picName = "3.png";
                        break;
                    case 4: picName = "4.png";
                        break;
                    case 5: picName = "5.png";
                        break;
                    case 6: picName = "6.png";
                        break;
                    case 7: picName = "7.png";
                        break;
                    case 8: picName = "8.png";
                        break;
                    case 9: picName = "9.png";
                        break;
                }

                string picPath = System.Windows.Forms.Application.StartupPath + "//图片缓存//地震标注图片//" + picName;
                Bitmap eqkDotPic = new Bitmap(picPath);
                marker = new GMarkerGoogle(new PointLatLng(eqkList[i].Latitude, eqkList[i].Longtitude), eqkDotPic);
                //marker.Tag = eqkList[i];

               // marker.ToolTipText = "震级：" + eqkList[i].Magntd + "\r\n时间：" + eqkList[i].EakDate.ToString() + "\r\n地点：" + eqkList[i].Place;

               // marker.ToolTipMode = MarkerTooltipMode.Never;
               // marker.ToolTip.Format.Alignment = StringAlignment.Near;

                EqkOverlay.Markers.Add(marker);
               
            }
        
            gMapCtrl.Position = chinaCenter;
            gMapCtrl.Zoom = 4;
            

        }


        /// <summary>
        /// 图层是否存在
        /// </summary>
        /// <param name="layername">图层名</param>
        /// <param name="gMapCtrl"></param>
        private static bool IsLayerExisted(string layername, GMapControl gMapCtrl)
        {
            bool res = false;
            for (int i = 0; i < gMapCtrl.Overlays.Count; i++)
            {
                if (gMapCtrl.Overlays[i].Id == layername)
                {
                    res = true;
                }

            }
            return res;
        }

        /// <summary>
        /// 获取图层
        /// </summary>
        /// <param name="layername">图层名</param>
        /// <param name="gMapCtrl"></param>
        /// <returns></returns>
        private static GMapOverlay GetOverlay(string layername, GMapControl gMapCtrl)
        {
            GMapOverlay res = null;

            for (int i = 0; i < gMapCtrl.Overlays.Count; i++)
            {
                if (gMapCtrl.Overlays[i].Id == layername)
                {
                    res = gMapCtrl.Overlays[i];
                }
            }
            return res;
        }

    }


    public class MapManagerLoader
    {
        private static readonly MapManagerLoader _instance = new MapManagerLoader();

        public static MapManagerLoader Instance
        {
            get { return _instance; }
        }

        private MapManagerLoader()
        {
        }

        private bool _isLoaded;

        public bool Load(string fileName)
        {
            if (!_isLoaded)
            {
                new Thread(() => GMaps.Instance.ImportFromGMDB(fileName)).Start();
                _isLoaded = true;
            }
            return _isLoaded;
        }
    }

    [Serializable]
    public class GMapMarkerCircle : GMapMarker, ISerializable
    {
        /// <summary>
        /// 外圈半径
        /// </summary>
        public int RadiusOut;

        /// <summary>
        /// 内圈半径
        /// </summary>
        public int RadiusInner;

        /// <summary>
        /// specifies how the outline is painted
        /// </summary>
        [NonSerialized]
        public Pen Stroke = new Pen(Color.FromArgb(155, Color.Red), 4);

        /// <summary>
        /// background color
        /// </summary>
        [NonSerialized]
        public Brush Fill = new SolidBrush(Color.Red);

        /// <summary>
        /// is filled
        /// </summary>
        public bool IsFilled = true;

        /// <param name="p">圆心</param>
        /// <param name="_raidus">半径</param>
        public GMapMarkerCircle(PointLatLng p, int _raidus1, int _raidus2)
            : base(p)
        {
            RadiusOut = _raidus1;
            RadiusInner = _raidus2; // 100m
            IsHitTestVisible = false;
        }

        public override void OnRender(Graphics g)
        {

            ////将距离转换成像素长度
            int R1 = (int)((RadiusOut) / Overlay.Control.MapProvider.Projection.GetGroundResolution((int)Overlay.Control.Zoom, Position.Lat)) * 2;

            int R2 = (int)((RadiusInner) / Overlay.Control.MapProvider.Projection.GetGroundResolution((int)Overlay.Control.Zoom, Position.Lat)) * 2;
            //if (IsFilled)
            //{
            //    g.FillEllipse(Fill, new Rectangle(LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R));
            //}
            //g.DrawEllipse(Stroke, new Rectangle(LocalPosition.X - R / 2, LocalPosition.Y - R / 2, R, R));


            GraphicsPath pTemp = new GraphicsPath();
            Rectangle r1 = new Rectangle(LocalPosition.X - R1 / 2, LocalPosition.Y - R1 / 2, R1, R1);
            Rectangle r2 = new Rectangle(LocalPosition.X - R2 / 2, LocalPosition.Y - R2 / 2, R2, R2);
            pTemp.AddEllipse(r1);
            pTemp.AddEllipse(r2);
            g.FillPath(new SolidBrush(Color.FromArgb(50, 0, 155, 255)), pTemp);

        }

        Rectangle GetSquareRec(double radius, int x, int y)
        {
            double r = radius;
            double side = Math.Sqrt(Math.Pow(r, 2) / 2);
            Rectangle rec = new Rectangle(x - ((int)side), y - ((int)side), (int)(side * 2) + x, (int)(side * 2) + y);

            return rec;
        }

        public override void Dispose()
        {
            if (Stroke != null)
            {
                Stroke.Dispose();
                Stroke = null;
            }

            if (Fill != null)
            {
                Fill.Dispose();
                Fill = null;
            }

            base.Dispose();
        }

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            // TODO: Radius, IsFilled
        }

        protected GMapMarkerCircle(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // TODO: Radius, IsFilled
        }

        #endregion
    }


}
