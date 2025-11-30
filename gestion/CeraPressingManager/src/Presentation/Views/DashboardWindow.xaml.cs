using CeraPressingManager.src.Presentation.ViewModels;
using CeraPressingManager.ViewModels;
using System.Windows;

namespace CeraPressingManager.Views;

public partial class DashboardWindow : Window
{
    public DashboardWindow()
    {
        InitializeComponent();
        DataContext = new DashboardViewModel();
    }
}