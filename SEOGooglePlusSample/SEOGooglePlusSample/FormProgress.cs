using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SEOGooglePlusSample
{
    public partial class FormProgress : Form
    {
        string url = "https://plus.google.com";

        List<string> email, password;

        string search = "music";

        bool flagFirst = true;

        //clear session
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

        public FormProgress()
        {
            InitializeComponent();

            //load file xml to get account
            string fileName = "account.xml";
            if (File.Exists(fileName))
            {

                FileStream fs = new FileStream(fileName, FileMode.Open);
                XmlTextReader xtr = new XmlTextReader(fs);
                while (!xtr.EOF)
                {
                    if (xtr.MoveToContent() == XmlNodeType.Element && xtr.Name == "Username")
                    {
                        email.Add(xtr.ReadElementString());
                    }
                    else if (xtr.MoveToContent() == XmlNodeType.Element && xtr.Name == "Password")
                    {
                        password.Add(xtr.ReadElementString());
                    }
                    else
                    {
                        xtr.Read();
                    }
                }
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (flagFirst)
            {
                InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
                webBrowser1.Document.Window.Navigate(url);
                flagFirst = false;
            }

            try
            {
                if (webBrowser1.Document.GetElementById("next") != null)
                {
                    //webBrowser1.Document.GetElementById("Email").SetAttribute("value", email);
                    //webBrowser1.Document.GetElementById("next").InvokeMember("click");
                }
                else if (webBrowser1.Document.GetElementById("signIn") != null)
                {
                    //webBrowser1.Document.GetElementById("Passwd").SetAttribute("value", passwd);
                    //webBrowser1.Document.GetElementById("signIn").InvokeMember("click");
                }
                else if (webBrowser1.Document.GetElementById("gbqfq") != null)
                {
                    webBrowser1.Document.GetElementById("gbqfq").SetAttribute("value", search);
                }
                else
                {
                    string text = webBrowser1.Document.Body.ToString();
                    // WriteAllText creates a file, writes the specified string to the file,
                    // and then closes the file.    You do NOT need to call Flush() or Close().
                    System.IO.File.WriteAllText("WriteText.txt", text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }

        }

        private void FormProgress_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
