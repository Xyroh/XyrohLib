using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;

namespace com.xyroh.lib.Services
{
    public static class AppCenterAnalytics
    {
        
        public static Config Config { get; set; }

        public static void SetConfig()
        {
            if (Config.CanUseAnalytics)
            {
                AppCenter.Start("ios=" + Config.AnalyticsKey + " ;android=" + Config.AnalyticsKey2 , typeof(Analytics));    //Analytics only
                LogEvent("App Started");
            }
        }


        public static void LogEvent(string eventToLog)
        {
            if (Config.CanUseAnalytics)
            {
                Analytics.TrackEvent("Event: " + eventToLog);
            }
        }

        public static void LogEvent(string eventToLog, Dictionary<string, string> dict)
        {
            if (Config.CanUseAnalytics)
            {
                Analytics.TrackEvent("Event: " + eventToLog, dict);
            }
        }

        public static void LogCrash(string eventToLog)
        {
            if (Config.CanUseAnalytics)
            {
                Analytics.TrackEvent("Crash: " + eventToLog);
            }
        }

    }


}
