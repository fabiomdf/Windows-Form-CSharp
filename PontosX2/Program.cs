using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Persistencia;
using System.IO;

namespace PontosX2
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
//#if DEBUG
//            args = new string[1];
//            args[0] = @"C:\Users\joao.cavalcanti\Desktop\fdsfds.LDX2";
//#endif
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MDIPrincipal(args));            
        }

      
    }
}
