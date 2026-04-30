using System.Data;
using System.Runtime.CompilerServices;
using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Enums;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Ecosystem.AccountService.Data.Repositories;

public class UserAffiliateInfoRepository : BaseRepository, IUserAffiliateInfoRepository
{
    public UserAffiliateInfoRepository(AccountServiceDbContext context) : base(context) { }

    public Task<List<UsersAffiliate>> GetAffiliatesAsync(long brandId)
        => Context.UsersAffiliates.Where(x => x.BrandId == brandId).AsNoTracking().ToListAsync();

    public Task<List<UsersAffiliate>> GetUsersWithoutAuthorization()
        => Context.UsersAffiliates.Where(x => !x.CardIdAuthorization && !string.IsNullOrEmpty(x.ImagePathId))
            .AsNoTracking().ToListAsync();

    public Task<UsersAffiliate?> GetAffiliateByIdAsync(long id, long brandId)
        => Context.UsersAffiliates.Include(x => x.CountryNavigation).FirstOrDefaultAsync(x => x.Id == id && x.BrandId == brandId);

    public Task<List<AffiliatePersonalNetwork>> GetPersonalNetwork(int userId)
    {
        return Context.Set<AffiliatePersonalNetwork>()
            .FromSqlRaw("SELECT * FROM account_service.get_personal_network({0})", userId)
            .ToListAsync();
    }

    public Task<UsersAffiliate?> GetChild(int id, byte side)
        => Context.UsersAffiliates.FirstOrDefaultAsync(x => x.BinarySponsor == id && x.Side == side);

    public Task<UsersAffiliate?> FindAffiliateByIdAsync(int id, long brandId)
        => Context.UsersAffiliates.FirstOrDefaultAsync(x => x.Id == id && x.BrandId == brandId);

    public Task<List<UsersAffiliate>> GetAffiliatesByIds(long[] ids, long brandId)
        => Context.UsersAffiliates.Where(x => ids.Contains(x.Id) && x.BrandId == brandId).ToListAsync();

    public Task<UsersAffiliate?> GetAffiliateByEmailAsync(string email, long brandId)
        => Context.UsersAffiliates.FirstOrDefaultAsync(x => x.Email == email && x.BrandId == brandId);

    public async Task<ExistenceStatus> CheckAffiliateExistenceAsync(string email, string userName, long brandId)
    {
        var normalizedEmail = email.Trim();
        var normalizedUserName = userName.Trim();

        var activeAffiliates = Context.UsersAffiliates
            .Where(a => a.BrandId == brandId && a.DeletedAt == null);

        var matchingAffiliates = await activeAffiliates
            .Where(a => EF.Functions.ILike(a.Email, normalizedEmail) || EF.Functions.ILike(a.Username, normalizedUserName))
            .ToListAsync();

        var emailExists = !string.IsNullOrEmpty(normalizedEmail) &&
                          matchingAffiliates.Any(a => normalizedEmail.Equals(a.Email, StringComparison.OrdinalIgnoreCase));

        var userNameExists = !string.IsNullOrEmpty(normalizedUserName) &&
                             matchingAffiliates.Any(a => normalizedUserName.Equals(a.Username, StringComparison.OrdinalIgnoreCase));

        if (emailExists && userNameExists) return ExistenceStatus.BothExist;
        if (emailExists) return ExistenceStatus.EmailExists;
        if (userNameExists) return ExistenceStatus.UserNameExists;

        return ExistenceStatus.None;
    }

    public async Task<bool> DeleteAffiliateAsync(UsersAffiliate affiliate)
    {
        affiliate.DeletedAt = DateTime.Now;
        Context.UsersAffiliates.Update(affiliate);
        await Context.SaveChangesAsync();
        return true;
    }

    public Task<List<Country>> GetCountries()
        => Context.Countries.AsNoTracking().ToListAsync();

    public Task<List<BinaryFamilyTree>> GetBinaryFamilyTree(int maxLevels, byte isAdmin, int id = 0)
    {
        var sql = FormattableStringFactory.Create(
            $"EXEC account_service.get_binary_family_tree @id = {{0}}, @maxLevel = {{1}}, @isAdmin = {{2}}", id, maxLevels, isAdmin);
        return Context.Set<BinaryFamilyTree>().FromSqlInterpolated(sql).ToListAsync();
    }

