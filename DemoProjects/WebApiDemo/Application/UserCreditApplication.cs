using WebApiDemo.Interfaces;
using WebApiDemo.Services;
using WebApiDemo.ViewModels;
using WebApiDemo.ViewModels.Ecpay;
using WebApiDemo.ViewModels.UserCredit;

namespace WebApiDemo.Application
{
    public class UserCreditApplication
    {
        private readonly IUserCreditService _userCreditService;
        private readonly IEcpayService _ecpayService;

        public UserCreditApplication(
            IUserCreditService userCreditService,
            IEcpayService ecpayService)
        {
            _userCreditService = userCreditService;
            _ecpayService = ecpayService;
        }

        public async Task<AppResult<CreateTradeTokenResponse>> CreateTradeToken(CreateTradeTokenRequest request)
        {
            var ecpayModel = new EcpayViewModel
            {
                CarruerNum = request.CarruerNum,
                CompanyAddr = request.CompanyAddr,
                CompanyTaxId = request.CompanyTaxId,
                CompanyTitle = request.CompanyTitle,
                PointAmount = request.PointAmount,
                Phone = request.Phone,
                InvoiceType = request.InvoiceType,
                Email = request.Email,
            };

            var result = await _ecpayService.CreateTradeTokenAsync(ecpayModel);

            var response = new AppResult<CreateTradeTokenResponse>()
            {
                Code = result.Code,
                Message = result.Message,
                Success = result.Success,
                Data = new CreateTradeTokenResponse
                {
                    MerchantID = result.Data.MerchantID,
                    MerchantTradeNo = result.Data.MerchantTradeNo,
                    Token = result.Data.Token,
                    TokenExpireDate = result.Data.TokenExpireDate
                }
            };

            return response;
        }
    }
}
