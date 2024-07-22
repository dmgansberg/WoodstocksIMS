/*=================================================================
 * 
 * File: Program.cs
 *
 * Description: The main program file for the WoodstocksIMS system.
 * 
 * (c) Darren Gansberg 2014-2015.
 * 
 * 
 =================================================================*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

using System.Runtime.Remoting.Messaging;

using System.Threading;

namespace Woodstocks.WoodstocksIMS
{
    using Woodstocks.WoodstocksIMS.Presentation;
    using Woodstocks.WoodstocksIMS.Domain;
    using Woodstocks.WoodstocksIMS.Data.CSV;
    using System.Text.RegularExpressions;
  
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
            Woodstocks.WoodstocksIMS.Presentation.WoodstocksIMSForm view = new Woodstocks.WoodstocksIMS.Presentation.WoodstocksIMSForm();
            Application.Run(view);
        }
    }
}
