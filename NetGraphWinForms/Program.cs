﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetGraphWinForms
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения. Changes from user2
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
