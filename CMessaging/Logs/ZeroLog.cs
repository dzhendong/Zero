using System;
using System.Collections;
using System.Diagnostics;
using log4net;
using log4net.Core;

namespace ILvYou.Zero.Logs
{
    /// <summary>
    /// 写入log4net
    /// </summary>
    public sealed class ZeroLog
    {
        private static readonly Hashtable loggers = new Hashtable();

        #region Message Formatting Methods

        private static string Format(string errorMsg, params object[] args)
        {
            return String.Format(errorMsg, args);
        }

        private static string FormatLine(string errorMsg, params object[] args)
        {
            return String.Format(errorMsg, args) + Environment.NewLine;
        }

        public static Type GetExternalCaller(Exception e)
        {
            int skipFrames = 1;
#if( DEBUG )
            skipFrames += 2;
#endif
            try
            {
                //有性能隐患
                StackTrace st = e != null ? new StackTrace(e, skipFrames, false) : new StackTrace(skipFrames, false);
                for (int i = 0; i < st.FrameCount - 1; i++)
                {
                    StackFrame sf = st.GetFrame(i);
                    Type dt = sf.GetMethod().DeclaringType;
                    if (dt != typeof(ZeroLog))
                        return dt;
                }
                return typeof(ZeroLog);
            }
            catch
            {
                return typeof(ZeroLog);
            }
        }

        #endregion

        #region Log Output
        /// <summary>
        /// Log a message.  The current logging level is used to determine
        ///		if the message is appended to the configured appender
        ///		or if it is ignored.
        /// </summary>
        /// <param name="category">The category to which this log statement belongs.</param>
        /// <param name="s">The severity of the logging message.</param>
        /// <param name="errorMsg">A concise description of the problem encountered.</param>
        /// <param name="args">Variable values that are to be captured with the logging statement.</param>
        public static void Log(Severity s, string errorMsg, params object[] args)
        {
            if (args != null && args.Length > 0)
                LogMessage(s, Format(s.ToString() + ": " + errorMsg, args), null);
            else
                LogMessage(s, errorMsg, null);
        }

        /// <summary>
        /// Log a message.  The specified <see cref="Severity"/> level is compared against
        /// the current logging levels to determine if the message is logged or ignored.
        /// </summary>
        /// <param name="category">The category to which this log statement belongs.</param>
        /// <param name="s">The severity level of the logging message.</param>
        /// <param name="msg">The message to log.</param>
        public static void Log(Severity s, string msg)
        {
            LogMessage(s, msg, null);
        }

        /// <summary>
        /// Log a message.  The specified <see cref="Severity"/> level is compared against
        /// the current logging levels to determine if the message is logged or ignored.
        /// </summary>
        /// <param name="category">The category to which this log statement belongs.</param>
        /// <param name="s">The severity level of the logging message.</param>
        /// <param name="msg">The message to log.</param>
        /// <param name="e">An exception to associate with the error being logged.</param>
        public static void Log(Severity s, string msg, Exception e)
        {
            LogMessage(s, msg, e);
        }

        /// <summary>
        /// Log a message.  Actually perform the logging message to the
        ///		appender specifified in the configuration file.
        /// </summary>
        /// <param name="category">The category to which this log statement belongs.</param>
        /// <param name="s">A <see cref="Severity"/> level which is used to determine if 
        /// the message should be logged or ignored.</param>
        /// <param name="msg">A string value describing the message.</param>
        /// <param name="e">An exception that has occurred.  If no exception has occurred, use <code>null</code>.</param>
        internal static void LogMessage(Severity s, string msg, Exception e)
        {
            LogMessage(typeof(ZeroLog), GetLevel(s), msg, e);
        }
        #endregion

        #region Log Output Convenience Methods
        /// <summary>
        /// Convenience methods for logging with a predefined severity level.
        /// </summary>
        public static void LogDebug(string errorMsg, params object[] args)
        {
            Log(Severity.Debug, errorMsg, args);
        }

        /// <summary>
        /// Convenience methods for logging with a predefined severity level.
        /// </summary>
        public static void LogInfo(string errorMsg, params object[] args)
        {
            Log(Severity.Info, errorMsg, args);
        }

        /// <summary>
        /// Convenience methods for logging with a predefined severity level.
        /// </summary>
        public static void LogWarning(string errorMsg, params object[] args)
        {
            Log(Severity.Warning, errorMsg, args);
        }

        /// <summary>
        /// Log an error message. The error message is delegated to the log4net 
        /// appender(s) in the app config file.
        /// </summary>
        /// <param name="category">The category to which this log statement belongs.</param>
        /// <param name="errorMsg">A description of what has occurred.</param>
        /// <param name="args">Variable values that were present at
        ///	the time the error occurred.</param>
        public static void LogError(string errorMsg, params object[] args)
        {
            Log(Severity.Error, errorMsg, args);
        }

        /// <summary>
        /// Log an error message. Include the exception that has occurred
        ///	in the text of the error message.
        /// </summary>
        /// <param name="category">The category to which this log statement belongs.</param>
        /// <param name="e">The exception to be logged.</param>
        public static void LogError(Exception e)
        {
            Log(Severity.Error, "", e);
        }
        #endregion

        #region Log4net Output Helpers
        private static Level GetLevel(Severity s)
        {
            switch (s)
            {
                case Severity.Debug:
                    return Level.Debug;
                case Severity.Info:
                    return Level.Info;
                case Severity.Warning:
                    return Level.Warn;
                case Severity.Error:
                    return Level.Error;
                case Severity.Critical:
                    return Level.Critical;
                default:
                    return Level.Debug;
            }
        }

        private static ILog GetLogger(Type originator)
        {
            if (loggers.ContainsKey(originator))
                return loggers[originator] as ILog;
            else
            {
                ILog log = LogManager.GetLogger(originator);
                loggers[originator] = log;
                return log;
            }
        }

        private static void LogMessage(Type originator, Level level, string msg, Exception e)
        {
            ILog log = GetLogger(originator);
            if (log != null)
                log.Logger.Log(originator, level, msg, e);
        }
        #endregion
    }
}
