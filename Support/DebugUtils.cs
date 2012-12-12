using System;
using System.Diagnostics;
using System.Globalization;

namespace Practic
{
    public static class DebugUtils
    {
        public const string GcFormat = "[GC ({0})]: Object {1} has been collected";
        public const string InfoFormat = "[INFO {0}]: {1}";
        public const string ErrorFormat = "[ERROR {0}]: {1}";
        public const string AssertionFailedFormat = "[ASSERT ERROR {0}]: {1}";


        [Conditional("DEBUG")]
        public static void SignalObjectCollected(Type obj)
        {
            var message = obj == null ? _InvariantFormatter(InfoFormat, DateTime.Now, "Something has failed :(") : _InvariantFormatter(GcFormat, DateTime.Now, obj.Name);
            Debug.WriteLine(message);
        }

        [Conditional("DEBUG")]
        public static void Info(String message)
        {
            if (String.IsNullOrEmpty(message))
                message = String.Empty;

            Debug.WriteLine(_InvariantFormatter(InfoFormat, DateTime.Now, message));
        }

        private static String _InvariantFormatter(String format, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, format, args);
        }

        public static void GenericAssert(bool condition, string description, params object[] args)
        {
#if DEBUG
            Debug.Assert(condition, "Assertion Failed!", String.Format(AssertionFailedFormat, DateTime.Now, String.Format(description, args)));
#else
            if(!condition)
                throw new ArgumentOutOfRangeException("condition", String.Format(AssertionFailedFormat, DateTime.Now, String.Format(description,args)));
#endif
        }

    }
}
