namespace Ecosystem.AccountService.Application.Adapters;

public interface IConfigurationServiceAdapter
{
    Task<MatrixConfigurationResult?> GetMatrixConfigurationAsync(long brandId, int matrixType);
}

public class MatrixConfigurationResult
{
    public string MatrixName { get; set; } = string.Empty;
}
