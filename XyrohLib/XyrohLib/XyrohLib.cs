using System;
using System.Diagnostics;
using System.Collections.Generic;

using com.xyroh.lib.Services;
using Microsoft.AppCenter.Analytics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace com.xyroh.lib
{


    /*
     *
     * In end project add reference to sharpraven (don't use sharpraven src unless have to) and this xyrohlib project, also add
     *
     *
     *  `System.Configuration.ConfigurationManager` referenced by assembly `SharpRaven, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null`.
`Newtonsoft.Json`
`Microsoft.AppCenter.Analytics`
`Microsoft.AppCenter`
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

        //Use LogCrash()
        /*
        public static void LogException(Exception exp)
        {
            LogException(exp.Message, exp);
        }

       public static void LogException(string eventToLog, Exception exp)
        {
            Logger.LogException(eventToLog, exp);
            LogCrash(eventToLog, exp);
        }*/

        public static void setFileLog(string fileName) //0.1 compatibility
        {
            Config.LogFile = fileName;
            Config.MaxLogSize = 1000000;  //default 1MB
        }

        public static void setFileLog(string fileName, int maxSize)
        {
            Config.LogFile = fileName;
            Config.MaxLogSize = maxSize;
        }

        public static string GetLogPath()
        {
            return Logger.GetLogPath();
        }

        #endregion


        #region helpdesk

        public static void SetHelpDesk(string url, string user, string pass)
        {
	        SetHelpDesk("Freshdesk", url, user, pass);
        }

        public static void SetHelpDesk(string type, string url, string user, string pass)
        {
	        Config.HelpDeskUrl = url;
	        Config.HelpDeskUser = user;
	        Config.HelpDeskPass = pass;
	        Config.HelpDeskType = type;
        }


        public static async Task<int> CreateTicket(string email, string subject, string description)
        {
	        int ticketId = 0;
	        try
	        {
		        ticketId = await CreateTicket(email, subject, description, new string[] { });
	        }
	        catch (Exception ex)
	        {
		        System.Diagnostics.Debug.WriteLine("EX:" + ex.StackTrace);
	        }
	        return ticketId;
        }
        public static async Task<int> CreateTicket(string email, string subject, string description, string[] tags)
        {
	        LogEvent("Creating Ticket: " + subject, "HELPDESK");

	        int ticketId = 0;

	        switch (Config.HelpDeskType)
	        {
		        case "Freshdesk":
			        Freshdesk.SetConfig();
			        try
			        {
				        ticketId = await Freshdesk.CreateTicket(email, subject, description, tags);
			        }
			        catch (Exception ex)
			        {
				        System.Diagnostics.Debug.WriteLine("EX:" + ex.StackTrace);
			        }

			        LogEvent("Created Ticket: " + ticketId, "HELPDESK");
			        break;
		        case "Zendesk":
			        Zendesk.SetConfig();
			        try
			        {
				        ticketId = await Zendesk.CreateRequest(email, subject, description, tags);
			        }
			        catch (Exception ex)
			        {
				        System.Diagnostics.Debug.WriteLine("EX:" + ex.StackTrace);
			        }

			        LogEvent("Created Ticket: " + ticketId, "HELPDESK");
			        break;
	        }

	        return ticketId;
        }

        public static async Task<int> CreateTicketWithAttachment(string email, string subject, string description, List<string> attachments)
        {
	        int ticketId = 0;
	        try
	        {
		        ticketId = await CreateTicketWithAttachment(email, subject, description, new string[] { }, attachments);
	        }
	        catch (Exception ex)
	        {
		        System.Diagnostics.Debug.WriteLine("EX:" + ex.StackTrace);
	        }
	        return ticketId;
        }
        public static async Task<int> CreateTicketWithAttachment(string email, string subject, string description, string[] tags, List<string> attachments)
        {
	        LogEvent("Creating Ticket with Attachment: " + subject, "HELPDESK");

	        int ticketId = 0;

	        switch (Config.HelpDeskType)
	        {
		        case "Freshdesk":
			        Freshdesk.SetConfig();
			        try
			        {
				        ticketId = await Freshdesk.CreateTicketWithAttachment(email, subject, description, tags, attachments);
			        }
			        catch (Exception ex)
			        {
				        System.Diagnostics.Debug.WriteLine("EX:" + ex.StackTrace);
			        }

			        LogEvent("Created Ticket: " + ticketId, "HELPDESK");
			        break;
		        case "Zendesk":
			        Zendesk.SetConfig();
			        try
			        {
				        ticketId = await Zendesk.CreateRequestWithAttachment(email, subject, description, tags, attachments);
			        }
			        catch (Exception ex)
			        {
				        System.Diagnostics.Debug.WriteLine("EX:" + ex.StackTrace);
			        }

			        LogEvent("Created Ticket: " + ticketId, "HELPDESK");
			        break;
	        }


	        return ticketId;
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
            Sentry.LogBreadcrumb(eventToLog, "event"); //bacwards compat
            Logger.LogEvent(eventToLog); //bump to one below
        }
        public static void LogEvent(string eventToLog, Dictionary<string, string> dict)
        {
            AppCenterAnalytics.LogEvent(eventToLog, dict);
            Sentry.LogBreadcrumb(eventToLog, "event"); //bacwards compat
            foreach(var item in dict)
            {
	            Sentry.LogBreadcrumb(eventToLog + " : " + item.Key + " : " + item.Value, "event");
            }
            Logger.LogEvent(eventToLog);
        }

        public static void LogEvent(string eventToLog, string category)
        {
            AppCenterAnalytics.LogEvent(category + " : " + eventToLog);
            Sentry.LogBreadcrumb(eventToLog, category);
            Logger.LogEvent(eventToLog); //bump to one below
        }
        public static void LogEvent(string eventToLog, Dictionary<string, string> dict, string category)
        {
            AppCenterAnalytics.LogEvent(category + " : " + eventToLog, dict);
            Sentry.LogBreadcrumb(eventToLog, category);
            foreach(var item in dict)
            {
	            Sentry.LogBreadcrumb(eventToLog + " : " + item.Key + " : " + item.Value, category);
            }
            Logger.LogEvent(eventToLog);
        }

        #endregion

        #region CrashReporter

        public static void setCrashreporter(string key) //this is the depreciated DSN in Sentry
        {
            Config.CrashReporterKey = key;
            Sentry.SetConfig();
        }

        public static void LogCrash(Exception exp)
        {
            Logger.LogException(exp.Message, exp);
            AppCenterAnalytics.LogCrash(exp);
            Sentry.LogException(exp.Message, exp);

        }
        public static void LogCrash(string eventToLog, Exception exp)
        {
            Logger.LogException(exp.Message, exp);
            AppCenterAnalytics.LogCrash(exp);
            Sentry.LogException(eventToLog, exp);
        }
        public static void LogException(string eventToLog, Exception exp) //0.1 compatibility
        {
            LogCrash(eventToLog, exp);
        }

        #endregion


        public static void Test()
        {
            //Debug.WriteLine("PLATFORM: " + XyrohUtilities.GetImplementation().ToString());
            Console.WriteLine("CONFIG: " + Config.LogFile);
        }




    }
}
