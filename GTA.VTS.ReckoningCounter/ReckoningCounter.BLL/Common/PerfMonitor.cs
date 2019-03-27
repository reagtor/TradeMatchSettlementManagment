using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using GTA.VTS.Common.CommonUtility;

namespace ReckoningCounter.BLL
{
    public static class PerfMonitor
    {
        private static int count = 0;
        private static Timer timer = new Timer();

        static PerfMonitor()
        {
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(count != 0)
                LogHelper.WriteDebug("PerfMonitor.ReceiveOrder=" + count);

            count = 0;
        }

        public static bool Enable
        {
            get
            {
                return timer.Enabled;
            }
            set
            {
                timer.Enabled = value;
            }
        }

        public static DateTime Begin()
        {
            return DateTime.Now;
        }

        public static void End(DateTime beginTime, string key)
        {
            if(!Enable)
                return;

            TimeSpan ts = DateTime.Now - beginTime;
            string format = "PerfMonitor.{0}[{1}]";
            string msg = string.Format(format, key, ts.TotalMilliseconds);
            LogHelper.WriteDebug(msg);
        }

        public static void ReceiveOrder()
        {
            if (!Enable)
                return;

            count++;
        }
    }
}
