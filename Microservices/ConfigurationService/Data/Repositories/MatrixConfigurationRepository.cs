using Microsoft.EntityFrameworkCore;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class MatrixConfigurationRepository : BaseRepository, IMatrixConfigurationRepository
{
    public MatrixConfigurationRepository(ConfigurationServiceDbContext context) : base(context) { }

    public async Task<MatrixConfiguration?> GetMatrixConfigurationByType(int matrixType)
        => await Context.MatrixConfigurations
            .FirstOrDefaultAsync(x => x.MatrixType == matrixType);

    public async Task<IEnumerable<MatrixConfiguration?>> GetAllMatrixConfigurations()
        => await Context.MatrixConfigurations.OrderBy(e => e.MatrixType).ToListAsync();
}
