using Ecosystem.AccountService.Domain.DTOs.PaginationDto;
using Ecosystem.AccountService.Domain.Enums;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using Ecosystem.AccountService.Domain.Requests.PaginationRequest;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IUserAffiliateInfoRepository
{
    Task<List<UsersAffiliate>> GetAffiliatesAsync(long brandId);
    Task<PaginationDto<UsersAffiliate>> GetAffiliatesPagedAsync(long brandId, PaginationRequest request);
    Task<List<UsersAffiliate>> GetUsersWithoutAuthorization();
    Task<UsersAffiliate?> GetAffiliateByIdAsync(long id, long brandId);
    Task<List<AffiliatePersonalNetwork>> GetPersonalNetwork(int userId);
    Task<AffiliateBinarySponsor?> GetBinarySponsor(int side, long father);
    Task<UsersAffiliate?> GetAffiliateByUserNameAsync(string userName, long brandId);
    Task<UsersAffiliate?> GetAffiliateByUserNameAuthAsync(string userName, long brandId);
    Task<UsersAffiliate?> GetAffiliateByGoogleAuthCodeAsync(string googleAuthCode, long brandId);
    Task<List<UsersAffiliate>> GetAffiliatesByIds(long[] ids, long brandId);
    Task<UsersAffiliate?> GetAffiliateByEmailAsync(string email, long brandId);
    Task<ExistenceStatus> CheckAffiliateExistenceAsync(string email, string userName, long brandId);
    Task<UsersAffiliate> UpdateAffiliateAsync(UsersAffiliate affiliate);
    Task<UsersAffiliate> UpdateImageAffiliateAsync(UsersAffiliate affiliate);
    Task UpdateBulkAffiliateAsync(List<UsersAffiliate> affiliates);
    Task<UsersAffiliate> CreateAffiliateAsync(UsersAffiliate affiliate);
    Task<bool> DeleteAffiliateAsync(UsersAffiliate affiliate);
    Task<List<Country>> GetCountries();
    Task<List<BinaryFamilyTree>> GetBinaryFamilyTree(int maxLevels, byte isAdmin, int id = 0);
    Task<UsersAffiliate?> GetChild(int id, byte side);
    Task<List<UniLevelFamilyTree>> GetUniLevelFamilyTree(int maxLevels, byte isAdmin, int externalGradingId, int id = 0);
    Task<UsersAffiliate> UpdateImageIdPathAffiliateAsync(UsersAffiliate affiliate);
    Task<UsersAffiliate> UpdateVerificationCodeAffiliateAsync(UsersAffiliate affiliate);
    Task<UsersAffiliate?> FindAffiliateByIdAsync(int id, long brandId);
    Task<int> GetTotalActiveMembers(long brandId);
    Task<UsersAffiliate?> GetAffiliateByVerificationCodeAsync(string code, long brandId);
    Task<List<CountryNetwork>> TotalAffiliatesByCountry(long brandId);
    Task<int> GetDirectAffiliatesCount(int affiliateId);
    Task<long[]> WhatUsersHave2Children(long[] affiliateIds);
    Task<ICollection<UsersAffiliate>?> GetLastRegisteredUsers(long brandId);
    Task<List<MonthlyRegistrations>> GetMonthlyRegistrationsTotals(long brandId, DateTime startDate);
    Task<List<UsersAffiliate>> GetChildrenByFatherId(int fatherId, long brandId);
    Task<List<MatrixTree>> GetMatrixFamilyTreeByMatrixType(int maxLevels, byte isAdmin, int matrixType, int id = 0);
    Task<int> CountQualifiedChildrenByMatrixAsync(int userId, int matrixType);
    Task<bool> IsUserActiveInMatrixAsync(int userId, int matrixType, int cycle);
}
