using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SEOGooglePlusSample
{
    public partial class FormProgress : Form
    {
        string url = "https://plus.google.com";
        string url_communities = "https://plus.google.com/communities";

        List<string> email = new List<string>();
        List<string> password = new List<string>();
        
        bool flagComplete = false;
        int countRun = 0;

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
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    email.Add(node.FirstChild.InnerText);
                    password.Add(node.LastChild.InnerText);

                }
            }

            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
            webBrowser1.Document.Window.Navigate(url);

            Thread t = new Thread(new ThreadStart(process));
            t.Start();
        }

        private void process()
        {
            while(true)
            {
                if(flagComplete)
                {
                    flagComplete = false;

                    if (countRun == 0)
                    {
                        inputAccount();
                    }
                    else if (countRun == 1)
                    {
                        inputPassword();
                    }
                    else if (countRun == 2)
                    {
                        goToCommunities();
                    }
                    Thread.Sleep(1000);
                }                
            }
        }

        void inputAccount()
        {
            webBrowser1.Invoke(
                new MethodInvoker(
                    delegate 
                    {
                        if (webBrowser1.Document.GetElementById("next") != null)
                        {
                            webBrowser1.Document.GetElementById("Email").SetAttribute("value", email.ElementAt(0));
                            webBrowser1.Document.GetElementById("next").InvokeMember("click");
                        }
                        else
                        {
                            //neu k dang nhap thi hoan tat phuong that dang nhap va chuyen qua buoc khac
                            flagComplete = true;
                        }
                    }
                )
            );
            countRun++;
        }

        void inputPassword()
        {
            webBrowser1.Invoke(
                new MethodInvoker(
                    delegate
                    {
                        if (webBrowser1.Document.GetElementById("signIn") != null)
                        {
                            webBrowser1.Document.GetElementById("Passwd").SetAttribute("value", password.ElementAt(0));
                            webBrowser1.Document.GetElementById("signIn").InvokeMember("click");
                        }
                        else
                        {
                            //neu k dang nhap thi hoan tat phuong that dang nhap va chuyen qua buoc khac
                            flagComplete = true;
                        }
                    }
                )
            );
            countRun++;
        }

        void goToCommunities()
        {
            webBrowser1.Invoke(
                new MethodInvoker(
                    delegate
                    {
                        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
                        webBrowser1.Document.Window.Navigate(url_communities);
                    }
                )
            );
            countRun++;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            flagComplete = true;
            //System.IO.File.WriteAllText("WriteText.txt", webBrowser1.Document.Body.ToString());           
        }

        private void FormProgress_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
