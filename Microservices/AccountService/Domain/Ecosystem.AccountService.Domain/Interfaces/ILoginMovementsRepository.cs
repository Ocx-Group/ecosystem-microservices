using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface ILoginMovementsRepository
{
    Task<List<LoginMovement>> GetLoginMovementsByAffiliateId(int affiliateId, long brandId);
    Task<LoginMovement> CreateAsync(LoginMovement loginMovements);
    Task<LoginMovement> UpdateAsync(LoginMovement loginMovements);
}
