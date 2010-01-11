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
    /// Shopping.xaml 的交互逻辑
    /// </summary>
    public partial class Shopping : Page
    {
        private int Item1Price = 50;
        private int Item2Price = 100;
        private int TotalPrice;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            comboBox_DropDownClosed(sender, e);
         
            txtWelcome.Text = "你好," + App.CurrentUser.name + ",欢迎来到本商店";
            txtGold.Text = App.CurrentUser.gold.ToString();
            txtItem1.Text = ",现有" + App.CurrentUser.item1.ToString() + "个";
            txtItem2.Text = ",现有" + App.CurrentUser.item2.ToString() + "个";
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            // 购买 物品数加 金币数减
            if (int.Parse(txtLeft.Text) >= 0)
            {
                App.CurrentUser.gold = int.Parse(txtLeft.Text);
                App.CurrentUser.item1 += int.Parse(comboBox1.Text);
                App.CurrentUser.item2 += int.Parse(comboBox2.Text);
            }
            else
            {
                txtPrice.Text = "您的金币不足,请检查.";
                return;
            }
            this.NavigationService.Navigate(new Uri("GameMain.xaml", UriKind.Relative));
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("GameMain.xaml", UriKind.Relative));
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            TotalPrice = int.Parse(comboBox1.Text) * Item1Price + int.Parse(comboBox2.Text) * Item2Price;
            txtPrice.Text = "总价:" + TotalPrice.ToString();
            txtLeft.Text = (App.CurrentUser.gold - TotalPrice).ToString();
        }
    }
}
