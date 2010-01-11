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
        protected int BackgroundMoveSpeed;
        protected bool isClickedSomeElement;
        protected int nClickedElement;

        public GamePage()
        {
            nPageIndex = null;
            BackgroundMoveSpeed = 10;
            isClickedSomeElement = false;
        }   

        protected void Load(Canvas CurrentCarrier,int page)
        {
            BaseCarrier = CurrentCarrier;
            this.Content = BaseCarrier;
            nPageIndex = page;

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

            for (int i = 1; i < Sprites.Length; i++)
            {
                Sprites[i].Sprite.MouseLeftButtonDown += MouseClick;
            }

            DispatcherTimer BaseTimer = new DispatcherTimer();
            BaseTimer.Tick += new EventHandler(Base_Tick);
            BaseTimer.Interval = TimeSpan.FromMilliseconds(200);
            BaseTimer.Start();
        }

        protected void Base_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Sprites.Length;i++ )
            {
                if (Sprites[i].isNeedCuted)
                {
                    Sprites[i].Sprite.Source = cutImage(
                        ElementInfos[i].sPath,
                        Sprites[i].CurrentColumn,
                        Sprites[i].CurrentRow,
                        ElementInfos[i].nWidth,
                        ElementInfos[i].nHeight);
                    Sprites[i].CurrentColumn = (Sprites[i].CurrentColumn + 1) % ElementInfos[i].nColumn;
                    Sprites[i].isCutted = true;
                }
            }
        }

        private void MouseClick(object sender, MouseButtonEventArgs e)
        {
            isClickedSomeElement = true;
            for (int i = 1; i < Sprites.Length; i++)
            {
                if (sender.GetHashCode() == Sprites[i].Sprite.GetHashCode())
                {
                    nClickedElement = i;
                    break;
                }
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
            public bool isNeedCuted;
            public bool isCutted;
            public CSprite()
            {
                CurrentColumn = 0;
                CurrentRow = 0;
                Sprite = null;
                isNeedCuted = false;
            }

            public void LoadElement(GamePage GP,int i)
            {
                Sprite = new Image();
                Sprite.Source = GP.cutImage(
                        GP.ElementInfos[i].sPath,
                        GP.Sprites[i].CurrentColumn,
                        GP.Sprites[i].CurrentRow,
                        GP.ElementInfos[i].nWidth,
                        GP.ElementInfos[i].nHeight);
                GP.BaseCarrier.Children.Add(Sprite);
                Canvas.SetLeft(Sprite,GP.ElementInfos[i].nX);
                Canvas.SetTop(Sprite,GP.ElementInfos[i].nY);
                Sprite.SetValue(Canvas.ZIndexProperty, GP.ElementInfos[i].nZ);
                if (GP.ElementInfos[i].nColumn > 1 || GP.ElementInfos[i].nRow > 1)
                    isNeedCuted = true;
            }

            public Point pos
            {
                get { return new Point(Canvas.GetLeft(Sprite), Canvas.GetTop(Sprite)); }
            }
        };

        protected double BackgroundX
        {
            get { return Canvas.GetLeft(Sprites[0].Sprite);}
            set { Canvas.SetLeft(Sprites[0].Sprite, value);}
        }

        protected double MoveBackgroundX
        {
            set 
            {
                foreach (CSprite i in Sprites)
                {
                    Canvas.SetLeft(i.Sprite, i.pos.X - value);
                }
            }
        }
        
    };
}
