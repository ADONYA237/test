using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CeraPressingManager.Core.Entities;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace CeraPressingManager.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty] private string login = "";
    [ObservableProperty] private RoleUtiliseur? selectedRole;

    public ObservableCollection<RoleUtiliseur> Roles { get; } = new()
    {
        new RoleUtiliseur { Id = 1, Nom = "Administrateur" },
        new RoleUtiliseur { Id = 2, Nom = "Employé" }
    };

    private readonly Window _window;

    public LoginViewModel(Window window)
    {
        _window = window;
        SelectedRole = Roles[0];
    }

    [RelayCommand]
    private void Connect(object parameter)
    {
        if (parameter is PasswordBox pb && !string.IsNullOrEmpty(pb.Password))
        {
            // Authentification simple (à sécuriser plus tard)
            CurrentUser.Role = SelectedRole!;
            CurrentUser.Nom = Login;

            _window.DialogResult = true;
            _window.Close();
        }
        else
        {
            MessageBox.Show("Mot de passe requis !");
        }
    }
}