
using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using xxkUI.Form;


namespace xxkUI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // Application.EnableVisualStyles();
                // Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new frm_SplashAhead());这是一个测试

                AppDomain.CurrentDomain.AssemblyResolve += Resolver;
                LoadApp();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            //Application.Run(new RibbonForm());
            
        }


        [MethodImpl(MethodImplOptions.NoInlining)]

        private static void LoadApp()

        {

            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            CefSettings settings = new CefSettings();
            // Set BrowserSubProcessPath based on app bitness at runtime
            settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                   Environment.Is64BitProcess ? "x64" : "x86",
                                                   "CefSharp.BrowserSubprocess.exe");
            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        
            Application.Run(new frm_SplashAhead());

        }



        // Will attempt to load missing assembly from either x86 or x64 subdir

        private static Assembly Resolver(object sender, ResolveEventArgs args)

        {

            if (args.Name.StartsWith("CefSharp"))

            {

                string assemblyName = args.Name.Split(new[] { ',' }, 2)[0] + ".dll";

                string archSpecificPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,

                                                       Environment.Is64BitProcess ? "x64" : "x86",

                                                       assemblyName);



                return File.Exists(archSpecificPath)

                           ? Assembly.LoadFile(archSpecificPath)

                           : null;

            }

            return null;

        }






    }
}
