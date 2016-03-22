using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RectangleImgur
{
    public partial class Form1 : Form
    {
        private TextBox textBox1;


        
        public Form1()
        {

            
            
            var bmp = SnippingTool.Snip();

            if (bmp != null)
            {
                string temppath = Path.Combine(Path.GetTempPath(), "rectangleimgur.bmp");

                bmp.Save(temppath, ImageFormat.Png);

                using (var w = new WebClient())
                {

                    string clientID = "xxxv1.0";
                    w.Headers.Add("Authorization", "Client-ID " + clientID);

                    var values = new NameValueCollection
                    {
                        { "image", Convert.ToBase64String(File.ReadAllBytes(temppath)) }
                    };
                    byte[] response = w.UploadValues("https://api.imgur.com/3/upload.xml", values);
                    var result = Encoding.ASCII.GetString(response);
                    
                    XDocument dox = XDocument.Parse(result);
                    var node = dox.Descendants().Where(n => n.Name == "link").FirstOrDefault();
                    string url = node.Value;
                    Process.Start(url);

                    Environment.Exit(0);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {


        }

    }
}
