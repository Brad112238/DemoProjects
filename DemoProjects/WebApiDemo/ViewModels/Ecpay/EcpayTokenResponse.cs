namespace WebApiDemo.ViewModels.Ecpay
{
    public class EcpayTokenResponse
    {
        public string MerchantID { get; set; } = null!;
        public string MerchantTradeNo { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string TokenExpireDate { get; set; } = null!;
    }
}
