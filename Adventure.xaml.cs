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
        Point MoveTo;
        double MouseWorldX;
        #endregion
        
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Load(CurrentCarrier,COriginalInfo.nAdventureMapInfo);
            BaseCarrier.MouseLeftButtonDown += new MouseButtonEventHandler(this.Carrier_MouseLeftButtonDown);
            
            Animal = new CAnimalControl();
            Animal.Load(CurrentCarrier,COriginalInfo.nAdventureMapInfo);

            MouseWorldX = AnimalWorldX;
            
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Current_Tick);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(40);
            dispatcherTimer.Start();
            
        }
       
        private void Carrier_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pMousePos = e.GetPosition(BaseCarrier);
            if (pMousePos.Y < LowestLine)
                return;
            MoveTo = pMousePos;
            MouseWorldX = MoveTo.X - BackgroundX;
            if (Math.Abs(AnimalWorldX - MouseWorldX) > Animal.AnimalMoveSpeed)
            {
                Animal.isMoving = true;
                if (MoveTo.X > 500 && BackgroundX > -1200)
                    Animal.MoveTo.X = 500;
                else if (MoveTo.X < 300 && BackgroundX < 0)
                    Animal.MoveTo.X = 300;
                else
                    Animal.MoveTo.X = MoveTo.X;
            }
        }

        private void Current_Tick(object sender, EventArgs e)
        {
            if (Math.Abs(AnimalWorldX - MouseWorldX) > Animal.AnimalMoveSpeed)
            {
                Animal.isMoving = true;
                if (MoveTo.X <= Animal.AnimalWindowX)
                    Animal.nDirection = 0;
                else
                    Animal.nDirection = 1;

                if (AnimalWorldX - MouseWorldX < 0)
                {
                    if (Math.Abs(Animal.AnimalWindowX - 500) <= Animal.AnimalMoveSpeed 
                        && BackgroundX > -1200)
                    {
                        MoveBackgroundX = BackgroundMoveSpeed;
                    }
                }
                else
                {
                    if (Math.Abs(Animal.AnimalWindowX - 300) <= Animal.AnimalMoveSpeed 
                        && BackgroundX < 0)
                    {
                        MoveBackgroundX = -BackgroundMoveSpeed;
                    }
                }
            }
            else
            {
                Animal.isMoving = false;
                if (isClickedSomeElement)
                {
                    if (Math.Abs(Animal.AnimalWindowX -
                        (Sprites[nClickedElement].pos.X +
                        Sprites[nClickedElement].Sprite.Source.Width / 2)) < 2 * Animal.AnimalMoveSpeed)
                    {
                        if (this.NavigationService == null)
                        {
                            return;
                        }
                        if (nClickedElement != 1)
                        {
                            this.NavigationService.Navigate(new Uri("Chaining.xaml", UriKind.Relative));
                        }
                        else
                        {
                            this.NavigationService.Navigate(new Uri("GameMain.xaml", UriKind.Relative));
                        }
                    }
                }
                isClickedSomeElement = false;
            }
        }
        
        protected double AnimalWorldX
        {
            get { return Animal.AnimalWindowX - BackgroundX; }
            set { Animal.AnimalWindowX = value + BackgroundX; }
        }
        
    };
}
