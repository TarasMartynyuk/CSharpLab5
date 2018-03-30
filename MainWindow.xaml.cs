using System.Windows;
using CSharpLab5.ViewModels;

namespace CSharpLab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ProcessesGrid.DataContext = new MainWindowViewModel();
            // still awful performance
            ProcessesGrid.EnableRowVirtualization = true;
            ProcessesGrid.EnableColumnVirtualization = true;
            
        }
    }
}
