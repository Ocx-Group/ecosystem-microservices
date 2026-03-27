using System.Collections.Concurrent;

namespace Ecosystem.WalletService.Domain.Responses;

public class BatchProcessingResult
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double ElapsedTimeSeconds { get; set; }
    public int TotalProcessed { get; set; }
    public int TotalQualified { get; set; }
    public string? ErrorMessage { get; set; }
    public ConcurrentBag<UserProcessingResult> ProcessedUsers { get; set; } = new();
}

public class UserProcessingResult
{
    public int UserId { get; set; }
    public DateTime ProcessedTime { get; set; }
    public bool WasQualified { get; set; }
    public double QualificationProgress { get; set; }
    public string? ErrorMessage { get; set; }
    public List<MatrixQualificationInfo> MatricesQualified { get; set; } = new();
}

public class MatrixQualificationInfo
{
    public int MatrixType { get; set; }
    public string MatrixName { get; set; } = string.Empty;
    public int QualificationCount { get; set; }
    public DateTime QualificationDate { get; set; }
}