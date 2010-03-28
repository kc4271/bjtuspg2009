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
using System.Windows.Threading;

namespace Demo
{
    /// <summary>
    /// Interaction logic for CAnimalControl.xaml
    /// </summary>
    public partial class CAnimalControl : UserControl
    {
        CElementInfo Info;
        Image Sprite;
        int nCurrentFrame;
        Point CurrentPos;
        int? nIndexOfPage;

        public bool isMoving;
        public int MoveUnit;
        public int nDirection;
        public CAnimalControl()
        {
            InitializeComponent();

            nIndexOfPage = null;
            nCurrentFrame = 0;
            nDirection = 1;
            isMoving = false;
            MoveUnit = 10;
            CurrentPos = new Point();
        }

        public void Load(Canvas Carrier,int page)
        {
            nIndexOfPage = page;
            Info = COriginalInfo.FillMapData(COriginalInfo.nDogInfo, nIndexOfPage.Value);
            CurrentPos.X = Info.nX;
            CurrentPos.Y = Info.nY;
            
            Sprite = new Image();
            Carrier.Children.Add(Sprite);
            Canvas.SetLeft(Sprite, Info.nX);
            Canvas.SetTop(Sprite, Info.nY);

            DispatcherTimer AnimalTimer = new DispatcherTimer();
            AnimalTimer.Tick += new EventHandler(Animal_Tick);
            AnimalTimer.Interval = TimeSpan.FromMilliseconds(40);
            AnimalTimer.Start();
            
        }

        protected BitmapSource cutImage(string imgaddress, int nColumn, int nRow, int width, int height)
        {
            return new CroppedBitmap(
                BitmapFrame.Create(new Uri(imgaddress, UriKind.Relative)),
                new Int32Rect(nColumn * width, nRow * height, width, height)
                 );
        }

        public void Animal_Tick(object sender, EventArgs e)
        {
            if (isMoving)
            {
                Sprite.Source = cutImage(@"Resources\Animal\Doggy.png", 
                    nCurrentFrame, nDirection, Info.nWidth, Info.nHeight);
                nCurrentFrame = (nCurrentFrame + 1) % Info.nColumn;
            }
            else
            {
                Sprite.Source = new BitmapImage(
               (new Uri(@"Resources\Animal\Stand" + nDirection + ".png",
                   UriKind.Relative)));
            }
        }

        public double WindowX
        {
            get { return Canvas.GetLeft(Sprite) + Info.nWidth/2;}
            set { Canvas.SetLeft(Sprite, value - Info.nWidth/2);}
        }

        public double Move
        {
            set { WindowX = WindowX + value;}
        }
    }
}
