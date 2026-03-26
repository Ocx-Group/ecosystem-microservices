using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IPaymentGroupRepository
{
    Task<PaymentGroups> CreatePaymentGroups(PaymentGroups paymentGroups);
    Task<List<PaymentGroups>> GetAllPaymentGroups(long brandId);
    Task<PaymentGroups> DeletePaymentGroup(PaymentGroups paymentGroup);
    Task<PaymentGroups?> GetPaymentGroupById(int id);
    Task<PaymentGroups> UpdatePaymentGroup(PaymentGroups paymentGroups);
}
