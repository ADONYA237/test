using CeraPressingManager.Core.Common;
using CeraPressingManager.Core.Entities;
using CeraPressingManager.Core.Interfaces;

namespace CeraPressingManager.Services;

public class ClientService : IClientService
{
    private readonly IRepository<Client> _repo;

    public ClientService(IRepository<Client> repo) => _repo = repo;

    public async Task<List<Client>> GetAllAsync()
        => (await _repo.GetAllAsync()).ToList();

    public async Task<Client?> GetByIdAsync(int id)
        => await _repo.GetByIdAsync(id);

    public async Task<Client?> GetByTelephoneAsync(string telephone)
        => (await _repo.GetAllAsync())
           .FirstOrDefault(c => c.NumeroTelephone == telephone ||
                        (c.IndicatifTelephone + c.NumeroTelephone) == telephone);

    public async Task<Client> AddAsync(Client client)
    {
        client.DateInscription = DateTime.Today;
        client.Categorie = CategorieClient.Occasionnel;
        return await _repo.AddAsync(client);
    }

    public async Task UpdateAsync(Client client)
    {
        // Mise à jour automatique de la catégorie
        if (client.PointsFidelite >= 500)
            client.Categorie = CategorieClient.VIP;
        else if (client.PointsFidelite >= 200)
            client.Categorie = CategorieClient.Regulier;

        await _repo.UpdateAsync(client);
    }

    public async Task DeleteAsync(int id)
    {
        var client = await GetByIdAsync(id);
        if (client != null)
            await _repo.DeleteAsync(client);
    }

    public async Task<List<Client>> SearchAsync(string search)
    {
        var all = (await _repo.GetAllAsync()).ToList();
        if (string.IsNullOrWhiteSpace(search)) return all;

        search = search.ToLower();
        return all.Where(c =>
            c.NomComplet.ToLower().Contains(search) ||
    (c.NumeroTelephone?.Contains(search) ?? false) ||
    (c.Email?.ToLower().Contains(search) ?? false)
).ToList();
    }

    // Appelé depuis le module Commande quand une commande est validée
    public async Task IncrementerPointsEtCommandes(int clientId)
    {
        var client = await GetByIdAsync(clientId);
        if (client == null) return;

        client.NombreCommandes++;
        client.PointsFidelite += 10; // 10 points par commande

        if (client.PointsFidelite >= 500)
            client.Categorie = CategorieClient.VIP;
        else if (client.PointsFidelite >= 200)
            client.Categorie = CategorieClient.Regulier;

        await _repo.UpdateAsync(client);
    }
}