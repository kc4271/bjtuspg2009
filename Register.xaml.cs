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

namespace Demo
{
    /// <summary>
    /// Register.xaml 的交互逻辑
    /// </summary>
    public partial class Register : Page
    {
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            /*
            App.ConfigClass.SaveConfig("sUserName", this.txtUsername.Text);
            */
            //Profile.CreateNewProfile(txtUsername.Text);


            this.NavigationService.Navigate(new Uri("GameMain.xaml", UriKind.Relative));
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Menu.xaml", UriKind.Relative));
        }
    }
}
