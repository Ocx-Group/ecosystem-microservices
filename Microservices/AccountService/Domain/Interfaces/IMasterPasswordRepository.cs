using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IMasterPasswordRepository
{
    Task<MasterPassword?> GetMasterPasswordByBrandId(int brandId);
}
