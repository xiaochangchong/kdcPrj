using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Text;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Windows.Forms;
using xxkUI.Bll;
using System.Linq;
using System.Data;

namespace xxkUI.GsProject
{
    /// <summary>
    /// InfoWindow
    /// </summary>
    [Serializable]
    public class InfoWindow : GMapToolTip
    {
        public float Radius = 2f;

        public static readonly Pen DefaultStroke = new Pen(ColorTranslator.FromHtml("#909090"));

        private object tag = null;

        static InfoWindow()
        {
            DefaultStroke.Width = 1;
            DefaultStroke.LineJoin = LineJoin.Round;
            DefaultStroke.StartCap = LineCap.RoundAnchor;
            

        }

        public InfoWindow(GMapMarker marker)
            : base(marker)
        {
            Foreground = new SolidBrush(ColorTranslator.FromHtml("#000"));
            Font = new Font("宋体", 14, GraphicsUnit.Pixel);
            TextPadding = new Size((int)Radius, (int)Radius);
            Stroke = DefaultStroke;
            Fill = new SolidBrush(ColorTranslator.FromHtml("#fafafa"));
            
            tag = marker.Tag;

        }


        public override void OnRender(Graphics g)
        {
            System.Drawing.Size st = new System.Drawing.Size(330, 260);
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(Marker.ToolTipPosition.X, Marker.ToolTipPosition.Y - st.Height, st.Width + TextPadding.Width, st.Height + TextPadding.Height);
            rect.Offset(Offset.X, Offset.Y);

            using (GraphicsPath objGP = new GraphicsPath())
            {
                objGP.AddLine(rect.X + 2 * Radius * 20, rect.Y + rect.Height, rect.X + Radius * 20, rect.Y + rect.Height + Radius * 20);
                objGP.AddLine(rect.X + Radius * 20, rect.Y + rect.Height + Radius * 20, rect.X + Radius * 20 + 10, rect.Y + rect.Height);

                objGP.AddArc(rect.X, rect.Y + rect.Height - (Radius * 2), Radius * 2, Radius * 2, 90, 90);
                objGP.AddLine(rect.X, rect.Y + rect.Height - (Radius * 2), rect.X, rect.Y + Radius);
                objGP.AddArc(rect.X, rect.Y, Radius * 2, Radius * 2, 180, 90);
                objGP.AddLine(rect.X + Radius, rect.Y, rect.X + rect.Width - (Radius * 2), rect.Y);
                objGP.AddArc(rect.X + rect.Width - (Radius * 2), rect.Y, Radius * 2, Radius * 2, 270, 90);
                objGP.AddLine(rect.X + rect.Width, rect.Y + Radius, rect.X + rect.Width, rect.Y + rect.Height - (Radius * 2));
                objGP.AddArc(rect.X + rect.Width - (Radius * 2), rect.Y + rect.Height - (Radius * 2), Radius * 2, Radius * 2, 0, 90); // Corner

                objGP.CloseFigure();

                g.FillPath(Fill, objGP);
                g.DrawPath(Stroke, objGP);
            }

            try
            {
                SiteBean sb = tag as SiteBean;
                InfoWindowContent content = new InfoWindowContent();
                content.SetInfoWindContent(sb.SiteCode, sb.SiteName, sb.SiteStatus, sb.SiteType, sb.ObsType, sb.Place, sb.FaultCode, sb.ObsUnit, sb.SiteSituation);
                Bitmap bit = new Bitmap(content.Width + 20, content.Height + 20);
                content.DrawToBitmap(bit, content.ClientRectangle);

                Rectangle r = rect;
                r.X = r.X + 4;
                r.Y = r.Y + 4;
                g.DrawImageUnscaled(bit, r);
            }
            catch
            { }

        }
    }
}
