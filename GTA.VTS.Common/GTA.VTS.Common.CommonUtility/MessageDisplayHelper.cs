using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GTA.VTS.Common.CommonUtility
{
    public delegate void OnUIEventDelegate(string eventMessage, ListBox lm);

    /// <summary>
    /// 信息显示帮助类、
    /// 作者：朱亮
    /// 日期：2008-11-25
    /// </summary>
    public class MessageDisplayHelper
    {
        public static void DoEvent(string eventMessage, ListBox lm)
        {
            Event(eventMessage, lm);
        }

        public static void Event(string eventMessage,  ListBox lstMessages)
        {
            if (lstMessages == null) return;
            try
            {
                if (lstMessages.InvokeRequired)
                {
                    lstMessages.Invoke(new OnUIEventDelegate(delegate(string s, ListBox lm) { DoEvent(s, lm); }),
                        eventMessage, lstMessages);
                }
                else
                {
                    string[] s = eventMessage.Split('\n');

                    for (int i = s.Length - 1; i >= 0; i--)
                    {
                        lstMessages.BeginUpdate();
                        lstMessages.Items.Insert(0, s[i]);

                        if (lstMessages.Items.Count > 100)
                            lstMessages.Items.RemoveAt(100);

                        lstMessages.EndUpdate();
                    }
                    ;
                }
            }
            catch
            {
                lstMessages = null;
            }


        }
    }
}
