using WebApiDemo.ViewModels.Ecpay;

namespace WebApiDemo.Interfaces
{
    public interface IEcpayTokenService
    {
        Task<EcpayTokenResponse> CreateTradeTokenAsync(EcpayViewModel request);

        Task<EcpayCreatePaymentResponse> CreatePaymentAsync(string payToken, string merchantTradeNo);

        Task<EcpayIssueInvoiceResult> IssueInvoiceAsync(EcpayInvoiceRequest request);
    }e
}
