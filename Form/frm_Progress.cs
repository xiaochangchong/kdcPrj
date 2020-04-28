/***********************************************************/
//---ģ    �飺ProgressForm��
//---�������������õ�ʱ�ȿ��ƺ���ʾ��
//---����ʱ�䣺2008-05-18
//---������Ա��������
//---��    λ��LREIS
/***********************************************************/
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using xxkUI.Tool;
using xxkUI.Controls;

namespace xxkUI.Form
{
    /// <summary>
    /// ���ȿ��ƺ���ʾ�Ի���
    /// </summary>
    public partial class frm_Progress : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// ��̨����̹�����
        /// </summary>
        private MyBackgroundWorker backgroundWorker = null;
        private DateTime sTime;
        /// <summary>
        /// ���̹߳�����
        /// </summary>
        public MyBackgroundWorker progressWorker
        {
            get
            {
                return backgroundWorker;
            }
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        public frm_Progress()
        {
            InitializeComponent();
            backgroundWorker = new MyBackgroundWorker();
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            //backgroudWorker.DoWork += new DoWorkEventHandler(backgroundCalculator_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            backgroundWorker.Disposed += new EventHandler(backgroundWorker_Disposed);            
            sTime = DateTime.Now;
            updateStatus("��ʼʱ��:" + sTime.ToString());
            updateStatus("*****************************************************************************");
            updateStatus("��ʼִ��");
        }

        /// <summary>
        /// �رնԻ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void backgroundWorker_Disposed(object sender, EventArgs e)
        { 
            this.Close();//pyl ���б���ǰ������,ԭ����ִ�в���.
            throw new Exception("��ǰ����δ���");         
        }

        /// <summary>
        /// ���ȷ����ı�ʱ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                progressBar.Value = e.ProgressPercentage;                
                updateStatus(backgroundWorker.StatusText);
                if (backgroundWorker.logType != LogType.Common)
                {
                    backgroundWorker.logType = LogType.Common;
                }
            }
            else
            {
                backgroundWorker.Dispose();
            }
        }
        //�������
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                updateStatus("��ǰ��������ֹ.");                
            }
            else if (e.Error != null)
            {
                updateStatus("ִ�й��̳��ִ�����������������Ƿ���ȷ!");
            }
            else
            {
                updateStatus("��ǰ������ִ�����!");
                if (check_AutoClose.Checked)
                {
                    this.Close();
                }
            }
            updateStatus("*****************************************************************************");
            this.progressBar.Style = ProgressBarStyle.Continuous;
            this.progressBar.Value = 100;
            DateTime eTime = DateTime.Now;
            updateStatus("����ʱ��:" + eTime.ToString());
            TimeSpan sSpan = eTime.Subtract(sTime);
            updateStatus("ʹ��ʱ��:" + sSpan.Days.ToString() + "��" + sSpan.Hours.ToString() + "Сʱ" +
                sSpan.Minutes.ToString() + "��" + sSpan.Seconds.ToString() + "��");
            this.Text = "��ǰ������ִ�����(" + this.Text + ")";
            this.Cancel.Enabled = true;
            this.Cancel.Text = "�ر�";
            this.Export.Enabled = true;
        }
        /// <summary>
        /// ����״̬��Ϣ
        /// </summary>
        /// <param name="strText">״̬��Ϣ</param>
        public void updateStatus(string strText)
        {
            try
            {
                if (!backgroundWorker.IsExitWithException)
                {
                    if (richTextBox != null && strText!="")
                    {
                        int p1 = richTextBox.TextLength;  //ȡ��δ���ʱ���ַ������ȡ�                       
                       
                        richTextBox.AppendText("\n" + strText);
                        int p2 = strText.Length;  //ȡ��Ҫ��ӵ��ı��ĳ���    
                        richTextBox.Select(p1, p2+1);        //ѡ��Ҫ��ӵ��ı� 
                        if (backgroundWorker.logType == LogType.Error)
                        {
                            richTextBox.SelectionColor = Color.Red;
                        }
                        else if (backgroundWorker.logType == LogType.Warning)
                        {
                            richTextBox.SelectionColor = Color.Orange;
                        }
                        else if (backgroundWorker.logType == LogType.Right)
                        {
                            richTextBox.SelectionColor = Color.Green;
                        }
                        else
                        {
                            richTextBox.SelectionColor = Color.Black;
                        }
                        richTextBox.ScrollToCaret();
                    }
                }                
            }
            catch (Exception excep)
            {
                throw excep;
            }
        }
        /// <summary>
        /// ��ʼ���ж��߳�
        /// </summary>
        public void beginWorking()
        {
            this.Export.Enabled = false;
            backgroundWorker.RunWorkerAsync();
        }

        /// <summary>
        /// �ر�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                updateStatus("����ȡ����ǰ���������Ժ�...");
                backgroundWorker.CancelAsync();
                this.Cancel.Text = "�ر�";
                this.Cancel.Enabled = false;
            }
            else if (this.Cancel.Text == "�ر�")
            {
                //this.Dispose();
                this.Close();
            }
        }
       
        //����ִ����־
        private void Export_Click(object sender, EventArgs e)
        {
            this.contextMenuStrip1.Show(this.Export, this.Export.Width-10, this.Export.Height-10);
        }
       

        private void SaveTxt(string fileName)
        {
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);

                foreach (string strText in richTextBox.Lines)
                {
                    sw.WriteLine(strText);
                }
                sw.Close();
                fs.Close();

                if (XtraMessageBox.Show("��־������ɣ��Ƿ����ڴ���־�ļ����鿴��", "��ʾ",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("д�ı��ļ�����," + ex.Message);
            } 
        }
        //�Ի���ر�ʱ�Ĳ���
        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {                
                backgroundWorker.CancelAsync();                
            }
            backgroundWorker.IsExitWithException = true;
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "menu_txt")
            {
                this.contextMenuStrip1.Close();
                FileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "�ı��ļ�(*.txt)|*.txt";
                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        string fileName = fileDialog.FileName;                        
                        SaveTxt(fileName);                       
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        XtraMessageBox.Show("д��־�ļ�����," + ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Cursor = Cursors.Default;
                }
            }
            else if (e.ClickedItem.Name == "menu_word")
            {
                this.contextMenuStrip1.Close();
                FileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "Word�ļ�(*.doc)|*.doc";
                if (fileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        Cursor = Cursors.WaitCursor;
                        string fileName = fileDialog.FileName;                        
                        //SaveDoc(fileName);                       
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        XtraMessageBox.Show("д��־�ļ�����," + ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    Cursor = Cursors.Default;
                }
            }
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // ProgressForm
        //    // 
        //    this.ClientSize = new System.Drawing.Size(284, 262);
        //    this.Name = "ProgressForm";
        //    this.ResumeLayout(false);

        //}
    }
}