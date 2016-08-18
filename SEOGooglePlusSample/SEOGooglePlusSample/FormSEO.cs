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
    public partial class FormSEO : Form
    {
        // link google plus
        string url = "https://plus.google.com";

        // link cong dong
        string urlCommunities = "https://plus.google.com/communities";

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

        //get position account from list
        int currentAccount = 0;

        const int INPUT_ACCOUNT = 0;//nhap account
        const int INPUT_PASSWORD = 1;
        const int GO_TO_COMMUNITIES = 2;
        const int GO_TO_JOIN = 3;
        const int GO_TO_POST = 4;

        const int GO_TO_REST = -1;


        //clear session
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);


        public FormSEO()
        {
            InitializeComponent();

            webBrowser1.ObjectForScripting = true;

            //load file xml to get account
            loadAccount();

            //load file list id communities
            loadCommunitiesId();

            //InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
            //webBrowser1.Document.Window.Navigate(url);

            Thread t = new Thread(new ThreadStart(process));
            t.Start();
        }

        //load file account xml to list account
        void loadAccount()
        {
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
        }

        //load communities id
        void loadCommunitiesId()
        {
            string fileName = "ListCommunities.txt";
            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName);
                foreach(string id in lines)
                {
                    if(id != "")
                    {
                        lstCommunities.Add(id);
                    }                    
                }
            }
        }

        private void process()
        {
            while (true)
            {
                if (flagComplete)
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
                    else if (countRun == GO_TO_JOIN)
                    {
                        goToJoin();
                    }
                    else if (countRun == GO_TO_POST)
                    {
                        goToPost();
                    }
                    Thread.Sleep(2000);
                }
                else if (countRun == GO_TO_REST)
                {
                    break;
                }
            }
            this.Invoke((MethodInvoker)delegate
            {
                // close the form on the forms thread
                //this.Close();
            });
        }

        void inputAccount()
        {
            webBrowser1.Invoke(
                new MethodInvoker(
                    delegate
                    {
                        if (webBrowser1.Document.GetElementById("next") != null)
                        {
                            webBrowser1.Document.GetElementById("Email").SetAttribute("value", lstEmail.ElementAt(currentAccount));
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
                            webBrowser1.Document.GetElementById("Passwd").SetAttribute("value", lstPassword.ElementAt(currentAccount));
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
                        Random r = new Random();
                        int id = r.Next(0, lstCommunities.Count - 1);
                        webBrowser1.Document.Window.Navigate(urlCommunities + "/" + "110649573479557722163");//"104655218227874201362");//"110649573479557722163");//lstCommunities.ElementAt(id));//104655218227874201362
                    }
                )
            );

            //loai id do khoi danh sach
            //lstCommunities.RemoveAt(id);

            countRun = GO_TO_JOIN;
        }

        

        void goToJoin()
        {
            webBrowser1.Invoke(
                new MethodInvoker(
                    delegate
                    {
                        var contents = webBrowser1.Document.GetElementById("contentPane");
                                                
                        var links = contents.GetElementsByTagName("div");
                        foreach (HtmlElement link in links)
                        {                            
                            if (link.GetAttribute("className") == "Wha")
                            {
                                var spans = link.GetElementsByTagName("span");
                                foreach (HtmlElement span in spans)
                                {
                                    if (span.GetAttribute("className") == "Jic")
                                    {
                                        span.InvokeMember("click");

                                        /*
                                        HtmlElement head = webBrowser1.Document.GetElementsByTagName("head")[0];
                                        HtmlElement scriptEl = webBrowser1.Document.CreateElement("script");
                                        IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
                                        element.text = "function sayHello() { alert('hello') }";
                                        head.AppendChild(scriptEl);
                                        webBrowser1.Document.InvokeScript("sayHello");
                                         * */
                                        HtmlDocument doc = webBrowser1.Document;
                                        HtmlElement head = doc.GetElementsByTagName("head")[0];
                                        HtmlElement s = doc.CreateElement("script");
                                        s.SetAttribute("text", "function performClick() { alert('hello'); }");
                                        head.AppendChild(s);
                                        webBrowser1.Document.InvokeScript("performClick");

                                        /*
                                        //search div tham gia cong dong
                                        var btDivs = span.GetElementsByTagName("div");
                                        foreach (HtmlElement div in btDivs)
                                        {
                                            if (div.GetAttribute("className") == "d-k-l b-c b-c-Wa b-c-da-ja cQc FSb")//"d-k-l b-c b-c-Wa cQc FSb b-c-da-ja")//e3L8lc Ub773
                                            {
                                                div.InvokeMember("click");
                                                break;
                                            }
                                        }
                                         * */
                                    }
                                }
                            } 
                        }
                    }
                )
            );

            //System.IO.File.WriteAllText("WriteText.txt", webBrowser1.Document.Body.ToString());
            countRun = GO_TO_POST;
        }

        //https://www.facebook.com/quanaoteenkorea/photos/pcb.1062006450595683/1062006297262365/?type=3&theater
        void goToPost()
        {
            webBrowser1.Invoke(
                new MethodInvoker(
                    delegate
                    {
                        var contents = webBrowser1.Document.GetElementById("contentPane");

                        var links = contents.GetElementsByTagName("div");
                        foreach (HtmlElement link in links)
                        {
                            if (link.GetAttribute("className") == "Wha")
                            {
                                
                            }
                        }
                    }
                )
            );

            //System.IO.File.WriteAllText("WriteText.txt", webBrowser1.Document.Body.ToString());
            countRun = GO_TO_REST;
        }

        void writeTextToFile(string content)
        {
            string fileName = "WriteText.txt";
            if (!File.Exists(fileName))
            {
                File.CreateText(fileName);
            }
            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.WriteLine(content);
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            flagComplete = true;
            //System.IO.File.WriteAllText("WriteText.txt", webBrowser1.Document.Body.ToString());           
        }
    }
}
