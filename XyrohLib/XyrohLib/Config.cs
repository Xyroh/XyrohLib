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

        static private bool useAnalytics = false;
        static private string analyticsKey { get; set; }
        static public string AnalyticsKey
        {
            get { return analyticsKey; }
            set
            {
                analyticsKey = value;
                if (!string.IsNullOrEmpty(analyticsKey))
                {
                    useAnalytics = true;
                }
                else
                {
                    useAnalytics = false;
                }
            }
        }

        static private bool useCrashReporter= false;
        static private string crashReporterKey { get; set; }
        static public string CrashReporterKey
        {
            get { return crashReporterKey; }
            set
            {
                crashReporterKey = value;
                if (!string.IsNullOrEmpty(crashReporterKey))
                {
                    useCrashReporter = true;
                }
                else
                {
                    useCrashReporter = false;
                }
            }
        }

    }
}
