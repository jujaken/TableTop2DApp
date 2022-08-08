using System.Windows.Media;
using TableTop2D.Core.Base.Interfaces;

namespace TableTop2D.Models.Figures
{
    internal class Polygon : IFigure
    {
        #region

        public string Name => "Polygon";
        public Brush Color { get; set; }
        public IFigure.SizeFigure Size { get; set; }

        public Polygon(Brush color, IFigure.SizeFigure size)
        {
            Color = color;
            Size = size;
        }

        #endregion
    }
}