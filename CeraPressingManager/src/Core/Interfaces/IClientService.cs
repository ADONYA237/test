using CeraPressingManager.Core.Entities;

namespace CeraPressingManager.Core.Interfaces;

public interface IClientService
{
    Task<List<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(int id);
    Task<Client?> GetByTelephoneAsync(string telephone);
    Task<Client> AddAsync(Client client);
    Task UpdateAsync(Client client);
    Task DeleteAsync(int id);
    Task<List<Client>> SearchAsync(string search); // recherche nom ou téléphone
}