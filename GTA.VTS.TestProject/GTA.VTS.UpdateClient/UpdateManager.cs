using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Configuration;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.IO;
using System.Collections;
using GTA.VTS.UpdateServerRemoting;

namespace GIMSClient
{
    public class UpdateManager
    {

        public UpdateManager()
        {
            this.clientGIMSPath = Environment.CurrentDirectory;
        }

        public UpdateManager(string clientGIMSPath)
        {
            this.clientGIMSPath = clientGIMSPath;
        }

        private string serverVersion;

        public string CurrentVersion
        {
            get { return Application.ProductVersion; }
        }

        public string ServerVersion
        {
            get { return serverVersion; }
            set { serverVersion = value; }
        }

        private string clientGIMSPath;

        public string ClientGIMSPath
        {
            get { return clientGIMSPath; }
            set { clientGIMSPath = value; }
        }
        /// <summary>
        /// 创建更新目录
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string CreateUpdatePath(string fileName)
        {

            //string path = ClientGIMSPath + "\\UPDATE";
            //string path = ClientGIMSPath;
            string path = ClientGIMSPath + fileName.Substring(0, fileName.LastIndexOf("\\"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return clientGIMSPath + fileName;
        }


        public bool CanUpdate()
        {
            return true;
        }

        /// <summary>
        /// 更新程序开启的服务器端口
        /// </summary>
        public static string UpdateIP
        {
            get
            {
                string result = "192.168.189.42";
                string str = ConfigurationManager.AppSettings["UpdateIP"];
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
                return result;
            }
        }


        /// <summary>
        /// 更新程序开启的服务器端口
        /// </summary>
        public static int UpdatePort
        {
            get
            {
                int result = 9999;
                string str = ConfigurationManager.AppSettings["UpdatePort"];
                if (!string.IsNullOrEmpty(str))
                {
                    if (int.TryParse(str, out result))
                    {
                        return result;
                    }
                    else
                    {
                        return 9999;
                    }
                }

                return result;
            }
        }


        public UpdateServer UpdateServer
        {
            get
            {

                if (ChannelServices.RegisteredChannels.Length == 0)
                    ChannelServices.RegisterChannel(new TcpClientChannel(), false);
                UpdateServer server = (UpdateServer)Activator.GetObject(
                    typeof(UpdateServer), "tcp://" + UpdateIP + ":" + UpdatePort.ToString() + "/UpdateServer");
                return server;
            }
        }

        public void UpdateFile(string fileName)
        {
            string clientFileName = CreateUpdatePath(fileName);
            FileStream stream = new FileStream(clientFileName, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(stream);
            byte[] contents = UpdateServer.Content(fileName);
            writer.Write(contents, 0, contents.Length);
            writer.Close();
            stream.Close();
        }

        public void LongTaskForUpdateAllFile(
            IList files,
            int p,
            out string info,
            params object[] objs)
        {
            string currentFile = (string)files[p];
            if (currentFile.ToLower().IndexOf(".config") > 0)
            {
                if (MessageBox.Show("是否要更新程序的配置文件" + currentFile + "?", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    info = "";
                    return;
                }
            }

            UpdateFile(currentFile);
            info = "Downloading  File: " + currentFile;
        }
    }
}
