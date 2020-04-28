using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using xxkUI.Tool;
using xxkUI.Form;
using Steema.TeeChart.Tools;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Drawing;
using Steema.TeeChart;
using DevExpress.XtraEditors;
using xxkUI.Bll;
using xxkUI.MyCls;
using System.IO;
using System.Linq;

namespace xxkUI.Controls
{
    public partial class TChartControl : UserControl
    {
        /// <summary>
        /// 选中点的结构体（点要素和值）,用于消突跳和消台阶
        /// </summary>
        struct SelectedPointStruct
        {
            public double PtDate;
            public double PtValue;
            public string Note;
        }

        private frm_EqkShow eqkfrm = null;
        private CursorTool cursorTool;
        private DragMarks dragMarks;//可拖拽标签工具
        private List<int> hitNoteIndex = new List<int>();//右键击中的标注索引

        #region 消突跳和台阶用到的变量

        /*记录鼠标操作类型(鼠标热线、消突跳、消台阶)，在Tchart事件交互事件中作为区分*/
        public TChartEventType tchartEventType = TChartEventType.NoProg;
        private Point start = new Point();//矩形起点
        private Point end = new Point();//矩形终点
        private Graphics g;
        private bool isDrawing = false;
        List<SelectedPointStruct> selectedPtlist = new List<SelectedPointStruct>();//选中的数据点集合
        List<SelectedPointStruct> notePtlist = new List<SelectedPointStruct>();//备注的数据点集合
        #endregion

        private Annotation annotation;

        private bool ShowMaxMin = false;//是否显示最大最小值

        private List<EqkAnotationStc> eqkAnolist = new List<EqkAnotationStc>();//选中的地震目录数据
        private ActionType actiontype = ActionType.NoAction;// 观测数据操作类型

        public TChartControl()
        {
            InitializeComponent();
            SetTitle("", titleFontEdit.SelectedItem.ToString(), float.Parse(titleSize.Value.ToString()), GetSelectedFontStyle(titleStyle.SelectedText), colorTitle.Color);
            this.cursorTool = new CursorTool();
            this.cursorTool.Chart = this.tChart.Chart;
            this.cursorTool.Active = false;
            this.cursorTool.FollowMouse = true;
           
            this.cursorTool.Style = CursorToolStyles.Vertical;
            this.cursorTool.UseChartRect = true;
            annotation = new Annotation(tChart.Chart);
            annotation.Active = false;
            annotation.Shape.CustomPosition = true;
            annotation.Shape.Gradient.Visible = true;
            annotation.Shape.Transparency = 30;
            this.tChart.MouseWheel += TChart_MouseWheel;
        }


        #region 标题样式设置与属性获取
        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="titlename">标题名</param>
        /// <param name="zt">字体</param>
        /// <param name="size">大小</param>
        /// <param name="fs">样式(Bold加粗、Italic倾斜、Regular普通、Strikeout中间有直线通过、Underline带下划线)</param>
        /// <param name="color">颜色</param>
        private void SetTitle(string titlename, string zt, float size, FontStyle fs, Color color)
        {
            this.tChart.Header.Text = titlename;
            ChartFont cf = new ChartFont(this.tChart.Chart, new Font(zt, size, fs));
            cf.Color = color;
            this.tChart.Header.Font = cf;
            //this.tChart.Header.Color = color;
            //this.tChart.Refresh();
        }

        /// <summary>
        ///返回取标题名称
        /// </summary>
        /// <returns></returns>
        private string GetTitleName()
        {
            return this.tChart.Header.Text;
        }

        /// <summary>
        /// 返回标题字体
        /// </summary>
        /// <returns></returns>
        private string GetTitleZt()
        {
            return this.tChart.Header.Font.DrawingFont.Name.ToString();
        }

        /// <summary>
        /// 返回标题大小
        /// </summary>
        /// <returns></returns>
        private float GetTitleSize()
        {
            return this.tChart.Header.Font.DrawingFont.Size;
        }

        /// <summary>
        /// 返回标题样式
        /// </summary>
        /// <returns>(Bold加粗、Italic倾斜、Regular普通、Strikeout中间有直线通过、Underline带下划线)</returns>
        private FontStyle GetTitleFontStyle()
        {
            return this.tChart.Header.Font.DrawingFont.Style;
        }

        /// <summary>
        /// 返回标题颜色
        /// </summary>
        /// <returns></returns>
        private Color GetTitleColor()
        {
            return this.tChart.Header.Font.Color;
        }

        private FontStyle GetSelectedFontStyle(string text)
        {
            switch (text)
            {
                case "加粗":
                    {
                        return FontStyle.Bold;
                    }
                    break;
                case "倾斜":
                    {
                        return FontStyle.Italic;
                    }
                    break;
                case "普通":
                    {
                        return FontStyle.Regular;
                    }
                    break;
                case "直线通过":
                    {
                        return FontStyle.Strikeout;
                    }
                    break;
                case "下划线":
                    {
                        return FontStyle.Underline;
                    }
                    break;
                default:
                    return FontStyle.Regular;
                    break;
            }
        }
        #endregion

        #region 纵坐标样式设置

        /// <summary>
        /// 设置AxesLeft样式
        /// </summary>
        /// <param name="ax">Axis</param>
        /// <param name="labelcolor">标签颜色</param>
        /// <param name="labelsize">标签大小</param>
        /// <param name="tickcount">刻度数目</param>
        private void SetAxesLeftStyle(Axis ax, Color labelcolor, float labelsize, int tickcount)
        {
            ax.AxisPen.Visible = true;
            ChartFont cf = new ChartFont(this.tChart.Chart, new Font("宋体", labelsize, FontStyle.Regular));
            cf.Color = labelcolor;
            ax.Labels.Font = cf;
            ax.MinorTickCount = tickcount;
            ax.MinorTicks.Visible = true;
            ax.Ticks.Visible = true;
            ax.TicksInner.Length = 1;
            ax.TicksInner.Style = System.Drawing.Drawing2D.DashStyle.Dash;
            ax.TicksInner.Visible = true;
            //this.tChart.Refresh();
        }

        #endregion

        #region  横坐标样式设置

        /// <summary>
        /// 设置横坐标样式
        /// </summary>
        /// <param name="ax">Axis</param>
        /// <param name="angle">标签角度</param>
        /// <param name="labelcolor">标签颜色</param>
        /// <param name="labelsize">标签大小</param>
        /// <param name="tickcount">小刻度数目</param>
        private void SetAxesBottomStyle(Axis ax, int angle,Color labelcolor,float labelsize,int tickcount)
        {
            ax.Labels.Angle = angle;
            ax.Labels.DateTimeFormat = "yyyy-MM-dd";
            ax.Labels.ExactDateTime = true;
            ax.Labels.Font.Brush.Color = Color.Black;

            ChartFont cf = new ChartFont(this.tChart.Chart, new Font("宋体", labelsize, FontStyle.Regular));
            cf.Color = labelcolor;
            ax.Labels.Font = cf;

            ax.MinorTickCount = tickcount;
            ax.MinorTicks.Visible = true;

            tChart.Axes.Bottom.Automatic = false;
            tChart.Axes.Bottom.AutomaticMaximum = false;
            tChart.Axes.Bottom.AutomaticMinimum = false;
            if (this.tChart.Series.Count <= 1)
            {
                tChart.Axes.Bottom.Maximum = this.tChart.Series[0].XValues.Maximum;
                tChart.Axes.Bottom.Minimum = this.tChart.Series[0].XValues.Minimum;
            }
            else
            {
                double xmax = double.MinValue;
                double xmin = double.MaxValue;
                for (int i = 0; i < this.tChart.Series.Count; i++)
                {
                    if (this.tChart.Series[i].XValues.Maximum > xmax)
                        xmax = this.tChart.Series[i].XValues.Maximum;
                    if (this.tChart.Series[i].XValues.Minimum < xmin)
                        xmin = this.tChart.Series[i].XValues.Minimum;
                }
                tChart.Axes.Bottom.Maximum = xmax;
                tChart.Axes.Bottom.Minimum = xmin;

            }
            //this.tChart.Refresh();
        }


        private void SetMarkStyle(string zt, float size, FontStyle fs, Color color, TextShapeStyle ss)
        {
            ChartFont cf = new ChartFont(this.tChart.Chart, new Font(zt, size, fs));
            cf.Color = Color.Black;
            this.tChart.Series[0].Marks.Font = cf;
            this.tChart.Series[0].Marks.MultiLine = true;               //是否允许标签多行显示(当标签太长时)
            this.tChart.Series[0].Marks.TailStyle = MarksTail.None;
            this.tChart.Series[0].Marks.ShapeStyle = ss;
            this.tChart.Series[0].Marks.Visible = true;
            this.tChart.Series[0].Marks.Pen.Color = color;

            this.tChart.Series[0].Marks.Arrow.Color = Color.Red;

            this.tChart.Series[0].Marks.Arrow.Width = 1;          //标签与单元之间连线的宽度
            this.tChart.Series[0].Marks.Arrow.Style = System.Drawing.Drawing2D.DashStyle.DashDot;       //标签与单元之间连线样式
            this.tChart.Series[0].Marks.Font.Color = Color.Black;

            //this.tChart.Series[0].Marks.Transparent = false;          //标签是否透明
            //this.tChart.Series[0].Marks.Font.Color = vbBlue;             //'标签文字色
            //this.tChart.Series[0].Marks.BackColor = Color.Red;            //标签背景色
            //this.tChart.Series[0].Marks.Gradient.Visible = True;          //是否起用标签渐变色
            //this.tChart.Series[0].Marks.Bevel = bvNone;                   //标签样式(凹,凸,平面)
            //this.tChart.Series[0].Marks.ShadowSize = 0;                   //标签阴影大小
 
            this.tChart.Series[0].Marks.AutoPosition = true;


        }
        #endregion


