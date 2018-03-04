using System;
using System.Diagnostics;
using System.Collections.Generic;

using com.xyroh.lib.Services;

namespace com.xyroh.lib
{


    /*
     * 
     * In end project add reference to sharpraven and this xyrohlib project, also add nuget for system.configuration.manager
     * 
     * 
     * added appcenter and appcenter.analytics - platforms for nugets????
     * 
     * 
     */


    public class XyrohLib
    {

        public XyrohLib()
        {

        }

        #region FileLogger

        public static void Log(string eventToLog)
        {
            Logger.Log(eventToLog);
        }
        public static void LogException(string eventToLog, Exception exp)
        {
            Logger.LogException(eventToLog, exp);
            LogCrash(eventToLog, exp);
        }

        public static void setFileLog(string fileName)
        {
            Config.LogFile = fileName;
        }

        #endregion



        #region Analytics

        public static void setAnalytics(string key1, string key2)
        {
            Config.AnalyticsKey = key1;
            Config.AnalyticsKey2 = key2;
            AppCenterAnalytics.SetConfig();
        }

        public static void LogEvent(string eventToLog)
        {
            AppCenterAnalytics.LogEvent(eventToLog);
            Logger.LogEvent(eventToLog);
        }
        public static void LogEvent(string eventToLog, Dictionary<string, string> dict)
        {
            AppCenterAnalytics.LogEvent(eventToLog, dict);
            Logger.LogEvent(eventToLog);
        }

        #endregion

        #region CrashReporter

        public static void setCrashreporter(string key)
        {
            Config.CrashReporterKey = key;
            Sentry.SetConfig();
        }
        public static void LogCrash(string eventToLog, Exception exp)
        {
            Sentry.LogException(eventToLog, exp);
        }

        #endregion


        public static void Test()
        {
            //Debug.WriteLine("PLATFORM: " + XyrohUtilities.GetImplementation().ToString());
            Console.WriteLine("CONFIG: " + Config.LogFile);
        }




    }
}
