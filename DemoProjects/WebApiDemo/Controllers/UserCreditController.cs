using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiDemo.Application;
using WebApiDemo.ViewModels.UserCredit;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCreditController : ControllerBase
    {
        private readonly UserCreditApplication _userCreditApplication;

        public UserCreditController(UserCreditApplication userCreditApplication)
        {
            _userCreditApplication = userCreditApplication;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _userCreditApplication.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _userCreditApplication.GetByIdAsync(id);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [AllowAnonymous]
        [HttpPost("trade-token")]
        public async Task<IActionResult> CreateTradeToken([FromBody] CreateTradeTokenRequest request)
        {
            var result = await _userCreditApplication.CreateTradeToken(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            var result = await _userCreditApplication.CreatePayment(request.PayToken, request.MerchantTradeNo);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// 3D驗證完成後，綠界 POST 跳轉到此，再 redirect 到 Angular 結果頁
        /// </summary>
        [AllowAnonymous]
        [HttpPost("payment-result")]
        public IActionResult PaymentResult()
        {
            try
            {
                var resultData = Request.Form["ResultData"].ToString();
                var resultJson = Newtonsoft.Json.Linq.JObject.Parse(resultData);
                var encryptedData = resultJson["Data"]?.ToString();
                if (!string.IsNullOrEmpty(encryptedData))
                {
                    var decrypted = _userCreditApplication.DecryptCallbackData(encryptedData);

                    var rtnCode = decrypted.ContainsKey("RtnCode") ? decrypted["RtnCode"]?.ToString() : "";
                    var rtnMsg = decrypted.ContainsKey("RtnMsg") ? decrypted["RtnMsg"]?.ToString() : "";
                    var orderInfo = decrypted.ContainsKey("OrderInfo") ? Newtonsoft.Json.Linq.JObject.Parse(decrypted["OrderInfo"]!.ToString()!) : null;
                    var merchantTradeNo = orderInfo?["MerchantTradeNo"]?.ToString() ?? "";

                    var redirectUrl = $"http://localhost:4200/payment/result?RtnCode={rtnCode}&RtnMsg={Uri.EscapeDataString(rtnMsg ?? "")}&MerchantTradeNo={merchantTradeNo}";
                    return Redirect(redirectUrl);
                }
                var url = $"http://localhost:4200/payment/result";
                return Redirect(url);
            }
            catch (Exception ex)
            {
                var url = $"http://localhost:4200/payment/result";
                return Redirect(url);
            }

        }

        /// <summary>
        /// 接收綠界的回傳資料
        /// </summary>
        [AllowAnonymous]
        [HttpPost("notify")]
        public async Task<IActionResult> GetResultReturnUrl()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();
                Console.WriteLine($"[Notify] Raw Body: {body}");

                string? encryptedData;
                if (body.TrimStart().StartsWith("{"))
                {
                    var root = Newtonsoft.Json.Linq.JObject.Parse(body);
                    encryptedData = root["Data"]?.ToString();
                }
                else
                {
                    var parsed = System.Web.HttpUtility.ParseQueryString(body);
                    encryptedData = parsed["Data"];
                }

                if (string.IsNullOrEmpty(encryptedData))
                    return Content("0|Error", "text/plain");

                await _userCreditApplication.ProcessPaymentCallbackAsync(encryptedData);

                return Content("1|OK", "text/plain");
            }
            catch (Exception)
            {
                return Content("0|Error", "text/plain");
            }
        }
    }
}
