using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace GTA.VTS.CustomersOrders.AppForm
{
    public class MdiFormBase : Form
    {
        protected override void WndProc(ref Message m)
        {
            const int WM_SIZE = 0x0005;
            const int WM_ACTIVATE = 0x0006;

            const int WM_SYSCOMMAND = 0x112;
            const int SC_CLOSE = 0xF060;
            const int SC_MINIMIZE = 0xF020;
            const int SC_MAXIMIZE = 0xF030;

            switch (m.Msg)
            {
                case WM_SIZE:
                    if (this.WindowState != FormWindowState.Maximized)
                    {
                        this.WindowState = FormWindowState.Maximized;
                    }
                    break;
                case WM_ACTIVATE:
                    break;
            }
            base.WndProc(ref m);

        }
        



    }
}
