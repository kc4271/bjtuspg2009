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
using System.Collections;
namespace Demo
{
    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public class GamePage : Page
    {
        protected Canvas BaseCarrier;
        protected CElementInfo[] ElementInfos;
        protected CSprite[] Sprites;
        protected int? nPageIndex;

        public GamePage()
        {
            nPageIndex = null;
        }   

        protected void init(Canvas CurrentCarrier)
        {
            BaseCarrier = CurrentCarrier;
            this.Content = BaseCarrier;

            if (nPageIndex == null)
                throw new Exception("没有初始化页号");

            #region ReadElementInfo
            ElementInfos = new CElementInfo[COriginalInfo.GetElementNum(nPageIndex.Value)];
            for (int i = 0; i < ElementInfos.Length; i++)
            {
                ElementInfos[i] = new CElementInfo();
                ElementInfos[i] = COriginalInfo.FillMapData(nPageIndex.Value,i);
            }
            #endregion

            #region LoadBackgroundElements
            Sprites = new CSprite[COriginalInfo.GetElementNum(nPageIndex.Value)];
            for (int i = 0; i < Sprites.Length; i++)
            {
                Sprites[i] = new CSprite();
                Sprites[i].LoadElement(this, i);
            }
            #endregion

            DispatcherTimer BaseDispatcherTimer = new DispatcherTimer();
            BaseDispatcherTimer.Tick += new EventHandler(BaseDispatcherTimer_Tick);
            BaseDispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
            BaseDispatcherTimer.Start();
        }

        protected void BaseDispatcherTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Sprites.Length;i++ )
            {
                Sprites[i].Sprite.Source = cutImage(
                    ElementInfos[i].sPath,
                    Sprites[i].CurrentColumn,
                    Sprites[i].CurrentRow,
                    ElementInfos[i].nWidth,
                    ElementInfos[i].nHeight);
                
                Sprites[i].CurrentColumn = (Sprites[i].CurrentColumn + 1) % ElementInfos[i].nColumn;
            }
        }

        
        protected BitmapSource cutImage(string imgaddress, int nColumn, int nRow, int width, int height)
        {
            return new CroppedBitmap(
                BitmapFrame.Create(new Uri(imgaddress, UriKind.Relative)),
                new Int32Rect(nColumn * width, nRow * height, width, height)
                 );
        }
        
        public class CSprite
        {
            public Image Sprite;
            public int CurrentColumn;
            public int CurrentRow;

            public CSprite()
            {
                CurrentColumn = 0;
                CurrentRow = 0;
                Sprite = null;
            }

            public void LoadElement(GamePage GP,int i)
            {
                Sprite = new Image();
                GP.BaseCarrier.Children.Add(Sprite);
                Canvas.SetLeft(Sprite,GP.ElementInfos[i].nX);
                Canvas.SetTop(Sprite, GP.ElementInfos[i].nY);
                Sprite.SetValue(Canvas.ZIndexProperty, GP.ElementInfos[i].nZ);
            }

            public Point pos
            {
                get { return new Point(Canvas.GetLeft(Sprite), Canvas.GetTop(Sprite)); }
            }
        };

        protected double BackgroundX
        {
            get { return Canvas.GetLeft(Sprites[0].Sprite); }
            set { Canvas.SetLeft(Sprites[0].Sprite, value); }
        }
    };
}
