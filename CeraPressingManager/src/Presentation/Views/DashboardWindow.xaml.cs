using System.Windows.Controls;
using CeraPressingManager.ViewModels;
using System.Windows;

namespace CeraPressingManager.Views;

public partial class DashboardWindow : UserControl
{
    public DashboardWindow(DashboardViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}