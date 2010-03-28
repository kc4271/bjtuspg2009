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
using System.Windows.Threading;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Demo
{
    /// <summary>
    /// GameMain.xaml 的交互逻辑
    /// </summary>
    public partial class GameMain : GamePage
    {
        CAnimalControl Animal;
        int LowestLine = 300; //鼠标最低有效范围线
        Point MoveTo;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // 加载用户信息
            txtUsername.Text = "HI," + App.CurrentUser.name;
            txtUsergold.Text = App.CurrentUser.gold.ToString() + "金币";

            // 加载背景
            Load(CurrentCarrier,COriginalInfo.nGameMainMapInfo);
            BaseCarrier.MouseLeftButtonDown += new MouseButtonEventHandler(this.Carrier_MouseLeftButtonDown);
            
            // 加载宠物
            Animal = new CAnimalControl();
            Animal.Load(CurrentCarrier, COriginalInfo.nGameMainMapInfo);
            MoveTo.X = Animal.WindowX;

             DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Current_Tick);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(40);
            dispatcherTimer.Start();
        }

        private void Current_Tick(object sender, EventArgs e)
        {
            if (Math.Abs(Animal.WindowX - MoveTo.X) > Animal.MoveUnit)
            {
                Animal.isMoving = true;
                if (MoveTo.X <= Animal.WindowX)
                    Animal.nDirection = 0;
                else
                    Animal.nDirection = 1;

                if (Animal.WindowX < MoveTo.X)
                {
                    Animal.Move = Animal.MoveUnit;
                }
                else
                {
                    Animal.Move = -Animal.MoveUnit;
                }
            }
            else
            {
                Animal.isMoving = false;
            }

            if (Animal.WindowX >= 700)
            {
                if (this.NavigationService == null)
                {
                    return;
                }
                this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
            }
        }

        private void Carrier_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pMousePos = e.GetPosition(BaseCarrier);
            if (pMousePos.Y < LowestLine)
                return;
            MoveTo = pMousePos;
        }
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
             // 存档
            try
            {
                App.Profile.SetXmlFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("存档异常" + ex.ToString());
            }
           
            // 修改最后登录信息
            try
            {
                XDocument myDoc = XDocument.Load("Profile/Profile.xml");
                XElement xElement = myDoc.XPathSelectElement("Profile/Name[1]");
                xElement.SetValue(App.CurrentUser.name.ToString());
                myDoc.Save("Profile/Profile.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("修改最后登录信息异常" + ex.ToString());
            }

            // 跳转到Menu
            this.NavigationService.Navigate(new Uri("Menu.xaml", UriKind.Relative));
        }
    }
}
