using Presentation.Concrete;
using System.Windows;
using View.Concrete;
using ViewModel;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new FileWriterViewModel(new ViewService(this), new PathProvider());
        }
    }
}