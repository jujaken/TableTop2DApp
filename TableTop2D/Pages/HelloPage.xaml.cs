using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TableTop2D.Pages
{
    /// <summary> Логика взаимодействия для HelloPage.xaml </summary>
    public partial class HelloPage : Page
    {
        public HelloPage()
        {
            InitializeComponent();
        }

        public HelloPage(Page MainWindow) : this()
        {
            
        }

        private void CreateNewProject(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
