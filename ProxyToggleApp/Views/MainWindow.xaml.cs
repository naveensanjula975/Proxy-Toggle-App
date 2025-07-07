using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ProxyToggleApp.ViewModels;

namespace ProxyToggleApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void SetViewModel(MainViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }
}