        #region 方法（添加数据、显示备注、获取可见Series、添加多个坐标轴）
          
      /// <summary>
      /// 添加数据
      /// </summary>
      /// <param name="hashKeys">缓存数据表的索引</param>
      /// <param name="excelPath">excel存放位置</param>
      /// <param name="ispretreated">是否做过预处理</param>
      /// <returns></returns>
        public bool AddSeries(List<string> hashKeys, string excelPath,bool ispretreated)
        {

            bool isok = false;
            if (hashKeys == null)
                return false;
            if (hashKeys.Count == 0)
                return false;
          
            try
            {
                selectedPtlist.Clear();
                this.tChart.Series.Clear();
               
                foreach (string DtKey in hashKeys)
                {
                    string linecode = DtKey.Split(',')[1];
                    if (!ObsdataCls.IsExisted(DtKey))
                        ObsdataCls.AddData(DtKey, excelPath, linecode);
                  
                    Line line = new Line();
                    tChart.Series.Add(line);
                    line.XValues.DataMember = "obvdate";
                    line.YValues.DataMember = "obvvalue";
                    line.XValues.DateTime = true;
                    line.DataSource = ObsdataCls.ObsdataHash[DtKey] as DataTable;
                    line.Marks.Visible = false;
                    line.Tag = DtKey;
                    line.Legend.Visible = false;
                    line.Color = (Color)colorSeries.EditValue;
                    line.MouseEnter += Line_MouseEnter;
                    line.MouseLeave += Line_MouseLeave;
                   
                }

                if (hashKeys.Count > 1)
                {
                    SetTitle("", titleFontEdit.SelectedItem.ToString(), float.Parse(titleSize.Value.ToString()), GetSelectedFontStyle(titleStyle.SelectedText), colorTitle.Color);
                   
                    this.gridControlObsdata.DataSource = null;
                    this.gridControlObsdata.Refresh();
                    xtraTabPage1.PageVisible = false;
                    xtraTabControl1.SelectedTabPage = xtraTabPage2;
                   
                }
                else if (hashKeys.Count == 1)
                {
                    Line line = this.tChart.Series[0] as Line;
                    line.GetSeriesMark += Line_GetSeriesMark;
              
                    string title = LineBll.Instance.GetNameByID("OBSLINENAME", "OBSLINECODE", hashKeys[0].Split(',')[1]);
                    if (ispretreated)
                        title += "(预处理)";
                    
                    SetTitle(title, titleFontEdit.SelectedItem.ToString(), float.Parse(titleSize.Value.ToString()), GetSelectedFontStyle(titleStyle.SelectedText), colorTitle.Color);

                    //只有一条曲线时不显示图例
                    this.tChart.Series[0].Legend.Visible = false;
                   
                    tChart.Axes.Left.Title.Caption = "观测值(mm)";
                    tChart.Axes.Left.Title.AutoPosition = true;

                    tChart.Axes.Bottom.Title.Caption = "时间(yyyy-MM-dd)";
                    tChart.Axes.Bottom.Title.Alignment = StringAlignment.Far;

                    //TextShapeStyle tss = TextShapeStyle.Rectangle;

                    //if (cmbNoteStyle.SelectedText == "矩形")
                    //    tss = TextShapeStyle.Rectangle;
                    //else if (cmbNoteStyle.SelectedText == "圆角矩形")
                    //    tss = TextShapeStyle.RoundRectangle;

                   // SetMarkStyle(fontNote.SelectedItem.ToString(), float.Parse(noteSize.EditValue.ToString()), GetSelectedFontStyle(fontNote.EditValue.ToString()), (Color)colorNote.EditValue, tss);
                    this.tChart.Refresh();


                    this.gridControlObsdata.DataSource = ObsdataCls.ObsdataHash[hashKeys[0]] as DataTable;
                    this.gridControlObsdata.Refresh();

                    xtraTabPage1.PageVisible = true;
                    xtraTabControl1.SelectedTabPage = xtraTabPage1;
                }


                AddVisibleLineVerticalAxis();
                GridVisible(false);//默认隐藏格网

              
            }
            catch (Exception ex)
            {
            }

            this.tChart.Refresh();
            return isok;
        }

        /// <summary>
        /// 处理数据曲线
        /// </summary>
        /// <param name="hashKeys"></param>
        /// <param name="excelPath"></param>
        /// <returns></returns>
        public bool AddSeries2(List<string> hashKeys, string excelPath)
        {

            bool isok = false;
            if (hashKeys == null)
                return false;
            if (hashKeys.Count == 0)
                return false;

            try
            {
                selectedPtlist.Clear();
                this.tChart.Series.Clear();

                foreach (string DtKey in hashKeys)
                {
                    string linename = DtKey.Split(',')[1];
                    if (!ObsdataCls.IsExisted(DtKey))
                        ObsdataCls.AddData(DtKey, excelPath, linename);
                    Line line = new Line();
                    tChart.Series.Add(line);
                    line.Title = linename;
                    line.XValues.DataMember = "obvdate";
                    line.YValues.DataMember = "obvvalue";
                    line.XValues.DateTime = true;
                    line.DataSource = ObsdataCls.ObsdataHash[DtKey] as DataTable;
                    line.Marks.Visible = false;
                    line.Tag = DtKey;
                    line.Legend.Visible = true;
                    line.Color = (Color)colorSeries.EditValue;
                    line.MouseEnter += Line_MouseEnter;
                    line.MouseLeave += Line_MouseLeave;
                    line.GetSeriesMark += Line_GetSeriesMark;
                }

                if (hashKeys.Count > 1)
                {
                    SetTitle("", titleFontEdit.SelectedItem.ToString(), float.Parse(titleSize.Value.ToString()), GetSelectedFontStyle(titleStyle.SelectedText), colorTitle.Color);
                    this.gridControlObsdata.DataSource = null;
                    this.gridControlObsdata.Refresh();
                    xtraTabPage1.PageVisible = false;
                    xtraTabControl1.SelectedTabPage = xtraTabPage2;

                }
                else if (hashKeys.Count == 1)
                {
                    string linename = hashKeys[0].Split(',')[1];
                    SetTitle(linename, titleFontEdit.SelectedItem.ToString(), float.Parse(titleSize.Value.ToString()), GetSelectedFontStyle(titleStyle.SelectedText), colorTitle.Color);

                    //只有一条曲线时不显示图例
                    this.tChart.Series[0].Legend.Visible = false;

                    tChart.Axes.Left.Title.Caption = "观测值(mm)";
                    tChart.Axes.Left.Title.AutoPosition = true;

                    tChart.Axes.Bottom.Title.Caption = "时间(yyyy-MM-dd)";
                    tChart.Axes.Bottom.Title.Alignment = StringAlignment.Far;

                    //TextShapeStyle tss = TextShapeStyle.Rectangle;

                    //if (cmbNoteStyle.SelectedText == "矩形")
                    //    tss = TextShapeStyle.Rectangle;
                    //else if (cmbNoteStyle.SelectedText == "圆角矩形")
                    //    tss = TextShapeStyle.RoundRectangle;

                    //SetMarkStyle(fontNote.SelectedItem.ToString(), float.Parse(noteSize.EditValue.ToString()), GetSelectedFontStyle(fontNote.EditValue.ToString()), (Color)colorNote.EditValue, tss);
                    this.tChart.Refresh();


                    this.gridControlObsdata.DataSource = ObsdataCls.ObsdataHash[hashKeys[0]] as DataTable;
                    this.gridControlObsdata.Refresh();

                    xtraTabPage1.PageVisible = true;
                    xtraTabControl1.SelectedTabPage = xtraTabPage1;
                }


                AddVisibleLineVerticalAxis();
                GridVisible(false);//默认隐藏格网

                
            }
            catch (Exception ex)
            {
                //   throw new Exception(ex.Message);
            }
            this.tChart.Refresh();
            return isok;
        }

        /// <summary>
        /// 协调比点状时序图
        /// </summary>
        /// <param name="dtlist">数据</param>
        /// <param name="title">标题</param>
        /// <returns></returns>
        public bool AddXtbSeries(List<DataTable> dtlist,string title)
        {
            bool isok = false;

            try
            {
                this.tChart.Series.Clear();
                foreach (DataTable dt in dtlist)
                {
                  
                   Points line = new Points();
                    tChart.Series.Add(line);
                   
                    line.XValues.DataMember = "obvdate";
                    line.YValues.DataMember = "obvvalue";
                    //line.XValues.DateTime = true;
                    //List<double> vvlist = new List<double>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        string vlstr = dr[1].ToString();
                        // vvlist.Add( );
                        line.Add(double.Parse(vlstr));
                    }

                    //line.Add(vs);
                    //line.DataSource = dt;
                    line.Marks.Visible = false;
                    line.Legend.Visible = true;
                    line.Legend.Text = dt.TableName;
                    //line.Color = (Color)colorSeries.EditValue;
             
                }
                SetTitle(title, titleFontEdit.SelectedItem.ToString(), float.Parse(titleSize.Value.ToString()), GetSelectedFontStyle(titleStyle.SelectedText), colorTitle.Color);

                //只有一条曲线时不显示图例
                this.tChart.Series[0].Legend.Visible = false;

                tChart.Axes.Left.Title.Caption = "观测值(mm)";
                tChart.Axes.Left.Title.AutoPosition = true;

                tChart.Axes.Bottom.Title.Caption = "时间(yyyy-MM-dd)";
                tChart.Axes.Bottom.Title.Alignment = StringAlignment.Far;

                this.tChart.Refresh();


                xtraTabPage1.PageVisible = true;
                xtraTabControl1.SelectedTabPage = xtraTabPage1;

             
                GridVisible(false);//默认隐藏格网

            }
            catch (Exception ex)
            {
                //   throw new Exception(ex.Message);
            }
            this.tChart.Refresh();
            return isok;
        }



