using System.Windows.Controls;
using CeraPressingManager.ViewModels;

namespace CeraPressingManager.Views;

public partial class NouvelleCommandeWindow : UserControl
{
    public NouvelleCommandeWindow(NouvelleCommandeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}