
namespace FeatureToggles.Util
{
    using System;
    using System.Diagnostics;

    public class TraceLogger : ILog
    {
        public void Debug(string message)
        {
            Trace.TraceInformation(message);
        }

        public void Error(string message)
        {
            Trace.TraceError(message);
        }

        public void Error(string message, Exception ex)
        {
            Trace.TraceError(string.Concat(message, Environment.NewLine, ex.StackTrace));
        }
    }
}
