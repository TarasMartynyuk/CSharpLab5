using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CSharpLab5.LogicClasses;
using CSharpLab5.ViewModels;
using CSharpLab5.Views;

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
