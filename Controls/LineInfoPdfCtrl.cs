/***********************************************************/
//---模    块：测线信息pdf操作类
//---功能描述：pdf文件的路径设置、创建表格、写入内容
//---编码时间：2020-3-24
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
    public partial class LineInfoPdfCtrl : UserControl
    {
        private string pFilePath = string.Empty;
        public LineInfoPdfCtrl()
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

        public void WriteContent(LineBean lb)
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
                iTextSharp.text.Paragraph Title = new iTextSharp.text.Paragraph(lb.OBSLINENAME + " 测线基本情况", fonttitle);     //添加段落，第二个参数指定使用fonttitle格式的字体，写入中文必须指定字体否则无法显示中文
                Title.Alignment = iTextSharp.text.Rectangle.ALIGN_CENTER;       //设置居中
                document.Add(Title);        //将标题段加入PDF文档中


                //空一行
                BaseFont bf1 = BaseFont.CreateFont("C:\\Windows\\Fonts\\SIMSUN.TTC,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);     //用系统中的字体文件SimSun.ttc创建文件字体
                iTextSharp.text.Font ftitle = new iTextSharp.text.Font(bf1, 15);
                iTextSharp.text.Paragraph nullp = new iTextSharp.text.Paragraph("      ", ftitle);
                document.Add(nullp);


                PdfPTable table = CreateTable(lb);

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

        private PdfPTable CreateTable(LineBean lb)
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

            PdfPCell cell = new PdfPCell(new Phrase("测线名称", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.OBSLINENAME, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            
            cell = new PdfPCell(new Phrase("所属场地", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(SiteBll.Instance.GetSitenameByID(lb.SITECODE), contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("曾用名称", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.NAMEBEFORE, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("基础测项", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.BASEOBSTYPE, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("辅助测项", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.AIDSOBSTYPE, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("观测周期", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.OBSCYCLE, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("上盘-下盘", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.UP_BOT, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("建立时间", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.BUILDDATE, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("开测时间", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.STARTDATE, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("停测时间", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.ENDDATE, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("测线长度", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.LENGTH, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("测站数", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.STATIONCOUNT, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("所属断层", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.FAULTZONE, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("断层走向", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.FAULTSTRIKE, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("断层倾向", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.FAULTTENDENCY, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("断层倾角", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.FAULTDIP, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("夹角", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.LINE_FAULT_ANGLE, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("测点岩性", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.PTROCK, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("仪器更换情况", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.INSTRREPLACEDISCRIP, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("运行状况", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.LINESTATUS, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);


            cell = new PdfPCell(new Phrase("起点", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.STARTPOINTCODE, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("终点", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.ENDPOINTCODE, contentfont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);



            cell = new PdfPCell(new Phrase("备注", namefont));
            cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            cell.MinimumHeight = 30;
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(lb.NOTE, contentfont));
            cell.Colspan = (3); //合并列; 
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
