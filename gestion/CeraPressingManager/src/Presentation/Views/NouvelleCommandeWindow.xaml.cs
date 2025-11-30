using System.Windows;
using CeraPressingManager.ViewModels;

namespace CeraPressingManager.Views;

public partial class NouvelleCommandeWindow : Window
{
    public NouvelleCommandeWindow()
    {
        InitializeComponent();
        DataContext = new NouvelleCommandeViewModel(this);
    }
}