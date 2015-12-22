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
        internal const String Server = @"Server";
        internal const String TerminalID = @"TerminalID";
        internal const String TerminalPassword = @"Password";
    }

    internal static class LogonData
    {
        internal static String Server { get; set; }
        internal static Int32 TerminalID { get; set; }
        internal static String TerminalPassword { get; set; }

        static String fileName = @"o_o";

        static String GetFilePath()
        {
            String path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            //path = Path.Combine(path, fileName);
            path = new Uri(path).LocalPath;
            return path;
        }

        internal static void SaveData(string server, int terminalID, string password)
        {
            string path = GetFilePath();
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
                File.SetAttributes(path, FileAttributes.Hidden);
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
            String path = GetFilePath();

            path = Path.Combine(path, fileName);
            if (!File.Exists(path))
                return;

            using(FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    while (!reader.EndOfStream)
                    {
                        String data = reader.ReadLine(); // Data: Value
                        data = data.Trim();
                        String[] dataPair = data.Split(':');
                        if (dataPair.Length == 2)
                        {
                            switch (dataPair[0])
                            {
                                case Strings.Server:
                                    Server = dataPair[1];
                                    break;
                                case Strings.TerminalID:
                                    TerminalID = Convert.ToInt32(dataPair[1]);
                                    break;
                                case Strings.TerminalPassword:
                                    TerminalPassword = dataPair[1];
                                    break;
                            }
                        }
                        
                    }
                }
            }
        }
    }
}
