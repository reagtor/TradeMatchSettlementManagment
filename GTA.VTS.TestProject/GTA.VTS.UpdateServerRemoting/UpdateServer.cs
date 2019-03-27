using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

namespace GTA.VTS.UpdateServerRemoting
{
    public class UpdateServer : MarshalByRefObject
    {
        private string appPath = ConfigurationManager.AppSettings["APPFilePath"];

        public string APPFilePath
        {
            get { return appPath; }
            set { appPath = value; }
        }

        public UpdateServer()
        {

        }

        public string[] FileNameUnderServerDirectory
        {
            get
            {
                return FileNameUnderPath(APPFilePath);
            }
        }

        public string[] FileNameUnderPath(string path)
        {

            List<string> result = new List<string>();
            string[] fInfo = Directory.GetFiles(path);
            string[] dInfo = Directory.GetDirectories(path);
            foreach (string fif in fInfo)
                result.Add(fif.Replace(APPFilePath, ""));
            foreach (string dif in dInfo)
                result.AddRange(FileNameUnderPath(dif));
            return result.ToArray();
        }

        public byte[] Content(string fileName)
        {
            fileName = APPFilePath + fileName;
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            byte[] result = new byte[stream.Length];
            stream.Read(result, 0, (int)stream.Length);
            stream.Close();
            return result;
        }
    }
}
