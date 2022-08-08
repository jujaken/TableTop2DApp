using System.Windows.Media;

namespace TableTop2D.Core.Base.Interfaces
{
    internal interface IFigure
    {
        string Name { get; }
        Brush Color { get; set; }
        SizeFigure Size { get; set; }

        enum SizeFigure : byte
        {
            Little,
            Middle,
            Big
        }
    }
}
