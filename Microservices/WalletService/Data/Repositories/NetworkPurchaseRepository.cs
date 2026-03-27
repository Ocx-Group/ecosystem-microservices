using Microsoft.EntityFrameworkCore;
using Npgsql;
using NpgsqlTypes;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Constants;

namespace Ecosystem.WalletService.Data.Repositories;

public class NetworkPurchaseRepository : BaseRepository, INetworkPurchaseRepository
{
    public NetworkPurchaseRepository(WalletServiceDbContext context) : base(context) { }

    public Task<List<NetworkPurchase>> GetNetworkPurchasesEcoPool(DateTime from, DateTime to)
        => Context.NetworkPurchases
            .Where(x => x.CreatedAt >= from && x.CreatedAt <= to
                                            && x.Origin == Constants.OriginEcoPoolPurchase).ToListAsync();
    
    public async Task<List<(int Year, int Month, int TotalPurchases)>> GetPurchasesMadeInMyNetwork(HashSet<int> ids)
    {
        if (ids.Count == 0)
            return new List<(int, int, int)>();

        var sql = $"SELECT * FROM get_total_purchases_in_my_network_by_affiliate(@p0)";
        
        var parameter = new NpgsqlParameter("@p0", NpgsqlDbType.Array | NpgsqlDbType.Integer)
        {
            Value = ids.ToArray()
        };

        var results = await Context.PurchasesPerMonth
            .FromSqlRaw(sql, parameter)
            .ToListAsync();

        return results.Select(r => (r.Year, r.Month, r.TotalPurchases)).ToList();
    }
}