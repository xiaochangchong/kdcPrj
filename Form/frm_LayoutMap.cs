using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

namespace xxkUI.Form
{
    public partial class frm_LayoutMap : DevExpress.XtraEditors.XtraForm
    {
        private int mouseX = int.MinValue;
        private int mouseY = int.MinValue;
        public frm_LayoutMap(int _mx,int _my)
        {
            InitializeComponent();

            mouseX = _mx;
            mouseY = _my;

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

        }



        private void btn_MouseHover(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.FlatAppearance.BorderSize = 1;
        }
        private void btn_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            btn.FlatAppearance.BorderSize = 0;
        }


        public void LoadLayoutMap(byte[] picbt,string layoutmapname)
        {
            try
            {
                MemoryStream ms = new MemoryStream(); //新建内存流
                ms.Write(picbt, 0, picbt.Length);

                this.pictureEdit.Image = Image.FromStream(ms);
                this.Text = layoutmapname;
            }
            catch
            {

            }
        }

        private void pictureEdit_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void LayoutMapFrm_Load(object sender, EventArgs e)
        {
            this.Top = mouseY;
            this.Left = mouseX;
            this.Width = 300;
            this.Height = 300;
        }

        private void LayoutMapFrm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Color FColor = Color.Red;
            Color TColor = Color.Yellow;
            Brush b = new LinearGradientBrush(this.ClientRectangle, FColor, TColor, LinearGradientMode.ForwardDiagonal);
            g.FillRectangle(b, this.ClientRectangle);
        }



        private Point ptMouseCurrrnetPos, ptMouseNewPos, ptFormPos, ptFormNewPos;
        private bool blnMouseDown = false;

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void btnZoomin_Click(object sender, EventArgs e)
        {
            this.Width += 10;
            this.Height += 10;
        }

        private void btnZoomout_Click(object sender, EventArgs e)
        {
            this.Width -= 10;
            this.Height -= 10;
        }

        private void btnZoomin_Click_1(object sender, EventArgs e)
        {

        }

        private void LayoutMapFrm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                blnMouseDown = true;
                // Save window position and mouse position
                ptMouseCurrrnetPos = Control.MousePosition;
                ptFormPos = Location;
            }
        }
        private void LayoutMapFrm_MouseMove(object sender, MouseEventArgs e)
        {
            if (blnMouseDown)
            {
                //Get the current position of the mouse in the screen
                ptMouseNewPos = Control.MousePosition;
                //Set window position
                ptFormNewPos.X = ptMouseNewPos.X - ptMouseCurrrnetPos.X + ptFormPos.X;
                ptFormNewPos.Y = ptMouseNewPos.Y - ptMouseCurrrnetPos.Y + ptFormPos.Y;
                //Save window position
                Location = ptFormNewPos;
                ptFormPos = ptFormNewPos;
                //Save mouse position
                ptMouseCurrrnetPos = ptMouseNewPos;
            }
        }
        private void LayoutMapFrm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                //Return back signal
                blnMouseDown = false;
        }


    }
}