        /// <summary>
        /// 获取测线数量
        /// </summary>
        /// <returns></returns>
        public int GetSeriesCont()
        {
            return this.tChart.Series.Count;
        }
   

        /// <summary>
        /// 获取可见series
        /// </summary>
        /// <returns></returns>
        private List<BaseLine> GetVisibleLine()
        {
            List<BaseLine> visibleSeries = new List<BaseLine>();
            for (int i = 0; i < tChart.Series.Count; i++)
            {
                if (tChart.Series[i].Visible)
                {
                    visibleSeries.Add((BaseLine)tChart.Series[i]);
                }
            }
            return visibleSeries;
        }


        /// <summary>
        /// 添加多个纵坐标轴
        /// </summary>
        /// <param name="isOneLine">是否合并纵轴</param>
        public void AddVisibleLineVerticalAxis()
        {

            int verticalAxisSpace = 3;
            List<BaseLine> visibleSeries = GetVisibleLine();

            double singleAxisLengthPercent;//单个纵轴占据的百分比

            //计算每个坐标轴占据的百分比
            if (visibleSeries.Count < 1)
            {
                return;
            }
            else
            {
                singleAxisLengthPercent = Convert.ToDouble(100 - verticalAxisSpace * (visibleSeries.Count + 1)) / (visibleSeries.Count);
            }

            //给可见的曲线加上纵轴
            for (int i = 0; i < visibleSeries.Count; i++)
            {
                Series s = visibleSeries[i];

                Axis axis;
                //设置纵轴的起始位置
                if (i == 0)
                {
                    axis = tChart.Axes.Left;
                    axis.StartPosition = verticalAxisSpace;
                    axis.Automatic = true;
                    axis.EndPosition = singleAxisLengthPercent;
                    
                }
                else
                {
                    axis = new Axis(false, false, tChart.Chart);
                    if (i == 1)
                    {
                        axis.StartPosition = tChart.Axes.Left.EndPosition + verticalAxisSpace;
                    }
                    else
                    {
                        axis.StartPosition = visibleSeries[i - 1].CustomVertAxis.EndPosition + verticalAxisSpace;
                    }
                }
                //设置纵轴的结束位置
                axis.EndPosition = axis.StartPosition + singleAxisLengthPercent;
               
                SetAxesLeftStyle(axis, (Color)leftLabelColor.EditValue, float.Parse(leftAxesLabelSize.EditValue.ToString()), int.Parse(leftAxisTick.EditValue.ToString()));
             
                SetAxesBottomStyle(tChart.Axes.Bottom,int.Parse(this.bottomLabelAngle.EditValue.ToString()),(Color)bottomLabelColor.EditValue,float.Parse(bottomAxesLabelSize.EditValue.ToString()),int.Parse(bottomMinorTickCount.EditValue.ToString()));

                if (i == 0)
                {
                    //曲线本身的纵轴，无需额外处理
                    //tChart.Axes.Custom.Add(axis);
                    ////将纵轴和对应的曲线关联
                    //s.CustomVertAxis = axis;
                }
                else
                {
                    //将自定义纵轴加入图表
                    tChart.Axes.Custom.Add(axis);
                    //将纵轴和对应的曲线关联
                    s.CustomVertAxis = axis;
                }
            }


        }

        /// <summary>
        /// 导出曲线图
        /// </summary>
        public void ExportChart()
        {
            this.tChart.Export.ShowExportDialog();
        }

        #endregion


        #region 数据处理方法

        /// <summary>
        /// 加减乘除
        /// </summary>
        public void PlusMinusMultiplyDivide()
        {
            tchartEventType = TChartEventType.NoProg;
            if (this.tChart == null)
                return;
            if (this.tChart.Series.Count == 0)
                return;

            Line ln = this.tChart.Series[0] as Line;

            string tablekey = ln.Tag.ToString();
            DataTable dt = ObsdataCls.ObsdataHash[tablekey] as DataTable;
            ObsdataCls.UndoTable = dt.Copy();
            frm_DataProgree dpf = new frm_DataProgree(dt.Rows.Count);
            if (dpf.ShowDialog(this) == DialogResult.OK)
            {
                PriAlgorithmHelper pralg = new PriAlgorithmHelper();
                ObsdataCls.ObsdataHash[tablekey] = pralg.PlusMinusMultiplyDivide(dt, dpf.progreeValue, dpf.dpm);

                AddSeries(new List<string>() { tablekey }, SystemInfo.DatabaseCache, true);
            }
            else
                ObsdataCls.UndoTable = null;
        }
        /// <summary>
        /// 进入消台阶或消突跳操作
        /// </summary>
        public void RemoStepOrJump(TChartEventType tep)
        {
         
            if (this.tChart == null)
                return;
            if (this.tChart.Series.Count != 1)
                return;

            this.tchartEventType = tep;
            start = new Point();//矩形起点
            end = new Point();//矩形终点
            g = this.tChart.CreateGraphics();

            this.tChart.Cursor = Cursors.Cross;

        }

        /// <summary>
        /// 测项合并
        /// </summary>
        public void LinesUnion()
        {
            tchartEventType = TChartEventType.NoProg;
            if (this.tChart == null)
                return;
            if (this.tChart.Series.Count == 0)
                return;

            Line ln = this.tChart.Series[0] as Line;

          
            frm_ObslineMerge olmf = new frm_ObslineMerge();
            if (olmf.ShowDialog(this) == DialogResult.OK)
            {

                DataTable dtone = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;
                ObsdataCls.UndoTable = dtone.Copy();

                string startdate = olmf.StartDateStr;
                string enddate = olmf.EndDateStr;

                List<DataTable> dttowlist = new List<DataTable>();
                foreach (string linecode in olmf.SelectedObsLineCode)
                {
                    DataTable tempDt = LineObsBll.Instance.GetDataTable(linecode, SystemInfo.DatabaseCache);
                    DataRow[] matches = tempDt.Select("obvdate > '"+ startdate + "' and obvdate < '"+enddate+"'");
                  

                    DataTable newDt = tempDt.Clone();
                    if (matches.Length > 0)
                    {
                        foreach (DataRow drVal in matches)
                        {
                            //向B中增加行
                            newDt.ImportRow(drVal);
                        }
                    }
                
                    dttowlist.Add(newDt);
                }
                if (dttowlist.Count == 0)
                    return;

                PriAlgorithmHelper pralg = new PriAlgorithmHelper();
                ObsdataCls.ObsdataHash[ln.Tag.ToString()] = pralg.Merge(dtone, dttowlist, olmf.MoveToAverage);
                AddSeries(new List<string>() { ln.Tag.ToString() }, SystemInfo.DatabaseCache, true);
            }
            else
            {
                ObsdataCls.UndoTable = null;
            }
        }
        /// <summary>
        /// 测项拆分
        /// </summary>
        public void LinesBreak(TChartEventType tep)
        {

            if (this.tChart == null)
                return;
            if (this.tChart.Series.Count == 0)
                return;

            //this.tChart.Cursor = Cursors.Cross;

            this.tchartEventType = tep;
            start = new Point();//矩形起点
            end = new Point();//矩形终点
            g = this.tChart.CreateGraphics();

            this.tChart.Cursor = Cursors.Cross;


        }
        /// <summary>
        /// 等间隔采样
        /// 2017.06.27
        /// 张超
        /// </summary>
        public void IntervalPross()
        {
            try
            {
                if (this.tChart == null)
                    return;
                if (this.tChart.Series.Count == 0)
                    return;

                tchartEventType = TChartEventType.NoProg;

                Line ln = this.tChart.Series[0] as Line;
                DataTable dt = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;
                ObsdataCls.UndoTable = dt.Copy();
                frm_Interval interval = new frm_Interval();
                int inter = 0;
                if (interval.ShowDialog(this) == DialogResult.OK)
                {
                    inter = interval.Interval;

                    PriAlgorithmHelper test = new PriAlgorithmHelper();
                    DataTable resdt = test.Interval(dt, inter, 1);
                    if (resdt == null)
                        return;
                    if (resdt.Rows.Count == 0)
                        return;
                    ObsdataCls.ObsdataHash[ln.Tag.ToString()] = resdt;
                    AddSeries(new List<string>() { ln.Tag.ToString() }, SystemInfo.DatabaseCache, true);
                }
                else
                {
                    ObsdataCls.UndoTable = null;
                }
            }
            catch (Exception ex)
            {
                ObsdataCls.UndoTable = null;
            }
        }

