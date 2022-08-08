using System.Windows.Media;
using TableTop2D.Core.Base.Interfaces;

namespace TableTop2D.Models.Figures
{
    internal class Rectangle : IFigure
    {
        #region IFigure

        public string Name => "Rectangle";
        public Brush Color { get; set; }
        public IFigure.SizeFigure Size { get; set; }

        public Rectangle(Brush color, IFigure.SizeFigure size)
        {
            Color = color;
            Size = size;
        }

        #endregion
    }
}
