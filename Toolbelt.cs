using libWiiSharp;
using Newtonsoft.Json;
using NUS_Downloader.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace NUS_Downloader
{
    public static class Toolbelt
    {
        public static string GetVersion()
        {
            return Resources.version.Trim('\n');
        }

        public static string SizeSuffix(ulong b)
        {
            var bytes = (long)b;

            const int scale = 1024;
            string[] orders = { "GB", "MB", "KB", "Bytes" };
            var max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (var order in orders)
            {
                if (bytes > max)
                    return $"{decimal.Divide(bytes, max):##.##} {order}";

                max /= scale;
            }
            return "0 Bytes";
        }

        public static bool IsValid(TMD_Content content, string contentFile)
        {
            if (!File.Exists(contentFile)) return false;

            return (ulong)new FileInfo(contentFile).Length == content.Size;
        }

        public static void CDecrypt(NusClient nus, string tdir)
        {
            try
            {
                if (!GZip.Decompress(Resources.CDecrypt, tdir + "/CDecrypt.exe"))
                    nus.FireDebug("Error decrypting contents!\r\n       Could not extract CDecrypt.");

                if (!GZip.Decompress(Resources.libeay32, tdir + "/libeay32.dll"))
                    nus.FireDebug("Error decrypting contents!\r\n       Could not extract libeay32.");

                var cdecryptP = new Process
                {
                    StartInfo =
                    {
                        FileName = tdir + "/CDecrypt.exe",
                        Arguments = "tmd cetk",
                        WorkingDirectory = tdir,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }
                };

                cdecryptP.Start();

                while (!cdecryptP.StandardOutput.EndOfStream)
                {
                    nus.FireDebug(cdecryptP.StandardOutput.ReadLine());
                    Application.DoEvents();
                }
                cdecryptP.WaitForExit();
                cdecryptP.Dispose();

                File.Delete(tdir + "/CDecrypt.exe");
                File.Delete(tdir + "/libeay32.dll");

                nus.FireDebug("Finished decrypting contents.");
            }
            catch (Exception ex)
            {
                nus.FireDebug("Error decrypting contents!\r\n" + ex.Message);
            }
        }
        
        public class WiiUTitle
        {
            public string TitleID { get; set; }
            public string TitleKey { get; set; }
            public string Name { get; set; }
            public string Region { get; set; }
            public string Ticket { get; set; }
        }

        private static string databaseFile => "database.json";

        public static WiiUTitle GetTitle(string tid)
        {
            using (var wc = new WebClient())
            {
                if (!File.Exists(databaseFile) || DateTime.Now > new FileInfo(databaseFile).LastWriteTime.AddDays(1))
                    wc.DownloadFile("https://wiiu.titlekeys.com/json", databaseFile);

                var json = File.ReadAllText(databaseFile, Encoding.UTF8);

                var database = JsonConvert.DeserializeObject<List<WiiUTitle>>(json);
                var titleInfo = database.Find(t => t.TitleID.ToLower() == tid.ToLower());

                TextInfo textInfo = new CultureInfo("en-US", true).TextInfo;
                titleInfo.Name = textInfo.ToTitleCase(titleInfo.Name.ToLower().Replace('\n', ' '));
                titleInfo.Name += $" ({titleInfo.Region})";

                return titleInfo;
            }
        }
    }
}