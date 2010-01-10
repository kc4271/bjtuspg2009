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

namespace Demo
{
    /// <summary>
    /// Menu.xaml 的交互逻辑
    /// </summary>
    public partial class Menu : Page
    {
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //textUsrname.Text = Profile.currentUser.username;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Register.xaml", UriKind.Relative));
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
