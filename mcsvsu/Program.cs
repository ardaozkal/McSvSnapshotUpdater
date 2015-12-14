using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace mcsvsu
{
    class Program
    {
        static WebClient wc = new WebClient();
        static string versionslink = "https://s3.amazonaws.com/Minecraft.Download/versions/versions.json";
        static string serverlink = "https://s3.amazonaws.com/Minecraft.Download/versions/{version}/minecraft_server.{version}.jar";
        static string filename = "minecraftsnapshotserver.jar"; //change this as you want
        static string versionfilename = "mcsvsnup_latestversion.txt"; //change this as you want
        static string downloadpath = Directory.GetCurrentDirectory(); //change this to whereever you want it to download

        static void Main(string[] args)
        {
            Console.WriteLine("===Minecraft Server Snapshot Updater===");
            Console.WriteLine("==By ardaozkal==");
            Console.WriteLine("Open source at http://github.com/ardaozkal/McSvSnapshotUpdater/");

            try
            {
                Console.WriteLine("MCSVSU: Starting download of versions.json");
                var jsonfile = wc.DownloadString(versionslink); //downloads the latest versions json file
                Console.WriteLine("MCSVSU: Downloaded versions.json");

                try
                {
                    var fromsnapshot = jsonfile.Substring(jsonfile.IndexOf("\"snapshot\": \"")).Replace("\"snapshot\": \"", ""); //replaces the part until the version
                    var latestsnapshotversion = fromsnapshot.Substring(0, fromsnapshot.IndexOf("\"")); //gets the part until ", which is the end of the version 
                    var downloadlastversion = false;
                    Console.WriteLine("MCSVSU: Parsed Json, latest version: {0}.", latestsnapshotversion);

                    if (!File.Exists(downloadpath + "\\" + filename) || !File.Exists(downloadpath + "\\" + versionfilename)) //if server jar or version txt doesn't exist
                    {
                        downloadlastversion = true;
                        File.WriteAllText((downloadpath + "\\" + versionfilename), ""); //just to create the file if it doesn't exist. Clearing doesn't affect anyone if the jar is missing, so yeah, no problem.
                    }

                    if (!downloadlastversion && File.ReadAllText((downloadpath + "\\" + versionfilename)) != latestsnapshotversion)
                    {
                        downloadlastversion = true;
                    }

                    if (downloadlastversion)
                    {
                        try
                        {
                            Console.WriteLine("MCSVSU: Starting update to version {0}.", latestsnapshotversion);
                            File.WriteAllText((downloadpath + "\\" + versionfilename), latestsnapshotversion);
                            var linkwithversion = serverlink.Replace("{version}", latestsnapshotversion);
                            wc.DownloadFile(linkwithversion, (downloadpath + "\\" + filename));
                            Console.WriteLine("MCSVSU: Updated to version {0}.", latestsnapshotversion);
                        }
                        catch
                        {
                            Console.WriteLine("MCSVSU: Error when downloading or saving file. Check file link and directory permissions.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("MCSVSU: Already up to date at version {0}.", latestsnapshotversion);
                    }
                }
                catch
                {
                    Console.WriteLine("MCSVSU: Error while \"parsing\" json. Did mojang change the file type?");
                }
            }
            catch
            {
                Console.WriteLine("MCSVSU: Error while downloading versions.json file, did mojang change the link/do you have internet access?");
            }
        }
    }
}
