using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using SharpRaven;
using SharpRaven.Data;
using System.Diagnostics;

namespace com.xyroh.lib.Services
{
    public static class Sentry
    {
        private static RavenClient sentryClient;

        public static Config Config { get; set; }

        public static void SetConfig()
        {
            if (Config.CanUseCrashReporter)
            {
                sentryClient = new RavenClient(Config.CrashReporterKey);
            }
        }

        public static void LogException(string eventToLog, Exception exp)
        {
            if (Config.CanUseCrashReporter)
            {
                sentryClient.Capture(new SentryEvent(exp));
            }
        }

        public static void LogBreadcrumb (string eventToLog, string category)
        {

            if (Config.CanUseCrashReporter)
            {
                Breadcrumb crumb = new Breadcrumb(category);
                crumb.Message = eventToLog;
                crumb.Level = BreadcrumbLevel.Info;

                sentryClient.AddTrail(crumb);
            }
        }


    }


}
