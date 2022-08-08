using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TableTop2D
{
    /// <summary> Логика взаимодействия для ColorSelection.xaml  </summary>
    public partial class ColorSelection : Window
    {
        public Brush? BrushColor { get; private set; }
        public ColorSelection()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BrushColor = new SolidColorBrush()
                {
                    Color = Color.FromArgb(Convert.ToByte(TextBlockA.Text), Convert.ToByte(TextBlockR.Text), Convert.ToByte(TextBlockG.Text), Convert.ToByte(TextBlockB.Text))
                };
                Close();
            }
            catch
            {
                MessageBox.Show("То, что вы ввели, не является цветом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}