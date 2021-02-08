using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        private static int maxlogSize { get; set; }
        public static int MaxLogSize
        {
            get { return maxlogSize; }
            set
            {
                maxlogSize = value;
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

        static private string releaseVersion { get; set; }
        static public string ReleaseVersion
        {
	        get { return releaseVersion; }
	        set
	        {
		        releaseVersion = value;
	        }
        }

        static public bool CanUseHelpDesk = false;
        static private string helpDeskUser { get; set; }
        static public string HelpDeskUser
        {
	        get { return helpDeskUser; }
	        set
	        {
		        helpDeskUser = value;
		        if (!string.IsNullOrEmpty(helpDeskUser) && !string.IsNullOrEmpty(helpDeskPass) &&
		            !string.IsNullOrEmpty(helpDeskUrl))
		        {
			        CanUseHelpDesk = true;
		        }
		        else
		        {
			        CanUseHelpDesk = false;
		        }
	        }
        }
        static private string helpDeskPass { get; set; }
        static public string HelpDeskPass
        {
	        get { return helpDeskPass; }
	        set
	        {
		        helpDeskPass = value;
		        if (!string.IsNullOrEmpty(helpDeskUser) && !string.IsNullOrEmpty(helpDeskPass) &&
		            !string.IsNullOrEmpty(helpDeskUrl))
		        {
			        CanUseHelpDesk = true;
		        }
		        else
		        {
			        CanUseHelpDesk = false;
		        }
	        }
        }
        static private string helpDeskUrl { get; set; }

        static public string HelpDeskUrl
        {
	        get { return helpDeskUrl; }
	        set
	        {
		        helpDeskUrl = value;
		        if (!string.IsNullOrEmpty(helpDeskUser) && !string.IsNullOrEmpty(helpDeskPass) &&
		            !string.IsNullOrEmpty(helpDeskUrl))
		        {
			        CanUseHelpDesk = true;
		        }
		        else
		        {
			        CanUseHelpDesk = false;
		        }
	        }
        }

    }
}
