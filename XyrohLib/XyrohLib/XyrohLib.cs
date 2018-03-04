using System;
using System.Diagnostics;

using com.xyroh.lib.Services;

namespace com.xyroh.lib
{


    /*
     * 
     * In end project add reference to sharpraven and this xyrohlib project, also add nuget for system.configuration.manager
     * 
     * 
     * 
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



        /*public static void setAnalytics(string key)
        {
            Config.AnalyticsKey = key;
        }*/


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
