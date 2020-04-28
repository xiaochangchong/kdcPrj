/***********************************************************/
//---ģ    �飺MyBackgroundWorker��
//---�������������̹߳�����
//---����ʱ�䣺2008-05-18
//---������Ա��������
//---��    λ��LREIS
/***********************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using xxkUI.Tool;

namespace xxkUI.Controls
{
    public partial class MyBackgroundWorker : BackgroundWorker
    {
        private string strStatusText = null;
        private bool exitWithException = false;
        public LogType logType = LogType.Common;
        /// <summary>
        /// ״̬��Ϣ
        /// </summary>
        public string StatusText
        {
            set
            {
                strStatusText = value;
            }
            get
            {
                return strStatusText;
            }
        }
        /// <summary>
        /// �Ƿ��쳣�˳�
        /// </summary>
        public bool IsExitWithException
        {
            set
            {
                exitWithException = value;
            }
            get
            {
                return exitWithException;
            }
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        public MyBackgroundWorker()
        {
            InitializeComponent();
        }
        /// <summary>
        /// ck add,2012-6-1.
        /// ������ReportProgress̫Ƶ��ʱ��������������ProgressForm��updateStatus������ʾ�������ʾ��
        /// ����д�˷������Է���ı�״̬�ı���
        /// </summary>
        /// <param name="sStatusText"></param>
        public void SetStatusText(string sStatusText)
        {
            this.StatusText = sStatusText;
            this.ReportProgress(50);
            System.Threading.Thread.Sleep(50);//����һ�ᣬ��backgroundWorker_ProgressChanged��ʱ��ִ��
        }

        /// <summary>
        /// ck add,2012-6-1.
        /// ��ֻ��ı���ȣ�������ı�״̬�ı�ʱ�����Ե������������
        /// </summary>
        public void ChangeProgressNoStatusText()
        {
            this.StatusText = "";
            this.ReportProgress(50);
            System.Threading.Thread.Sleep(50);
        }
    }
}
