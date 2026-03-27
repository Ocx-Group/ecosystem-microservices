using MediatR;

namespace Ecosystem.WalletService.Application.Queries.MatrixQualification;

public record ProcessQualificationQuery(long UserId) : IRequest<(bool AnyQualified, List<int> QualifiedMatrixTypes)>;
