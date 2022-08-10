using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TableTop2D.Core.Base.Interfaces;

namespace TableTop2D.Core.WorkTable
{
    internal class FigureCreator
    {
        private readonly Canvas _FigureCanvas;

        private bool _isDown;
        private bool _isDragging;

        private double _OriginalLeft;
        private double _OriginalTop;
        private Point _StartPoint;

        private FigureAdorner? _OverlayElement;
        private UIElement? _OriginalElement;
        private IFigure _Figure;
        private ProjectTable _WorkTable;
        private readonly double _Width;
        private readonly double _Height;

        public event Action? FigureDeleted;
        public event Action? FigureMoving;

        public FigureCreator(ref ProjectTable workTable, IFigure figure, Image? image = null)
        {
            _WorkTable = workTable;

            _FigureCanvas = new Canvas();
            _Figure = figure;

            var color = _Figure.Color;

            var cellWidth = workTable.TableCanvas.Width / workTable.Size.Width;
            var cellHeight = workTable.TableCanvas.Height / workTable.Size.Height;

            (_Width, _Height) = _Figure.Size switch
            {
                IFigure.SizeFigure.Point => (cellWidth / 5, cellHeight / 5),
                IFigure.SizeFigure.Little => (cellWidth / 2, cellHeight / 2),
                IFigure.SizeFigure.Middle => (cellWidth - cellWidth / 4, cellHeight - cellHeight / 4),
                IFigure.SizeFigure.Big => (cellWidth, cellHeight),
                _ => throw new NotImplementedException()
            };

            var pos = _WorkTable.GetPointFigureInCell(workTable.CursorPosition, _Width, _Height);

            switch (_Figure.Name)
            {
                #region Ellipse

                case "Ellipse":
                    var ellipse = new Ellipse()
                    {
                        Width = _Width,
                        Height = _Height,
                        Fill = image == null ? color : new ImageBrush(image.Source),
                    };
                    Canvas.SetLeft(ellipse, pos.X);
                    Canvas.SetTop(ellipse, pos.Y);

                    _FigureCanvas.Children.Add(ellipse);
                    break;

                #endregion

                #region Polygon

                case "Polygon":
                    var polygon = new Polygon()
                    {
                        Width = _Width,
                        Height = _Height,
                        Fill = image == null ? color : new ImageBrush(image.Source),
                    };
                    polygon.Points.Add(new Point(0, polygon.Height));
                    polygon.Points.Add(new Point(polygon.Width, polygon.Height));
                    polygon.Points.Add(new Point(polygon.Width / 2, 0));
                    Canvas.SetLeft(polygon, pos.X);
                    Canvas.SetTop(polygon, pos.Y);
                    _FigureCanvas.Children.Add(polygon);
                    break;

                #endregion

                #region Rectangle

                case "Rectangle":
                    var rectangle = new Rectangle()
                    {
                        Width = _Width,
                        Height = _Height,
                        Fill = image == null ? color : new ImageBrush(image.Source),
                    };
                    Canvas.SetLeft(rectangle, pos.X);
                    Canvas.SetTop(rectangle, pos.Y);
                    _FigureCanvas.Children.Add(rectangle);
                    break;

                    #endregion
            }

            _FigureCanvas.PreviewMouseLeftButtonDown += PreviewMouseLeftButtonDown;
            _FigureCanvas.PreviewMouseMove += PreviewMouseMove;
            _FigureCanvas.PreviewMouseLeftButtonUp += PreviewMouseLeftButtonUp;

            var contextMenu = new ContextMenu();
            _FigureCanvas.ContextMenu = contextMenu;

            var deleteFigureItem = new MenuItem() { Header = "Удалить фигуру" };
            deleteFigureItem.Click += DeleteFigure;

            contextMenu.Items.Add(deleteFigureItem);

            workTable.TableCanvas.Children.Add(_FigureCanvas);
        }

        private void DeleteFigure(object sender, RoutedEventArgs e)
        {
            FigureDeleted?.Invoke();
            DeleteFigure();
        }

        public void DeleteFigure() =>_FigureCanvas.Children.Clear();

        #region Previews
        private void PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDown = true;
            _StartPoint = e.GetPosition(_FigureCanvas);
            _OriginalElement = e.Source as UIElement;
            _FigureCanvas.CaptureMouse();
            e.Handled = true;
        }

        private void PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDown) return;

            var xIsCorrect = Math.Abs(e.GetPosition(_FigureCanvas).X - _StartPoint.X) > SystemParameters.MinimumHorizontalDragDistance;
            var yIsCorrect = Math.Abs(e.GetPosition(_FigureCanvas).Y - _StartPoint.Y) > SystemParameters.MinimumVerticalDragDistance;

            if (!_isDragging && (xIsCorrect || yIsCorrect))
                DragStarted();

            if (_isDragging)
                DragMoved();
        }

        private void PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDown)
            {
                DragFinished(false);
                e.Handled = true;
            }
        }

        #endregion

        #region Drags

        private void DragStarted()
        {
            FigureMoving?.Invoke();

            _isDragging = true;
            _OriginalLeft = Canvas.GetLeft(_OriginalElement);
            _OriginalTop = Canvas.GetTop(_OriginalElement);

            _OverlayElement = new FigureAdorner(_OriginalElement ?? throw new ArgumentNullException());
            var layer = AdornerLayer.GetAdornerLayer(_OriginalElement);
            layer.Add(_OverlayElement);
        }

        private void DragMoved()
        {
            FigureMoving?.Invoke();

            if (_OverlayElement == null) return;

            var currentPosition = Mouse.GetPosition(_FigureCanvas);

            _OverlayElement.LeftOffset = currentPosition.X - _StartPoint.X;
            _OverlayElement.TopOffset = currentPosition.Y - _StartPoint.Y;
        }

        private void DragFinished(bool cancelled)
        {
            FigureMoving?.Invoke();

            Mouse.Capture(null);
            if (_isDragging && _OverlayElement != null)
            {
                AdornerLayer.GetAdornerLayer(_OverlayElement.AdornedElement).Remove(_OverlayElement);

                if (cancelled == false)
                {
                    try
                    {
                        var pos = _WorkTable.GetPointFigureInCell(Mouse.GetPosition(_FigureCanvas), _Width, _Height);
                        Canvas.SetLeft(_OriginalElement, pos.X);
                        Canvas.SetTop(_OriginalElement, pos.Y);
                    }
                    catch
                    {
                        Canvas.SetLeft(_OriginalElement, _OriginalLeft + _OverlayElement.LeftOffset);
                        Canvas.SetTop(_OriginalElement, _OriginalTop + _OverlayElement.TopOffset);
                    }
                }
                _OverlayElement = null;
            }
            _isDragging = false;
            _isDown = false;
        }

        #endregion
    }
}