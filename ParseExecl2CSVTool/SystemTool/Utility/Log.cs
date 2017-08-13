using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Utility
{
    /// <summary>
    /// Log interface
    /// <para>Log interface</para>
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Write an action of a certain type in the log
        /// </summary>
        void WriteAction(string text, Log.ActTypes actType);

        /// <summary>
        /// Write an informational action to the journal
        /// </summary>
        void WriteInfo(string text);

        /// <summary>
        /// Write a normal action to the log
        /// </summary>
        void WriteAction(string text);

        /// <summary>
        /// Record error in log
        /// </summary>
        void WriteError(string text);

        /// <summary>
        /// Write an exception to the log
        /// </summary>
        void WriteException(Exception ex, string errMsg = "", params object[] args);

        /// <summary>
        /// Write a string to the log
        /// </summary>
        void WriteLine(string text = "");

        /// <summary>
        /// Write a delimiter in the log
        /// </summary>
        void WriteBreak();
    }
    /// <summary>
    /// Log file implementation
    /// <para>Implementation of the log file</para>
    /// </summary>
    public class Log : ILog
    {
        /// <summary>
        /// Types of actions to be logged
        /// </summary>
        public enum ActTypes
        {
            /// <summary>
            /// Information
            /// </summary>
            Information,
            /// <summary>
            /// Action
            /// </summary>
            Action,
            /// <summary>
            /// Error
            /// </summary>
            Error,
            /// <summary>
            /// Exception
            /// </summary>
            Exception,
        }

        /// <summary>
        /// Log Formats
        /// </summary>
        public enum Formats
        {
            /// <summary>
            /// Simple (date, time, description)
            /// </summary>
            Simple,
            /// <summary>
            /// Full (date, time, computer, user, action, description)
            /// </summary>
            Full
        }

        /// <summary>
        /// Delegate records a line in the log
        /// </summary>
        public delegate void WriteLineDelegate(string text);

        /// <summary>
        /// The maximum file capacity (maximum size), 1 MB
        /// </summary>
        public const int DefCapacity = 1048576;

        private readonly Formats format;   // Formats
        private readonly object writeLock; // Object for synchronizing access to the log from different threads
        private StreamWriter writer;       // Object to write to file
        private FileInfo fileInfo;         // Information about the file


        /// <summary>
        /// Create a new instance of the Log class
        /// </summary>
        protected Log()
        {
            format = Formats.Simple;
            writeLock = new object();
            writer = null;
            fileInfo = null;

            FileName = "";
            Encoding = Encoding.Default;
            Capacity = DefCapacity;
            CompName = Environment.MachineName;
            UserName = Environment.UserName;
            Break = new string('-', 80);
            DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
        }

        /// <summary>
        /// Create a new instance of the Log class with the specified record format
        /// </summary>
        public Log(Formats logFormat)
            : this()
        {
            format = logFormat;
        }


        /// <summary>
        /// Get or set the name of the log
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Get or set the encoding of the log
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Get or set the Capacity of the log
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Get or set the CompName of the log
        /// </summary>
        public string CompName { get; private set; }

        /// <summary>
        /// Get or set the UserName of the log
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// Get or set the Break of the log
        /// </summary>
        public string Break { get; set; }

        /// <summary>
        /// Get or set the DateTimeFormat of the log
        /// </summary>
        public string DateTimeFormat { get; set; }


        /// <summary>
        /// Open the log to add information
        /// </summary>
        protected void Open()
        {
            try
            {
                writer = new StreamWriter(FileName, true, Encoding);
                fileInfo = new FileInfo(FileName);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Close Log
        /// </summary>
        protected void Close()
        {
            try
            {
                if (writer != null)
                {
                    writer.Close();
                    writer = null;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Get a string representation of the type of action
        /// </summary>
        protected string ActTypeToStr(ActTypes actType)
        {
            switch (actType)
            {
                case ActTypes.Exception:
                    return "EXC";
                case ActTypes.Error:
                    return "ERR";
                case ActTypes.Action:
                    return "ACT";
                default: // ActTypes.Information:
                    return "INF";
            }
        }


        /// <summary>
        /// Write an action of a certain type in the log
        /// </summary>
        public void WriteAction(string text, ActTypes actType)
        {
            StringBuilder sb = new StringBuilder(DateTime.Now.ToString(DateTimeFormat));

            if (format == Formats.Simple)
            {
                WriteLine(sb.Append(" ").Append(text).ToString());
            }
            else
            {
                WriteLine(sb.Append(" <")
                    .Append(CompName).Append("><")
                    .Append(UserName).Append("><")
                    .Append(ActTypeToStr(actType)).Append("> ")
                    .Append(text).ToString());
            }
        }

        /// <summary>
        /// Write an informational action to the journal
        /// </summary>
        public void WriteInfo(string text)
        {
            WriteAction(text, ActTypes.Information);
        }

        /// <summary>
        /// Write a normal action to the log
        /// </summary>
        public void WriteAction(string text)
        {
            WriteAction(text, ActTypes.Action);
        }

        /// <summary>
        /// Record error in log
        /// </summary>
        public void WriteError(string text)
        {
            WriteAction(text, ActTypes.Error);
        }

        /// <summary>
        /// Write an exception to the log
        /// </summary>
        public void WriteException(Exception ex, string errMsg = "", params object[] args)
        {
            if (string.IsNullOrEmpty(errMsg))
            {
                WriteAction(ex.ToString(), ActTypes.Exception);
            }
            else
            {
                WriteAction(new StringBuilder()
                    .Append(args == null || args.Length == 0 ? errMsg : string.Format(errMsg, args))
                    .Append(":").Append(Environment.NewLine)
                    .Append(ex.ToString()).ToString(),
                    ActTypes.Exception);
            }
        }

        /// <summary>
        /// Write a string to the log
        /// </summary>
        public void WriteLine(string text = "")
        {
            try
            {
                Monitor.Enter(writeLock);
                Open();
                if (fileInfo.Length > Capacity)
                {
                    string bakName = FileName + ".bak";
                    writer.Close();
                    File.Delete(bakName);
                    File.Move(FileName, bakName);

                    writer = new StreamWriter(FileName, true, Encoding);
                }
                writer.WriteLine(text);
                writer.Flush();
            }
            catch
            {
            }
            finally
            {
                Close();
                Monitor.Exit(writeLock);
            }
        }

        /// <summary>
        /// Write a delimiter in the log
        /// </summary>
        public void WriteBreak()
        {
            WriteLine(Break);
        }
    }
}
