using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace NetGraphWinForms
{
    class Log
    {
        public static string LogFileName = "netgraphlog.txt";

        public Log() { }

        public static void DisplayMessage(string msg)
        {
            System.Windows.Forms.MessageBox.Show(msg);
        }

        public static void DisplayMessage(Exception ex)
        {
            System.Windows.Forms.MessageBox.Show(ex.ToString());
        }

        public static void WriteToLog(Exception ex)
        {
            WriteToLog(ex.ToString());
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static void WriteToLog(string message)
        {
            try { System.IO.File.AppendAllText(LogFileName, message); }
            catch (Exception ex) { DisplayMessage(ex); }
        }
    }
}
