using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TableTop2D
{
    /// <summary> Логика взаимодействия для ColorSelection.xaml  </summary>
    public partial class ColorSelection : Window
    {
        public Action? Done;
        private bool _MouseIsDown = false;
        private Ellipse _PointCurrentColor;
        public Brush? BrushColor { get; private set; }

        public ColorSelection()
        {
            InitializeComponent();
            _PointCurrentColor = new Ellipse()
            {
                Fill = new SolidColorBrush() { Color = Color.FromArgb(100, 0, 0, 0) },
                Width = 8,
                Height = 8,
            };

            var canvasPoint = new Canvas() { Cursor = Cursors.None };

            canvasPoint.Children.Add(_PointCurrentColor);
            ColorGradient.Children.Add(canvasPoint);

            ColorGradient.MouseLeftButtonDown += ColorGradient_MouseLeftButtonDown;
            ColorGradient.MouseMove += ColorGradient_MouseMove;
            ColorGradient.MouseLeftButtonUp += ColorGradient_MouseLeftButtonUp;
        }

        #region Mouse Events
        private void ColorGradient_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _MouseIsDown = true;

            SetPointPos();
        }

        private void ColorGradient_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_MouseIsDown || !CanSetPointPos()) return;

            SetPointPos();
        }

        private void ColorGradient_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            => _MouseIsDown = false;

        private void SetPointPos()
        {
            var pos = Mouse.GetPosition(ColorGradient);

            Canvas.SetLeft(_PointCurrentColor, pos.X - _PointCurrentColor.Width / 2);
            Canvas.SetTop(_PointCurrentColor, pos.Y - _PointCurrentColor.Height / 2);
        }

        private bool CanSetPointPos()
        {
            return Mouse.GetPosition(ColorGradient).X >= 0
                && Mouse.GetPosition(ColorGradient).Y >= 0
                && Mouse.GetPosition(ColorGradient).X <= ColorGradient.Width
                && Mouse.GetPosition(ColorGradient).Y <= ColorGradient.Height;
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BrushColor = ViewColor.Background;
                Done?.Invoke();
                Close();
            }
            catch
            {
                MessageBox.Show("То, что вы ввели, не является цветом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}