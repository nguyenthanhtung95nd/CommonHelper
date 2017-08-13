using System;
using System.Text;

namespace Utility.LogEx
{
    public class LogEx
    {
        private static LogEx _instance;

        public static LogEx Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LogEx();
                return _instance;
            }
        }

        public void WriteExceptionLog(Exception objErr, string sFunc)
        {
            StringBuilder err = new StringBuilder();
            err.AppendLine("Function: " + sFunc);
            err.AppendLine("Datetime: " + DateTime.Now);
            err.AppendLine("Error message: " + objErr.Message.ToString());
            err.AppendLine("Stack trace: " + objErr.StackTrace);
            writeFileLog(err.ToString());
        }

        public void writeFileLog(string message)
        {
            try
            {
                string logFile = string.Empty;
                System.IO.StreamWriter logWriter = null;
                logFile = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                if (System.IO.File.Exists(logFile))
                {
                    logWriter = System.IO.File.AppendText(logFile);
                }
                else
                {
                    logWriter = System.IO.File.CreateText(logFile);
                }
                logWriter.WriteLine(message);
                logWriter.Close();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Error in writing log :" + e.Message);
            }
        }
    }
}