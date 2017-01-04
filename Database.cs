///////////////////////////////////////////
// NUS Downloader: Database.cs           //
// $Rev:: 113                          $ //
// $Author:: givememystuffplease       $ //
// $Date:: 2011-01-15 00:37:44 +0000 (#$ //
///////////////////////////////////////////

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;

namespace NUS_Downloader
{
    class Database
    {
        private static string DatabaseFile => Directory.GetCurrentDirectory() + "/database.json";

        private static string WIIU_TITLE_DB => "https://wiiu.titlekeys.com/json";

        private string databaseString;

        public void LoadDatabaseToStream()
        {
            if (!File.Exists(DatabaseFile))
                throw new FileNotFoundException("Can't find the database file!", "database.json");
            
            databaseString = File.ReadAllText(DatabaseFile);
        }

        public static string GetDatabaseVersion()
        {
            if (!File.Exists(DatabaseFile))
            {
                throw new Exception("Can't find database file! Does it exist?");
            }

            return new FileInfo(DatabaseFile).CreationTime.ToShortDateString();
        }

        public static WiiUTitle GetTitle(string tid)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    if (!File.Exists(DatabaseFile) || DateTime.Now > new FileInfo(DatabaseFile).LastWriteTime.AddDays(1))
                        wc.DownloadFile(WIIU_TITLE_DB, DatabaseFile);

                    var json = File.ReadAllText(DatabaseFile, Encoding.UTF8);

                    var database = JsonConvert.DeserializeObject<List<WiiUTitle>>(json);
                    var titleInfo = database.Find(t => t.TitleID.ToLower() == tid.ToLower());

                    TextInfo textInfo = new CultureInfo("en-US", true).TextInfo;
                    titleInfo.Name = textInfo.ToTitleCase(titleInfo.Name.ToLower().Replace('\n', ' '));
                    titleInfo.Name += $" ({titleInfo.Region})";

                    return titleInfo;
                }
            }
            catch
            {
                return new WiiUTitle();
            }
        }

        public static string GetTitleName(string tid)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    if (!File.Exists(DatabaseFile) || DateTime.Now > new FileInfo(DatabaseFile).LastWriteTime.AddDays(1))
                        wc.DownloadFile("https://wiiu.titlekeys.com/json", DatabaseFile);

                    var json = File.ReadAllText(DatabaseFile, Encoding.UTF8);

                    var database = JsonConvert.DeserializeObject<List<WiiUTitle>>(json);
                    var titleInfo = database.Find(t => t.TitleID.ToLower() == tid.ToLower());

                    var textInfo = new CultureInfo("en-US", true).TextInfo;
                    titleInfo.Name = textInfo.ToTitleCase(titleInfo.Name.ToLower().Replace('\n', ' '));
                    titleInfo.Name += $" ({titleInfo.Region})";

                    return titleInfo.Name;
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
