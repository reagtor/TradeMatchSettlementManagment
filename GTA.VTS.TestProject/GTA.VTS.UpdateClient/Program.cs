using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Configuration;

namespace GIMSClient
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。

        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FmProgress progress = new FmProgress();
            UpdateManager manager = new UpdateManager();
            progress.CancelClick += new EventHandler(progress_CancelClick);
            try
            {
                progress.StepOperation = new FmProgress.StepOperationRepetition(manager.LongTaskForUpdateAllFile);
                progress.ExeRepetition(manager.UpdateServer.FileNameUnderServerDirectory, 0, "Downloading Update File...", null);
                if (MessageBox.Show("Update Success, Do you want start " + ConfigurationManager.AppSettings["UpdateAppName"] + "?",
                    "Info",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                    System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["UpdateAppName"] + ".exe");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (MessageBox.Show("Update Failed, Do you want start " + ConfigurationManager.AppSettings["UpdateAppName"] + "?",
                    "Info",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                    System.Diagnostics.Process.Start(ConfigurationManager.AppSettings["UpdateAppName"] + ".exe");
            }

        }

        static void progress_CancelClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}