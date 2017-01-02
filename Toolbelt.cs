using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using libWiiSharp;
using NUS_Downloader.Properties;

namespace NUS_Downloader
{
    public static class Toolbelt
    {
        public static string SizeSuffix(ulong b)
        {
            var bytes = (long) b;

            const int scale = 1024;
            string[] orders = {"GB", "MB", "KB", "Bytes"};
            var max = (long) Math.Pow(scale, orders.Length - 1);

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

            return (ulong) new FileInfo(contentFile).Length == content.Size;
        }

        public static void CDecrypt(NusClient nus, TMD tmdstring, string tdir)
        {
            try
            {
                if (!GZip.Decompress(Resources.CDecrypt, tdir + "/CDecrypt.exe"))
                    nus.fireDebug("Error decrypting contents!\r\n       Could not extract CDecrypt.");
                
                if (!GZip.Decompress(Resources.libeay32, tdir + "/libeay32.dll"))
                    nus.fireDebug("Error decrypting contents!\r\n       Could not extract libeay32.");

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
                nus.fireDebug("Decrypting contents...\r\n");

                cdecryptP.Start();

                while (!cdecryptP.StandardOutput.EndOfStream)
                {
                    nus.fireDebug(cdecryptP.StandardOutput.ReadLine());
                    Application.DoEvents();
                }
                cdecryptP.WaitForExit();
                cdecryptP.Dispose();
                
                File.Delete(tdir + "/CDecrypt.exe");
                File.Delete(tdir + "/libeay32.dll");

                nus.fireDebug("Finished decrypting contents.");
            }
            catch (Exception ex)
            {
                nus.fireDebug("Error decrypting contents!\r\n" + ex.Message);
            }
        }
    }
}