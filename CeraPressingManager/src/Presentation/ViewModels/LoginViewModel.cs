using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CeraPressingManager.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using CeraPressingManager.Core.Entities;

namespace CeraPressingManager.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty] private string login = "";
    [ObservableProperty] private string motDePasse = "";

    private readonly AuthService _authService;
    private readonly CurrentUser _currentUser;
    private readonly Window _window;

    public LoginViewModel(AuthService authService, CurrentUser currentUser)
    {
        _authService = authService;
        _currentUser = currentUser;
    }

    private Window? _window;
    public void SetWindow(Window window) => _window = window;

    [RelayCommand]
    private async Task Connect(object parameter)
    {
            if (_window == null) return; // Sécurité

            if (parameter is PasswordBox pb && !string.IsNullOrEmpty(pb.Password))
        {
            try
            {
                var employe = await _authService.LoginAsync(Login, pb.Password);

                if (employe != null)
                {
                    _window.DialogResult = true;
                    _window.Close();
                }
                else
                {
                    MessageBox.Show("Identifiants incorrects.", "Erreur de Connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue: {ex.Message}", "Erreur Système", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else
        {
            MessageBox.Show("Veuillez entrer votre mot de passe.", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}