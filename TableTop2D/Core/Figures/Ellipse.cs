using System.Windows.Media;
using TableTop2D.Core.Base.Interfaces;

namespace TableTop2D.Core.Figures
{
    internal class Ellipse : IFigure
    {
        #region IFigure

        public string Name => "Ellipse";
        public Brush Color { get; set; }
        public IFigure.SizeFigure Size { get; set; }

        public Ellipse(Brush color, IFigure.SizeFigure size)
        {
            Color = color;
            Size = size;
        }

        #endregion
    }
}