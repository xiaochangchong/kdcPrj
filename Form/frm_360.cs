using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xxkUI.Form
{
    public partial class frm_360 : DevExpress.XtraEditors.XtraForm
    {


        private ChromiumWebBrowser browser;
        public frm_360()
        {
            InitializeComponent();

            //Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser("http://127.0.0.1:8022/panoramaPage.html?key=test");
            Font font = new Font("微软雅黑", 10.5f);
            this.Controls.Add(browser);
            browser.Font = font;
            browser.Dock = DockStyle.Fill;
            browser.LoadingStateChanged += new EventHandler<LoadingStateChangedEventArgs>(LoadingStateChangeds);

            //chromiumWebBrowser1.Load("http://127.0.0.1:8022/panoramaPage.html?key=test");
        }

        //加载状态
        private void LoadingStateChangeds(object sender, EventArgs e)
        {


        }






    }
}
