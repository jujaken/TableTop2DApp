using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TableTop2D.Core
{
    internal class PickColor
    {
        public Bitmap ImageElement { get; private set; }

        private int _Width;
        private int _Height;

        public PickColor(UIElement element)
        {
            var rect = new Rect(element.RenderSize);
            var visual = new DrawingVisual();

            using (var dc = visual.RenderOpen())
            {
                dc.DrawRectangle(new VisualBrush(element), null, rect);
            }

            _Width = (int)rect.Width;
            _Height = (int)rect.Height;

            var bmp = new RenderTargetBitmap(_Width, _Height, 96, 96, PixelFormats.Default);
            bmp.Render(visual);

            var stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(stream);

            ImageElement = new Bitmap(stream);
        }

        public System.Windows.Media.Color GetPixelColor(int x, int y)
        {
            var color = ImageElement.GetPixel(x, y);
            return new System.Windows.Media.Color() { A = color.A, R = color.R, G = color.G, B = color.B};
        }
    }
}