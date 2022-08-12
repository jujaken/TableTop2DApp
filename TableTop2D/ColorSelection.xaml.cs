using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TableTop2D
{
    /// <summary> Логика взаимодействия для ColorSelection.xaml  </summary>
    public partial class ColorSelection : Window
    {
        public Action? Done;
        public Brush? BrushColor { get; private set; }
        public ColorSelection()
        {
            InitializeComponent();
        }

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