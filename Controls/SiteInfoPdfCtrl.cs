/***********************************************************/
//---模    块：pdf操作类
//---功能描述：pdf文件的路径设置、创建表格、写入内容
//---编码时间：2017-10-30
//---编码人员：刘文龙
//---单    位：一测中心
/***********************************************************/

using System.Windows.Forms;
using System.IO;
using DevExpress.XtraRichEdit.API.Native;
using xxkUI.MyCls;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing;
using System;
using xxkUI.Bll;
using System.Collections.Generic;
using System.Linq;

namespace xxkUI.Controls
{
    public partial class SiteInfoPdfCtrl : UserControl
    {
        private string pFilePath = string.Empty;
        public SiteInfoPdfCtrl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 设置pdf路径
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public bool SetPdfFilePath(string filepath)
        {
            bool isok = false;

            if (!string.IsNullOrEmpty(filepath))
            {
                pFilePath = filepath;
                isok = true;
            }
            return isok;
        }
        /// <summary>
        /// 打开PDF文件
        /// </summary>
        public void OpenFile()
        {
            this.pdfViewer.LoadDocument(pFilePath);
        }

        public void WriteContent(SiteBean sb)
        {
            try
            {
                this.pdfViewer.CloseDocument();

                if (File.Exists(pFilePath))
                    File.Delete(pFilePath);

                FileStream myStream = new FileStream(pFilePath, FileMode.Create);           //文件流

                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4);         //创建A4纸、横向PDF文档
                PdfWriter writer = PdfWriter.GetInstance(document, myStream);   //将PDF文档写入创建的文件中
                document.Open();


                //要在PDF文档中写入中文必须指定中文字体，否则无法写入中文
                BaseFont bftitle = BaseFont.CreateFont("C:\\Windows\\Fonts\\SIMHEI.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);   //用系统中的字体文件SimHei.ttf创建文件字体
                iTextSharp.text.Font fonttitle = new iTextSharp.text.Font(bftitle, 22);     //标题字体，大小30
                                                                                            //单元格中的字体，大小12

                //添加标题
                iTextSharp.text.Paragraph Title = new iTextSharp.text.Paragraph(sb.SiteName + " 跨断层监测场地基本情况", fonttitle);     //添加段落，第二个参数指定使用fonttitle格式的字体，写入中文必须指定字体否则无法显示中文
                Title.Alignment = iTextSharp.text.Rectangle.ALIGN_CENTER;       //设置居中
                document.Add(Title);        //将标题段加入PDF文档中


                //空一行
                BaseFont bf1 = BaseFont.CreateFont("C:\\Windows\\Fonts\\SIMSUN.TTC,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);     //用系统中的字体文件SimSun.ttc创建文件字体
                iTextSharp.text.Font ftitle = new iTextSharp.text.Font(bf1, 15);
                iTextSharp.text.Paragraph nullp = new iTextSharp.text.Paragraph("      ", ftitle);
                document.Add(nullp);


                PdfPTable table = CreateTable(sb);

                document.Add(table); //将表格加入PDF文档中 
                document.Close();
                myStream.Close();


                //    PDFOperation pdf = new PDFOperation();
                //pdf.Open(new FileStream(pFilePath, FileMode.Create));
                //pdf.SetBaseFont("C:\\WINDOWS\\FONTS\\STSONG.TTF");
                ////pdf.SetFont(20);
                //pdf.AddParagraph(sb.SiteName + " 跨断层监测场地基本情况", 15, 1, 20, 0, 0);
                //pdf.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private PdfPTable CreateTable(SiteBean sb)
        {

            BaseFont bf1 = BaseFont.CreateFont("C:\\Windows\\Fonts\\SIMSUN.TTC,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);     //用系统中的字体文件SimSun.ttc创建文件字体
            iTextSharp.text.Font namefont = new iTextSharp.text.Font(bf1, 12);
            iTextSharp.text.Font contentfont = new iTextSharp.text.Font(bf1, 10);
            contentfont.SetColor(105, 105, 105);

            PdfPTable table = new PdfPTable(4);
            //table.TableEvent = new AlternatingBackground(); 
            table.WidthPercentage = 85;//设置表格占的宽度，百分比 
            table.SetWidths(new int[] { 35, 65, 35, 65 }); //两个单元格所占比例 20% 80%
                                                           //table.TotalWidth = 200;//设置表格占的宽度，单位点数 
                                                           //table.SetTotalWidth(); 
                                                           //table.SetWidthPercentage(); 
            //cell.Colspan = (4); //合并列; 
            //cell.Rowspan = (1); //合并行;

            PdfPCell cell = new PdfPCell(new Phrase("场地名称", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.SiteName, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("所跨断裂断层", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.FaultCode, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("运行状况", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.SiteStatus, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("观测周期", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.ObsCyc, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("历史场地", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.Historysite, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("场地类型", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.SiteType, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("观测类型", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.ObsType, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("标石类型", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.MarkStoneType, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("所在地", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.Place, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            //cell.Colspan = (3); //合并列; 
            cell.MinimumHeight = 30;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("场地坐标", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.Longtitude + "," + sb.Latitude, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("海拔高程", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.Altitude.ToString(), contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("起测时间", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.StartDate, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("建设单位", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.BuildUnit, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("监测单位", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.ObsUnit, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("资料变更", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.Datachg, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            //cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            cell.Colspan = (3); //合并列; 
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("场地概况", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.SiteSituation, contentfont));
            cell.Colspan = (3); //合并列; 
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("地质状况", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.GeoSituation, contentfont));
            cell.Colspan = (3); //合并列; 
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("备注", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.Note, contentfont));
            cell.Colspan = (3); //合并列; 
            table.AddCell(cell);

            try
            {
                cell = new PdfPCell(new Phrase("卫星图", namefont));
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                cell.MinimumHeight = 30;
                table.AddCell(cell);
                iTextSharp.text.Image imgremote = null;
                List<RemoteMapBean> rblist = RemotemapBll.Instance.GetWhere(new { sitecode = sb.SiteCode }).ToList();
                if (rblist.Count != 0)
                { 
                    if (rblist[0].remotemap != null && rblist[0].remotemap.Length > 0)
                    {
                        imgremote = iTextSharp.text.Image.GetInstance(rblist[0].remotemap);
                        imgremote.ScaleToFit(200f, 200f);

                        cell = new PdfPCell(imgremote, false);
                        cell.Colspan = (3); //合并列; 
                        cell.MinimumHeight = 220;
                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        table.AddCell(cell);
                    }
                    else
                    {
                        imgremote = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\\图片缓存\\noPic.png");
                        imgremote.ScaleToFit(50f, 50f);

                        cell = new PdfPCell(imgremote, false);
                        cell.Colspan = (3); //合并列; 
                        cell.MinimumHeight = 60;
                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        table.AddCell(cell);
                    }
                }
                else
                {
                    imgremote = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\\图片缓存\\noPic.png");
                    imgremote.ScaleToFit(50f, 50f);

                    cell = new PdfPCell(imgremote, false);
                    cell.Colspan = (3); //合并列; 
                    cell.MinimumHeight = 60;
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    table.AddCell(cell);
                }

               

            }
            catch
            { }

            try
            {
                cell = new PdfPCell(new Phrase("场地图", namefont));
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                cell.MinimumHeight = 30;
                table.AddCell(cell);
                iTextSharp.text.Image imglayout = null;
                List<LayoutmapBean> lblist = LayoutmapBll.Instance.GetWhere(new { sitecode = sb.SiteCode }).ToList();
                if (lblist.Count != 0)
                { 
                    if (lblist[0].layoutmap != null && lblist[0].layoutmap.Length > 0)
                    {
                        imglayout = iTextSharp.text.Image.GetInstance(lblist[0].layoutmap);
                        imglayout.ScaleToFit(200f, 200f);

                        cell = new PdfPCell(imglayout, false);
                        cell.Colspan = (3); //合并列; 
                        cell.MinimumHeight = 220;
                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        table.AddCell(cell);
                    }
                    else
                    {
                        imglayout = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\\图片缓存\\noPic.png");
                        imglayout.ScaleToFit(50f, 50f);

                        cell = new PdfPCell(imglayout, false);
                        cell.Colspan = (3); //合并列; 
                        cell.MinimumHeight = 60;
                        cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                        table.AddCell(cell);
                    }
                }
                else
                {
                    imglayout = iTextSharp.text.Image.GetInstance(Application.StartupPath + "\\图片缓存\\noPic.png");
                    imglayout.ScaleToFit(50f, 50f);

                    cell = new PdfPCell(imglayout, false);
                    cell.Colspan = (3); //合并列; 
                    cell.MinimumHeight = 60;
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                    table.AddCell(cell);
                }
                    
                
                
            }
            catch
            { }

            cell = new PdfPCell(new Phrase("其他情况", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(sb.OtherSituation, contentfont));
            cell.Colspan = (3); //合并列; 
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            return table;
        }

        /// <summary>
        /// 另存为PDF文件
        /// </summary>
        public void SaveAs()
        {
            try
            {
                string dir = System.Environment.CurrentDirectory;
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = dir;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    this.pdfViewer.SaveDocument(sfd.FileName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// PDF文件打印
        /// </summary>
        public void Print()
        {
            this.pdfViewer.Print();
        }
    }
}
