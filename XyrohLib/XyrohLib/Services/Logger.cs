using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace com.xyroh.lib.Services
{
    public static class Logger
    {
        private static object locker = new object();
        public static Config Config { get; set; }


        public static void LogException(string eventToLog, Exception exp)
        {
            Log("EXCEPTION: " + eventToLog + " ***** " + exp.StackTrace);
        }

        public static void Log(string eventToLog)
        {
            System.Diagnostics.Debug.WriteLine("Debug Log: " + eventToLog);

            if (Config.CanUseFileLog)
            {
                lock (locker)
                {
                    /*
                     * 
                     * TODO:    Better file path / folder / xplatform handling of the logfilepath
                     */

                    string logFilePath = Config.LogFile;

                    using (FileStream file = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.None))
                    {
                        StreamWriter writer = new StreamWriter(file);

                        writer.Write(DateTime.Now.ToString() + " : " + eventToLog + System.Environment.NewLine);
                        writer.Flush();
                        file.Dispose();
                    }
                }
            }

        }

    }


}
