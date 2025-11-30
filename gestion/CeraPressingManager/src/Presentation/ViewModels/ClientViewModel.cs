using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CeraPressingManager.Core.Entities;
using CeraPressingManager.Core.Interfaces;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace CeraPressingManager.Presentation.ViewModels;

public partial class ClientViewModel : ObservableObject
{
    private readonly IClientService _clientService;

    [ObservableProperty] private ObservableCollection<Client> clients = new();
    [ObservableProperty] private Client selectedClient = new();
    [ObservableProperty] private string recherche = string.Empty;

    public IReadOnlyList<(string Code, string Pays)> Indicatifs { get; } =
        CeraPressingManager.Core.Common.Indicatifs.Liste;

    public ClientViewModel(IClientService clientService)
    {
        _clientService = clientService;
        ChargerClientsCommand.Execute(null);
    }

    partial void OnRechercheChanged(string value) => ChargerClientsCommand.Execute(null);

    [RelayCommand]
    private async Task ChargerClients()
    {
        var result = string.IsNullOrWhiteSpace(Recherche)
            ? await _clientService.GetAllAsync()
            : await _clientService.SearchAsync(Recherche);

        Clients = new ObservableCollection<Client>(result);
    }

    [RelayCommand]
    private async Task Enregistrer()
    {
        if (string.IsNullOrWhiteSpace(SelectedClient.NomComplet))
        {
            MessageBox.Show("Le nom est obligatoire.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!string.IsNullOrWhiteSpace(SelectedClient.NumeroTelephone))
        {
            bool valide = SelectedClient.IndicatifTelephone switch
            {
                "+237" => Regex.IsMatch(SelectedClient.NumeroTelephone, @"^[6-9]\d{8}$"),
                "+33" => Regex.IsMatch(SelectedClient.NumeroTelephone, @"^[1-9]\d{8}$"),
                _ => SelectedClient.NumeroTelephone.Length >= 8 && SelectedClient.NumeroTelephone.All(char.IsDigit)
            };

            if (!valide)
            {
                MessageBox.Show("Numéro invalide pour cet indicatif.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        if (SelectedClient.Id == 0)
            await _clientService.AddAsync(SelectedClient);
        else
            await _clientService.UpdateAsync(SelectedClient);

        SelectedClient = new Client { IndicatifTelephone = "+237" };
        await ChargerClients();
    }

    [RelayCommand]
    private async Task Supprimer()
    {
        if (SelectedClient.Id == 0) return;

        var res = MessageBox.Show(
            $"Supprimer {SelectedClient.NomComplet} ?", "Confirmation",
            MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (res == MessageBoxResult.Yes)
        {
            await _clientService.DeleteAsync(SelectedClient.Id);
            await ChargerClients();
        }
    }

    [RelayCommand]
    private void Nouveau() => SelectedClient = new Client { IndicatifTelephone = "+237" };
}