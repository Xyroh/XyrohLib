using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Sentry;
using System.Diagnostics;
using Sentry.Protocol;

namespace com.xyroh.lib.Services
{
    public static class SentryAbstraction
    {
        private static SentryClient sentryClient;

        public static Config Config { get; set; }

        public static void SetConfig()
        {
            if (Config.CanUseCrashReporter)
            {
                //sentryClient = new SentryClient(Config.CrashReporterKey);
                SentrySdk.Init(Config.CrashReporterKey);

                if (!string.IsNullOrEmpty(Config.ReleaseVersion))
                {
                    SentrySdk.Init(o => o.Release = Config.ReleaseVersion);
                }
            }
        }

        public static void LogException(string eventToLog, Exception exp)
        {
            if (Config.CanUseCrashReporter)
            {
                //sentryClient.Capture(new SentryEvent(exp));
                SentrySdk.CaptureException(exp);
            }
        }

        public static void LogBreadcrumb (string eventToLog, string category)
        {

            if (Config.CanUseCrashReporter)
            {
                SentrySdk.AddBreadcrumb(
                    message: eventToLog,
                    category: category,
                    level: BreadcrumbLevel.Info
                    );
            }
        }


    }


}
