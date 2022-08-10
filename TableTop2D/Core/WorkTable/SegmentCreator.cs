using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TableTop2D.Core.Base.Interfaces;

namespace TableTop2D.Core.WorkTable
{
    internal class SegmentCreator
    {
        private Canvas _SegmentCanvas;
        private ProjectTable _WorkTable;
        
        #region Components of the Segment

        private FigureCreator _SegmentOrigin;
        private Line _SegmentLine;
        private FigureCreator _SegmentEnd;
        
        #endregion

        public SegmentCreator(ref ProjectTable workTable, Brush color)
        {
            _WorkTable = workTable;

            _SegmentCanvas = new Canvas();

            var cellWidth = _WorkTable.TableCanvas.Width / _WorkTable.Size.Width;
            var cellHeight = _WorkTable.TableCanvas.Height / _WorkTable.Size.Height;

            var pos = _WorkTable.GetPointFigureInCell(_WorkTable.CursorPosition, 0, 0);

            #region _SegmentLine

            _SegmentLine = new Line()
            {
                Stroke = color,
                StrokeThickness = cellWidth < cellHeight ? cellHeight / 15 : cellWidth / 15,
            };

            Canvas.SetLeft(_SegmentLine, pos.X);
            Canvas.SetTop(_SegmentLine, pos.Y);
            _SegmentCanvas.Children.Add(_SegmentLine);

            #endregion

            var pointFigure = new Figures.Ellipse(Brushes.DarkRed, IFigure.SizeFigure.Point);
            _SegmentOrigin = new FigureCreator(ref workTable, pointFigure);
            _SegmentEnd = new FigureCreator(ref workTable, pointFigure);

            _SegmentOrigin.FigureMoving += LinePrintOrigin;
            _SegmentEnd.FigureMoving += LinePrintEnd;

            _SegmentOrigin.FigureDeleted += SegmentDeleted;
            _SegmentEnd.FigureDeleted += SegmentDeleted;

            workTable.TableCanvas.Children.Add(_SegmentCanvas);
        }

        private void LinePrintOrigin()
        {
            var pos = _WorkTable.GetPointFigureInCell(_WorkTable.CursorPosition, 0, 0);
            (_SegmentLine.X1, _SegmentLine.Y1) = (pos.X, pos.Y);
        }

        private void LinePrintEnd()
        {
            var pos = _WorkTable.GetPointFigureInCell(_WorkTable.CursorPosition, 0, 0);
            (_SegmentLine.X2, _SegmentLine.Y2) = (pos.X, pos.Y);
        }

        private void SegmentDeleted()
        {
            _SegmentOrigin.DeleteFigure();
            _SegmentEnd.DeleteFigure();
            _SegmentCanvas.Children.Clear();
        }
    }
}