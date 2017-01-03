/* This file is part of libWiiSharp
 * Copyright (C) 2009 Leathl
 * 
 * libWiiSharp is free software: you can redistribute it and/or
 * modify it under the terms of the GNU General Public License as published
 * by the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * libWiiSharp is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied warranty
 * of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using NUS_Downloader;

namespace libWiiSharp
{
    public enum StoreType
    {
        EncryptedContent = 0,
        DecryptedContent = 1,
        WAD = 2,
        All = 3,
        Empty = 4
    }

    public class NusClient : IDisposable
    {
        private const string WII_TIK_URL = "https://wiiu.titlekeys.com/ticket/";
        private const string WII_NUS_URL2 = "http://nus.cdn.shop.wii.com/ccs/download/";
        private const string WII_NUS_URL = "http://ccs.cdn.wup.shop.nintendo.net/ccs/download/";
        private const string DSI_NUS_URL = "http://nus.cdn.t.shop.nintendowifi.net/ccs/download/";

        private const string WII_USER_AGENT = "wii libnup/1.0";
        private const string DSI_USER_AGENT = "Opera/9.50 (Nintendo; Opera/154; U; Nintendo DS; en)";

        private string nusUrl = WII_NUS_URL;
        private string nusUrl2 = WII_NUS_URL2;
        private WebClient wcNus = new WebClient();
        private bool useLocalFiles = false;
        private bool continueWithoutTicket = false;

        private int titleversion;

        public int TitleVersion { get { return titleversion; } }

        /// <summary>
        /// If true, existing local files will be used.
        /// </summary>
        public bool UseLocalFiles { get { return useLocalFiles; } set { useLocalFiles = value; } }
        /// <summary>
        /// If true, the download will be continued even if no ticket for the title is avaiable (WAD packaging and decryption are disabled).
        /// </summary>
        public bool ContinueWithoutTicket { get { return continueWithoutTicket; } set { continueWithoutTicket = value; } }

		#region IDisposable Members
        private bool isDisposed = false;

        ~NusClient()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                wcNus.Dispose();
            }

            isDisposed = true;
        }
        #endregion

        #region Public Functions

        public void ConfigureNusClient(WebClient wcReady)
        {
            wcNus = wcReady;
        }

        public void SetToWiiServer()
        {
            nusUrl = WII_NUS_URL;
            wcNus.Headers.Add("User-Agent", WII_USER_AGENT);
        }

        public void SetToDSiServer()
        {
            nusUrl = DSI_NUS_URL;
            wcNus.Headers.Add("User-Agent", DSI_USER_AGENT);
        }

        /// <summary>
        /// Grabs a single content file and decrypts it.        
        /// Leave the title version empty for the latest. 
        /// </summary>
        /// <param name="titleId"></param>
        /// <param name="titleVersion"></param>
        /// <param name="contentId"></param>
        /// <param name="savePath"></param>
        public void DownloadSingleContent(string titleId, string titleVersion, string contentId, string savePath)
        {
            if (titleId.Length != 16) throw new Exception("Title ID must be 16 characters long!");
            if (!Directory.Exists(Path.GetDirectoryName(savePath))) Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            if (File.Exists(savePath)) File.Delete(savePath);

            byte[] content = DownloadSingleContent(titleId, titleVersion, contentId);
            File.WriteAllBytes(savePath, content);
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Grabs a single content file and decrypts it.        
        /// Leave the title version empty for the latest. 
        /// </summary>
        /// <param name="titleId"></param>
        /// <param name="titleVersion"></param>
        /// <param name="contentId"></param>
        /// <returns></returns>
        private byte[] DownloadSingleContent(string titleId, string titleVersion, string contentId)
        {
            if (titleId.Length != 16) throw new Exception("Title ID must be 16 characters long!");

            uint cId = uint.Parse(contentId, System.Globalization.NumberStyles.HexNumber);
            contentId = cId.ToString("x8");

            FireDebug("Downloading Content (Content ID: {0}) of Title {1} v{2}...", contentId, titleId, (string.IsNullOrEmpty(titleVersion)) ? "[Latest]" : titleVersion);

            FireDebug("   Checking for Internet connection...");
            if (!CheckInet())
            { FireDebug("   Connection not found..."); throw new Exception("You're not connected to the internet!"); }

            FireProgress(0);

            string tmdFile = "tmd" + (string.IsNullOrEmpty(titleVersion) ? string.Empty : string.Format(".{0}", titleVersion));
            string titleUrl = $"{nusUrl}{titleId}/";
            string contentIdString = string.Empty;
            int cIndex = 0;

            //Download TMD
            FireDebug("   Downloading TMD...");
            byte[] tmdArray = wcNus.DownloadData(titleUrl + tmdFile);
            FireDebug("   Parsing TMD...");
            TMD tmd = TMD.Load(tmdArray);

            FireProgress(20);

            //Search for Content ID in TMD
            FireDebug("   Looking for Content ID {0} in TMD...", contentId);
            bool foundContentId = false;
            for (int i = 0; i < tmd.Contents.Length; i++)
                if (tmd.Contents[i].ContentID == cId)
                {
                    FireDebug("   Content ID {0} found in TMD...", contentId);
                    foundContentId = true;
                    contentIdString = tmd.Contents[i].ContentID.ToString("x8");
                    cIndex = i;
                    break;
                }

            if (!foundContentId)
            { FireDebug("   Content ID {0} wasn't found in TMD...", contentId); throw new Exception("Content ID wasn't found in the TMD!"); }

            //Download Ticket
            FireDebug("   Downloading Ticket...");
            byte[] tikArray = wcNus.DownloadData(titleUrl + "cetk");
            FireDebug("   Parsing Ticket...");
            Ticket tik = Ticket.Load(tikArray);

            FireProgress(40);

            //Download and Decrypt Content
            FireDebug("   Downloading Content... ({0} bytes)", tmd.Contents[cIndex].Size);
            byte[] encryptedContent = wcNus.DownloadData(titleUrl + contentIdString);

            FireProgress(80);

            FireDebug("   Decrypting Content...");
            byte[] decryptedContent = DecryptContent(encryptedContent, cIndex, tik, tmd);
            Array.Resize(ref decryptedContent, (int)tmd.Contents[cIndex].Size);

            //Check SHA1
            SHA1 s = SHA1.Create();
            byte[] newSha = s.ComputeHash(decryptedContent);

            if (!Shared.CompareByteArrays(newSha, tmd.Contents[cIndex].Hash))
            { FireDebug(@"/!\ /!\ /!\ Hashes do not match /!\ /!\ /!\"); throw new Exception("Hashes do not match!"); }

            FireProgress(100);

            FireDebug("Downloading Content (Content ID: {0}) of Title {1} v{2} Finished...", contentId, titleId, (string.IsNullOrEmpty(titleVersion)) ? "[Latest]" : titleVersion);
            return decryptedContent;
        }

        private Ticket DownloadTicket(string titleId)
        {
            if (!CheckInet())
                throw new Exception("You're not connected to the internet!");

            string titleUrl = $"{nusUrl}{titleId}/";
            byte[] tikArray = wcNus.DownloadData(titleUrl + "cetk");

            return Ticket.Load(tikArray);
        }

        private TMD DownloadTmd(string titleId, string titleVersion)
        {
            if (!CheckInet())
                throw new Exception("You're not connected to the internet!");

            string titleUrl = $"{nusUrl}{titleId}/";
            string tmdFile = "tmd" + (string.IsNullOrEmpty(titleVersion) ? string.Empty : string.Format(".{0}", titleVersion));

            byte[] tmdArray = wcNus.DownloadData(titleUrl + tmdFile);

            return TMD.Load(tmdArray);
        }
        
        /// <summary>
        /// Grabs a title from NUS, you can define several store types.
        /// Leave the title version empty for the latest.
        /// </summary>
        /// <param name="titleId"></param>
        /// <param name="titleVersion"></param>
        /// <param name="outputDir"></param>
        /// <param name="storeTypes"></param>
        public void DownloadTitle(string titleId, string titleVersion, string outputDir, string wadName, StoreType[] storeTypes)
        {
            FireDebug("Downloading Title {0} v{1}...", titleId, (string.IsNullOrEmpty(titleVersion)) ? "[Latest]" : titleVersion);

            if (storeTypes.Length < 1)
            { FireDebug("  No store types were defined..."); throw new Exception("You must at least define one store type!"); }
            
            var titleInfo = Toolbelt.GetTitle(titleId);

            string titleUrl = $"{nusUrl}{titleId}/";
            string titleUrl2 = $"{nusUrl2}{titleId}/";
            
            bool storeEncrypted = false;
            bool storeDecrypted = false;

            FireProgress(0);

            foreach (StoreType st in storeTypes)
            {
                switch (st)
                {
                    case StoreType.DecryptedContent:
                        FireDebug("    [=] Storing Decrypted Content...");
                        storeDecrypted = true;
                        break;
                    case StoreType.EncryptedContent:
                        FireDebug("    [=] Storing Encrypted Content...");
                        storeEncrypted = true;
                        break;
                    case StoreType.All:
                        FireDebug("    [=] Storing Decrypted Content...");
                        FireDebug("    [=] Storing Encrypted Content...");
                        FireDebug("    [=] Storing WAD...");
                        storeDecrypted = true;
                        storeEncrypted = true;
                        break;
                    case StoreType.Empty:
                        break;
                }
            }

            FireDebug("  - Checking for Internet connection...");
            if (!CheckInet())
            { FireDebug("   + Connection not found..."); throw new Exception("You're not connected to the internet!"); }
            
            if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
            if (!Directory.Exists(Path.Combine(outputDir, titleInfo.Name))) Directory.CreateDirectory(Path.Combine(outputDir, titleInfo.Name));
            outputDir = Path.Combine(outputDir, titleInfo.Name);

            string tmdFile = "tmd" + (string.IsNullOrEmpty(titleVersion) ? string.Empty : string.Format(".{0}", titleVersion));

            //Download TMD
            FireDebug("  - Downloading TMD...");
            TMD tmd;
            byte[] tmdFileWithCerts;
            try
            {
                tmdFileWithCerts = wcNus.DownloadData(titleUrl + tmdFile);
                tmd = TMD.Load(tmdFileWithCerts);
            }
            catch (Exception ex) { FireDebug("   + Downloading TMD Failed..."); throw new Exception("Downloading TMD Failed:\n" + ex.Message); }

            //Parse TMD
            FireDebug("  - Parsing TMD...");

            if (string.IsNullOrEmpty(titleVersion)) { FireDebug("    + Title Version: {0}", tmd.TitleVersion); }
            FireDebug("    + {0} Contents", tmd.NumOfContents);

            if (!Directory.Exists(Path.Combine(outputDir, tmd.TitleVersion.ToString())))
                Directory.CreateDirectory(Path.Combine(outputDir, tmd.TitleVersion.ToString()));
            outputDir = Path.Combine(outputDir, tmd.TitleVersion.ToString());

            titleversion = tmd.TitleVersion;

            File.WriteAllBytes(Path.Combine(outputDir, tmdFile), tmdFileWithCerts);

            FireProgress(5);

            //Download cetk
            FireDebug("  - Downloading Ticket...");
            try
            {
                wcNus.DownloadFile(Path.Combine(titleUrl, "cetk"), Path.Combine(outputDir, "cetk"));
            }
            catch (Exception ex)
            {
                try
                {
                    if (titleInfo.Ticket == "1")
                    {
                        var cetkUrl = $"{WII_TIK_URL}{titleId.ToLower()}.tik";
                        wcNus.DownloadFile(cetkUrl, Path.Combine(outputDir, "cetk"));
                    }
                }
                catch
                {
                    continueWithoutTicket = false;
                    if (!continueWithoutTicket || !storeEncrypted)
                    {
                        FireDebug("   + Downloading Ticket Failed...");
                        throw new Exception("Downloading Ticket Failed:\n" + ex.Message);
                    }

                    if (!(File.Exists(Path.Combine(outputDir, "cetk"))))
                    {
                        storeDecrypted = false;
                    }
                }
            }

            FireProgress(10);

            // Parse Ticket
            Ticket tik = new Ticket();

            if (File.Exists(Path.Combine(outputDir, "cetk")))
            {
                FireDebug("   + Parsing Ticket...");
                tik = Ticket.Load(Path.Combine(outputDir, "cetk"));

                // DSi ticket? Must make sure to use DSi Key :D
                if (nusUrl == DSI_NUS_URL)
                    tik.DSiTicket = true;
            }
            else
            {
                FireDebug("   + Ticket Unavailable...");
            }

            string[] encryptedContents = new string[tmd.NumOfContents];

            //Download Content
            for (int i = 0; i < tmd.NumOfContents; i++)
            {
                var size = Toolbelt.SizeSuffix(tmd.Contents[i].Size);
                FireDebug("  - Downloading Content #{0} of {1}... ({2} bytes)", i + 1, tmd.NumOfContents, size);
                FireProgress(((i + 1) * 60 / tmd.NumOfContents) + 10);

                var contentPath = Path.Combine(outputDir, tmd.Contents[i].ContentID.ToString("x8"));
                if (useLocalFiles && Toolbelt.IsValid(tmd.Contents[i], contentPath))
                {
                    FireDebug("   + Using Local File, Skipping..."); continue;
                }

                try
                {
                    var downloadUrl = titleUrl + tmd.Contents[i].ContentID.ToString("x8");
                    var outputdir = Path.Combine(outputDir, tmd.Contents[i].ContentID.ToString("x8"));
                    wcNus.DownloadFile(downloadUrl, outputdir);

                    encryptedContents[i] = tmd.Contents[i].ContentID.ToString("x8");
                }
                catch (Exception ex)
                {
                    FireDebug("  - Downloading Content #{0} of {1} failed...", i + 1, tmd.NumOfContents);
                    throw new Exception("Downloading Content Failed:\n" + ex.Message);
                }
            }

            //Decrypt Content
            if (storeDecrypted)
            {
                FireDebug("  - Decrypting Content...");
                Toolbelt.CDecrypt(this, outputDir);
            }

            //Delete not wanted files
            if (!storeEncrypted)
            {
                FireDebug("  - Deleting Encrypted Contents...");
                for (int i = 0; i < tmd.Contents.Length; i++)
                    if (File.Exists(Path.Combine(outputDir, tmd.Contents[i].ContentID.ToString("x8"))))
                        File.Delete(Path.Combine(outputDir, tmd.Contents[i].ContentID.ToString("x8")));
            }

            if (!storeDecrypted && !storeEncrypted)
            {
                FireDebug("  - Deleting TMD and Ticket...");
                File.Delete(Path.Combine(outputDir, tmdFile));
                File.Delete(Path.Combine(outputDir, "cetk"));
            }

            FireDebug("Downloading Title {0} v{1} Finished...", titleId, tmd.TitleVersion /*(string.IsNullOrEmpty(titleVersion)) ? "[Latest]" : titleVersion*/);
            FireProgress(100);
        }

        private byte[] DecryptContent(byte[] content, int contentIndex, Ticket tik, TMD tmd)
        {
            Array.Resize(ref content, Shared.AddPadding(content.Length, 16));
            var titleKey = tik.TitleKey;
            var iv = new byte[16];

            var tmp = BitConverter.GetBytes(tmd.Contents[contentIndex].Index);
            iv[0] = tmp[1];
            iv[1] = tmp[0];

            var rm = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None,
                KeySize = 128,
                BlockSize = 128,
                Key = titleKey,
                IV = iv
            };
            var decryptor = rm.CreateDecryptor();

            var ms = new MemoryStream(content);
            var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);

            var decCont = new byte[content.Length];
            cs.Read(decCont, 0, decCont.Length);

            cs.Dispose();
            ms.Dispose();

            return decCont;
        }

        private bool CheckInet()
        {
            try
            {
                IPHostEntry ipHost = Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch { return false; }
        }
        #endregion

        #region Events
        /// <summary>
        /// Fires the Progress of various operations
        /// </summary>
        public event EventHandler<ProgressChangedEventArgs> Progress;
        /// <summary>
        /// Fires debugging messages. You may write them into a log file or log textbox.
        /// </summary>
        public event EventHandler<MessageEventArgs> Debug;

        public void FireDebug(string debugMessage, params object[] args)
        {
            Debug?.Invoke(new object(), new MessageEventArgs(string.Format(debugMessage, args)));
        }

        private void FireProgress(int progressPercentage)
        {
            Progress?.Invoke(new object(), new ProgressChangedEventArgs(progressPercentage, string.Empty));
        }
        #endregion
    }
}
