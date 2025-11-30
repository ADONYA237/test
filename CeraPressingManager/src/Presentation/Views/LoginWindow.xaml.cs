using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CeraPressingManager.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow(ViewModels.LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.SetWindow(this); // Ajout d'une méthode pour passer la fenêtre au ViewModel
        }
    }
}
