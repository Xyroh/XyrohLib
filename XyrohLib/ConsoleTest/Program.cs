using System;
using com.xyroh.lib;


namespace ConsoleTest
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //XyrohLib.Test();

            XyrohLib.setFileLog("test.log");
            //XyrohLib.Test();
            XyrohLib.Log("test log 6");

            //logger.Log("test from console");
        }
    }
}
