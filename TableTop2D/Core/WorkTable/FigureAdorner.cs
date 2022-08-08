using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TableTop2D.Core.WorkTable
{
    internal class FigureAdorner : Adorner
    {
        private readonly Rectangle _ChildElement;

        #region LeftOffset

        private double _leftOffset;
        public double LeftOffset
        {
            get { return _leftOffset; }
            set
            {
                _leftOffset = value;
                UpdatePosition();
            }
        }

        #endregion

        #region TopOffset

        private double _topOffset;
        public double TopOffset
        {
            get { return _topOffset; }
            set
            {
                _topOffset = value;
                UpdatePosition();
            }
        }

        #endregion

        public FigureAdorner(UIElement adornedElement) : base(adornedElement)
        {
            var brush = new VisualBrush(adornedElement);

            _ChildElement = new Rectangle()
            {
                Width = adornedElement.RenderSize.Width,
                Height = adornedElement.RenderSize.Height,
            };

            var animation = new DoubleAnimation(0.3, 1, new Duration(TimeSpan.FromSeconds(1)))
            {
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };
            brush.BeginAnimation(Brush.OpacityProperty, animation);

            _ChildElement.Fill = brush;
        }

        private void UpdatePosition()
        {
            var adornerLayer = Parent as AdornerLayer;
            adornerLayer?.Update(AdornedElement);
        }

        #region override Adorner

        protected override int VisualChildrenCount => 1;

        protected override Size MeasureOverride(Size constraint)
        {
            _ChildElement.Measure(constraint);
            return _ChildElement.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _ChildElement.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index) => _ChildElement;

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(_leftOffset, _topOffset));
            return result;
        }

        #endregion
    }
}