    public Task<List<UniLevelFamilyTree>> GetUniLevelFamilyTree(int maxLevels, byte isAdmin, int externalGradingId, int id = 0)
    {
        bool isAdminBool = isAdmin != 0;
        var sql = FormattableStringFactory.Create(
            $"SELECT * FROM account_service.get_unilevel_family_tree({id}, {maxLevels}, {isAdminBool}, {externalGradingId})");
        return Context.Set<UniLevelFamilyTree>().FromSqlInterpolated(sql).ToListAsync();
    }

    public async Task<UsersAffiliate> UpdateAffiliateAsync(UsersAffiliate affiliate)
    {
        affiliate.UpdatedAt = DateTime.Now;
        Context.UsersAffiliates.Update(affiliate);
        await Context.SaveChangesAsync();
        return affiliate;
    }

    public async Task<UsersAffiliate> UpdateImageAffiliateAsync(UsersAffiliate affiliate)
    {
        var now = DateTime.Now;
        await Context.UsersAffiliates
            .Where(x => x.Id == affiliate.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.ImageProfileUrl, affiliate.ImageProfileUrl)
                .SetProperty(x => x.UpdatedAt, now));
        affiliate.UpdatedAt = now;
        return affiliate;
    }

    public async Task UpdateBulkAffiliateAsync(List<UsersAffiliate> affiliates)
    {
        const int take = 1000;
        var packageCount = affiliates.Count / take;
        for (var i = 0; i < packageCount; i++)
        {
            var packageList = affiliates.Take(take).Skip(i).ToList();
            Context.UsersAffiliates.UpdateRange(packageList);
        }
        await Context.SaveChangesAsync();
    }

    public async Task<UsersAffiliate> CreateAffiliateAsync(UsersAffiliate affiliate)
    {
        var today = DateTime.Now;
        affiliate.UpdatedAt = today;
        affiliate.CreatedAt = today;
        await Context.UsersAffiliates.AddAsync(affiliate);
        await Context.SaveChangesAsync();
        return affiliate;
    }

    public Task<UsersAffiliate?> GetAffiliateByUserNameAsync(string userName, long brandId)
        => Context.UsersAffiliates
            .FirstOrDefaultAsync(x => x.BrandId == brandId && EF.Functions.ILike(x.Username, userName));

    public Task<UsersAffiliate?> GetAffiliateByUserNameAuthAsync(string userName, long brandId)
        => Context.UsersAffiliates.FirstOrDefaultAsync(x =>
            x.Username.ToLower() == userName.ToLower() && x.Status == 1 && x.BrandId == brandId);

    public async Task<AffiliateBinarySponsor?> GetBinarySponsor(int side, long father)
    {
        var sql = FormattableStringFactory.Create(
            $"EXEC account_service.get_binary_sponsor @fatherId = {{0}}, @side = {{1}}", father, side);
        var result = await Context.Set<AffiliateBinarySponsor>().FromSqlInterpolated(sql).ToListAsync();
        return result.FirstOrDefault();
    }

    public async Task<UsersAffiliate> UpdateImageIdPathAffiliateAsync(UsersAffiliate affiliate)
    {
        var now = DateTime.Now;
        await Context.UsersAffiliates
            .Where(x => x.Id == affiliate.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.ImagePathId, affiliate.ImagePathId)
                .SetProperty(x => x.CardIdAuthorization, affiliate.CardIdAuthorization)
                .SetProperty(x => x.CardIdMessage, affiliate.CardIdMessage)
                .SetProperty(x => x.AuthorizationDate, affiliate.AuthorizationDate)
                .SetProperty(x => x.UpdatedAt, now));
        affiliate.UpdatedAt = now;
        return affiliate;
    }

    public async Task<UsersAffiliate> UpdateVerificationCodeAffiliateAsync(UsersAffiliate affiliate)
    {
        var now = DateTime.Now;
        await Context.UsersAffiliates
            .Where(x => x.Id == affiliate.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.VerificationCode, affiliate.VerificationCode)
                .SetProperty(x => x.UpdatedAt, now));
        affiliate.UpdatedAt = now;
        return affiliate;
    }

    public async Task<int> GetTotalActiveMembers(long brandId)
    {
        return await Context.UsersAffiliates.CountAsync(x => x.Status == 1 && x.BrandId == brandId);
    }

    public Task<UsersAffiliate?> GetAffiliateByVerificationCodeAsync(string code, long brandId)
        => Context.UsersAffiliates.FirstOrDefaultAsync(x => x.VerificationCode == code && x.BrandId == brandId);

    public async Task<List<CountryNetwork>> TotalAffiliatesByCountry(long brandId)
    {
        var countryNetworks = new List<CountryNetwork>();

        await using var command = Context.Database.GetDbConnection().CreateCommand();
        command.CommandText = $"SELECT * FROM account_service.get_total_affiliates_by_country(:p_brand_id)";
        command.CommandType = CommandType.Text;

        var parameter = command.CreateParameter();
        parameter.ParameterName = "p_brand_id";
        parameter.Value = brandId;
        parameter.DbType = DbType.Int64;
        command.Parameters.Add(parameter);

        await Context.Database.OpenConnectionAsync();

        await using var result = await command.ExecuteReaderAsync();
        while (await result.ReadAsync())
        {
            var countryNetwork = new CountryNetwork
            {
                Title = result.GetString(result.GetOrdinal("country_name")),
                Value = result.GetInt64(result.GetOrdinal("total_persons")),
                Lat = result.GetDecimal(result.GetOrdinal("latitude")),
                Lng = result.GetDecimal(result.GetOrdinal("longitude")),
            };
            countryNetworks.Add(countryNetwork);
        }
        return countryNetworks;
    }

    public Task<int> GetDirectAffiliatesCount(int affiliateId)
        => Context.UsersAffiliates.CountAsync(x => x.Father == affiliateId && x.Status == 1);

    public Task<long[]> WhatUsersHave2Children(long[] affiliateIds)
    {
        return Context.UsersAffiliates
            .Where(x => affiliateIds.Contains(x.Id)
                        && Context.UsersAffiliates.Count(y => y.Father == x.Id) >= 1)
            .Select(x => x.Id)
            .ToArrayAsync();
    }

    public async Task<ICollection<UsersAffiliate>?> GetLastRegisteredUsers(long brandId)
    {
        return await Context.UsersAffiliates.Where(x => x.BrandId == brandId)
            .OrderByDescending(x => x.CreatedAt)
            .Take(8).AsNoTracking().ToListAsync();
    }

    public async Task<List<UsersAffiliate>> GetChildrenByFatherId(int fatherId, long brandId)
    {
        return await Context.UsersAffiliates
            .Where(u => u.Father == fatherId && u.BrandId == brandId && u.DeletedAt == null)
            .ToListAsync();
    }

    public Task<List<MatrixTree>> GetMatrixFamilyTreeByMatrixType(int maxLevels, byte isAdmin, int matrixType, int id = 0)
    {
        bool isAdminBool = isAdmin != 0;
        var sql = FormattableStringFactory.Create(
            $"SELECT * FROM account_service.get_matrix_family_tree_by_matrix_type({id}, {maxLevels}, {isAdminBool}, {matrixType})");
        return Context.Set<MatrixTree>().FromSqlInterpolated(sql).ToListAsync();
    }

    public async Task<int> CountQualifiedChildrenByMatrixAsync(int userId, int matrixType)
    {
        const string sql = "SELECT account_service.count_qualified_children_by_matrix(@p_user_id, @p_matrix_type)";
        var conn = Context.Database.GetDbConnection();
        try
        {
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new NpgsqlParameter("p_user_id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = userId });
            cmd.Parameters.Add(new NpgsqlParameter("p_matrix_type", NpgsqlTypes.NpgsqlDbType.Integer) { Value = matrixType });
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                await conn.CloseAsync();
        }
    }

    public async Task<bool> IsUserActiveInMatrixAsync(int userId, int matrixType, int cycle)
    {
        const string sql = "SELECT account_service.is_user_active_in_matrix(@p_user_id, @p_matrix_type, @p_cycle)";
        var conn = Context.Database.GetDbConnection();
        try
        {
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(new NpgsqlParameter("p_user_id", NpgsqlTypes.NpgsqlDbType.Integer) { Value = userId });
            cmd.Parameters.Add(new NpgsqlParameter("p_matrix_type", NpgsqlTypes.NpgsqlDbType.Integer) { Value = matrixType });
            cmd.Parameters.Add(new NpgsqlParameter("p_cycle", NpgsqlTypes.NpgsqlDbType.Integer) { Value = cycle });
            var result = await cmd.ExecuteScalarAsync();
            return Convert.ToBoolean(result);
        }
        finally
        {
            if (conn.State == ConnectionState.Open)
                await conn.CloseAsync();
        }
    }
}
