using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.IO;
using System.Configuration;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Demo
{
    /// <summary>
    /// Menu.xaml 的交互逻辑
    /// </summary>
    public partial class Menu : Page
    {
        private string LastUser = "Guest"; 

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // 显示上次登录用户名
            XDocument myDoc = XDocument.Load("Profile/Profile.xml");
            XElement xElement = myDoc.XPathSelectElement("Profile/Name[1]");
            LastUser = textUsrname.Text = xElement.Value;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 如果上次是Guest 则Register 否则GameMain
                if (LastUser == "Guest")
                {
                    this.NavigationService.Navigate(new Uri("Register.xaml", UriKind.Relative));
                }
                else
                {
                    string str = "Profile/" + LastUser + ".xml";
                    App.Profile.XmlReload(str);
                    App.Profile.GetXmlReader();
                    this.NavigationService.Navigate(new Uri("GameMain.xaml", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("初次加载Profile.xml异常" + ex.ToString());
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("LoadGame.xaml", UriKind.Relative));
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
