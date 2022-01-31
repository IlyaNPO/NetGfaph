using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetGraphWinForms
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения. Edited by user1 from "bruser1"
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