        /// <summary>
        /// 保存处理数据
        /// </summary>
        public void SaveHandleData()
        {
            try
            {
                Line ln = this.tChart.Series[0] as Line;
                frm_SaveToManip stmfrm = new frm_SaveToManip(ln.Title);
                if (stmfrm.ShowDialog(this) == DialogResult.OK)
                {

                    DataTable dt = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;
                    NpoiCreator npcreator = new NpoiCreator();
                    npcreator.TemplateFile = SystemInfo.HandleDataCache;
                    npcreator.NpoiExcel(dt,  SystemInfo.HandleDataCache + "/" + stmfrm.targitFileName + ".xls");
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("保存失败！：" + ex.Message, "错误");
            }
        }
        /// <summary>
        /// 画消突跳或者台阶的标注点
        /// </summary>
        private void DrawJumOrStepPoints(MouseEventArgs e)
        {
            if (isDrawing)
            {
                //清空选中点数组的内容
                selectedPtlist.Clear();

                g.DrawRectangle(new Pen(Color.Blue), start.X, start.Y, e.X - start.X, e.Y - start.Y);
                int minX = Math.Min(start.X, e.X);
                int minY = Math.Min(start.Y, e.Y);
                int maxX = Math.Max(start.X, e.X);
                int maxY = Math.Max(start.Y, e.Y);

                try
                {
                    if (tChart != null)
                    {
                        if (tChart.Series.Count > 0)
                        {
                            Series series = tChart.Series[0];
                            Line ln = series as Line;
                            DataTable dt = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;

                            //this.tChart.Refresh();
                            for (int i = 0; i < ln.Count; i++)
                            {
                                int screenX = series.CalcXPosValue(ln[i].X);
                                int screenY = series.CalcYPosValue(ln[i].Y);

                                if (screenY < 0)
                                {
                                    if (screenX >= minX && screenX <= maxX)
                                    {
                                        //RemoveJumpORStepPoints.Add(ln[i].X, ln[i].Y);
                                        selectedPtlist.Add(new SelectedPointStruct() { PtDate = ln[i].X, PtValue = double.NaN, Note = ln[i].Label });
                                    }
                                }
                                else
                                {
                                    if (screenX >= minX && screenX <= maxX && screenY >= minY && screenY <= maxY)
                                    {
                                        //RemoveJumpORStepPoints.Add(ln[i].X, ln[i].Y);
                                        selectedPtlist.Add(new SelectedPointStruct() { PtDate = ln[i].X, PtValue = ln[i].Y, Note = ln[i].Label });
                                    }
                                }
                            }
                            this.tChart.Refresh();

                            switch (tchartEventType)
                            {
                                case TChartEventType.RemoveJump://消突跳
                                    {

                                        DataTable selectdt = dt.Clone();
                                        for (int i = 0; i < selectedPtlist.Count; i++)
                                        {
                                            DataRow newdr = selectdt.NewRow();
                                            newdr[0] = DateTime.FromOADate(selectedPtlist[i].PtDate);
                                            newdr[1] = selectedPtlist[i].PtValue;
                                            newdr[2] = selectedPtlist[i].Note;
                                            selectdt.Rows.Add(newdr);
                                        }
                                        if (selectdt == null)
                                            return;
                                        if (selectdt.Rows.Count == 0)
                                            return;
                                        
                                            frm_RemoveJump dpf = new frm_RemoveJump(dt, selectdt);
                                        if (dpf.ShowDialog(this) == DialogResult.OK)
                                        {
                                            ObsdataCls.ObsdataHash[ln.Tag.ToString()] = dpf.dataout;
                                            /*重画曲线*/
                                            AddSeries(new List<string>() { ln.Tag.ToString() }, SystemInfo.DatabaseCache,true);
                                            //AddSeries(new List<string>() { ln.Tag.ToString() }, SystemInfo.DatabaseCache);
                                            //AddSingleSeries(this.tChart.Header.Text, ln.Tag.ToString());
                                            tchartEventType = TChartEventType.NoProg;
                                        }
                                        else
                                        {
                                            selectedPtlist.Clear();
                                            tChart.Refresh();
                                            g = this.tChart.CreateGraphics();
                                        }
                                        tChart.Refresh();
                                    }
                                    break;
                                case TChartEventType.RemoveStep://消台阶
                                    {

                                        DataTable selectdt = dt.Clone();
                                        for (int i = 0; i < selectedPtlist.Count; i++)
                                        {
                                            DataRow newdr = selectdt.NewRow();
                                            newdr[0] = DateTime.FromOADate(selectedPtlist[i].PtDate);
                                            newdr[1] = selectedPtlist[i].PtValue;
                                            newdr[2] = selectedPtlist[i].Note;
                                            selectdt.Rows.Add(newdr);
                                        }
                                        if (selectdt == null)
                                            return;
                                        if (selectdt.Rows.Count == 0)
                                            return;

                                        ObsdataCls.UndoTable = dt.Copy();
                                        frm_RemoveStep dpf = new frm_RemoveStep(dt, selectdt);
                                        if (dpf.ShowDialog(this) == DialogResult.OK)
                                        {
                                            ObsdataCls.ObsdataHash[ln.Tag.ToString()] = dpf.dataout;
                                            /*重画曲线*/
                                            AddSeries(new List<string>() { ln.Tag.ToString() }, SystemInfo.DatabaseCache,true);
                                            // AddSeries(new List<string>() { ln.Tag.ToString() }, SystemInfo.DatabaseCache);
                                            //AddSingleSeries(this.tChart.Header.Text, ln.Tag.ToString());
                                            tchartEventType = TChartEventType.NoProg;
                                        }

                                        else
                                        {
                                            selectedPtlist.Clear();
                                            tChart.Refresh();
                                            g = this.tChart.CreateGraphics();
                                            ObsdataCls.UndoTable = null;
                                        }
                                    }
                                    break;

                                case TChartEventType.LineBreak://测项拆分
                                    {
                                        DataTable selectdt = dt.Clone();
                                        for (int i = 0; i < selectedPtlist.Count; i++)
                                        {
                                            DataRow newdr = selectdt.NewRow();
                                            newdr[0] = DateTime.FromOADate(selectedPtlist[i].PtDate);
                                            newdr[1] = selectedPtlist[i].PtValue;
                                            newdr[2] = selectedPtlist[i].Note;
                                            selectdt.Rows.Add(newdr);
                                        }
                                        if (selectdt == null) return;
                                        if (selectdt.Rows.Count == 0) return;

                                        ObsdataCls.UndoTable  = dt.Copy();
                                        ObsdataCls.ObsdataHash[ln.Tag.ToString()] = selectdt;
                                        /*重画曲线*/
                                        AddSeries(new List<string>() { ln.Tag.ToString() }, SystemInfo.DatabaseCache, true);
                                        tchartEventType = TChartEventType.NoProg;
                                    }
                                    break;
                            }

                        }

                    }
                }
                catch
                {
                }
                isDrawing = false;
            }

        }

        /// <summary>
        /// 撤销
        /// </summary>
        public void Undo()
        {
            if (ObsdataCls.UndoTable == null)
                return;
            try
            {
                Line ln = this.tChart.Series[0] as Line;
                string tablekey = ln.Tag.ToString();
                DataTable dt = ObsdataCls.ObsdataHash[tablekey] as DataTable;
                ObsdataCls.RedoTable = dt.Copy();
                ObsdataCls.ObsdataHash[tablekey] = ObsdataCls.UndoTable;
                AddSeries(new List<string>() { tablekey }, SystemInfo.DatabaseCache, true);
            }
            catch { 
            }
            finally {
                ObsdataCls.UndoTable = null;
            }
         
        }

        /// <summary>
        /// 恢复
        /// </summary>
        public void Redo()
        {
            if (ObsdataCls.RedoTable == null)
                return;
            try
            {
                Line ln = this.tChart.Series[0] as Line;
                string tablekey = ln.Tag.ToString();
                DataTable dt = ObsdataCls.ObsdataHash[tablekey] as DataTable;
                ObsdataCls.UndoTable = dt.Copy();
                ObsdataCls.ObsdataHash[tablekey] = ObsdataCls.RedoTable;
                AddSeries(new List<string>() { tablekey }, SystemInfo.DatabaseCache, true);
            }
            catch { }
            finally
            {
                ObsdataCls.RedoTable = null;
            }

        }
        #endregion


        private void Line_GetSeriesMark(Series series, GetSeriesMarkEventArgs e)
        {
            try
            {
                Line line1 = series as Line;
                DataTable ds = (DataTable)line1.DataSource;

                string notestr = ds.Rows[e.ValueIndex]["note"].ToString();
                if (notestr.Length > 30)
                {
                    notestr = notestr.Substring(0, 30) + "...";
                }
                e.MarkText = notestr;
                foreach (int hitid in hitNoteIndex)
                {
                    if (hitid == e.ValueIndex)
                        e.MarkText = "";
                }                
                
            }
            catch
            { }

        }
        private void TChart_MouseWheel(object sender, MouseEventArgs e)
        {

            try
            {
                if (GetLineSeriesCount() != 1)
                    return;
                if (e.Delta > 0)
                {
                    tChart.Zoom.ZoomPercent(105);
                }
                else
                {
                    tChart.Zoom.ZoomPercent(95);
                }
            }
            catch { }

        }


        private void tChart_AfterDraw(object sender, Steema.TeeChart.Drawing.Graphics3D g3d)
        {
            try
            {
                if (this.tChart.Series.Count > 1)
                {
                    for (int i = 0; i < this.tChart.Series.Count; i++)
                    {
                        Series ss = this.tChart.Series[i];
                        string title = LineBll.Instance.GetNameByID("OBSLINENAME", "OBSLINECODE", ss.Tag.ToString().Split(',')[1]);

                        //起点
                        Point StartPt = ss.ValuePointToScreenPoint(ss.XValues[0], ss.YValues.Maximum);
                        //终点
                        Point EndtPt = ss.ValuePointToScreenPoint(ss.XValues[ss.Count - 1], ss.YValues.Maximum);

                        g3d.TextOut((StartPt.X + EndtPt.X) / 2, StartPt.Y-20, title);

                    }
                }
                else
                {
                    //最大最小值
                    if (ShowMaxMin)
                    {
                        try
                        {
                            List<BaseLine> visibleSeries = GetVisibleLine();
                            foreach (BaseLine vSeri in visibleSeries)
                            {
                                ValueList listXValue = vSeri.XValues;
                                ValueList listYValue = vSeri.YValues;

                                double maxY = vSeri.YValues.Maximum;
                                double minY = vSeri.YValues.Minimum;
                                int indexMax = vSeri.YValues.IndexOf(maxY);
                                int indexMin = vSeri.YValues.IndexOf(minY);

                                //最大值线起点
                                Point maxStartPt = vSeri.ValuePointToScreenPoint(vSeri.XValues[0], maxY);
                                //最大值线终点
                                Point maxEndtPt = vSeri.ValuePointToScreenPoint(vSeri.XValues[vSeri.Count - 1], maxY);

                                //最小值线起点
                                Point minStartPt = vSeri.ValuePointToScreenPoint(vSeri.XValues[0], minY);
                                //最小值线终点
                                Point minEndtPt = vSeri.ValuePointToScreenPoint(vSeri.XValues[vSeri.Count - 1], minY);

                                g3d.Brush.Color = Color.Red;
                                g3d.Pen.Color = Color.Red;

                                g3d.Line(maxStartPt, maxEndtPt);
                                g3d.TextOut((maxStartPt.X + maxEndtPt.X) / 2, maxStartPt.Y - 15, "最大值:" + maxY.ToString());

                                g3d.Brush.Color = Color.Red;
                                g3d.Pen.Color = Color.Red;
                                g3d.Line(minStartPt, minEndtPt);
                                g3d.TextOut((minStartPt.X + minEndtPt.X) / 2, minStartPt.Y - 15, "最小值:" + minY.ToString());

                            }

                        }
                        catch (Exception ex)
                        {
                        }
                    }


                    //显示地震目录

                    if (ShowEqkAnno)
                    {
                        try
                        {
                            foreach (EqkArraCls eac in EqkArralist)
                            {
                                if (eac != null)
                                {
                                    g3d.Pen.Color = Color.Red;
                                    g3d.Pen.Width = 2;
                                    Point mainPt1 = this.tChart.Series[0].ValuePointToScreenPoint(eac.x1, eac.y1);
                                    Point mainPt2 = new Point(mainPt1.X, mainPt1.Y + 40);
                                    g3d.Line(mainPt1.X, mainPt1.Y, mainPt2.X, mainPt2.Y);//主线


                                    g3d.Line(mainPt2.X - 7, mainPt2.Y - 7, mainPt2.X, mainPt2.Y);//箭头线1
                                    g3d.Line(mainPt2.X + 7, mainPt2.Y - 7, mainPt2.X, mainPt2.Y);//箭头线2

                                    g3d.TextOut(mainPt1.X, mainPt1.Y, eac.text);
                                }


                            }
                        }
                        catch { }
                    }

                    if (this.dragMarks != null)
                    {
                        if (this.dragMarks.Active)
                        {
                            Series vSeri = this.tChart.Series[0];
                            DataTable dt = (DataTable)vSeri.DataSource;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(dt.Rows[i]["note"].ToString()))
                                {
                                    Point pt1 = vSeri.ValuePointToScreenPoint(DateTime.Parse(dt.Rows[i]["obvdate"].ToString()).ToOADate(), double.Parse(dt.Rows[i]["obvvalue"].ToString()));

                                    Rectangle rt = new Rectangle(pt1.X - 2, pt1.Y - 2, 4, 4);
                                    g3d.Pen.Color = Color.Green;
                                    g3d.Brush.Color = Color.Green;
                                    g3d.Pen.Style = System.Drawing.Drawing2D.DashStyle.Solid;
                                    g3d.Pen.Width = 2;
                                    g3d.Ellipse(rt);



                                }
                            }

                        }
                    }
                    try
                    {
                        Series vSeri = this.tChart.Series[0];
                        if (this.selectedPtlist.Count == 1)
                        {
                            Point pt1 = vSeri.ValuePointToScreenPoint(this.selectedPtlist[0].PtDate, this.selectedPtlist[0].PtValue);

                            DrawSelectPoint(pt1.X, pt1.Y, this.selectedPtlist[0].PtValue.ToString(), g3d);
                        }
                        else
                        {
                            for (int i = 1; i < this.selectedPtlist.Count; i++)
                            {
                                Point pt1 = vSeri.ValuePointToScreenPoint(this.selectedPtlist[i - 1].PtDate, this.selectedPtlist[i - 1].PtValue);
                                Point pt2 = vSeri.ValuePointToScreenPoint(this.selectedPtlist[i - 1].PtDate, this.selectedPtlist[i].PtValue);
                                Point pt3 = vSeri.ValuePointToScreenPoint(this.selectedPtlist[i].PtDate, this.selectedPtlist[i].PtValue);

                                g3d.Pen.Color = Color.Black;
                                g3d.Pen.Style = System.Drawing.Drawing2D.DashStyle.Dash;
                                g3d.Pen.Width = 1;
                                //g3d.Line(pt1, pt2);
                                //g3d.Line(pt2, pt3);

                                string text = Math.Round((this.selectedPtlist[i].PtValue - this.selectedPtlist[i - 1].PtValue), 3).ToString();
                                g3d.TextOut(pt3.X, pt3.Y, text);

                                DrawSelectPoint(pt1.X, pt1.Y, "", g3d);
                                //补上最后一个选中点
                                if (i == this.selectedPtlist.Count - 1)
                                    DrawSelectPoint(pt3.X, pt3.Y, "", g3d);
                            }
                        }
                    }
                    catch { }

                }
            }
            catch { }
        }

        private void tChart_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)//右键菜单
            {
                if (this.dragMarks == null)
                    return;
                if (this.dragMarks.Active)
                {
                    try
                    {
                        DataTable ds = (DataTable)tChart.Series[0].DataSource;

                        lxp = this.tChart.Series[0].ScreenPointToValuePoint(e.X, e.Y).X;
                        lyp = this.tChart.Series[0].ScreenPointToValuePoint(e.X, e.Y).Y;

                        for (int i = 0; i < ds.Rows.Count; i++)
                        {
                            string notestr = ds.Rows[i]["note"].ToString();
                            if (notestr == "")
                                continue;
                            Point mainPt1 = this.tChart.Series[0].ValuePointToScreenPoint(DateTime.Parse(ds.Rows[i]["obvdate"].ToString()).ToOADate(), double.Parse(ds.Rows[i]["obvvalue"].ToString()));
                            double centerx = mainPt1.X;
                            double centery = mainPt1.Y;

                            double dtx = e.X - centerx;
                            double dty = e.Y - centery;
                            double dist = Math.Sqrt(dtx * dtx + dty * dty);

                            if (dist < 10)
                            {
                                ContextMenu menu = new ContextMenu();
                                MenuItem menu1 = new MenuItem("隐藏标注");
                                menu1.Tag = i;
                                menu1.Click += Menu1_Click;
                                menu.MenuItems.Add(menu1);
                                MenuItem menu2 = new MenuItem("显示标注");
                                menu2.Tag = i;
                                menu2.Click += Menu1_Click;
                                menu.MenuItems.Add(menu2);
                                menu.Show(tChart, new Point(e.X, e.Y));//在点(e.X, e.Y)处显示menu
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message.ToString(), "错误");
                    }
                }

            }
            if (!isAltDown)
            {
                if (e.Button == MouseButtons.Left)
                {
                    selectedPtlist.Clear();
                    tChart.Refresh();
                    g = this.tChart.CreateGraphics();
                    start.X = e.X;
                    start.Y = e.Y;
                    end.X = e.X;
                    end.Y = e.Y;
                    isDrawing = true;
                }
            }
            else//如果按下shift，进入移动地震标注
            {
                lxp = this.tChart.Series[0].ScreenPointToValuePoint(e.X, e.Y).X;
                lyp = this.tChart.Series[0].ScreenPointToValuePoint(e.X, e.Y).Y;

                //查询出与鼠标最近的地震标注
                double mindist = double.MaxValue;

                for (int i = 0; i < EqkArralist.Count; i++)
                {

                    Point mainPt1 = this.tChart.Series[0].ValuePointToScreenPoint(EqkArralist[i].x1, EqkArralist[i].y1);
                    Point mainPt2 = new Point(mainPt1.X, mainPt1.Y + 40);

                    double centerx = mainPt1.X;
                    double centery = (mainPt1.Y + mainPt2.Y) / 2;

                    double dtx = e.X - centerx;
                    double dty = e.Y - centery;
                    double dist = Math.Sqrt(dtx * dtx + dty * dty);

                    if (dist < mindist)
                    {
                        mindist = dist;
                        moveingEqkArr = EqkArralist[i];
                    }

                }

                timer.Enabled = true;
            }
        }

        /// <summary>
        /// 右键菜单项点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu1_Click(object sender, EventArgs e)
        {
            MenuItem mi = sender as MenuItem;
            if (mi.Text == "隐藏标注")
            { 
                hitNoteIndex.Add((int)mi.Tag);
            }
            else if (mi.Text == "显示标注")
            {
                if (hitNoteIndex.Contains((int)mi.Tag))
                    hitNoteIndex.Remove((int)mi.Tag);
            }
            this.tChart.Refresh();
        }

        private void tChart_MouseMove(object sender, MouseEventArgs e)
        {
            if (tchartEventType == TChartEventType.Hotline)//鼠标热线
            {
                int maxX = tChart.Chart.ChartRect.X + tChart.Chart.ChartRect.Width;
                int minX = tChart.Chart.ChartRect.X;
                int maxY = tChart.Chart.ChartRect.Y + tChart.Chart.ChartRect.Height;
                int minY = tChart.Chart.ChartRect.Y;
                List<BaseLine> visibleSeries = GetVisibleLine();
                if (visibleSeries.Count == 0)
                    return;
                PointDouble scrToVa = visibleSeries[0].ScreenPointToValuePoint(e.X, e.Y);

                if (e.X < maxX && e.X > minX && e.Y < maxY && e.Y > minY)
                {
                    if (!this.cursorTool.Active)
                    {
                        return;
                    }
                    else
                    {
                        ValueList listXValue = visibleSeries[0].XValues;
                        ValueList listYValue = visibleSeries[0].YValues;

                        int minIndex = 0;
                        double deltX = Math.Abs(listXValue[0] - scrToVa.X), deltX1;

                        for (int i = 1; i < listXValue.Count; i++)
                        {
                            deltX1 = Math.Abs(listXValue[i] - scrToVa.X);
                            if (deltX > deltX1)
                            {
                                minIndex = i;
                                deltX = deltX1;
                            }
                            else break;
                        }
                        Point poToScr = visibleSeries[0].ValuePointToScreenPoint(listXValue[minIndex], listYValue[minIndex]);
                        DateTime showTime = DateTime.FromOADate(listXValue[minIndex]);
                        string showTxt = "观测时间:" + showTime.ToShortDateString() + "\r\n" + "观测值:" + listYValue[minIndex].ToString();

                        annotation.Top = int.Parse(poToScr.Y.ToString());
                        annotation.Left = int.Parse(poToScr.X.ToString());
                        annotation.Text = showTxt;
                    }
                }
            }
            else if(isAltDown)
            {
               
                if (!timer.Enabled)
                    return;
                if (MouseButtons != MouseButtons.Left)
                {
                    isAltDown = false;
                    timer.Enabled = false;
                }
                double dxp, dyp;
                dxp = this.tChart.Series[0].ScreenPointToValuePoint(e.X,e.Y).X - lxp;
                dyp = this.tChart.Series[0].ScreenPointToValuePoint(e.X, e.Y).Y - lyp;
                lxp = this.tChart.Series[0].ScreenPointToValuePoint(e.X, e.Y).X;
                lyp = this.tChart.Series[0].ScreenPointToValuePoint(e.X, e.Y).Y;
                if (moveingEqkArr != null)
                { 
                    moveingEqkArr.move(0, dyp);
                   
                }
                tChart.Refresh();
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (isDrawing)
                    {
                        this.tChart.Cursor = Cursors.Cross;
                        //先擦除
                        g.DrawRectangle(new Pen(Color.White), start.X, start.Y, end.X - start.X, end.Y - start.Y);
                        end.X = e.X;
                        end.Y = e.Y;
                        //再画
                        g.DrawRectangle(new Pen(Color.Blue), start.X, start.Y, end.X - start.X, end.Y - start.Y);
                    }
                }
            }

        }

        private void tChart_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.tChart.Cursor = Cursors.Arrow;
                DrawJumOrStepPoints(e);
            }
        }

        private void tChart_ClickSeries(object sender, Series s, int valueIndex, MouseEventArgs e)
        {
            try
            {
                Line ln = s as Line;
                if (this.tChart.Series.Count > 1)
                {
                    if (ln.Tag.ToString().Split(',')[0] == "处理数据缓存") 
                    {
                        AddSeries2(new List<string>() { ln.Tag.ToString() }, SystemInfo.HandleDataCache);
                    }
                    else
                    {
                        AddSeries(new List<string>() { ln.Tag.ToString() }, SystemInfo.DatabaseCache,false);
                    }
                }
                   
                //AddSingleSeries(ln.Title, ln.Tag.ToString());
            }
            catch (Exception ex)
            {
                // XtraMessageBox.Show("错误", ex.Message);
            }
        }


        /// <summary>
        /// 鼠标离开测线，变窄
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Line_MouseLeave(object sender, EventArgs e)
        {
            Line ln = sender as Line;
            ln.LinePen.Width--;

        }

        /// <summary>
        /// 鼠标进入测线，变宽
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Line_MouseEnter(object sender, EventArgs e)
        {
            Line ln = sender as Line;
            ln.LinePen.Width++;
        }

        #region 观测数据的显示、增加、、修改


        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddData_Click(object sender, EventArgs e)
        {
            Line ln = this.tChart.Series[0] as Line;
            DataTable dt = ObsdataCls.ObsdataHash[ln.Tag.ToString()] as DataTable;
            actiontype = ActionType.Add;
            int focusedRow = this.gridView1.FocusedRowHandle;
            this.gridView1.AddNewRow();
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteData_Click(object sender, EventArgs e)
        {
            actiontype = ActionType.Delete;
            //int focusedRow = this.gridView1.FocusedRowHandle;
            try
            {
                DataTable dt = this.gridControlObsdata.DataSource as DataTable;
                ObsdataCls.UndoTable = dt.Copy();
                int[] selectedIndexs = this.gridView1.GetSelectedRows();
                for (int i = 0; i < selectedIndexs.Length; i++)
                {
                    DataRowView drv = (DataRowView)this.gridView1.GetRow(selectedIndexs[i]);
                    dt.Rows[selectedIndexs[i]]["obvvalue"] = double.NaN;
                    //DateTime obsdate = new DateTime();
                    //DateTime.TryParse(drv["obvdate"].ToString(), out obsdate);
                    //double obsv = double.NaN;
                    //double.TryParse(drv["obvvalue"].ToString(), out obsv);


                    //Line ln = this.tChart.Series[0] as Line;
                    //for (int j = 0; i < this.tChart.Series[0].Count; j++)
                    //{
                    //    if (DateTime.FromOADate(ln[j].X) == obsdate && obsv == ln[j].Y)
                    //    {
                    //        ln.Delete(i);
                    //        //ln[j].Y = double.NaN;
                    //    }
                    //}
                }

                Line ln = this.tChart.Series[0] as Line;
                ObsdataCls.ObsdataHash[ln.Tag.ToString()] = dt;
                AddSeries(new List<string>() { ln.Tag.ToString() }, SystemInfo.DatabaseCache, true);

                
            }
            catch (Exception ex)
            {
                actiontype = ActionType.NoAction;
                ObsdataCls.UndoTable = null;
            }

            this.tChart.Refresh();
            gridView1.UpdateCurrentRow();
        }

 
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
               
                int focusedRow = e.RowHandle;
                DataRowView drv = (DataRowView)this.gridView1.GetRow(focusedRow);

                if (drv["obvdate"].ToString() == "" || drv["obvvalue"].ToString() == "")
                    return;

                DateTime obsdate = new DateTime();
                DateTime.TryParse(drv["obvdate"].ToString(), out obsdate);

                double obdv = double.NaN;
                double.TryParse(drv["obvvalue"].ToString(), out obdv);
                gridView1.UpdateCurrentRow();



                Line ln = this.tChart.Series[0] as Line;

                DataView dv = gridView1.DataSource as DataView;
                ObsdataCls.ObsdataHash[ln.Tag.ToString()] = dv.Table;
                AddSeries(new List<string>() { ln.Tag.ToString() }, SystemInfo.DatabaseCache, true);

                //if (actiontype == ActionType.Add)
                //{
                //    AddChartlineData(obsdate, obdv);
                //    actiontype = ActionType.NoAction;
                //}
                //else
                //{
                //    ModifyChartlineData(focusedRow, obsdate, obdv);
                //}
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "错误");
            }

        }

        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (actiontype == ActionType.Modify)
                e.Cancel = false;
            else if (actiontype == ActionType.Add)
            {
                /*
                 * 新增状态下只有新增行可以编辑
                 */
                DataRowView drv = (DataRowView)this.gridView1.GetRow(this.gridView1.FocusedRowHandle);
                if (drv["obvvalue"].ToString() == "" || drv["obvdate"].ToString() == "")
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }


        #endregion

        /// <summary>
        /// 获取曲线Series数量
        /// </summary>
        /// <returns></returns>
        private int GetLineSeriesCount()
        {
            int seriesCount = 0;
            for (int i = 0; i < this.tChart.Series.Count; i++)
            {
                try
                {
                    Line ln = this.tChart.Series[i] as Line;
                    if (ln != null)
                        seriesCount++;
                }
                catch
                {
                    continue;
                }
            }
            return seriesCount;
        }

        public void GetEqkShowForm()
        {
            tchartEventType = TChartEventType.NoProg;

            frm_EqkSelect esf = new frm_EqkSelect();
            if (esf.ShowDialog() == DialogResult.OK)
            {
                if (esf.isSelectedDb)
                {
                    if (this.tChart.Series.Count > 0)
                    {
                        eqkfrm = new frm_EqkShow(this.tChart.Series[0].Tag, this.tChart);
                        eqkfrm.ShowEqkNote += Eqkfrm_ShowEqkNote;
                        eqkfrm.ShowDialog(this);
                        eqkfrm.Focus();
                    }
                }
                else
                {
                    if (this.tChart.Series.Count > 0)
                    {
                        OpenFileDialog ofd = new OpenFileDialog();

                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            ShowEqkAnno = true;
                            //EqkArralist.Clear();

                            string eqkfile = ofd.FileName;
                            StreamReader sr = new StreamReader(eqkfile, Encoding.Default);

                            string eakText = "";
                            string eqkTimeStr = "";
                            string eqkMgdStr = "";
                            string eqkPlaceStr = "";
                            double scale = 1.0;
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {

                                if (string.IsNullOrEmpty(line))
                                    continue;
                                try
                                {
                                    eqkTimeStr = line.Split(',')[0];
                                    eqkMgdStr = line.Split(',')[1];
                                    eqkPlaceStr = line.Split(',')[2];

                                    DateTime eqkTime = DateTime.Parse(eqkTimeStr);
                                    double maxX = tChart.Series[0].XValues.Maximum;
                                    double minX = tChart.Series[0].XValues.Minimum;
                                    DateTime maxEqkT = DateTime.FromOADate(maxX);
                                    DateTime minEqkT = DateTime.FromOADate(minX);
                                    TimeSpan spanT = eqkTime.Subtract(minEqkT);
                                    double eqkT = spanT.Days + minX;

                                    double maxY = tChart.Chart.Series[0].MaxYValue();
                                    double minY = tChart.Chart.Series[0].MinYValue();
                                    scale = maxY - minY;

                                    int index0 = tChart.Chart.Series[0].XValues.IndexOf(maxX);
                                    int index1 = tChart.Chart.Series[0].XValues.IndexOf(minX);
                                    int index2 = tChart.Chart.Series[0].XValues.IndexOf((minX + maxX) / 2.0);

                                    //观测时间距离地震时间最近索引
                                    int minIndex = 0;
                                    double deltX = Math.Abs(tChart.Chart.Series[0].XValues[0] - eqkT), deltX1;

                                    for (int j = 1; j < tChart.Chart.Series[0].XValues.Count; j++)
                                    {
                                        deltX1 = Math.Abs(tChart.Chart.Series[0].XValues[j] - eqkT);
                                        if (deltX > deltX1)
                                        {
                                            minIndex = j;
                                            deltX = deltX1;
                                        }
                                        else break;
                                    }

                                    //标注地震事件
                                    if (maxEqkT.CompareTo(eqkTime) > 0 && minEqkT.CompareTo(eqkTime) < 0)
                                    {
                                        eakText = eqkPlaceStr + "\r\n" + "ML=" + eqkMgdStr;

                                        EqkArraCls eqkarrcals = new EqkArraCls(eqkTime.ToOADate(), tChart.Chart.Series[0].YValues[minIndex], eakText);
                                        EqkArralist.Add(eqkarrcals);

                                    }

                                }
                                catch { continue; }
                            }

                            this.tChart.Refresh();
                        }
                    }
                }
            }
            else
                return;
        }

        /// <summary>
        /// 显示地震目录
        /// </summary>
        /// <param name="selectedEqkAno"></param>
        private void Eqkfrm_ShowEqkNote(List<EqkArraCls> selectedEqkAno)
        {
            if (selectedEqkAno.Count > 0)
            {
                ShowEqkAnno = true;
                
                //Series vSeri = this.tChart.Series[0];
                EqkArralist = selectedEqkAno;
                //foreach (EqkArraCls ea in selectedEqkAno)
                //{
                //    Point pt1 = vSeri.ValuePointToScreenPoint(ea.dateTime.ToOADate(), ea.value);


                //    EqkArraCls eqkarr = new EqkArraCls(pt1.X, pt1.Y, ea.text);
                //    EqkArralist.Add(eqkarr);
                //}
                //this.dragMarks = new DragMarks();
                //this.tChart.Tools.Add(this.dragMarks);
                //this.dragMarks.Series = this.tChart.Series[0];
                //this.dragMarks.Active = true;
                this.tChart.Series[0].Marks.ResetPositions();
            }
        }


        public void ExportToExcel(string pah)
        {
            this.gridControlObsdata.ExportToXls(pah);
        }
        public void ExportToTxt(string pah)
        {
            this.gridControlObsdata.ExportToText(pah);
        }

        private void DrawSelectPoint(int x, int y, string value, Graphics3D g3d)
        {
            Rectangle rt = new Rectangle(x - 8, y - 8, 15, 15);
            g3d.Pen.Color = Color.Red;
            g3d.Brush.Color = Color.FromArgb(0, 0, 0, 0);
            g3d.Pen.Style = System.Drawing.Drawing2D.DashStyle.Solid;
            g3d.Pen.Width = 2;
            g3d.Ellipse(rt);
            g3d.TextOut(x, y, value);

        }


        private double lxp, lyp;
        private bool isAltDown;//是否按下shift按钮
        private bool ShowEqkAnno;//是否显示地震标注
        private List<EqkArraCls> EqkArralist =new List<EqkArraCls>();
        private EqkArraCls moveingEqkArr;//当前移动的地震目录

        /// <summary>
        /// 清除震例
        /// </summary>
        public void DeleteEqkAnnotation()
        {
            EqkArralist.Clear();
            this.tChart.Refresh();
        }

        private void tChart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
                isAltDown = true;
            if (e.KeyCode == Keys.Delete)
            {
                
            }
        }

        private void tChart_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
                isAltDown = false;
        }

        /// <summary>
        /// 标题显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsTitle_Toggled(object sender, EventArgs e)
        {
            if (this.tChart.Series.Count > 1 || this.tChart.Series.Count == 0)
            {
                tsTitle.IsOn = false;
                return;
            }

            ToggleSwitch ts = sender as ToggleSwitch;
            if (ts.IsOn)
            {
                this.tChart.Header.Visible = true;
            }
            else
            {
                this.tChart.Header.Visible = false;
            }
           
        }
        /// <summary>
        /// 鼠标热线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsHotline_Toggled(object sender, EventArgs e)
        {
            if (this.tChart.Series.Count > 1 || this.tChart.Series.Count == 0)
            {
                tsHotline.IsOn = false;
                return;
            }

            ToggleSwitch ts = sender as ToggleSwitch;
            if (ts.IsOn)
            {
                this.cursorTool.Active = true;
            }
            else
            {
                this.cursorTool.Active = false;
            }

            try
            {
                annotation.Active = this.cursorTool.Active;

                if (annotation.Active && this.cursorTool.Active)
                {
                    this.tchartEventType = TChartEventType.Hotline;
                }
                else
                {
                    this.tchartEventType = TChartEventType.NoProg;
                }
            }
            catch
            { }
        }


        /// <summary>
        /// 显示备注
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsNote_Toggled(object sender, EventArgs e)
        {
            if (this.tChart.Series.Count > 1|| this.tChart.Series.Count == 0)
            {
                tsNote.IsOn = false;
                return;
            }

            try
            {
                ToggleSwitch ts = sender as ToggleSwitch;
                if (ts.IsOn)
                {
                    this.hitNoteIndex = new List<int>();
                    this.dragMarks = new DragMarks();
                    this.tChart.Tools.Add(this.dragMarks);
                    this.dragMarks.Active = true;
                    Line ln = this.tChart.Series[0] as Line;

                    if (this.tChart.Series.Count > 1)
                    {
                        this.tChart.Series.RemoveAt(1);
                    }

                    int j = 0;
                    foreach (DataRow dr in ((DataTable)ln.DataSource).Rows)
                    {
                        if (dr["note"].ToString() != "")
                        {

                            notePtlist.Add(new SelectedPointStruct() { PtDate = ln[j].X, PtValue = ln[j].Y });
                        }
                        j++;
                    }


                    TextShapeStyle tss = TextShapeStyle.Rectangle;
                    if (cmbNoteStyle.SelectedText == "矩形")
                        tss = TextShapeStyle.Rectangle;
                    else if (cmbNoteStyle.SelectedText == "圆角矩形")
                        tss = TextShapeStyle.RoundRectangle;

                    SetMarkStyle(fontNote.SelectedItem.ToString(), float.Parse(noteSize.EditValue.ToString()), GetSelectedFontStyle(fontNote.EditValue.ToString()), (Color)colorNote.EditValue, tss);
                    this.tChart.Refresh();


                    this.dragMarks.Series = this.tChart.Series[0];
                }
                else
                {
                    this.dragMarks.Active = false;
                    this.tChart.Series[0].Marks.Visible =false;
                    if (this.tChart.Series.Count > 1)
                    {
                        this.tChart.Series.RemoveAt(1);
                    }
                }
            }
            catch { }

        }

        /// <summary>
        /// 格网显示方法
        /// </summary>
        /// <param name="visibility"></param>
        public void GridVisible(bool visibility)
        {
            for (int i = 0; i < this.tChart.Axes.Custom.Count; i++)
            {
                this.tChart.Axes.Custom[i].Grid.DrawEvery = 1;
                this.tChart.Axes.Custom[i].Grid.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                this.tChart.Axes.Custom[i].Grid.Transparency = 0;
                this.tChart.Axes.Custom[i].Grid.Visible = visibility;
            }

            this.tChart.Axes.Left.Grid.DrawEvery = 1;
            this.tChart.Axes.Left.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dot;
            this.tChart.Axes.Left.Grid.Transparency = 0;
            this.tChart.Axes.Left.Grid.Visible = visibility;

            this.tChart.Axes.Bottom.Grid.DrawEvery = 1;
            this.tChart.Axes.Bottom.Grid.Style = System.Drawing.Drawing2D.DashStyle.Dot;
            this.tChart.Axes.Bottom.Grid.Transparency = 0;
            this.tChart.Axes.Bottom.Grid.Visible = visibility;
        }


        /// <summary>
        /// 显示格网
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsGrid_Toggled(object sender, EventArgs e)
        {
            if (this.tChart.Series.Count > 1 || this.tChart.Series.Count == 0)
            {
                tsGrid.IsOn = false;
                return;
            }
            ToggleSwitch ts = sender as ToggleSwitch;
            if (ts.IsOn)
            {
                GridVisible(true);
            }
            else
            {
                GridVisible(false);
            }
        }

        /// <summary>
        /// 显示最大最小值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsMaxMin_Toggled(object sender, EventArgs e)
        {
            if (this.tChart.Series.Count > 1 || this.tChart.Series.Count == 0)
            {
                tsMaxMin.IsOn = false;
                return;
            }
            ToggleSwitch ts = sender as ToggleSwitch;
            if (ts.IsOn)
            {
                ShowMaxMin = true;
            }
            else
            {
                ShowMaxMin = false;
            }
            this.tChart.Refresh();
        }

        /// <summary>
        /// 曲线颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void colorSeries_EditValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < this.tChart.Series.Count; i++)
            {
                this.tChart.Series[i].Color = (Color)colorSeries.EditValue;
            }
        }


        /// <summary>
        /// 标题颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void colorTitle_EditValueChanged(object sender, EventArgs e)
        {
            SetTitle(GetTitleName(), GetTitleZt(), GetTitleSize(), GetTitleFontStyle(), (Color)colorTitle.EditValue);
         
        }
        /// <summary>
        /// 标题字体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void titleFontEdit_SelectedValueChanged(object sender, EventArgs e)
        {
            SetTitle(GetTitleName(), titleFontEdit.SelectedItem.ToString(), GetTitleSize(), GetTitleFontStyle(), GetTitleColor());
        
        }
        /// <summary>
        /// 标题大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void titleSize_EditValueChanged(object sender, EventArgs e)
        {
            SetTitle(GetTitleName(), GetTitleZt(), float.Parse(titleSize.Value.ToString()), GetTitleFontStyle(), GetTitleColor());

        }

        /// <summary>
        /// 标题样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void titleStyle_SelectedValueChanged(object sender, EventArgs e)
        {
            SetTitle(GetTitleName(), GetTitleZt(), GetTitleSize(), GetSelectedFontStyle(titleStyle.SelectedText), GetTitleColor());
        }

        /// <summary>
        /// 标签颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leftLabelColor_EditValueChanged(object sender, EventArgs e)
        {
            SetAxesLeftStyle(this.tChart.Axes.Left, (Color)leftLabelColor.EditValue, float.Parse(leftAxesLabelSize.EditValue.ToString()), int.Parse(leftAxisTick.EditValue.ToString()));
        }

        /// <summary>
        /// 标签字体大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leftAxesLabelSize_EditValueChanged(object sender, EventArgs e)
        {
            SetAxesLeftStyle(this.tChart.Axes.Left, (Color)leftLabelColor.EditValue, float.Parse(leftAxesLabelSize.EditValue.ToString()), int.Parse(leftAxisTick.EditValue.ToString()));

        }
      
        /// <summary>
        /// 小刻度数量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void leftAxisTick_EditValueChanged(object sender, EventArgs e)
        {
            SetAxesLeftStyle(this.tChart.Axes.Left, (Color)leftLabelColor.EditValue, float.Parse(leftAxesLabelSize.EditValue.ToString()), int.Parse(leftAxisTick.EditValue.ToString()));

        }

        private void bottomLabelCount_EditValueChanged(object sender, EventArgs e)
        {
            SetAxesBottomStyle(tChart.Axes.Bottom, int.Parse(this.bottomLabelAngle.EditValue.ToString()), (Color)bottomLabelColor.EditValue, float.Parse(bottomAxesLabelSize.EditValue.ToString()), int.Parse(bottomMinorTickCount.EditValue.ToString()));

        }

        private void bottomLabelColor_EditValueChanged(object sender, EventArgs e)
        {
            SetAxesBottomStyle(tChart.Axes.Bottom, int.Parse(this.bottomLabelAngle.EditValue.ToString()), (Color)bottomLabelColor.EditValue, float.Parse(bottomAxesLabelSize.EditValue.ToString()), int.Parse(bottomMinorTickCount.EditValue.ToString()));

        }

        private void bottomAxesLabelSize_EditValueChanged(object sender, EventArgs e)
        {
            SetAxesBottomStyle(tChart.Axes.Bottom, int.Parse(this.bottomLabelAngle.EditValue.ToString()), (Color)bottomLabelColor.EditValue, float.Parse(bottomAxesLabelSize.EditValue.ToString()), int.Parse(bottomMinorTickCount.EditValue.ToString()));

        }

       
        private void bottomMinorTickCount_EditValueChanged(object sender, EventArgs e)
        {
            SetAxesBottomStyle(tChart.Axes.Bottom, int.Parse(this.bottomLabelAngle.EditValue.ToString()), (Color)bottomLabelColor.EditValue, float.Parse(bottomAxesLabelSize.EditValue.ToString()), int.Parse(bottomMinorTickCount.EditValue.ToString()));

        }


        private void colorNote_EditValueChanged(object sender, EventArgs e)
        {
            TextShapeStyle tss = TextShapeStyle.Rectangle;

            if (cmbNoteStyle.SelectedText == "矩形")
                tss = TextShapeStyle.Rectangle;
            else if (cmbNoteStyle.SelectedText == "圆角矩形")
                tss = TextShapeStyle.RoundRectangle;

            SetMarkStyle(fontNote.SelectedItem.ToString(), float.Parse(noteSize.EditValue.ToString()), GetSelectedFontStyle(fontNote.EditValue.ToString()), (Color)colorNote.EditValue, tss);
            this.tChart.Refresh();
        }

        private void noteSize_EditValueChanged(object sender, EventArgs e)
        {
            TextShapeStyle tss = TextShapeStyle.Rectangle;

            if (cmbNoteStyle.SelectedText == "矩形")
                tss = TextShapeStyle.Rectangle;
            else if (cmbNoteStyle.SelectedText == "圆角矩形")
                tss = TextShapeStyle.RoundRectangle;

            SetMarkStyle(fontNote.SelectedItem.ToString(), float.Parse(noteSize.EditValue.ToString()), GetSelectedFontStyle(fontNote.EditValue.ToString()), (Color)colorNote.EditValue, tss);
            this.tChart.Refresh();
        }

        private void cmbNoteStyle_EditValueChanged(object sender, EventArgs e)
        {
            TextShapeStyle tss = TextShapeStyle.Rectangle;

            if (cmbNoteStyle.SelectedText == "矩形")
                tss = TextShapeStyle.Rectangle;
            else if (cmbNoteStyle.SelectedText == "圆角矩形")
                tss = TextShapeStyle.RoundRectangle;

            SetMarkStyle(fontNote.SelectedItem.ToString(), float.Parse(noteSize.EditValue.ToString()), GetSelectedFontStyle(fontNote.EditValue.ToString()), (Color)colorNote.EditValue, tss);
            this.tChart.Refresh();
        }

        private void fontNote_EditValueChanged(object sender, EventArgs e)
        {
            TextShapeStyle tss = TextShapeStyle.Rectangle;

            if (cmbNoteStyle.SelectedText == "矩形")
                tss = TextShapeStyle.Rectangle;
            else if (cmbNoteStyle.SelectedText == "圆角矩形")
                tss = TextShapeStyle.RoundRectangle;

            SetMarkStyle(fontNote.SelectedItem.ToString(), float.Parse(noteSize.EditValue.ToString()), GetSelectedFontStyle(fontNote.EditValue.ToString()), (Color)colorNote.EditValue, tss);
            this.tChart.Refresh();
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            try
            {
                int[] selectedIndexs = this.gridView1.GetSelectedRows();

                selectedPtlist.Clear();

                for (int i = 0; i < selectedIndexs.Length; i++)
                {
                    int selectedIndex = selectedIndexs[i];
                    DataRowView drv = (DataRowView)this.gridView1.GetRow(selectedIndex);
                    DateTime obsdate = new DateTime();
                    DateTime.TryParse(drv["obvdate"].ToString(), out obsdate);
                    double obsv = double.NaN;
                    double.TryParse(drv["obvvalue"].ToString(), out obsv);
                    selectedPtlist.Add(new SelectedPointStruct() { PtDate = obsdate.ToOADate(), PtValue = obsv });
                }
                this.tChart.Refresh();
            }
            catch
            { }
        }


        private void tChart_Zoomed(object sender, EventArgs e)
        {
            if (this.dragMarks != null)
            {
                if (this.dragMarks.Active)
                    this.tChart.Series[0].Marks.ResetPositions();
            }
        }


    }


}
