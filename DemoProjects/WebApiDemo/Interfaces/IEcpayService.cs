using System.Threading.Tasks;
using WebApiDemo.ViewModels;
using WebApiDemo.ViewModels.Ecpay;

namespace WebApiDemo.Interfaces
{
    public interface IEcpayService
    {
        Task<AppResult<EcpayTokenResponse>> CreateTradeTokenAsync(EcpayViewModel request);

        Task<AppResult<EcpayCreatePaymentResponse>> CreatePaymentAsync(string payToken, string merchantTradeNo);

        Task<AppResult<EcpayIssueInvoiceResult>> IssueInvoiceAsync(EcpayInvoiceRequest request);
    }
}
