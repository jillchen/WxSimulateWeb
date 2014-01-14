using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WxSimulateWebHelper
{
    public class Logger
    {
        private string logFile;

        public Logger(string logFile)
        {
            this.logFile = logFile;
            File.Delete(logFile);
        }

        public void Log(string msg)
        {
            using (StreamWriter sw = new StreamWriter(new FileStream(logFile, FileMode.Append,
                                                        FileAccess.Write, FileShare.Read)))
            {
                sw.WriteLine("{0} - {1}", DateTime.Now, msg);
            }
        }
    }
}
