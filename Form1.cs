///////////////////////////////////////////
// NUS Downloader: Form1.cs              //
// $Rev:: 121                          $ //
// $Author:: gb.luke                   $ //
// $Date:: 2011-03-19 18:20:49 +0000 (#$ //
///////////////////////////////////////////

///////////////////////////////////////
// Copyright (C) 2010
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>
///////////////////////////////////////


using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NUS_Downloader
{
    partial class Form1 : Form
    {
        private readonly string CURRENT_DIR = Directory.GetCurrentDirectory();

#if DEBUG
        private static string version = $"Git {Toolbelt.GetVersion()}";
#else
        // TODO: Always remember to change version!
        private string version = $"v2.0.0-ahlpa";
#endif

        // Cross-thread Windows Formsing
        private delegate void AddToolStripItemToStripCallback(ToolStripMenuItem menulist, ToolStripMenuItem[] additionitems);
        private delegate void WriteStatusCallback(string Update, Color writecolor);
        private delegate void BootChecksCallback();
        private delegate void SetEnableForDownloadCallback(bool enabled);
        private delegate void SetPropertyThreadSafeCallback(System.ComponentModel.Component what, object setto, string property);
        private delegate string OfficialWADNamingCallback(string whut);
                
        // Proxy stuff...
        private string proxy_url;
        private string proxy_usr;
        private string proxy_pwd;

        // Database threads
        private BackgroundWorker databaseWorker;

        // Colours for status box
        private Color normalcolor = Color.FromName("Black");
        private Color warningcolor = Color.FromName("DarkGoldenrod");
        private Color errorcolor = Color.FromName("Crimson");
        private Color infocolor = Color.FromName("RoyalBlue");

        // Statuses of disabled things
        private bool[] disabledStorage = new bool[13];

        private bool isRunning { get; set; }

        public static Task downloadTask { get; set; }

        // This is the standard entry to the GUI
        public Form1()
        {
            InitializeComponent();

            GUISetup();

            BootChecks();
        }

        // CLI Mode
        public Form1(string[] args)
        {
            InitializeComponent();
            Debug.WriteLine("CLI Parameters passed");

            GUISetup();

            if ((args.Length == 1) && (File.Exists(args[0])))
            {
                BootChecks();

                string script_content = File.ReadAllText(args[0]);
                FileInfo script_file = new FileInfo(args[0]);
                script_content += String.Format(";{0}", script_file.Name.Replace("." + script_file.Extension, ""));
            }
            else if (args.Length >= 2)
            {
                RunCommandMode(args);
                Environment.Exit(0);
                //this.Close();
            }
            else
            {
                BootChecks();
            }           
        }

        private void RunCommandMode(string[] args)
        {
            // CLI mode, inspired and taken from wiiNinja's mod.

            // Initialize the checkboxes and radio boxes
            localuse.Checked = true; // Use local content if already downloaded - default ON
            decryptbox.Checked = false;
            keepenccontents.Checked = false;

            Console.WriteLine("NUS Downloader - v{0}", version);
            
            if (args.Length < 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("    nusd <titleID> <titleVersion | *> [optionalArgs]");
                Console.WriteLine("\nWhere:");
                Console.WriteLine("    titleID = The ID of the title to be downloaded");
                Console.WriteLine("    titleVersion = The version of the title to be downloaded");
                Console.WriteLine("              Use \"*\" (no quotes) to get the latest version");
                Console.WriteLine("    OptionalArgs:");
                Console.WriteLine("        packwad = A wad file will be generated");
                Console.WriteLine("        localuse = Use local contents if available");
                Console.WriteLine("        decrypt = Create decrypted contents");
                Console.WriteLine("        keepencrypt = Keep encrypted contents");
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    Console.WriteLine("{0}", args[i]);
                    switch (i)
                    {
                        case 0:
                            // First command line argument is ALWAYS the TitleID
                            titleidbox.Text = args[i];
                            break;

                        case 1:
                            // Second command line argument is ALWAYS the TitleVersion. 
                            // User may specify a "*" to retrieve the latest version
                            if (args[i] == "*")
                                titleversion.Text = "";
                            else
                                titleversion.Text = args[i];
                            break;

                        default:
                            // Any other arguments beyond the 2nd one are considered optional
                            if (args[i] == "localuse")
                                localuse.Checked = true;
                            else if (args[i] == "decrypt")
                                decryptbox.Checked = true;
                            else if (args[i] == "keepencrypt")
                                keepenccontents.Checked = true;
                            else
                                Console.WriteLine("\n>>>> Warning: Unrecognized command line argument: {0}. This option is ignored...", args[i]);
                            break;
                    }
                }

                // Call to get the files from server
                //NUSDownloader_DoWork(null, null);
                Task.Run(() => NUSDownloader_DoWork());

                Console.WriteLine("\nSuccessfully downloaded the title {0} version {1}", args[0], args[1]);
            }
        }

        private void GUISetup()
        {
            titleName.Text = string.Empty;
            MaximumSize = MinimumSize = Size; // Lock size down PATCHOW :D
            if (Type.GetType("Mono.Runtime") != null)
            {
                clearButton.Text = "Clear";
                keepenccontents.Text = "Keep Enc. Contents";
                clearButton.Left -= 41;
            }
            else
                statusbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7);
            statusbox.SelectionColor = statusbox.ForeColor = normalcolor;
            if (version.StartsWith("SVN"))
            {
                WriteStatus("                 !!! THIS IS A DEBUG BUILD FROM SVN !!!");
                WriteStatus("               Features CAN and WILL be broken in this build!");
                WriteStatus("----------------------------------------------------------------------------------");
                WriteStatus("\r\n");
            }

            // Database BackgroundWorker
            databaseWorker = new BackgroundWorker();
            databaseWorker.DoWork += new DoWorkEventHandler(DoAllDatabaseyStuff);
            databaseWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DoAllDatabaseyStuff_Completed);
            databaseWorker.ProgressChanged += new ProgressChangedEventHandler(DoAllDatabaseyStuff_ProgressChanged);
            databaseWorker.WorkerReportsProgress = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("GrapeVine - {0}", version); ;
            this.Size = this.MinimumSize;
            serverLbl.Text = "Wii";
        }

        private bool NUSDFileExists(string filename)
        {
            return File.Exists(Path.Combine(CURRENT_DIR, filename));
        }

        /// <summary>
        /// Checks certain file existances, etc.
        /// </summary>
        /// <returns></returns>
        private void BootChecks()
        {
            //Check if correct thread...
            if (this.InvokeRequired)
            {
                Debug.WriteLine("InvokeRequired...");
                BootChecksCallback bcc = new BootChecksCallback(BootChecks);
                this.Invoke(bcc);
                return;
            }

            if (NUSDFileExists("database.json") == true)
            {
                Database db = new Database();
                db.LoadDatabaseToStream();
                string version = Database.GetDatabaseVersion();
                WriteStatus("Database.json detected.");
                WriteStatus(" - Version: " + version);
                updateDatabaseToolStripMenuItem.Text = "Update Database";
                databaseButton.Text = "  [    ]";
                databaseButton.Image = Properties.Resources.arrow_ticker;
                // Load it up...
                this.databaseWorker.RunWorkerAsync();
            }

            // Check for Proxy Settings file...
            if (NUSDFileExists("proxy.txt") == true)
            {
                WriteStatus("Proxy settings detected.");
                string[] proxy_file = File.ReadAllLines(Path.Combine(CURRENT_DIR, "proxy.txt"));
                proxy_url = proxy_file[0];

                // If proxy\nuser\npassword
                if (proxy_file.Length > 2)
                {
                    proxy_usr = proxy_file[1];
                    proxy_pwd = proxy_file[2];
                }
                else if (proxy_file.Length > 1)
                {
                    proxy_usr = proxy_file[1];
                    SetAllEnabled(false);
                    ProxyVerifyBox.Visible = true;
                    ProxyVerifyBox.Enabled = true;
                    ProxyPwdBox.Enabled = true;
                    SaveProxyBtn.Enabled = true;
                    ProxyVerifyBox.Select();
                }
            }
        }

        private void DoAllDatabaseyStuff(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            ShowInnerToolTips(false);
        }

        private void DoAllDatabaseyStuff_Completed(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.databaseButton.Text = "Database...";
            this.databaseButton.Image = null;
        }

        private void DoAllDatabaseyStuff_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 25)
                databaseButton.Text = "  [.   ]";
            else if (e.ProgressPercentage == 50)
                databaseButton.Text = "  [..  ]";
            else if (e.ProgressPercentage == 75)
                databaseButton.Text = "  [... ]";
            else if (e.ProgressPercentage == 100)
                databaseButton.Text = "  [....]";
        }

        private void SetAllEnabled(bool enabled)
        {
            for (int a = 0; a < this.Controls.Count; a++)
            {
                try
                {
                    this.Controls[a].Enabled = enabled;
                }
                catch
                {
                    // ...
                }
            }
        }
        
        private void ExtrasMenuButton_Click(object sender, EventArgs e)
        {
            // Show extras menu
            extrasStrip.Text = "Showing";
            extrasStrip.Show(Extrasbtn, 2, (2 + Extrasbtn.Height));

            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer()
            {
                Interval = 52
            };
            timer.Tick += new EventHandler(ContextmenusTimer_Tick);
            timer.Start();
        }

        /// <summary>
        /// Loads the title info from TMD.
        /// </summary>
        private void LoadTitleFromTMD()
        {
            // Show dialog for opening TMD file...
            OpenFileDialog opentmd = new OpenFileDialog()
            {
                Filter = "TMD Files|*tmd*",
                Title = "Open TMD"
            };
            if (opentmd.ShowDialog() != DialogResult.Cancel)
            {
                libWiiSharp.TMD tmdLocal = new libWiiSharp.TMD();
                tmdLocal.LoadFile(opentmd.FileName);
                WriteStatus(String.Format("TMD Loaded ({0} blocks)", tmdLocal.GetNandBlocks()));

                titleidbox.Text = tmdLocal.TitleID.ToString("X16");
                WriteStatus("Title ID: " + tmdLocal.TitleID.ToString("X16"));

                titleversion.Text = tmdLocal.TitleVersion.ToString();
                WriteStatus("Version: " + tmdLocal.TitleVersion);

                if (tmdLocal.StartupIOS.ToString("X") != "0")
                    WriteStatus("Requires: IOS" + int.Parse(tmdLocal.StartupIOS.ToString("X").Substring(7, 2).ToString(), System.Globalization.NumberStyles.HexNumber));
                
                WriteStatus("Content Count: " + tmdLocal.NumOfContents);

                for (int a = 0; a < tmdLocal.Contents.Length; a++)
			    {
                    WriteStatus(String.Format("   Content {0}: {1} ({2} bytes)", a, tmdLocal.Contents[a].ContentID.ToString("X8"), tmdLocal.Contents[a].Size.ToString()));
                    WriteStatus(String.Format("    - Index: {0}", tmdLocal.Contents[a].Index.ToString()));
                    WriteStatus(String.Format("    - Type: {0}", tmdLocal.Contents[a].Type.ToString()));
                    WriteStatus(String.Format("    - Hash: {0}...", DisplayBytes(tmdLocal.Contents[a].Hash, String.Empty).Substring(0, 8)));
                }

                WriteStatus("TMD information parsed!");
            }
        }

        /// <summary>
        /// Writes the status to the statusbox.
        /// </summary>
        /// <param name="Update">The update.</param>
        /// <param name="writecolor">The color to use for writing text into the text box.</param>
        public void WriteStatus(string Update, Color writecolor)
        {
            // Check if thread-safe
            if (statusbox.InvokeRequired)
            {
                Debug.WriteLine("InvokeRequired...");
                WriteStatusCallback wsc = new WriteStatusCallback(WriteStatus);
                this.Invoke(wsc, new object[] { Update, writecolor });
                return;
            }
            // Small function for writing text to the statusbox...
            int startlen = statusbox.TextLength;
            if (statusbox.Text == "")
                statusbox.Text = Update;
            else
                statusbox.AppendText("\r\n" + Update);
            int endlen = statusbox.TextLength;

            // Set the color
            statusbox.Select(startlen, endlen - startlen);
            statusbox.SelectionColor = writecolor;

            // Scroll to end of text box.
            statusbox.SelectionStart = statusbox.TextLength;
            statusbox.SelectionLength = 0;
            statusbox.ScrollToCaret();

            // Also write to console
            Console.WriteLine(Update);
        }

        /// <summary>
        /// Writes the status to the statusbox.
        /// </summary>
        /// <param name="Update">The update.</param>
        public void WriteStatus(string Update)
        {
            WriteStatus(Update, normalcolor);
        }

        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        public static CancellationToken token = tokenSource.Token;

        private async void DownloadBtn_Click(object sender, EventArgs e)
        {
            if (titleidbox.Text == String.Empty)
            {
                // Prevent mass deletion and fail
                WriteStatus("Please enter a Title ID!", errorcolor);
                return;
            }
            else if (!(decryptbox.Checked) && !(keepenccontents.Checked))
            {
                // Prevent pointless running by n00bs.
                WriteStatus("Running with your current settings will produce no output!", errorcolor);
                WriteStatus(" - To amend this, look below and check an output type.", errorcolor);
                return;
            }

            // Running Downloads in background so no form freezing

            if (isRunning)
            {
                downloadstartbtn.Enabled = false;

                tokenSource.Cancel();
                WriteStatus("Download Cancellation Requested. Please be patient!", infocolor);
                await Task.Run(() => { Task.WaitAll(downloadTask); });
                
                dlprogress.Value = 0;

                WriteStatus("NUS Download Cancelled!", infocolor);
                SetEnableforDownload(true);

                downloadstartbtn.Text = "Start NUS Download!";
                downloadstartbtn.Enabled = true;

                tokenSource = new CancellationTokenSource();
                token = tokenSource.Token;
    }
            else
            {
                downloadTask = new Task(()=> NUSDownloader_DoWork(), token);
                downloadTask.Start();
            }

            isRunning = !isRunning;
        }
        
        private void SetPropertyThreadSafe(System.ComponentModel.Component what, object setto, string property)
        {
            if (this.InvokeRequired)
            {
                SetPropertyThreadSafeCallback sptscb = new SetPropertyThreadSafeCallback(SetPropertyThreadSafe);
                try
                {
                    this.Invoke(sptscb, new object[] { what, setto, property });
                }
                catch (Exception)
                {
                    // FFFFF!
                }
                return;
            }
            what.GetType().GetProperty(property).SetValue(what, setto, null);
            //what.Text = setto;
        }

        private void NUSDownloader_DoWork()
        {
            CheckForIllegalCrossThreadCalls = false; // this function would need major rewriting to get rid of this...
            
            WriteStatus("Starting NUS Download. Please be patient!", infocolor);
            SetEnableforDownload(false);
            downloadstartbtn.Text = "Cancel NUS Download!";

            // WebClient configuration
            WebClient nusWC = new WebClient();
            nusWC = ConfigureWithProxy(nusWC);
            
            // Create\Configure NusClient
            libWiiSharp.NusClient nusClient = new libWiiSharp.NusClient();
            nusClient.ConfigureNusClient(nusWC);
            nusClient.UseLocalFiles = localuse.Checked;
            nusClient.ContinueWithoutTicket = true;

            // Server
            if (serverLbl.Text == "Wii")
                nusClient.SetToWiiServer();
            else if (serverLbl.Text == "3DS")
                nusClient.SetToDSiServer();

            // Events
            nusClient.Debug += new EventHandler<libWiiSharp.MessageEventArgs>(NusClient_Debug);
            nusClient.Progress += new EventHandler<ProgressChangedEventArgs>(NusClient_Progress);

            var empty = libWiiSharp.StoreType.Empty;
            libWiiSharp.StoreType[] storeTypes = new libWiiSharp.StoreType[3] { empty, empty, empty };
            if (decryptbox.Checked) storeTypes[1] = libWiiSharp.StoreType.DecryptedContent; else storeTypes[1] = libWiiSharp.StoreType.Empty;
            if (keepenccontents.Checked) storeTypes[2] = libWiiSharp.StoreType.EncryptedContent; else storeTypes[2] = libWiiSharp.StoreType.Empty;

            try
            {
                nusClient.DownloadTitle(titleidbox.Text, titleversion.Text, CURRENT_DIR, storeTypes);
            }
            catch (Exception ex)
            {
                WriteStatus("Download failed: \"" + ex.Message + "\"", errorcolor);
            }

            WriteStatus("NUS Download Finished.");

            Task.Run(() => NUSDownloader_RunWorkerCompleted());
        }

        private void NUSDownloader_RunWorkerCompleted()
        {
            SetEnableforDownload(true);
            downloadstartbtn.Text = "Start NUS Download!";
            dlprogress.Value = 0;

            if (IsWin7())
                dlprogress.ShowInTaskbar = false;
        }

        void NusClient_Progress(object sender, ProgressChangedEventArgs e)
        {
            dlprogress.Value = e.ProgressPercentage;
        }

        void NusClient_Debug(object sender, libWiiSharp.MessageEventArgs e)
        {
            WriteStatus(e.Message);
        }
        
        private void Titleversion_TextChanged(object sender, EventArgs e)
        {

        }
        
        /// <summary>
        /// Displays the bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="spacer">What separates the bytes</param>
        /// <returns></returns>
        public string DisplayBytes(byte[] bytes, string spacer)
        {
            string output = "";
            for (int i = 0; i < bytes.Length; ++i)
            {
                output += bytes[i].ToString("X2") + spacer;
            }
            return output;
        }

        private void DatabaseButton_Click(object sender, EventArgs e)
        {
            // Open Database button menu...
            databaseStrip.Text = "Showing";
            databaseStrip.Show(databaseButton, 2, (2+databaseButton.Height));

            //if (!e.Equals(EventArgs.Empty))
            {
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer()
                {
                    Interval = 50
                };
                timer.Tick += new EventHandler(ContextmenusTimer_Tick);
                timer.Start();
            }
        }

        void ContextmenusTimer_Tick(object sender, EventArgs e)
        {
            if (databaseButton.ClientRectangle.Contains(databaseButton.PointToClient(MousePosition)) && ((System.Windows.Forms.Timer)sender).Interval != 50)
            {
                databaseStrip.Close();
                extrasStrip.Close();
                DatabaseButton_Click(sender, EventArgs.Empty);
                ((System.Windows.Forms.Timer)sender).Stop();
            }

            if (Extrasbtn.ClientRectangle.Contains(Extrasbtn.PointToClient(MousePosition)) && ((System.Windows.Forms.Timer)sender).Interval != 52)
            {
                databaseStrip.Close();
                extrasStrip.Close();
                ExtrasMenuButton_Click(sender, EventArgs.Empty);
                ((System.Windows.Forms.Timer)sender).Stop();
            }

            if ((databaseStrip.Visible == false) && (extrasStrip.Visible == false))
                ((System.Windows.Forms.Timer)sender).Stop();
        }
        
        private void ClearStatusbox(object sender, EventArgs e)
        {
            // Clear Statusbox.text
            statusbox.Text = "";
        }

        /// <summary>
        /// Makes everything disabled/enabled.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        private void SetEnableforDownload(bool enabled)
        {
            if (this.InvokeRequired)
            {
                SetEnableForDownloadCallback sefdcb = new SetEnableForDownloadCallback(SetEnableforDownload);
                this.Invoke(sefdcb, new object[] { enabled });
                return;
            }
            // Disable things the user should not mess with during download...
            if (enabled)
            { // we're enabling things that were enabled BEFORE:
                //downloadstartbtn.Enabled = disabledStorage[0];
                titleidbox.Enabled = disabledStorage[1];
                titleversion.Enabled = disabledStorage[2];
                Extrasbtn.Enabled = disabledStorage[3];
                databaseButton.Enabled = disabledStorage[4];
                localuse.Enabled = disabledStorage[6];
                decryptbox.Enabled = disabledStorage[8];
                keepenccontents.Enabled = disabledStorage[9];
                serverLbl.Enabled = disabledStorage[11];
            }
            else
            {
                //disabledStorage[0] = downloadstartbtn.Enabled;
                disabledStorage[1] = titleidbox.Enabled;
                disabledStorage[2] = titleversion.Enabled;
                disabledStorage[3] = Extrasbtn.Enabled;
                disabledStorage[4] = databaseButton.Enabled;
                disabledStorage[6] = localuse.Enabled;
                disabledStorage[8] = decryptbox.Enabled;
                disabledStorage[9] = keepenccontents.Enabled;
                disabledStorage[11] = serverLbl.Enabled;

                //downloadstartbtn.Enabled = enabled;
                titleidbox.Enabled = enabled;
                titleversion.Enabled = enabled;
                Extrasbtn.Enabled = enabled;
                databaseButton.Enabled = enabled;
                localuse.Enabled = enabled;
                decryptbox.Enabled = enabled;
                keepenccontents.Enabled = enabled;
                serverLbl.Enabled = enabled;
            }
        }
        
        private void ShowInnerToolTips(bool enabled)
        {
            // Force tooltips to GTFO in sub menus...
            foreach (ToolStripItem item in databaseStrip.Items)
            {
                try
                {
                    ToolStripMenuItem menuitem = (ToolStripMenuItem) item;
                    menuitem.DropDown.ShowItemToolTips = false;
                }
                catch (Exception)
                {
                    // Do nothing, some objects will not cast.
                }
            }
        }

        /// <summary>
        /// Determines whether OS is win7.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if OS = win7; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsWin7()
        {
            return (Environment.OSVersion.VersionString.Contains("6.1") == true);
        }
        
        private WebClient ConfigureWithProxy(WebClient client)
        {
                // Proxy
                if (!(String.IsNullOrEmpty(proxy_url)))
                {
                WebProxy customproxy = new WebProxy()
                {
                    Address = new Uri(proxy_url)
                };
                if (String.IsNullOrEmpty(proxy_usr))
                        customproxy.UseDefaultCredentials = true;
                    else
                    {
                    NetworkCredential cred = new NetworkCredential()
                    {
                        UserName = proxy_usr
                    };
                    if (!(String.IsNullOrEmpty(proxy_pwd)))
                            cred.Password = proxy_pwd;

                        customproxy.Credentials = cred;
                    }
                    client.Proxy = customproxy;
                    WriteStatus("  - Custom proxy settings applied!");
                }
                else
                {
                    try
                    {
                    client.Proxy = WebRequest.GetSystemWebProxy();
                    client.UseDefaultCredentials = true;
                    }
                    catch (NotImplementedException)
                    {
                        // Linux support
                        WriteStatus("This operating system does not support automatic system proxy usage. Operating without a proxy...");
                    }
                }
            return client;
        }
        
        private void RetrieveNewDatabase(object sender, DoWorkEventArgs e)
        {
            // Retrieve Wiibrew/DSiBrew database page source code
            using (WebClient wc = new WebClient())
            {
                // Proxy
                //databasedl = ConfigureWithProxy(databasedl);

                string databaseSource = wc.DownloadString(e.Argument.ToString());

                // Return parsed xml database...
                e.Result = databaseSource;
            }
        }

        private void RetrieveNewDatabase_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            string database = e.Result.ToString();
            string databaseFilename = "database.json";

            try
            {
                Database db = new Database();
                db.LoadDatabaseToStream();
                string currentversion = Database.GetDatabaseVersion();
                string onlineversion = Database.GetDatabaseVersion();
                WriteStatus(String.Format(" - Database successfully parsed! ({0})", databaseFilename));
                WriteStatus("   - Current Database Version: " + currentversion);
                WriteStatus("   - Online Database Version: " + onlineversion);

                if (currentversion == onlineversion)
                {
                    WriteStatus(" - You have the latest database version!");
                    return;
                }
            }
            catch (FileNotFoundException)
            {
                WriteStatus(" - Database does not yet exist.");
                WriteStatus("   - Online Database Version: " + Database.GetDatabaseVersion());
            }

            bool isCreation = false;
            if (File.Exists(databaseFilename))
            {
                WriteStatus(" - Overwriting your current database...");
                WriteStatus(String.Format(" - The old database will become 'old{0}' in case the new one is faulty.", databaseFilename));

                string olddatabase = File.ReadAllText(databaseFilename);
                File.WriteAllText("old" + databaseFilename, olddatabase);
                File.Delete(databaseFilename);
                File.WriteAllText(databaseFilename, database);
            }
            else
            {
                WriteStatus(String.Format(" - {0} has been created.", databaseFilename));
                File.WriteAllText(databaseFilename, database);
                isCreation = true;
            }

            // Load it up...
            this.databaseWorker.RunWorkerAsync();

            if (isCreation)
            {
                WriteStatus("Database successfully created!");
                databaseButton.Visible = true;
                //databaseButton.Enabled = false;
                updateDatabaseToolStripMenuItem.Text = "Download Database";
            }
            else
            {
                WriteStatus("Database successfully updated!");
            }
        }

        private void UpdateDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusbox.Text = "";
            WriteStatus("Updating your databases from Wiibrew/DSibrew");

            string[] wiibrewValues = new string[] { "https://wiiu.titlekeys.com/json" };

            foreach(var brew in wiibrewValues)
            {
                BackgroundWorker dbFetcher = new BackgroundWorker();
                dbFetcher.DoWork += new DoWorkEventHandler(RetrieveNewDatabase);
                dbFetcher.RunWorkerCompleted += new RunWorkerCompletedEventHandler(RetrieveNewDatabase_Completed);
                dbFetcher.RunWorkerAsync(brew);
            }
        }

        private void LoadInfoFromTMDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Extras menu -> Load TMD...
            LoadTitleFromTMD();
        }
        
        private void SaveProxyBtn_Click(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(ProxyURL.Text)) && (String.IsNullOrEmpty(ProxyUser.Text)) &&
                ((File.Exists(Path.Combine(CURRENT_DIR, "proxy.txt")))))
            {
                File.Delete(Path.Combine(CURRENT_DIR, "proxy.txt"));
                proxyBox.Visible = false;
                proxy_usr = "";
                proxy_url = "";
                proxy_pwd = "";
                WriteStatus("Proxy settings deleted!");
                return;
            }
            else if ((String.IsNullOrEmpty(ProxyURL.Text)) && (String.IsNullOrEmpty(ProxyUser.Text)) &&
                     ((!(File.Exists(Path.Combine(CURRENT_DIR, "proxy.txt"))))))
            {
                proxyBox.Visible = false;
                WriteStatus("No proxy settings saved!");
                return;
            }

            string proxy_file = "";

            if (!(String.IsNullOrEmpty(ProxyURL.Text)))
            {
                proxy_file += ProxyURL.Text + "\n";
                proxy_url = ProxyURL.Text;
            }

            if (!(String.IsNullOrEmpty(ProxyUser.Text)))
            {
                proxy_file += ProxyUser.Text;
                proxy_usr = ProxyUser.Text;
            }

            if (!(String.IsNullOrEmpty(proxy_file)))
            {
                File.WriteAllText(Path.Combine(CURRENT_DIR, "proxy.txt"), proxy_file);
                WriteStatus("Proxy settings saved!");
            }

            proxyBox.Visible = false;

            SetAllEnabled(false);
            ProxyVerifyBox.Visible = true;
            ProxyVerifyBox.Enabled = true;
            ProxyPwdBox.Enabled = true;
            SaveProxyBtn.Enabled = true;
            ProxyVerifyBox.Select();
        }

        private void ProxySettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check for Proxy Settings file...
            if (File.Exists(Path.Combine(CURRENT_DIR, "proxy.txt")) == true)
            {
                string[] proxy_file = File.ReadAllLines(Path.Combine(CURRENT_DIR, "proxy.txt"));

                ProxyURL.Text = proxy_file[0];
                if (proxy_file.Length > 1)
                {
                    ProxyUser.Text = proxy_file[1];
                }
            }

            proxyBox.Visible = true;
        }

        private void SaveProxyPwdButton_Click(object sender, EventArgs e)
        {
            proxy_pwd = ProxyPwdBox.Text;
            ProxyVerifyBox.Visible = false;
            SetAllEnabled(true);
        }

        private void ProxyPwdBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                SaveProxyPwdButton_Click("LOLWUT", EventArgs.Empty);
        }

        private void ProxyAssistBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you are behind a proxy, set these settings to get through to NUS." +
                            " If you have an alternate port for accessing your proxy, add ':' followed by the port.");
        }

        private void AboutNUSDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Display About Text...
            statusbox.Text = "";
            WriteStatus("NUS Downloader (NUSD)");
            WriteStatus("You are running version: " + version);
            if (version.StartsWith("SVN"))
                WriteStatus("SVN BUILD: DO NOT REPORT BROKEN FEATURES!");

            WriteStatus("This application created by WB3000");
            WriteStatus("Various contributions by lukegb");
            WriteStatus(String.Empty);
            
            if (NUSDFileExists("key.bin") == true)
                WriteStatus("Wii Decryption: Local (key.bin)");


            if (NUSDFileExists("kkey.bin") == true)
                WriteStatus("Wii Korea Decryption: Local (kkey.bin)");


            if (NUSDFileExists("dsikey.bin") == true)
                WriteStatus("DSi Decryption: Local (dsikey.bin)");

            if (NUSDFileExists("database.xml") == false)
                WriteStatus("Database (Wii): Need (database.xml)");
            else
                WriteStatus("Database (Wii): OK");

            if (NUSDFileExists("dsidatabase.xml") == false)
                WriteStatus("Database (DSi): Need (dsidatabase.xml)");
            else
                WriteStatus("Database (DSi): OK");

            if (IsWin7())
                WriteStatus("Windows 7 Features: Enabled");

            WriteStatus("");
            WriteStatus("Special thanks to:");
            WriteStatus(" * Crediar for his wadmaker tool + source, and for the advice!");
            WriteStatus(" * Leathl for libWiiSharp.");
            WriteStatus(" * SquidMan/Galaxy/comex/Xuzz/#WiiDev for advice.");
            WriteStatus(" * Pasta for impressive database contributions.");
            WriteStatus(" * Napo7 for testing proxy settings.");
            WriteStatus(" * Wyatt O'Day for the Windows7ProgressBar Control.");
            WriteStatus(" * Famfamfam for the Silk Icon Set.");
            WriteStatus(" * Anyone who has helped beta test!");
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            SaveProxyPwdPermanentBtn.Enabled = checkBox1.Checked;
        }

        private void SaveProxyPwdPermanentBtn_Click(object sender, EventArgs e)
        {
            proxy_pwd = ProxyPwdBox.Text;

            string proxy_file = File.ReadAllText(Path.Combine(CURRENT_DIR, "proxy.txt"));

            proxy_file += String.Format("\n{0}", proxy_pwd);

            File.WriteAllText(Path.Combine(CURRENT_DIR, "proxy.txt"), proxy_file);

            ProxyVerifyBox.Visible = false;
            SetAllEnabled(true);
            WriteStatus("To delete all traces of proxy settings, delete the proxy.txt file!");
        }

        private void ClearButton_MouseEnter(object sender, EventArgs e)
        {
            // expand clear button
            /*button3.Left = 194;
            button3.Size = new System.Drawing.Size(68, 21);*/
            clearButton.Text = "Clear";
            //button3.ImageAlign = ContentAlignment.MiddleLeft;
        }

        private void ClearButton_MouseLeave(object sender, EventArgs e)
        {
            // shrink clear button
            /*button3.Left = 239;
            button3.Size = new System.Drawing.Size(23, 21);*/
            if (Type.GetType ("Mono.Runtime") == null)
                clearButton.Text = String.Empty;
            //button3.ImageAlign = ContentAlignment.MiddleCenter;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // This prevents errors when exiting before the database is parsed.
            // This is also probably not the best way to accomplish this...
            Environment.Exit(0);
        }
                
        private void OpenNUSDDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Opens the directory NUSD is working in... (CURREND_DIR)
            Process.Start(CURRENT_DIR);
        }
        
        private void AnyStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            ((ContextMenuStrip)sender).Text = "Hidden";
        }

        private void DonateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Organize how this will work...
            Process.Start("http://wb3000.atspace.name/donations.html");
        }

        private void ServerLbl_Click(object sender, EventArgs e)
        {
            string[] serverLblServers = new string[2] { "Wii", "3DS" };

            for (int a = 0; a < serverLblServers.Length; a++)
            {
                if (serverLbl.Text == serverLblServers[a])
                {
                    if (serverLblServers.Length == (a + 1))
                        serverLbl.Text = serverLblServers[0];
                    else
                        serverLbl.Text = serverLblServers[a+1];
                    break;
                }
            }
        }

        private void titleidbox_TextChanged(object sender, EventArgs e)
        {
            titleName.Text = Database.GetTitleName(titleidbox.Text);
        }
    }
}