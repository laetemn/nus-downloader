///////////////////////////////////////////
// NUS Downloader: Form1.Designer.cs     //
// $Rev:: 117                          $ //
// $Author:: givememystuffplease       $ //
// $Date:: 2011-01-19 22:33:05 +0000 (#$ //
///////////////////////////////////////////

namespace NUS_Downloader
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Extrasbtn = new System.Windows.Forms.Button();
            this.downloadstartbtn = new System.Windows.Forms.Button();
            this.statusbox = new System.Windows.Forms.RichTextBox();
            this.NUSDownloader = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.extrasStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.loadInfoFromTMDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.openNUSDDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.proxySettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.donateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutNUSDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richPanel = new System.Windows.Forms.Panel();
            this.clearButton = new System.Windows.Forms.Button();
            this.ProxyVerifyBox = new System.Windows.Forms.GroupBox();
            this.SaveProxyPwdPermanentBtn = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SaveProxyPwdBtn = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.ProxyPwdBox = new System.Windows.Forms.TextBox();
            this.proxyBox = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.ProxyUser = new System.Windows.Forms.TextBox();
            this.SaveProxyBtn = new System.Windows.Forms.Button();
            this.ProxyAssistBtn = new System.Windows.Forms.Button();
            this.ProxyURL = new System.Windows.Forms.TextBox();
            this.databaseButton = new System.Windows.Forms.Button();
            this.keepenccontents = new System.Windows.Forms.CheckBox();
            this.decryptbox = new System.Windows.Forms.CheckBox();
            this.localuse = new System.Windows.Forms.CheckBox();
            this.serverLbl = new System.Windows.Forms.Label();
            this.titleidbox = new wmgCMS.WaterMarkTextBox();
            this.dlprogress = new wyDay.Controls.Windows7ProgressBar();
            this.titleversion = new wmgCMS.WaterMarkTextBox();
            this.updateDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.titleName = new System.Windows.Forms.TextBox();
            this.extrasStrip.SuspendLayout();
            this.richPanel.SuspendLayout();
            this.ProxyVerifyBox.SuspendLayout();
            this.proxyBox.SuspendLayout();
            this.databaseStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Extrasbtn
            // 
            this.Extrasbtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Extrasbtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Extrasbtn.Location = new System.Drawing.Point(243, 5);
            this.Extrasbtn.Name = "Extrasbtn";
            this.Extrasbtn.Size = new System.Drawing.Size(68, 27);
            this.Extrasbtn.TabIndex = 2;
            this.Extrasbtn.Text = "Extras...";
            this.Extrasbtn.UseVisualStyleBackColor = true;
            this.Extrasbtn.Click += new System.EventHandler(this.ExtrasMenuButton_Click);
            // 
            // downloadstartbtn
            // 
            this.downloadstartbtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.downloadstartbtn.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadstartbtn.Location = new System.Drawing.Point(12, 89);
            this.downloadstartbtn.Name = "downloadstartbtn";
            this.downloadstartbtn.Size = new System.Drawing.Size(299, 25);
            this.downloadstartbtn.TabIndex = 5;
            this.downloadstartbtn.Text = "Start NUS Download!";
            this.downloadstartbtn.UseVisualStyleBackColor = true;
            this.downloadstartbtn.Click += new System.EventHandler(this.DownloadBtn_Click);
            // 
            // statusbox
            // 
            this.statusbox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.statusbox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusbox.Location = new System.Drawing.Point(-2, -2);
            this.statusbox.Name = "statusbox";
            this.statusbox.ReadOnly = true;
            this.statusbox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.statusbox.Size = new System.Drawing.Size(300, 269);
            this.statusbox.TabIndex = 0;
            this.statusbox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(207, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "v";
            // 
            // extrasStrip
            // 
            this.extrasStrip.AllowMerge = false;
            this.extrasStrip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extrasStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadInfoFromTMDToolStripMenuItem,
            this.toolStripSeparator3,
            this.openNUSDDirectoryToolStripMenuItem,
            this.toolStripSeparator6,
            this.proxySettingsToolStripMenuItem,
            this.toolStripSeparator7,
            this.donateToolStripMenuItem,
            this.aboutNUSDToolStripMenuItem});
            this.extrasStrip.Name = "extrasStrip";
            this.extrasStrip.Size = new System.Drawing.Size(178, 132);
            this.extrasStrip.Text = "Hidden";
            this.extrasStrip.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.AnyStrip_Closed);
            // 
            // loadInfoFromTMDToolStripMenuItem
            // 
            this.loadInfoFromTMDToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.page_white_magnify;
            this.loadInfoFromTMDToolStripMenuItem.Name = "loadInfoFromTMDToolStripMenuItem";
            this.loadInfoFromTMDToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.loadInfoFromTMDToolStripMenuItem.Text = "Load Info from TMD";
            this.loadInfoFromTMDToolStripMenuItem.Click += new System.EventHandler(this.LoadInfoFromTMDToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(174, 6);
            // 
            // openNUSDDirectoryToolStripMenuItem
            // 
            this.openNUSDDirectoryToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.folder;
            this.openNUSDDirectoryToolStripMenuItem.Name = "openNUSDDirectoryToolStripMenuItem";
            this.openNUSDDirectoryToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openNUSDDirectoryToolStripMenuItem.Text = "Open NUSD Directory";
            this.openNUSDDirectoryToolStripMenuItem.Click += new System.EventHandler(this.OpenNUSDDirectoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(174, 6);
            // 
            // proxySettingsToolStripMenuItem
            // 
            this.proxySettingsToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.server_link;
            this.proxySettingsToolStripMenuItem.Name = "proxySettingsToolStripMenuItem";
            this.proxySettingsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.proxySettingsToolStripMenuItem.Text = "Proxy Settings";
            this.proxySettingsToolStripMenuItem.Click += new System.EventHandler(this.ProxySettingsToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(174, 6);
            // 
            // donateToolStripMenuItem
            // 
            this.donateToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.money;
            this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
            this.donateToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.donateToolStripMenuItem.Text = "Donate!";
            this.donateToolStripMenuItem.Visible = false;
            this.donateToolStripMenuItem.Click += new System.EventHandler(this.DonateToolStripMenuItem_Click);
            // 
            // aboutNUSDToolStripMenuItem
            // 
            this.aboutNUSDToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.information;
            this.aboutNUSDToolStripMenuItem.Name = "aboutNUSDToolStripMenuItem";
            this.aboutNUSDToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.aboutNUSDToolStripMenuItem.Text = "About NUSD";
            this.aboutNUSDToolStripMenuItem.Click += new System.EventHandler(this.AboutNUSDToolStripMenuItem_Click);
            // 
            // richPanel
            // 
            this.richPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richPanel.Controls.Add(this.clearButton);
            this.richPanel.Controls.Add(this.ProxyVerifyBox);
            this.richPanel.Controls.Add(this.proxyBox);
            this.richPanel.Controls.Add(this.statusbox);
            this.richPanel.Location = new System.Drawing.Point(12, 141);
            this.richPanel.Name = "richPanel";
            this.richPanel.Size = new System.Drawing.Size(299, 268);
            this.richPanel.TabIndex = 56;
            // 
            // clearButton
            // 
            this.clearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearButton.AutoSize = true;
            this.clearButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.clearButton.BackColor = System.Drawing.Color.Transparent;
            this.clearButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearButton.Image = global::NUS_Downloader.Properties.Resources.bin_closed;
            this.clearButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.clearButton.Location = new System.Drawing.Point(274, 243);
            this.clearButton.MaximumSize = new System.Drawing.Size(0, 24);
            this.clearButton.MinimumSize = new System.Drawing.Size(0, 24);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(24, 24);
            this.clearButton.TabIndex = 12;
            this.clearButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.clearButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.clearButton.UseVisualStyleBackColor = false;
            this.clearButton.Click += new System.EventHandler(this.ClearStatusbox);
            this.clearButton.MouseEnter += new System.EventHandler(this.ClearButton_MouseEnter);
            this.clearButton.MouseLeave += new System.EventHandler(this.ClearButton_MouseLeave);
            // 
            // ProxyVerifyBox
            // 
            this.ProxyVerifyBox.BackColor = System.Drawing.SystemColors.Control;
            this.ProxyVerifyBox.Controls.Add(this.SaveProxyPwdPermanentBtn);
            this.ProxyVerifyBox.Controls.Add(this.checkBox1);
            this.ProxyVerifyBox.Controls.Add(this.SaveProxyPwdBtn);
            this.ProxyVerifyBox.Controls.Add(this.label14);
            this.ProxyVerifyBox.Controls.Add(this.ProxyPwdBox);
            this.ProxyVerifyBox.Location = new System.Drawing.Point(17, 33);
            this.ProxyVerifyBox.Name = "ProxyVerifyBox";
            this.ProxyVerifyBox.Size = new System.Drawing.Size(212, 133);
            this.ProxyVerifyBox.TabIndex = 46;
            this.ProxyVerifyBox.TabStop = false;
            this.ProxyVerifyBox.Text = "Verify Credentials";
            this.ProxyVerifyBox.Visible = false;
            // 
            // SaveProxyPwdPermanentBtn
            // 
            this.SaveProxyPwdPermanentBtn.Enabled = false;
            this.SaveProxyPwdPermanentBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SaveProxyPwdPermanentBtn.Location = new System.Drawing.Point(9, 104);
            this.SaveProxyPwdPermanentBtn.Name = "SaveProxyPwdPermanentBtn";
            this.SaveProxyPwdPermanentBtn.Size = new System.Drawing.Size(197, 23);
            this.SaveProxyPwdPermanentBtn.TabIndex = 36;
            this.SaveProxyPwdPermanentBtn.Text = "Save (To File)";
            this.SaveProxyPwdPermanentBtn.UseVisualStyleBackColor = true;
            this.SaveProxyPwdPermanentBtn.Click += new System.EventHandler(this.SaveProxyPwdPermanentBtn_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 72);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(214, 30);
            this.checkBox1.TabIndex = 35;
            this.checkBox1.Text = "I understand that NUSD stores proxy\r\npasswords in plain text.";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // SaveProxyPwdBtn
            // 
            this.SaveProxyPwdBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SaveProxyPwdBtn.Location = new System.Drawing.Point(9, 43);
            this.SaveProxyPwdBtn.Name = "SaveProxyPwdBtn";
            this.SaveProxyPwdBtn.Size = new System.Drawing.Size(197, 23);
            this.SaveProxyPwdBtn.TabIndex = 34;
            this.SaveProxyPwdBtn.Text = "Save (This Session Only)";
            this.SaveProxyPwdBtn.UseVisualStyleBackColor = true;
            this.SaveProxyPwdBtn.Click += new System.EventHandler(this.SaveProxyPwdButton_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 21);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(62, 13);
            this.label14.TabIndex = 33;
            this.label14.Text = "Proxy Pass:";
            // 
            // ProxyPwdBox
            // 
            this.ProxyPwdBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProxyPwdBox.Location = new System.Drawing.Point(71, 19);
            this.ProxyPwdBox.Name = "ProxyPwdBox";
            this.ProxyPwdBox.Size = new System.Drawing.Size(135, 22);
            this.ProxyPwdBox.TabIndex = 32;
            this.ProxyPwdBox.UseSystemPasswordChar = true;
            this.ProxyPwdBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ProxyPwdBox_KeyPress);
            // 
            // proxyBox
            // 
            this.proxyBox.BackColor = System.Drawing.Color.White;
            this.proxyBox.Controls.Add(this.label13);
            this.proxyBox.Controls.Add(this.label12);
            this.proxyBox.Controls.Add(this.ProxyUser);
            this.proxyBox.Controls.Add(this.SaveProxyBtn);
            this.proxyBox.Controls.Add(this.ProxyAssistBtn);
            this.proxyBox.Controls.Add(this.ProxyURL);
            this.proxyBox.Location = new System.Drawing.Point(17, 33);
            this.proxyBox.Name = "proxyBox";
            this.proxyBox.Size = new System.Drawing.Size(212, 114);
            this.proxyBox.TabIndex = 45;
            this.proxyBox.TabStop = false;
            this.proxyBox.Text = "Proxy Settings";
            this.proxyBox.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 55);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(33, 13);
            this.label13.TabIndex = 32;
            this.label13.Text = "User:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 29);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 13);
            this.label12.TabIndex = 31;
            this.label12.Text = "Proxy:";
            // 
            // ProxyUser
            // 
            this.ProxyUser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProxyUser.Location = new System.Drawing.Point(55, 53);
            this.ProxyUser.Name = "ProxyUser";
            this.ProxyUser.Size = new System.Drawing.Size(151, 22);
            this.ProxyUser.TabIndex = 30;
            // 
            // SaveProxyBtn
            // 
            this.SaveProxyBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SaveProxyBtn.Location = new System.Drawing.Point(6, 79);
            this.SaveProxyBtn.Name = "SaveProxyBtn";
            this.SaveProxyBtn.Size = new System.Drawing.Size(161, 26);
            this.SaveProxyBtn.TabIndex = 29;
            this.SaveProxyBtn.Text = "Save Proxy Settings";
            this.SaveProxyBtn.UseVisualStyleBackColor = true;
            this.SaveProxyBtn.Click += new System.EventHandler(this.SaveProxyBtn_Click);
            // 
            // ProxyAssistBtn
            // 
            this.ProxyAssistBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ProxyAssistBtn.Image = global::NUS_Downloader.Properties.Resources.help;
            this.ProxyAssistBtn.Location = new System.Drawing.Point(177, 79);
            this.ProxyAssistBtn.Name = "ProxyAssistBtn";
            this.ProxyAssistBtn.Size = new System.Drawing.Size(29, 26);
            this.ProxyAssistBtn.TabIndex = 28;
            this.ProxyAssistBtn.UseVisualStyleBackColor = true;
            this.ProxyAssistBtn.Click += new System.EventHandler(this.ProxyAssistBtn_Click);
            // 
            // ProxyURL
            // 
            this.ProxyURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ProxyURL.Location = new System.Drawing.Point(55, 27);
            this.ProxyURL.Name = "ProxyURL";
            this.ProxyURL.Size = new System.Drawing.Size(151, 22);
            this.ProxyURL.TabIndex = 0;
            // 
            // databaseButton
            // 
            this.databaseButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.databaseButton.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.databaseButton.Location = new System.Drawing.Point(12, 5);
            this.databaseButton.Name = "databaseButton";
            this.databaseButton.Size = new System.Drawing.Size(85, 27);
            this.databaseButton.TabIndex = 0;
            this.databaseButton.Text = "Database...";
            this.databaseButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.databaseButton.UseVisualStyleBackColor = true;
            this.databaseButton.Click += new System.EventHandler(this.DatabaseButton_Click);
            // 
            // keepenccontents
            // 
            this.keepenccontents.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keepenccontents.Image = global::NUS_Downloader.Properties.Resources.package;
            this.keepenccontents.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.keepenccontents.Location = new System.Drawing.Point(12, 441);
            this.keepenccontents.Name = "keepenccontents";
            this.keepenccontents.Size = new System.Drawing.Size(177, 26);
            this.keepenccontents.TabIndex = 8;
            this.keepenccontents.Text = "Keep Encrypted Contents";
            this.keepenccontents.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.keepenccontents.UseVisualStyleBackColor = true;
            // 
            // decryptbox
            // 
            this.decryptbox.Checked = true;
            this.decryptbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.decryptbox.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.decryptbox.Image = global::NUS_Downloader.Properties.Resources.package_green;
            this.decryptbox.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.decryptbox.Location = new System.Drawing.Point(12, 415);
            this.decryptbox.Name = "decryptbox";
            this.decryptbox.Size = new System.Drawing.Size(142, 26);
            this.decryptbox.TabIndex = 9;
            this.decryptbox.Text = "Decrypt and Extract";
            this.decryptbox.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.decryptbox.UseVisualStyleBackColor = true;
            // 
            // localuse
            // 
            this.localuse.AutoSize = true;
            this.localuse.Checked = true;
            this.localuse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.localuse.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.localuse.Image = global::NUS_Downloader.Properties.Resources.drive_disk;
            this.localuse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.localuse.Location = new System.Drawing.Point(12, 470);
            this.localuse.MinimumSize = new System.Drawing.Size(0, 22);
            this.localuse.Name = "localuse";
            this.localuse.Size = new System.Drawing.Size(167, 22);
            this.localuse.TabIndex = 12;
            this.localuse.Text = "Use Local Files If Present";
            this.localuse.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.localuse.UseVisualStyleBackColor = true;
            // 
            // serverLbl
            // 
            this.serverLbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverLbl.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverLbl.Location = new System.Drawing.Point(282, 59);
            this.serverLbl.Name = "serverLbl";
            this.serverLbl.Size = new System.Drawing.Size(29, 22);
            this.serverLbl.TabIndex = 57;
            this.serverLbl.Text = "Wii";
            this.serverLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.serverLbl.Click += new System.EventHandler(this.ServerLbl_Click);
            // 
            // titleidbox
            // 
            this.titleidbox.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.titleidbox.Location = new System.Drawing.Point(11, 60);
            this.titleidbox.MaxLength = 16;
            this.titleidbox.Name = "titleidbox";
            this.titleidbox.Size = new System.Drawing.Size(190, 22);
            this.titleidbox.TabIndex = 3;
            this.titleidbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.titleidbox.WaterMarkColor = System.Drawing.Color.Silver;
            this.titleidbox.WaterMarkText = "Title ID";
            this.titleidbox.TextChanged += new System.EventHandler(this.titleidbox_TextChanged);
            // 
            // dlprogress
            // 
            this.dlprogress.ContainerControl = this;
            this.dlprogress.Location = new System.Drawing.Point(12, 120);
            this.dlprogress.Name = "dlprogress";
            this.dlprogress.Size = new System.Drawing.Size(299, 15);
            this.dlprogress.TabIndex = 47;
            // 
            // titleversion
            // 
            this.titleversion.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.titleversion.Location = new System.Drawing.Point(218, 59);
            this.titleversion.MaxLength = 8;
            this.titleversion.Name = "titleversion";
            this.titleversion.Size = new System.Drawing.Size(58, 22);
            this.titleversion.TabIndex = 4;
            this.titleversion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.titleversion.WaterMarkColor = System.Drawing.Color.Silver;
            this.titleversion.WaterMarkText = "Version";
            this.titleversion.TextChanged += new System.EventHandler(this.Titleversion_TextChanged);
            // 
            // updateDatabaseToolStripMenuItem
            // 
            this.updateDatabaseToolStripMenuItem.Image = global::NUS_Downloader.Properties.Resources.database_save;
            this.updateDatabaseToolStripMenuItem.Name = "updateDatabaseToolStripMenuItem";
            this.updateDatabaseToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.updateDatabaseToolStripMenuItem.Text = "Update Databases";
            this.updateDatabaseToolStripMenuItem.Click += new System.EventHandler(this.UpdateDatabaseToolStripMenuItem_Click);
            // 
            // databaseStrip
            // 
            this.databaseStrip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.databaseStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateDatabaseToolStripMenuItem});
            this.databaseStrip.Name = "databaseStrip";
            this.databaseStrip.ShowItemToolTips = false;
            this.databaseStrip.Size = new System.Drawing.Size(164, 26);
            this.databaseStrip.Text = "Hidden";
            this.databaseStrip.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.AnyStrip_Closed);
            // 
            // titleName
            // 
            this.titleName.BackColor = System.Drawing.SystemColors.Control;
            this.titleName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.titleName.Cursor = System.Windows.Forms.Cursors.Default;
            this.titleName.Font = new System.Drawing.Font("Segoe UI Semibold", 7.75F, System.Drawing.FontStyle.Bold);
            this.titleName.Location = new System.Drawing.Point(12, 39);
            this.titleName.Name = "titleName";
            this.titleName.ReadOnly = true;
            this.titleName.Size = new System.Drawing.Size(309, 14);
            this.titleName.TabIndex = 58;
            this.titleName.Text = "null title";
            this.titleName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(323, 500);
            this.Controls.Add(this.titleName);
            this.Controls.Add(this.serverLbl);
            this.Controls.Add(this.titleidbox);
            this.Controls.Add(this.dlprogress);
            this.Controls.Add(this.titleversion);
            this.Controls.Add(this.databaseButton);
            this.Controls.Add(this.downloadstartbtn);
            this.Controls.Add(this.keepenccontents);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Extrasbtn);
            this.Controls.Add(this.richPanel);
            this.Controls.Add(this.decryptbox);
            this.Controls.Add(this.localuse);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.extrasStrip.ResumeLayout(false);
            this.richPanel.ResumeLayout(false);
            this.richPanel.PerformLayout();
            this.ProxyVerifyBox.ResumeLayout(false);
            this.ProxyVerifyBox.PerformLayout();
            this.proxyBox.ResumeLayout(false);
            this.proxyBox.PerformLayout();
            this.databaseStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Extrasbtn;
        private System.Windows.Forms.Button downloadstartbtn;
        private System.Windows.Forms.RichTextBox statusbox;
        private System.Windows.Forms.CheckBox localuse;
        private System.ComponentModel.BackgroundWorker NUSDownloader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox decryptbox;
        private System.Windows.Forms.Button databaseButton;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.ContextMenuStrip extrasStrip;
        private System.Windows.Forms.ToolStripMenuItem loadInfoFromTMDToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem proxySettingsToolStripMenuItem;
        private wyDay.Controls.Windows7ProgressBar dlprogress;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private wmgCMS.WaterMarkTextBox titleidbox;
        private wmgCMS.WaterMarkTextBox titleversion;
        private System.Windows.Forms.ToolStripMenuItem aboutNUSDToolStripMenuItem;
        private System.Windows.Forms.CheckBox keepenccontents;
        private System.Windows.Forms.Panel richPanel;
        private System.Windows.Forms.ToolStripMenuItem openNUSDDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem donateToolStripMenuItem;
        private System.Windows.Forms.Label serverLbl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem updateDatabaseToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip databaseStrip;
        private System.Windows.Forms.GroupBox ProxyVerifyBox;
        private System.Windows.Forms.Button SaveProxyPwdPermanentBtn;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button SaveProxyPwdBtn;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox proxyBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox ProxyUser;
        private System.Windows.Forms.Button SaveProxyBtn;
        private System.Windows.Forms.Button ProxyAssistBtn;
        private System.Windows.Forms.TextBox ProxyURL;
        private System.Windows.Forms.TextBox ProxyPwdBox;
        private System.Windows.Forms.TextBox titleName;
    }
}

