using System;
using System.Windows.Forms;
using GTA.VTS.Common.CommonUtility;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;


namespace MatchServiceManager
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadExit += new EventHandler(Application_ThreadExit);
 
            Application.Run(new MatchForm());

        }

        static void Application_ThreadExit(object sender, EventArgs e)
        {
            try
            {

                if (!EventLog.SourceExists("MatchCenterService"))
                {
                    EventLog.CreateEventSource("MatchCenterService", "MatchCenterServiceLog");
                }

                EventLog myLog = new EventLog();
                myLog.Source = "MatchCenterService";

                myLog.WriteEntry(sender.GetType().BaseType.Namespace.ToString() + "线程退出");

            }
            catch (Exception ex1)
            {
                // LogHelper.WriteError("********************撮合中心Domain异常:*******************\r\n", ex1);
            }
        }


        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            try
            {

                if (!EventLog.SourceExists("MatchCenterService"))
                {
                    EventLog.CreateEventSource("MatchCenterService", "MatchCenterServiceLog");
                }

                EventLog myLog = new EventLog();
                myLog.Source = "MatchCenterService";

                myLog.WriteEntry(ex.Message + " : " + ex.StackTrace);

            }
            catch (Exception ex1)
            {
                LogHelper.WriteError("********************撮合中心Domain异常:*******************\r\n", ex1);
            }
            LogHelper.WriteError("********************撮合中心Domain异常:*******************\r\n", ex);
        }

        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            // LogHelper.WriteError(e.Exception.Message, e.Exception);
            try
            {

                if (!EventLog.SourceExists("MatchCenterService"))
                {
                    EventLog.CreateEventSource("MatchCenterService", "MatchCenterServiceLog");
                }

                EventLog myLog = new EventLog();
                myLog.Source = "MatchCenterService";

                myLog.WriteEntry(e.Exception.Message + " : " + e.Exception.StackTrace);

            }
            catch (Exception ex1)
            {
                LogHelper.WriteError("********************撮合中心Domain异常:*******************\r\n", ex1);
            }

            LogHelper.WriteError("********************撮合中心Application异常:*******************\r\n", e.Exception);
        }

        //public bool CreateEvtLogFile()
        //{
        //    //确认事件源是否在本机器上注册 
        //    if (!EventLog.Exists(LogName))
        //    {
        //        try
        //        {
        //            EventLog.CreateEventSource(SourceName, LogName, ".");
        //        }
        //        catch (Exception ex)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }
        //    else
        //        return false;
        //}
        ///// <summary> 
        /////  写日志 
        ///// </summary> 
        ///// <param name="level"> </param> 
        //public void WriteEvtLog(EventLevel level)
        //{
        //    if (!EventLog.Exists(LogName))
        //        CreateEvtLogFile();
        //    EventLogEntryType Eventtype = new EventLogEntryType();
        //    switch (level)
        //    {
        //        case EventLevel.EVENT_ERROR:
        //            Eventtype = EventLogEntryType.Error;
        //            break;
        //        case EventLevel.EVENT_FAILUREAUDIT:
        //            Eventtype = EventLogEntryType.FailureAudit;
        //            break;
        //        case EventLevel.EVENT_INFORMATION:
        //            Eventtype = EventLogEntryType.Information;
        //            break;
        //        case EventLevel.EVENT_SUCCESSAUDIT:
        //            Eventtype = EventLogEntryType.SuccessAudit;
        //            break;
        //        case EventLevel.EVENT_WARNING:
        //            Eventtype = EventLogEntryType.Warning;
        //            break;
        //    }
        //    try
        //    {
        //        EventLog.WriteEntry(SourceName, Message, Eventtype, EventID, CateGory, RawData);
        //    }
        //    catch (Exception ex)
        //    { }
        //} 


    }
}
