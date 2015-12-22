using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LXR.Counter
{
    internal static class Strings
    {
        internal static String Server = @"Server";
        internal static String TerminalID = @"TerminalID";
        internal static String TerminalPassword = @"Password";
    }

    internal static class LogonData
    {
        //internal String Server { get; set; }
        //internal Int32 TerminalID { get; set; }
        //internal String TerminalPassword { get; set; }

        static String fileName = @"logonData";

        static String GetFilePath()
        {
            String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            //path = Path.Combine(path, fileName);
            return path;
        }

        internal static void SaveData(string server, int terminalID, string password)
        {
            string path = GetFilePath();
            path = new Uri(path).LocalPath;
            //if (!File.Exists(path))
            //    File.Create(path);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path = Path.Combine(path, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            Dictionary<String, String> DataDir = new Dictionary<String, String>();
            DataDir.Add(Strings.Server, server);
            DataDir.Add(Strings.TerminalID, terminalID.ToString());
            DataDir.Add(Strings.TerminalPassword, password);
            
                // Save data
                // Server:127.0.0.1;TerminalID:3;Password:password
            using (FileStream fileStream = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter fileWriter = new StreamWriter(fileStream))
                {
                    foreach (KeyValuePair<String, String> pair in DataDir)
                    {
                        fileWriter.WriteLine("{0}:{1}",pair.Key, pair.Value);
                    }
                }
            }
 
        }

        internal static void LoadData()
        {

        }
    }
}
