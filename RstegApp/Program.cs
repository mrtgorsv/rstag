using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RstegApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SetDllDirectory(AppDomain.CurrentDomain.BaseDirectory + "Libraries");

            Application.Run(new MainForm());
        }

        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool SetDllDirectory(String lpPathName);
    }
}
