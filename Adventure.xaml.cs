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
    public partial class Adventure : Page
    {
        #region variables
        int count = 0;
        CLoadElement[] BackgroundElements;
        Image Animal;
        int nIndexOfBox = 3;
        bool isMoving = false;
        bool isClickedBox = false;
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
            #region Background
            BackgroundElements = new CLoadElement[(int)CElementInformation.eBackground.ELEMENT_NUM];
            for (int i = 0; i < (int)CElementInformation.eBackground.ELEMENT_NUM; i++)
            {
                BackgroundElements[i] = new CLoadElement();
                BackgroundElements[i].LoadElement(Carrier, i);
            }
            #endregion

            #region Foreground
            // 加载宠物
            Animal = new Image();
            Carrier.Children.Add(Animal);
            AnimalWindowX = 300;
            MouseWorldX = 300;
            Canvas.SetTop(Animal, 450);
            #endregion

            for (int i = 2; i < (int)CElementInformation.eBackground.ELEMENT_NUM; i++ )
                this.BackgroundElements[i].Sprite.MouseLeftButtonDown += BOX_MouseLeftButtonDown;
            this.BackgroundElements[1].Sprite.MouseLeftButtonDown += btnBack_Click;
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
                Animal.Source = cutImage(@"Resources\Animal\Doggy.png", count * 64, 0, 64, 64);
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
            Point pMousePos = e.GetPosition(Carrier);
            if (pMousePos.Y < LowestLine)
                return;
            MoveTo = pMousePos;
            MouseWorldX = MoveTo.X - BackgroundX;
        }

        private void Move()
        {
            this.textBlock1.Text = "Animal:" + AnimalWindowX.ToString();
            this.textBlock2.Text = "Box:" + BackgroundElements[nIndexOfBox].pos.X.ToString();

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
                        foreach (CLoadElement i in BackgroundElements)
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
                        foreach (CLoadElement i in BackgroundElements)
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
                        (BackgroundElements[nIndexOfBox].pos.X + 
                        BackgroundElements[nIndexOfBox].Sprite.Source.Width / 2)) < 2 * AnimalMoveSpeed)
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

        /// <summary>
        /// 截取图片
        /// </summary>
        /// <param name="imgaddress">文件名(包括地址+扩展名)</param>
        /// <param name="x">左上角点X</param>
        /// <param name="y">左上角点Y</param>
        /// <param name="width">截取的图片宽</param>
        /// <param name="height">截取的图片高</param>
        /// <returns>截取后图片数据源</returns>
        private BitmapSource cutImage(string imgaddress, int x, int y, int width, int height)
        {
            return new CroppedBitmap(
                BitmapFrame.Create(new Uri(imgaddress, UriKind.Relative)),
                new Int32Rect(x, y + 64 * (int)dir, width, height)
                 );
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
            for (int i = 2; i < (int)CElementInformation.eBackground.ELEMENT_NUM; i++)
            {
                if (sender.GetHashCode() == BackgroundElements[i].Sprite.GetHashCode())
                {
                    nIndexOfBox = i;
                    break;
                }
            }
        }

        private double AnimalWindowX
        {
            get { return Canvas.GetLeft(Animal) + 32; }
            set { Canvas.SetLeft(Animal, value - 32); }
        }

        private double BackgroundX
        {
            get { return Canvas.GetLeft(BackgroundElements[(int)CElementInformation.eBackground.MAP].Sprite); }
            set { Canvas.SetLeft(BackgroundElements[(int)CElementInformation.eBackground.MAP].Sprite, value); }
        }

        private double AnimalWorldX
        {
            get { return AnimalWindowX - BackgroundX; }
            set { AnimalWindowX = value + BackgroundX; }
        }
    };


    public class CLoadElement
    {
        public Image Sprite;

        public void LoadElement(Canvas carrier, int i)
        {
            Sprite = new Image();
            Sprite.Source = new BitmapImage(
                (new Uri(@"Resources\Background\" + CElementInformation.sBackground[i] + ".png",
                    UriKind.Relative)));
            carrier.Children.Add(Sprite);
            Canvas.SetLeft(Sprite, CElementInformation.nBackground[i].X);
            Canvas.SetTop(Sprite, CElementInformation.nBackground[i].Y);
            Sprite.SetValue(Canvas.ZIndexProperty, -1);
        }

        public Point pos
        {
            get { return new Point(Canvas.GetLeft(Sprite), Canvas.GetTop(Sprite)); }
        }

    };

    public class CElementInformation
    {
        public enum eBackground
        {
            MAP, ARROW, BOX1, BOX2, BOX3,
            ELEMENT_NUM //保证该元素在最后，用于返回元素个数
        }

        public static string[] sBackground = 
        {
            "Map","Arrow", "Box0", "Box0", "Box0"
        };

        public static Point[] nBackground = 
        {
            new Point(0,0),new Point(0,435), new Point(400, 450), new Point(800, 420), new Point(1400, 460)
        };
    };

}
