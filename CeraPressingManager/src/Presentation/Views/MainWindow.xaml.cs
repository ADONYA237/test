using System.Windows;

namespace CeraPressingManager.Views;

public partial class MainWindow : Window
{
    public MainWindow(ViewModels.MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}