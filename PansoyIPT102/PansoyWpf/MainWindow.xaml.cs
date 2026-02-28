using PansoyWpf.ViewModels;
using PansoyWpf.Stores;
using System.Windows;

namespace PansoyWpf
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
