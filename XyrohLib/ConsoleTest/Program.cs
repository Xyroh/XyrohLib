using System;
using System.Collections.Generic;
using com.xyroh.lib;


namespace ConsoleTest
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            //XyrohLib.Test();

            XyrohLib.setFileLog("test.log");
            //XyrohLib.setFileLog("test.log", 5000);
            XyrohLib.setCrashreporter("https://7eaf2508f8fe4db58d63aec2ce57a525:aed37d0dcd8843128068ea7cce894036@o129318.ingest.sentry.io/5210766"); 
            //XyrohLib.Test();
            //XyrohLib.Log("test log 10 ");

            XyrohLib.setAnalytics("9410392a-f382-42c8-8fd5-0dd720e779b4", ""); //worst apps android

            /*for (int i = 0; i < 100; i++)
            {
                XyrohLib.LogEvent("Test Analytics Event" + i);
            }*/

            Console.WriteLine("File Path: " + XyrohLib.GetLogPath());

           


            //logger.Log("test from console");

            /*XyrohLib.LogEvent("Test Event");
            XyrohLib.LogEvent("Test Event 2", "Test Cat");

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("Test Key 1", "Test Result 1");
            dict.Add("Test Key 2", "Test Result 2");
            dict.Add("Test Key 3", "Test Result 3");
            XyrohLib.LogEvent("Test Event 3", dict, "Test Cat");*/
            
            XyrohLib.LogCrash("Test", new Exception("KaBlam3"));
            //throw new Exception("KaBlam5");
        }



        static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            XyrohLib.LogCrash("unhandled exception", ex);
            //XyrohLib.LogCrash("unhandled exception", ex);
            Environment.Exit(1);
        }
    }
}
