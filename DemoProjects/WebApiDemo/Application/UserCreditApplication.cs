using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WebApiDemo.Interfaces;
using WebApiDemo.Models.TestDb;
using WebApiDemo.ViewModels;
using WebApiDemo.ViewModels.Ecpay;
using WebApiDemo.ViewModels.UserCredit;

namespace WebApiDemo.Application
{
    public class UserCreditApplication
    {
        private readonly IUserCreditService _userCreditService;
        private readonly IUserTopUpService _userTopUpService;
        private readonly IEcpayService _ecpayService;

        public UserCreditApplication(
            IUserCreditService userCreditService,
            IUserTopUpService userTopUpService,
            IEcpayService ecpayService)
        {
            _userCreditService = userCreditService;
            _userTopUpService = userTopUpService;
            _ecpayService = ecpayService;
        }

        public async Task<List<UserCredit>> GetAllAsync()
        {
            return await _userCreditService.Query().ToListAsync();
        }

        public async Task<UserCredit?> GetByIdAsync(int id)
        {
            return await _userCreditService.Query()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AppResult<CreateTradeTokenResponse>> CreateTradeToken(CreateTradeTokenRequest request)
        {
            string partialGuid = Guid.NewGuid().ToString("N").Substring(0, 10);
            string todayDate = DateTime.Now.ToString("yyyyMMdd");
            string dateTimeString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string guidString = $"{todayDate}{partialGuid}";

            var userTopUp = new UserTopUp
            {
                TPSTradeNo = guidString,
                UserId = request.UserId,
                Amount = Convert.ToDouble(request.PointAmount),
                HostId = request.HostId,
                CreateUser = request.UserId,
                Product = "資訊服務",
                CarruerNum = request.CarruerNum,
                CarruerType = string.IsNullOrEmpty(request.CarruerNum) ? "綠界電子發票" : "個人手機載具",
                InvoiceType = request.InvoiceType == "公司發票" ? "公司發票" : "個人發票",
                CompName = request.CompanyTitle,
                CompIdNumber = request.CompanyTaxId,
                CompAddr = request.CompanyAddr,
                Email = request.Email,
                PayType = "信用卡",
                CreateTime = DateTime.Now,
                Status = 0
            };

            _userTopUpService.Add(userTopUp);
            await _userTopUpService.SaveChangesAsync();

            var ecpayModel = new EcpayViewModel
            {
                CarruerNum = request.CarruerNum,
                CompanyAddr = request.CompanyAddr,
                CompanyTaxId = request.CompanyTaxId,
                CompanyTitle = request.CompanyTitle,
                PointAmount = request.PointAmount,
                Phone = request.Phone,
                InvoiceType = request.InvoiceType,
                Email = request.Email
            };

            var result = await _ecpayService.CreateTradeTokenAsync(ecpayModel, guidString);

            if (!result.Success)
            {
                return new AppResult<CreateTradeTokenResponse>
                {
                    Code = result.Code,
                    Message = result.Message,
                    Success = false
                };
            }

            return new AppResult<CreateTradeTokenResponse>
            {
                Code = 200,
                Success = true,
                Message = result.Message,
                Data = new CreateTradeTokenResponse
                {
                    MerchantID = result.Data.MerchantID,
                    MerchantTradeNo = result.Data.MerchantTradeNo,
                    Token = result.Data.Token,
                    TokenExpireDate = result.Data.TokenExpireDate
                }
            };
        }

        public async Task<AppResult<EcpayCreatePaymentResponse>> CreatePayment(string payToken, string merchantTradeNo)
        {
            var result = await _ecpayService.CreatePaymentAsync(payToken, merchantTradeNo);
            return result;
        }

        /// <summary>
        /// 處理綠界付款回傳通知
        /// </summary>
        public Dictionary<string, object> DecryptCallbackData(string encryptedData)
        {
            return _ecpayService.DecryptCallbackData(encryptedData);
        }

        public async Task<bool> ProcessPaymentCallbackAsync(string encryptedData)
        {
            var data = _ecpayService.DecryptCallbackData(encryptedData);
            var dataObj = JObject.FromObject(data);

            var rtnCode = dataObj["RtnCode"]?.ToString();
            var rtnMsg = dataObj["RtnMsg"]?.ToString();

            // 綠界確認通知 (server-to-server)
            if (rtnMsg == "Success" && rtnCode == "1")
                return true;

            var orderInfo = JObject.Parse(dataObj["OrderInfo"]!.ToString());
            var merchantTradeNo = orderInfo["MerchantTradeNo"]!.ToString();

            if (rtnCode == "1" && rtnMsg == "Succeeded.")
            {
                var tradeAmount = Convert.ToDouble(orderInfo["TradeAmt"]);

                var userTopUp = await _userTopUpService.Query()
                    .FirstOrDefaultAsync(x => x.TPSTradeNo == merchantTradeNo);

                if (userTopUp != null)
                {
                    var userCredit = await _userCreditService.QueryTracking()
                        .FirstOrDefaultAsync(x => x.UserId == userTopUp.UserId);

                    if (userCredit != null)
                    {
                        userCredit.Amount += tradeAmount;
                        userCredit.ModifyUser = userTopUp.UserId;
                        userCredit.ModifyTime = DateTime.Now;
                        await _userCreditService.SaveChangesAsync();
                    }

                    userTopUp.Message = "付款成功";
                    userTopUp.Status = 1;
                }

                // 解析 CustomField 開立發票
                var customFieldRaw = dataObj["CustomField"]?.ToString() ?? "";
                var parts = customFieldRaw.Split('&')
                    .Select(p => p.Split('='))
                    .Where(p => p.Length == 2)
                    .ToDictionary(p => p[0], p => p[1]);

                var invoiceRequest = new EcpayInvoiceRequest
                {
                    CompanyAddr = parts.GetValueOrDefault("CompanyAddr"),
                    InvoiceCode = parts.GetValueOrDefault("InvoiceCode"),
                    CompanyTitle = parts.GetValueOrDefault("CompanyTitle"),
                    CompanyTaxId = parts.GetValueOrDefault("CompanyTaxId"),
                    CustomerEmail = userTopUp?.Email ?? "",
                    MerchantTradeNo = merchantTradeNo,
                    TotalAmount = (int)orderInfo["TradeAmt"]!
                };

                var invoiceResult = await _ecpayService.IssueInvoiceAsync(invoiceRequest);

                if (invoiceResult.Success && userTopUp != null)
                {
                    userTopUp.InvoiceNumber = invoiceResult.Data?.InvoiceNumber;
                    userTopUp.InvoiceNote = invoiceResult.Data?.Message;
                }
            }
            else
            {
                var userTopUp = await _userTopUpService.Query().FirstOrDefaultAsync(x => x.TPSTradeNo == merchantTradeNo);

                if (userTopUp != null)
                {
                    userTopUp.Message = rtnMsg;
                    userTopUp.Status = 2;
                }
            }

            await _userTopUpService.SaveChangesAsync();

            return true;
        }
    }
}
