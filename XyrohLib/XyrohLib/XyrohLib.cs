using System;
using System.Diagnostics;


namespace com.xyroh.lib
{
    public static class XyrohLib
    {
        public static void Test()
        {
            Debug.WriteLine("PLATFORM: " + Utilities.GetImplementation().ToString());
        }




    }
}
