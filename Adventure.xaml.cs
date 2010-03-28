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
        CAnimalControl Animal;
        int LowestLine = 400; //鼠标最低有效范围线
        Point MouseWindowX;
        double MouseWorldX;

        static double? LastAnimalPosX = null;
        static double? LastBackgroundX = null;
        static int? LastDirection = null;
        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Load(CurrentCarrier, COriginalInfo.nAdventureMapInfo);
            BaseCarrier.MouseLeftButtonDown +=
                new MouseButtonEventHandler(this.Carrier_MouseLeftButtonDown);
            if (LastBackgroundX.HasValue)
                MoveBackgroundX = BackgroundX - LastBackgroundX.Value;

            Animal = new CAnimalControl();
            Animal.Load(CurrentCarrier, COriginalInfo.nAdventureMapInfo);
            if (LastAnimalPosX.HasValue)
                Animal.WindowX = LastAnimalPosX.Value;
            if (LastDirection.HasValue)
                Animal.nDirection = LastDirection.Value;
            MouseWorldX = AnimalWorldX;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Current_Tick);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(40);
            dispatcherTimer.Start();
        }

        private void Carrier_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pMousePos = e.GetPosition(BaseCarrier);
            if (pMousePos.Y <= LowestLine)
                return;
            MouseWindowX = pMousePos;
            MouseWorldX = MouseWindowX.X - BackgroundX;
        }

        private void Current_Tick(object sender, EventArgs e)
        {
            /*
            textBlock1.Text = "BackgroundX:" + BackgroundX.ToString() + " "
                           + "MouseWindowX:" + MouseWindowX.X.ToString()
                           + " AnimalWindow: " + Animal.WindowX
                           + " AnimalWorldX: " + AnimalWorldX.ToString()
                           + " MouseWorldX: " + MouseWorldX.ToString();
             * */
            if (Math.Abs(AnimalWorldX - MouseWorldX) > Animal.MoveUnit)
            {
                Animal.isMoving = true;
                if (MouseWindowX.X <= Animal.WindowX)
                    Animal.nDirection = 0;
                else
                    Animal.nDirection = 1;

                if (AnimalWorldX - MouseWorldX < 0)
                {
                    if (BackgroundX > -1200)
                    {
                        if (Animal.WindowX < 500)
                        {
                            Animal.Move = Animal.MoveUnit;
                        }
                        else
                        {
                            MoveBackgroundX = BackgroundMoveSpeed;
                        }
                    }
                    else
                    {
                        Animal.Move = Animal.MoveUnit;
                    }
                }
                else
                {
                    if (BackgroundX < 0)
                    {
                        if (Animal.WindowX > 300)
                        {
                            Animal.Move = -Animal.MoveUnit;
                        }
                        else
                        {
                            MoveBackgroundX = -BackgroundMoveSpeed;
                        }
                    }
                    else
                    {
                        Animal.Move = -Animal.MoveUnit;
                    }
                }
            }
            else
            {
                Animal.isMoving = false;
                if (isClickedSomeElement)
                {
                    if (Math.Abs(Animal.WindowX -
                        (Sprites[nClickedElement].pos.X +
                        Sprites[nClickedElement].Sprite.Source.Width / 2)) < 2 * Animal.MoveUnit)
                    {
                        if (this.NavigationService == null)
                        {
                            return;
                        }
                        if (nClickedElement != 1)
                        {
                            LastAnimalPosX = Animal.WindowX;
                            LastBackgroundX = BackgroundX;
                            LastDirection = Animal.nDirection;
                            this.NavigationService.Navigate(new Uri("Chaining.xaml", UriKind.Relative));
                        }
                        else
                        {
                            LastAnimalPosX = null;
                            LastBackgroundX = null;
                            LastDirection = null;
                            this.NavigationService.Navigate(new Uri("GameMain.xaml", UriKind.Relative));
                        }
                    }
                }
                isClickedSomeElement = false;
            }
        }

        protected double AnimalWorldX
        {
            get { return Animal.WindowX - BackgroundX; }
            set { Animal.WindowX = value + BackgroundX; }
        }

    };
}
