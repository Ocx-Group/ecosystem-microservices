using Microsoft.EntityFrameworkCore;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;

namespace Ecosystem.WalletService.Data.Repositories;

public class MatrixQualificationRepository(WalletServiceDbContext context) : BaseRepository(context), IMatrixQualificationRepository
{
    public async Task<MatrixQualification?> GetByUserAndMatrixTypeAsync(long userId, int matrixType)
        => await Context.MatrixQualifications.FirstOrDefaultAsync(e =>
            e.UserId == userId && e.MatrixType == matrixType);
    
    public async Task<MatrixQualification?> GetQualificationById(int qualificationId)
    => await Context.MatrixQualifications
        .FirstOrDefaultAsync(e => e.QualificationId == qualificationId);

    public async Task<MatrixQualification?> CreateAsync(MatrixQualification qualification)
    {
        var today = DateTime.Now;
        qualification.CreatedAt = today;
        qualification.UpdatedAt = today;

        await Context.MatrixQualifications.AddAsync(qualification);
        await Context.SaveChangesAsync();

        return qualification;
    }
    
    public async Task<bool>UpdateAsync(MatrixQualification qualification)
    {
        try
        {
            var today = DateTime.Now;
            qualification.UpdatedAt = today;
            
            Context.MatrixQualifications.Update(qualification);
            await Context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    
    public async Task<IEnumerable<MatrixQualification>> GetAllByUserIdAsync(long userId)
    {
        return await Context.MatrixQualifications
            .Where(e => e.UserId == userId)
            .ToListAsync();
    }   
    
    public async Task<IEnumerable<MatrixQualification>> GetAllInconsistentRecordsAsync()
        => await Context.MatrixQualifications
            .AsNoTracking()
            .Where(e => e.QualificationCount > 0 && 
                        (e.LastQualificationDate == null || 
                         e.LastQualificationTotalEarnings == 0 || 
                         e.LastQualificationWithdrawnAmount == 0))
            .ToListAsync();
}