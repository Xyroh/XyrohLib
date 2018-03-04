using System;
using System.Collections.Generic;
using System.Text;

namespace com.xyroh.lib
{
    public class Config
    {
        public static bool CanUseFileLog = false;
        private static string  logFile { get; set; }
        public static string LogFile
        {
            get { return logFile; }
            set {
                logFile = value;
                if(! string.IsNullOrEmpty(logFile))
                {
                    CanUseFileLog = true;
                }
                else
                {
                    CanUseFileLog = false;
                }
            }
        }

        public static bool CanUseAnalytics = false;
        static private string analyticsKey { get; set; }
        static public string AnalyticsKey
        {
            get { return analyticsKey; }
            set
            {
                analyticsKey = value;
                if (!string.IsNullOrEmpty(analyticsKey))
                {
                    CanUseAnalytics = true;
                }
            }
        }
        static private string analyticsKey2 { get; set; }
        static public string AnalyticsKey2
        {
            get { return analyticsKey2; }
            set
            {
                analyticsKey2 = value;
                if (!string.IsNullOrEmpty(analyticsKey2))
                {
                    CanUseAnalytics = true;
                }
            }
        }

        static public bool CanUseCrashReporter= false;
        static private string crashReporterKey { get; set; }
        static public string CrashReporterKey
        {
            get { return crashReporterKey; }
            set
            {
                crashReporterKey = value;
                if (!string.IsNullOrEmpty(crashReporterKey))
                {
                    CanUseCrashReporter = true;
                }
                else
                {
                    CanUseCrashReporter = false;
                }
            }
        }

    }
}
