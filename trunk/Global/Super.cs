using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace Demo.Global
{
    public static class Super
    {       
        /// <summary>
        /// 将N帧图片合成一张8方向角色各动作分布图
        /// </summary>
        /// <param name="SourcePath">源文件路径</param>
        /// <param name="SavaToPath">保存文件到目标路径</param>
        /// <param name="imgnum">源图片数量</param>
        /// <param name="imgwidth">单位图片宽</param>
        /// <param name="imgheight">单位图片高</param>
        public static void ComposeImage(string SourcePath, string SavaToPath, int imgNum, int imgWidth, int imgHeight)
        {
            System.IO.FileStream fileStream = new System.IO.FileStream(SavaToPath, System.IO.FileMode.Create);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            for (int i = 1; i <= 2; i++)
            {
                for (int j = 0; j < imgNum / 2; j++)
                {
                    drawingContext.DrawImage(new BitmapImage(new Uri(SourcePath + i + "_" + (j).ToString() + ".png")), 
                        new Rect(j * imgWidth, (i-1) * imgHeight, imgWidth, imgHeight));
                }
            }
            drawingContext.Close();
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((imgNum / 2) * imgWidth, 2 * imgHeight, 0, 0, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(drawingVisual);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            encoder.Save(fileStream);
            fileStream.Close();
        }
    }
}
