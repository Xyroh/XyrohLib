using System;
namespace com.xyroh.lib
{
    public static class Utilities
    {
        public enum Implementation { Classic, Core, Xamarin }

        public static Implementation GetImplementation()
        {
            if (Type.GetType("Xamarin.Forms.Device") != null)
            {
                return Implementation.Xamarin;
            }
            else if (Environment.Version != null)
            {
                return Implementation.Classic;
            }
            else if (Environment.Version == null)
            {
                return Implementation.Core;
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
