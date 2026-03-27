namespace Ecosystem.WalletService.Domain.DTOs.MatrixDashboardDto;

public class MatrixDashboardDto
{
    public UserInfoDto User { get; set; }
    public List<MatrixInfoDto> Matrices { get; set; }
}

public class UserInfoDto
{
    public int Id { get; set; }
    public string Username { get; set; }
}

public class MatrixInfoDto
{
    public int MatrixType { get; set; }
    public string MatrixName { get; set; }
    public decimal ThresholdAmount { get; set; }
    public decimal FeeAmount { get; set; }
    public decimal MinWithdraw { get; set; }
    public decimal MaxWithdraw { get; set; }
    public QualificationInfoDto Qualification { get; set; }
    public int UplineCount { get; set; }
    public int DownlineCount { get; set; }
    public decimal CommissionsEarned { get; set; }
    public decimal CommissionsPaid { get; set; }
    public PositionInfoDto Position { get; set; }
    public int CycleCount { get; set; } // Contador de ciclos completados (qualification.QualificationCount)
    public decimal QualificationProgress { get; set; } // Progreso hacia el próximo ciclo (0-100%)
    public decimal CycleReward { get; set; } // Recompensa por ciclo completo
}

public class QualificationInfoDto
{
    public int? QualificationId { get; set; }
    public bool IsQualified { get; set; }
    public int QualificationCount { get; set; }
    public decimal TotalEarnings { get; set; }
    public decimal WithdrawnAmount { get; set; }
    public decimal AvailableBalance { get; set; }
    public DateTime? LastQualificationDate { get; set; }
}

public class PositionInfoDto
{
    public int PositionId { get; set; }
    public int Level { get; set; }
    public int? ParentId { get; set; }
    public DateTime Created { get; set; }
}

public class CycleInfoDto
{
    public int? CycleId { get; set; }
    public bool IsCompleted { get; set; }
    public int TotalPositions { get; set; }
    public int MaxPositions { get; set; }
    public int? InitiatorUserId { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool RewardPaid { get; set; }
}

public class MatrixTreeResponse
{
    public int? Code { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
}

public class MatrixCycleResponse
{
    public int? Code { get; set; }
    public string? Message { get; set; }
    public MatrixCycleResponseData? Data { get; set; }
}

public class MatrixCycleResponseData
{
    public int CycleId { get; set; }
    public int MatrixType { get; set; }
    public int InitiatorUserId { get; set; }
    public int TotalPositions { get; set; }
    public int MaxPositions { get; set; }
    public bool IsCompleted { get; set; }
    public bool RewardPaid { get; set; }
    public DateTime? CompletedAt { get; set; }
}