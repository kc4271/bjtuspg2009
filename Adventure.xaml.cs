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
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Demo
{
    public partial class Adventure : GamePage
    {
        #region variables
        
        int nIndexOfBox = 3;
        bool isClickedBox = false;
        bool isMoving = false;

        int count = 0;
        Image Animal;

        eDirection dir = eDirection.FORWARD;
        int AnimalMoveSpeed = 10;
        int BackgroundMoveSpeed = 10;
        int LowestLine = 400; //鼠标最低有效范围线
        Point MoveTo;
        double MouseWorldX;
        public enum eDirection
        {
            BACK, FORWARD
        }
        #endregion
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            nPageIndex = 0;
            init(CurrentCarrier);
            BaseCarrier.MouseLeftButtonDown += new MouseButtonEventHandler(this.Carrier_MouseLeftButtonDown);

            #region Foreground
            // 加载宠物
            Animal = new Image();
            BaseCarrier.Children.Add(Animal);
            AnimalWindowX = 300;
            MouseWorldX = 300;
            Canvas.SetTop(Animal, 450);
            #endregion

            for (int i = 2; i < 5; i++ )
                this.Sprites[i].Sprite.MouseLeftButtonDown += BOX_MouseLeftButtonDown;
            this.Sprites[1].Sprite.MouseLeftButtonDown += btnBack_Click;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(40);
            dispatcherTimer.Start();

            MoveTo.X = AnimalWindowX;
        }
       
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (isMoving)
            {
                Animal.Source = cutImage(@"Resources\Animal\Doggy.png", count,(int)dir, 64, 64);
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
            MouseWorldX = MoveTo.X - BackgroundX;
        }

        private void Move()
        {
            if (Math.Abs(AnimalWorldX - MouseWorldX) > AnimalMoveSpeed)
            {
                isMoving = true;
                if (MoveTo.X <= AnimalWindowX)
                    dir = eDirection.BACK;

                else
                    dir = eDirection.FORWARD;

                if (AnimalWorldX - MouseWorldX < 0)
                {
                    if (AnimalWindowX > 500 && BackgroundX > -1200)
                    {
                        foreach (CSprite i in Sprites)
                        {
                            Canvas.SetLeft(i.Sprite, i.pos.X - BackgroundMoveSpeed);
                        }
                    }
                    else
                        AnimalWorldX = AnimalWorldX + AnimalMoveSpeed;
                }
                else
                {
                    if (AnimalWindowX < 300 && BackgroundX < 0)
                    {
                        foreach (CSprite i in Sprites)
                        {
                            Canvas.SetLeft(i.Sprite, i.pos.X + BackgroundMoveSpeed);
                        }
                    }
                    else
                        AnimalWorldX = AnimalWorldX - AnimalMoveSpeed;
                }
            }
            else
            {
                if (isClickedBox)
                {
                    if (Math.Abs(AnimalWindowX - 
                        (Sprites[nIndexOfBox].pos.X + 
                        Sprites[nIndexOfBox].Sprite.Source.Width / 2)) < 2 * AnimalMoveSpeed)
                    {
                        if (this.NavigationService == null)
                        {
                            return;
                        }
                        if (this.NavigationService.Source.ToString() != "Chaining.xaml")
                        {
                            this.NavigationService.Navigate(new Uri("Chaining.xaml", UriKind.Relative));
                        }
                    }
                }
                isClickedBox = false;
                isMoving = false;
            }
        }

        

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("GameMain.xaml", UriKind.Relative));
        }

        private void btnChaining_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Chaining.xaml", UriKind.Relative));
        }

        private void BOX_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isClickedBox = true;
            for (int i = 2; i < 5; i++)
            {
                if (sender.GetHashCode() == Sprites[i].Sprite.GetHashCode())
                {
                    nIndexOfBox = i;
                    break;
                }
            }
        }
        protected double AnimalWindowX
        {
            get { return Canvas.GetLeft(Animal) + 32; }
            set { Canvas.SetLeft(Animal, value - 32); }
        }

        

        protected double AnimalWorldX
        {
            get { return AnimalWindowX - BackgroundX; }
            set { AnimalWindowX = value + BackgroundX; }
        }
        
    };
}
