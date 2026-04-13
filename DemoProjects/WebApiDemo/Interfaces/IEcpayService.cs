using System.Threading.Tasks;
using WebApiDemo.ViewModels;
using WebApiDemo.ViewModels.Ecpay;

namespace WebApiDemo.Interfaces
{
    public interface IEcpayService
    {
        Task<AppResult<EcpayTokenResponse>> CreateTradeTokenAsync(EcpayViewModel request, string merchantTradeNo);

        Task<AppResult<EcpayCreatePaymentResponse>> CreatePaymentAsync(string payToken, string merchantTradeNo);

        Task<AppResult<EcpayIssueInvoiceResult>> IssueInvoiceAsync(EcpayInvoiceRequest request);

        /// <summary>
        /// 解密綠界回傳的加密資料
        /// </summary>
        Dictionary<string, object> DecryptCallbackData(string encryptedData);
    }
}
