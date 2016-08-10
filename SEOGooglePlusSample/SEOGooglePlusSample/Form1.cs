using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SEOGooglePlusSample
{
    public partial class Form1 : Form
    {
        string email = "";
        string passwd = "";

        string search = "music";

        public Form1()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                if(webBrowser1.Document.GetElementById("next") != null)
                {
                    webBrowser1.Document.GetElementById("Email").SetAttribute("value", email);
                    webBrowser1.Document.GetElementById("next").InvokeMember("click");
                }
                else if(webBrowser1.Document.GetElementById("signIn") != null)
                {
                    webBrowser1.Document.GetElementById("Passwd").SetAttribute("value", passwd);
                    webBrowser1.Document.GetElementById("signIn").InvokeMember("click");
                }
                else if(webBrowser1.Document.GetElementById("gbqfq") != null)
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
            catch(Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }
    }
}
