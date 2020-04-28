using DevExpress.XtraRichEdit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xxkUI.Controls;

namespace xxkUI.MyCls
{
    public class MyPrint
    {
        private string printPath = Application.StartupPath + "\\打印缓存\\";
        private string printFile = "";
        private bool printPic = true;


        public void Print(Control ct)
        {
            //if (ct.Name == "siteInfoDocCtrl1")
            //    printPic = false;
            //else
            //    printPic = true;

            
            if (printPic)
            {
                printFile = CreateImageCache(ct);
                if (!File.Exists(printFile))
                    return;

             
                PrintDocument pd = new PrintDocument();
                //设置边距
               // Margins margin = new Margins(20, 20, 20, 20);
                //pd.DefaultPageSettings.Margins = margin;
                ////纸张设置默认
                PaperSize pageSize = new PaperSize("First custom size", 600, 800);
                pd.DefaultPageSettings.PaperSize = pageSize;
                //打印事件设置
                pd.PrintPage += Pd_PrintPage;
                PrintDialog pdl = new PrintDialog();
                pdl.Document = pd;
                if (DialogResult.OK == pdl.ShowDialog()) //如果确认，将会覆盖所有的打印参数设置
                {
                    //页面设置对话框（可以不使用，其实PrintDialog对话框已提供页面设置）
                    PageSetupDialog psd = new PageSetupDialog();
                    psd.Document = pd;
                    if (DialogResult.OK == psd.ShowDialog())
                    {
                        //打印预览
                        PrintPreviewDialog ppd = new PrintPreviewDialog();
                        ppd.Document = pd;
                        if (DialogResult.OK == ppd.ShowDialog())
                            pd.Print(); //打印
                    }

                }
            }
            else
            {
               ((SiteInfoPdfCtrl)ct).Print();
            }
          

        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (!printPic)
                return;

            Image layoutImage = Image.FromFile(@printFile);
            double scale = layoutImage.Width / layoutImage.Height;//图片长宽比，固定的用来保证图片不变形

            int x = e.PageSettings.PaperSize.Width-40;//打印机默认纸张大小
            if (x > layoutImage.Width)
                x = layoutImage.Width;
            int y = int.Parse(Math.Round(x / scale, 0).ToString());
 
            Rectangle destRect = new Rectangle(40, 40, x, y);//背景图片打印区域
         
            e.Graphics.DrawImage(layoutImage, destRect, 0, 0, layoutImage.Width, layoutImage.Height, System.Drawing.GraphicsUnit.Pixel);


        }


        /// <summary>
        /// 创建图片缓存
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        private string CreateImageCache(Control ct)
        {
            string imagefile = "";
            try
            {
                //创建图象，保存将来截取的图象
                Point screenPoint = ct.PointToScreen(ct.Location);
                Bitmap image = new Bitmap(ct.Width, ct.Height);
                Graphics imgGraphics = Graphics.FromImage(image);
                //设置截屏区域
                imgGraphics.CopyFromScreen(screenPoint.X, screenPoint.Y, 0, 0, new Size(ct.Width, ct.Height));
                //随机生成文件名并保存
                string imgfilecache = printPath + Guid.NewGuid().ToString();
                image.Save(imgfilecache, ImageFormat.Jpeg);

                imagefile = imgfilecache;


            }
            catch (Exception)
            {
                throw new Exception("图片缓存失败");
            }
            return imagefile;
        }

    }


}
