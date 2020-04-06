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
            Log("Crash: " + eventToLog + " ***** " + exp.StackTrace);
        }

        public static void LogEvent(string eventToLog)
        {
            Log("Event: " + eventToLog);
        }

        public static void Log(string eventToLog)
        {
            System.Diagnostics.Debug.WriteLine("Debug Log: " + eventToLog);

            if (Config.CanUseFileLog)
            {
                lock (locker)
                {

                    string logFilePath = Config.LogFile;

                    if (!File.Exists(logFilePath))
                    {
                        File.Create(logFilePath).Dispose();  //don't leave it open
                    }
                    else
                    {

                        Console.WriteLine("*** Log Size: " + new FileInfo(logFilePath).Length.ToString());

                        if (new FileInfo(logFilePath).Length > Config.MaxLogSize) 
                        {
                            Console.WriteLine("*** Recycling Log File ***");
                            File.WriteAllText(logFilePath, String.Empty);
                        }
                    }

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
