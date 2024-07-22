/*==============================================================
 * File name: Program.cs
 *
 * Description: A static class for a small test program to verify
 * write integrity of ToyExporterCSV for the WoodstocksIMS. The 
 * functionality of the program is contained in the TestForm class.
 * 
 * (c) Darren Gansberg, 2014-2015.
 * 
 * 
 ===============================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WoodstocksIMSTest
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
            Application.Run(new TestForm());
        }
    }
}
