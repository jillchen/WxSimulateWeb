using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace WxSimulateWebHelper
{
    public class UserDirectory
    {
        public static void Creat(string filePath, string uidName)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string userXml = string.Format(@"<?xml version=""1.0""?><configuration><system.web><authorization><allow users=""{0}""/></authorization></system.web></configuration>", uidName);
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(userXml);
            xdoc.Save(Path.Combine(filePath, "web.config"));
        }

        public static void Clean(string filePath, string uidName,int seconds)
        {
            if (!Directory.Exists(filePath))
            {
                Creat(filePath, uidName);
                return;
            }

            string[] files = Directory.GetFiles(filePath);
            foreach (string file in files)
            {
                if (file.ToLower().Contains("web.config"))
                {
                    continue;
                }
                try
                {
                    if (File.GetLastWriteTimeUtc(file) < DateTime.UtcNow.AddSeconds(seconds * -1))
                        File.Delete(file);
                }
                catch { }  // ignore locked files
            }

        }
    }
}
