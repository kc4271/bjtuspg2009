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
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Collections;

namespace Demo
{
    /// <summary>
    /// LoadGame.xaml 的交互逻辑
    /// </summary>
    public partial class LoadGame : Page
    {
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ListBinding();
        }

        private void ListBinding()
        {
            listBox1.Items.Clear();
            XDocument myDoc = XDocument.Load("Profile/Profile.xml");
            XElement parentXElement = myDoc.XPathSelectElement("Profile");
            IEnumerable<XElement> xElements = parentXElement.Elements("Name");
            List<XElement> list = xElements.ToList();
            for (int i = 1; i < list.Count; i++)
            {
                listBox1.Items.Add(list[i].Value);
            }
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string str = listBox1.SelectedItem.ToString();
            try
            {
                // 修改Profile.xml里第一项为Guest
                App.CurrentUser.name = "Guest";
                XDocument myDoc = XDocument.Load("Profile/Profile.xml");
                XElement xElement = myDoc.XPathSelectElement("Profile/Name[1]");
                xElement.SetValue("Guest");
                App.Profile.XmlReload("Profile/Guest.xml");

                // 删除Profile.xml里对应项
                XElement parentXElement = myDoc.XPathSelectElement("Profile");
                IEnumerable<XElement> xElements = parentXElement.Elements("Name");
                foreach (XElement ele in xElements)
                {
                    if (ele.Value == str)
                    {
                        ele.Remove();
                        break;
                    }
                }
                
                // 保存Profile.xml
                myDoc.Save("Profile/Profile.xml");

                // 删除Username.xml
                System.IO.File.Delete("Profile/" + str + ".xml"); 

            }
            catch (Exception ex)
            {
                MessageBox.Show("删除存档异常" + ex.ToString());
            }

            // 刷新
            ListBinding();

            // Disable Buttons
            btnConfirm.IsEnabled = false;
            btnDelete.IsEnabled = false;
            
      
        }

        private void btnAdventure_Click(object sender, RoutedEventArgs e)
        {
            // 加载存档
            try
            {
                string str = "Profile/" + listBox1.SelectedItem.ToString() + ".xml";
                //MessageBox.Show(str);
                App.Profile.XmlReload(str);
                App.Profile.GetXmlReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载存档异常" + ex.ToString());
            }

            // 跳转到GameMain
            this.NavigationService.Navigate(new Uri("GameMain.xaml", UriKind.Relative));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Menu.xaml", UriKind.Relative));
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnConfirm.IsEnabled = true;
            btnDelete.IsEnabled = true;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Register.xaml", UriKind.Relative));
        }
    }
}
