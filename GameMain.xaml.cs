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

namespace Demo
{
    /// <summary>
    /// GameMain.xaml 的交互逻辑
    /// </summary>
    public partial class GameMain : GamePage
    {
        bool isMoving = false;
        int count = 0;
        int AnimalMoveSpeed = 10;
        int LowestLine = 300; //鼠标最低有效范围线

        Image Animal;
      
        Point MoveTo;

        public enum eDirection
        {
            BACK, FORWARD
        }
        eDirection dir = eDirection.FORWARD;
        

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            nPageIndex = COriginalInfo.nGameMainMapInfo;
            init(CurrentCarrier);
            BaseCarrier.MouseLeftButtonDown += new MouseButtonEventHandler(this.Carrier_MouseLeftButtonDown);
            
            // 加载宠物
            Animal = new Image();
            BaseCarrier.Children.Add(Animal);
            Canvas.SetTop(Animal, 350);
            Canvas.SetLeft(Animal, 50);
            MoveTo.X = 50;

            this.Sprites[1].Sprite.MouseLeftButtonDown += imgArrow1_MouseLeftButtonDown;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(40);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (isMoving)
            {
                Animal.Source = cutImage(@"Resources\Animal\Doggy.png", count, (int)dir, 64, 64);
                count = (count + 1) % 6;
            }
            else
            {
                Animal.Source = new BitmapImage(
               (new Uri(@"Resources\Animal\Stand" + (int)dir + ".png",
                   UriKind.Relative)));
            }
            Move();
        }

        private void Carrier_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pMousePos = e.GetPosition(BaseCarrier);
            if (pMousePos.Y < LowestLine)
                return;
            MoveTo = pMousePos;
        }

        private void Move()
        {
            double Animal_X = Canvas.GetLeft(Animal);
            if (Math.Abs(Animal_X - MoveTo.X) > AnimalMoveSpeed)
            {
                isMoving = true;
                if (MoveTo.X <= Animal_X)
                    dir = eDirection.BACK;
                else
                    dir = eDirection.FORWARD;
                txtStatus.Text = Animal_X.ToString();
                Canvas.SetLeft(Animal, Animal_X + (Animal_X < MoveTo.X ? AnimalMoveSpeed : -AnimalMoveSpeed));
            }
            else
            {
                isMoving = false;
            }
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
            this.NavigationService.Navigate(new Uri("menu.xaml", UriKind.Relative));
            // autosave
        }

        private void imgArrow1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Math.Abs(Canvas.GetLeft(Animal) - MoveTo.X) < AnimalMoveSpeed * 2)
                this.NavigationService.Navigate(new Uri("Adventure.xaml", UriKind.Relative));
        }

    }
}
