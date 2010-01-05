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

namespace Demo
{
    /// <summary>
    /// GameMain.xaml 的交互逻辑
    /// </summary>
    public partial class GameMain : Page
    {
        private void btnAdventure_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
        }

        private void btnFeed_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Feed.xaml", UriKind.Relative));
        }

        private void btnShopping_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Shopping.xaml", UriKind.Relative));
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Voice Idioms Chaining Game\nBJTU Speech Group 2009\nKc4271 Song Zoozy");
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("menu.xaml", UriKind.Relative));
            // atuosave
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string tmp1, tmp2;
            App.ConfigClass.ReadConfig(out tmp1,out tmp2);

            txtUsername.Text = "HI," + tmp1;
            txtUserMoney.Text = "Gold: " + tmp2;
        }
    }
}
