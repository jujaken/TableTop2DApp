using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TableTop2D.Core.Base.Interfaces;
using TableTop2D.Core.WorkTable;
using TableTop2D.Core.Figures;
using System.Windows.Media.Imaging;

namespace TableTop2D
{
    /// <summary> Interaction logic for MainWindow.xaml </summary>
    public partial class MainWindow : Window
    {
        #region Selected params

        private IFigure.SizeFigure _SelectedSize = IFigure.SizeFigure.Middle;
        private string _SelectedFigureName = "Ellipse";
        private Brush _SelectedColor = Brushes.Red;

        #endregion

        private ProjectTable? _ProjectTable;
        private ColorSelection? _ColorSelection;
        Button? CurrentButton;
        public MainWindow()
        {
            InitializeComponent();
        }

        #region ButtonsClick

        private void CreateNewProject(object sender, RoutedEventArgs e)
        {

            try
            {
                var height = Convert.ToByte(HelloMenuHeight.Text);
                var width = HelloMenuWidth.Text == "Высота" ? height : Convert.ToByte(HelloMenuWidth.Text);

                var canvas = new Canvas() { Width = 500, Height = 500, Background = Brushes.White };
                _ProjectTable = new ProjectTable(ref canvas, width, height);
                CenterMenu.Children.Add(canvas);
            }
            catch (FormatException)
            {
                MessageBox.Show("Высота и ширина должны быть числами!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (OverflowException)
            {
                MessageBox.Show("Максимальное число - 255!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetFigureSizeButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button ?? throw new Exception("Я не знаю как, но ты всё сломал");

            switch (button.Content)
            {
                case "Little": _SelectedSize = IFigure.SizeFigure.Little; break;
                case "Middle": _SelectedSize = IFigure.SizeFigure.Middle; break;
                case "Big": _SelectedSize = IFigure.SizeFigure.Big; break;
            }
        }

        private void SetFigureButtonClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button ?? throw new Exception("Я не знаю как, но ты всё сломал");

            switch (button.Content)
            {
                case "▲": _SelectedFigureName = "Polygon"; break;
                case "⚫": _SelectedFigureName = "Ellipse"; break;
                case "⬛️": _SelectedFigureName = "Rectangle"; break;
            }
        }

        private void SetFigureColorClick(object sender, RoutedEventArgs e)
        {
            CurrentButton = sender as Button ?? throw new Exception("Я не знаю как, но ты всё сломал");

            if (CurrentButton.Foreground == Brushes.Teal)
            {
                _ColorSelection = new ColorSelection
                {
                    Owner = this
                };

                _ColorSelection.Done += SetColor;

                _ColorSelection.Show();
            }
            else SetFigureStandartColorClick(CurrentButton, e);
        }

        private void SetColor()
        {
            if (CurrentButton == null || _ColorSelection == null) throw new Exception("Я не знаю как, но ты всё сломал");
            CurrentButton.Foreground = _ColorSelection.BrushColor;
            CurrentButton.Content = "🔥";
        }

        private void SetFigureStandartColorClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button ?? throw new Exception("Я не знаю как, но ты всё сломал");
            _SelectedColor = button.Foreground;
        }

        #endregion

        #region ContextMenuClick

        private void CreateNewFigure(object sender, RoutedEventArgs e)
        {
            var figure = GetIFigureType();

            if (_ProjectTable == null) throw new Exception();

            _ProjectTable.CreateNewFigure(ref _ProjectTable, figure);
        }

        private void CreateNewImage(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".png",
                Filter = "Картинка (.png)|*.png;*.jpg;*.jpeg;*.gif"
            };
            var result = dialog.ShowDialog();

            if (result != true) return;

            string path = dialog.FileName;

            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(path, UriKind.Absolute);
            bitmapImage.EndInit();

            var image = new Image()
            {
                Stretch = Stretch.Fill,
                Source = bitmapImage
            };

            var figure = GetIFigureType();

            if (_ProjectTable == null) throw new Exception();

            _ProjectTable.CreateNewImage(ref _ProjectTable, figure, image);
        }

        private void CreateNewSegment(object sender, RoutedEventArgs e)
        {
            var figure = GetIFigureType();

            if (_ProjectTable == null) throw new Exception();

            _ProjectTable.CreateNewSegment(ref _ProjectTable, figure);
        }

        private IFigure GetIFigureType()
        {
            return _SelectedFigureName switch
            {
                "Polygon" => new Polygon(_SelectedColor, _SelectedSize),
                "Ellipse" => new Ellipse(_SelectedColor, _SelectedSize),
                "Rectangle" => new Rectangle(_SelectedColor, _SelectedSize),
                _ => throw new Exception("План пошёл не по плану")
            };
        }

        private void ResetColor(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem ?? throw new Exception("Я не знаю как, но ты всё сломал");
            var button = ((ContextMenu)menuItem.Parent).PlacementTarget as Button;
            if (button != null)
            {
                button.Foreground = Brushes.Teal;
                button.Content = "⭕";
            }    
        }

        #endregion
    }
}