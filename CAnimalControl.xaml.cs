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
        CElementInfo AnimalInfo;
        Image Sprite;
        int nCurrentFrame;
        Point CurrentPos;
        int? nIndexOfPage;

        public bool isMoving;
        public int AnimalMoveSpeed;
        public int nDirection;
        public Point MoveTo;
        public CAnimalControl()
        {
            InitializeComponent();

            nIndexOfPage = null;
            nCurrentFrame = 0;
            nDirection = 1;
            isMoving = false;
            AnimalMoveSpeed = 10;
            CurrentPos = new Point();
            MoveTo = new Point();
        }

        public void Load(Canvas Carrier,int page)
        {
            nIndexOfPage = page;
            AnimalInfo = COriginalInfo.FillMapData(COriginalInfo.nDogInfo, nIndexOfPage.Value);
            CurrentPos.X = AnimalInfo.nX;
            CurrentPos.Y = AnimalInfo.nY;
            
            Sprite = new Image();
            Carrier.Children.Add(Sprite);
            Canvas.SetLeft(Sprite, AnimalInfo.nX);
            Canvas.SetTop(Sprite, AnimalInfo.nY);

            MoveTo.X = AnimalWindowX;

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
                    nCurrentFrame, nDirection, AnimalInfo.nWidth, AnimalInfo.nHeight);
                nCurrentFrame = (nCurrentFrame + 1) % AnimalInfo.nColumn;
            }
            else
            {
                Sprite.Source = new BitmapImage(
               (new Uri(@"Resources\Animal\Stand" + nDirection + ".png",
                   UriKind.Relative)));
            }

            if (Math.Abs(MoveTo.X - AnimalWindowX) > AnimalMoveSpeed)
            {
                if (MoveTo.X - AnimalWindowX > 0)
                {
                    AnimalWindowX = AnimalWindowX + AnimalMoveSpeed;
                }
                else
                {
                    AnimalWindowX = AnimalWindowX - AnimalMoveSpeed;
                }
            }
        }

        public double AnimalWindowX
        {
            get { return Canvas.GetLeft(Sprite) + AnimalInfo.nWidth/2;}
            set { Canvas.SetLeft(Sprite, value - AnimalInfo.nWidth/2);}
        }
    }
}
