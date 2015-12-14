using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCSVSnapshotUpdater
{
    public partial class Form1 : Form
    {
        WebClient wc = new WebClient();
        public Form1()
        {
            InitializeComponent();

    //        {
    //            "latest": {
    //                "snapshot": "15w50a",
    //"release": "1.8.9"
    //            },
    
            var jsonfile = wc.DownloadString("https://s3.amazonaws.com/Minecraft.Download/versions/versions.json");
            var fromsnapshot = jsonfile.Substring(jsonfile.IndexOf("\"snapshot\": \"")).Replace("\"snapshot\": \"", "");
            var latestsnapshotversion = fromsnapshot.Substring(0, fromsnapshot.IndexOf("\""));
            MessageBox.Show(latestsnapshotversion);
        }
    }
}
