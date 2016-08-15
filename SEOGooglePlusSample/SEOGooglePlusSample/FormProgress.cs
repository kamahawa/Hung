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
        // link google plus
        string url = "https://plus.google.com";

        // link cong dong
        string urlCommunities = "https://plus.google.com/communities";

        // link search
        string urlSearch = "https://plus.google.com/s/";//https://plus.google.com/s/sach%20hay/communities

        // tu khoa tim kiem cua cong dong
        string search = "sach hay";

        // so luong thanh vien cua cong dong
        int numberUser = 50000;

        // list id cong dong
        List<string> lstCommunities = new List<string>();
        
        // list email dang nhap
        List<string> lstEmail = new List<string>();

        // list password
        List<string> lstPassword = new List<string>();
        
        // flag to check load page complete
        bool flagComplete = false;

        // count to step
        int countRun = 0;

        const int INPUT_ACCOUNT = 0;//nhap account
        const int INPUT_PASSWORD = 1;
        const int GO_TO_COMMUNITIES = 2;
        const int GO_TO_SEARCH = 3;
        const int GO_TO_JOIN = 4;

        const int GO_TO_REST = -1;

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
                    lstEmail.Add(node.FirstChild.InnerText);
                    lstPassword.Add(node.LastChild.InnerText);

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

                    if (countRun == INPUT_ACCOUNT)
                    {
                        inputAccount();
                    }
                    else if (countRun == INPUT_PASSWORD)
                    {
                        inputPassword();
                    }
                    else if (countRun == GO_TO_COMMUNITIES)
                    {
                        goToCommunities();
                    }
                    else if (countRun == GO_TO_SEARCH)
                    {
                        goToSearch();
                    }
                    else if (countRun == GO_TO_JOIN)
                    {
                        goToJoin();
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
                            webBrowser1.Document.GetElementById("Email").SetAttribute("value", lstEmail.ElementAt(0));
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
            countRun = INPUT_PASSWORD;
        }

        void inputPassword()
        {
            webBrowser1.Invoke(
                new MethodInvoker(
                    delegate
                    {
                        if (webBrowser1.Document.GetElementById("signIn") != null)
                        {
                            webBrowser1.Document.GetElementById("Passwd").SetAttribute("value", lstPassword.ElementAt(0));
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
            countRun = GO_TO_COMMUNITIES;
        }

        void goToCommunities()
        {
            webBrowser1.Invoke(
                new MethodInvoker(
                    delegate
                    {
                        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
                        webBrowser1.Document.Window.Navigate(urlCommunities);
                    }
                )
            );
            countRun = GO_TO_SEARCH;
        }

        void goToSearch()
        {
            webBrowser1.Invoke(
                new MethodInvoker(
                    delegate
                    {
                        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
                        webBrowser1.Document.Window.Navigate(urlSearch + search + "/communities");
                    }
                )
            );
            countRun = GO_TO_JOIN;
        }

        void goToJoin()
        {
            webBrowser1.Invoke(
                new MethodInvoker(
                    delegate
                    {
                        var links = webBrowser1.Document.GetElementsByTagName("a");
                        foreach (HtmlElement link in links)
                        {
                            if (link.GetAttribute("className") == "d-s ob b7a n9c Dgc s7c hWd")
                            {
                                MessageBox.Show("here");
                                lstCommunities.Add(link.GetAttribute("data-comm"));
                            }
                        }
                    }
                )
            );

            //System.IO.File.WriteAllText("WriteText.txt", webBrowser1.Document.Body.ToString());
            countRun = GO_TO_REST;
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
