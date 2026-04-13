namespace WebApiDemo.ViewModels.UserCredit
{
    public class CreatePaymentRequest
    {
        public string PayToken { get; set; } = null!;
        public string MerchantTradeNo { get; set; } = null!;
    }
}
