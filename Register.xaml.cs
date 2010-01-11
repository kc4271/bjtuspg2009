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
using System.Configuration;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Demo
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Page
    {
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            // 修改CurrentUser
            App.CurrentUser.name = txtUsername.Text;
            App.CurrentUser.gold = 888;
            App.CurrentUser.pet = "0";  // 宠物选择
            App.CurrentUser.item1 = 5;
            App.CurrentUser.item2 = 5;

            // 创建新的Username.xml
            try
            {
                App.Profile.CreateNewXml();
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建新Profile异常" + ex.ToString());
            }
            
            // 添加新的结点到Profile.xml
            try
            {
                XDocument myDoc = XDocument.Load("Profile/Profile.xml");
                XElement parentXElement = myDoc.XPathSelectElement("Profile");
                parentXElement.Add(new XElement("Name", App.CurrentUser.name));
                myDoc.Save("Profile/Profile.xml");
            }
            catch (Exception ex)
            { 
                 MessageBox.Show("添加新节点到Profile异常" + ex.ToString());
            }
           
            // 跳转到GameMain
            this.NavigationService.Navigate(new Uri("GameMain.xaml", UriKind.Relative));
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Menu.xaml", UriKind.Relative));
        }
    }
}
