using System;
using System.Windows;
using System.Windows.Controls;
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
            if (txtUsername.Text == "" || txtPetname.Text == "")
            {
                txtUsername.Text = "玩家1";
                txtPetname.Text = "旺财";
                return;
            }

            // 修改CurrentUser
            App.CurrentUser.name = txtUsername.Text;
            App.CurrentUser.gold = 888;
            App.CurrentUser.pet = txtPetname.Text;  // 宠物选择
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
                parentXElement.Add(new XElement("Pet", App.CurrentUser.pet));
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
