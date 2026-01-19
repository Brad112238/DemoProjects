using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebApiDemo.Interfaces;
using WebApiDemo.ViewModels.Ecpay;

namespace WebApiDemo.Services
{
    public class EcpayService : IEcpayTokenService
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public EcpayService(
            IConfiguration config,
            IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<EcpayTokenResponse> CreateTradeTokenAsync(EcpayViewModel request)
        {
            string tradeNo = $"{DateTime.Now:yyyyMMdd}{Guid.NewGuid():N}".Substring(0, 18);

            string tradeDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            var payload = new Dictionary<string, object>
            {
                ["MerchantID"] = _config["EcPay:MerchantID"],
                ["RememberCard"] = 0,
                ["PaymentUIType"] = 2,
                ["ChoosePaymentList"] = "1",
                ["OrderInfo"] = new Dictionary<string, object>
                {
                    ["MerchantTradeDate"] = tradeDate,
                    ["MerchantTradeNo"] = tradeNo,
                    ["TotalAmount"] = request.PointAmount,
                    ["ReturnURL"] = _config["EcPay:ReturnUrl"],
                    ["TradeDesc"] = "資訊服務",
                    ["ItemName"] = "資訊服務"
                },
                ["CardInfo"] = new Dictionary<string, object>
                {
                    ["OrderResultURL"] = _config["EcPay:OrderResultUrl"]
                },
                ["ConsumerInfo"] = new Dictionary<string, object>
                {
                    ["Email"] = request.Email,
                    ["Phone"] = request.Phone,
                    ["CountryCode"] = "158"
                },
                ["CustomField"] =
                    $"CompanyAddr={request.CompanyAddr}&" +
                    $"InvoiceCode={request.CarruerNum}&" +
                    $"CompanyTitle={request.CompanyTitle}&" +
                    $"CompanyTaxId={request.CompanyTaxId}"
            };

            string payloadJson = JsonConvert.SerializeObject(payload,
                new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                });

            string encrypted = Encrypt(payloadJson, _config["EcPay:HashKey"], _config["EcPay:HashIV"]);

            var apiRequest = new
            {
                MerchantID = _config["EcPay:MerchantID"],
                RqHeader = new { Timestamp = GetTimeStamp() },
                Data = encrypted
            };

            var client = _httpClientFactory.CreateClient();

            using var response = await client.PostAsync(
                _config["EcPay:TokenApiUrl"],
                new StringContent(
                    JsonConvert.SerializeObject(apiRequest),
                    Encoding.UTF8,
                    "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new Exception("ECPay GetToken API 呼叫失敗");

            string responseText = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseText) ?? throw new Exception("ECPay 回傳為空");

            if (!responseJson.TryGetValue("Data", out var encryptedResponse))
                throw new Exception("ECPay 回傳缺少 Data");

            string decrypted = Decrypt(encryptedResponse.ToString(), _config["EcPay:HashKey"], _config["EcPay:HashIV"]);

            string decoded = HttpUtility.UrlDecode(decrypted);

            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(decoded) ?? throw new Exception("ECPay 解密失敗");

            if (data["RtnCode"]?.ToString() != "1")
                throw new Exception($"ECPay 交易失敗：{data["RtnMsg"]}");

            return new EcpayTokenResponse
            {
                MerchantID = data["MerchantID"]!.ToString()!,
                MerchantTradeNo = tradeNo,
                Token = data["Token"]!.ToString()!,
                TokenExpireDate = data["TokenExpireDate"]!.ToString()!
            };
        }

        public async Task<EcpayCreatePaymentResponse> CreatePaymentAsync(string payToken, string merchantTradeNo)
        {
            var payload = new Dictionary<string, object>
            {
                ["MerchantID"] = _config["EcPay:MerchantID"],
                ["PayToken"] = payToken,
                ["MerchantTradeNo"] = merchantTradeNo
            };

            string payloadJson = JsonConvert.SerializeObject(
                payload,
                new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                });

            string encrypted = Encrypt(payloadJson, _config["EcPay:HashKey"], _config["EcPay:HashIV"]);

            var requestBody = new
            {
                MerchantID = _config["EcPay:MerchantID"],
                RqHeader = new { Timestamp = GetTimeStamp() },
                Data = encrypted
            };

            var client = _httpClientFactory.CreateClient();

            using var response = await client.PostAsync(
                _config["EcPay:CreatePaymentApiUrl"],
                new StringContent(
                    JsonConvert.SerializeObject(requestBody),
                    Encoding.UTF8,
                    "application/json")
            );

            if (!response.IsSuccessStatusCode)
                throw new Exception("ECPay CreatePayment API 呼叫失敗");

            string responseText = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseText) ?? throw new Exception("ECPay 回傳為空");

            if (!responseJson.TryGetValue("Data", out var encryptedResponse))
                throw new Exception("ECPay 回傳缺少 Data");

            string decrypted = Decrypt(encryptedResponse.ToString(), _config["EcPay:HashKey"], _config["EcPay:HashIV"]);

            string decoded = WebUtility.UrlDecode(decrypted);

            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(decoded) ?? throw new Exception("ECPay 解密失敗");

            if (data["RtnCode"]?.ToString() != "1")
                throw new Exception($"ECPay CreatePayment 失敗：{data["RtnMsg"]}");

            if (!data.TryGetValue("ThreeDInfo", out var threeDInfoRaw))
                throw new Exception("ECPay 回傳缺少 ThreeDInfo");

            var threeDInfo = JObject.Parse(threeDInfoRaw.ToString()!);

            var threeDUrl = threeDInfo["ThreeDURL"]?.ToString();

            if (string.IsNullOrWhiteSpace(threeDUrl))
                throw new Exception("ThreeDURL 為空");

            return new EcpayCreatePaymentResponse
            {
                ThreeDURL = threeDUrl
            };
        }

        public async Task<EcpayIssueInvoiceResult> IssueInvoiceAsync(EcpayInvoiceRequest request)
        {
            var invoicePayload = new Dictionary<string, object>
            {
                ["MerchantID"] = _config["EcPay:MerchantID"],
                ["RelateNumber"] = $"Inv{request.MerchantTradeNo}",
                ["CustomerEmail"] = request.CustomerEmail,
                ["Print"] = "0",
                ["Donation"] = "0",
                ["TaxType"] = "1",
                ["SalesAmount"] = request.TotalAmount,
                ["InvType"] = "07",
                ["Items"] = new List<Dictionary<string, object>>
            {
                new()
                {
                    ["ItemName"] = "資訊服務",
                    ["ItemCount"] = request.TotalAmount,
                    ["ItemWord"] = "點",
                    ["ItemPrice"] = "1",
                    ["ItemTaxType"] = "1",
                    ["ItemAmount"] = request.TotalAmount
                }
            }
            };

            if (!string.IsNullOrWhiteSpace(request.CompanyTaxId))
            {
                invoicePayload["CustomerIdentifier"] = request.CompanyTaxId;
                invoicePayload["CustomerName"] = request.CompanyTitle!;
                invoicePayload["CustomerAddr"] = request.CompanyAddr!;
                invoicePayload["Print"] = "1";
            }

            if (!string.IsNullOrWhiteSpace(request.InvoiceCode))
            {
                invoicePayload["CarruerType"] = "3";
                invoicePayload["CarruerNum"] = request.InvoiceCode;
            }

            string payloadJson = JsonConvert.SerializeObject(
                invoicePayload,
                new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                });

            string encrypted = Encrypt(payloadJson, _config["EcPay:HashKey"], _config["EcPay:HashIV"]);

            var apiRequest = new
            {
                MerchantID = _config["EcPay:MerchantID"],
                RqHeader = new { Timestamp = GetTimeStamp() },
                Data = encrypted
            };

            var client = _httpClientFactory.CreateClient();

            using var response = await client.PostAsync(
                _config["EcPay:IssueInvoiceApiUrl"],
                new StringContent(
                    JsonConvert.SerializeObject(apiRequest),
                    Encoding.UTF8,
                    "application/json")
            );

            if (!response.IsSuccessStatusCode)
            {
                return new EcpayIssueInvoiceResult
                {
                    Success = false,
                    Message = "ECPay 發票 API 呼叫失敗"
                };
            }

            string responseText = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseText);

            if (responseJson == null || !responseJson.TryGetValue("Data", out var encryptedResponse))
            {
                return new EcpayIssueInvoiceResult
                {
                    Success = false,
                    Message = "ECPay 回傳格式錯誤"
                };
            }

            string decrypted = Decrypt(encryptedResponse.ToString(), _config["EcPay:HashKey"], _config["EcPay:HashIV"]);

            string decoded = WebUtility.UrlDecode(decrypted);
            var data =JsonConvert.DeserializeObject<Dictionary<string, object>>(decoded);

            if (data == null)
            {
                return new EcpayIssueInvoiceResult
                {
                    Success = false,
                    Message = "發票資料解析失敗"
                };
            }

            if (data["RtnCode"]?.ToString() == "1")
            {
                return new EcpayIssueInvoiceResult
                {
                    Success = true,
                    InvoiceNumber = data["InvoiceNo"]?.ToString(),
                    Message = data["RtnMsg"]?.ToString()
                };
            }

            return new EcpayIssueInvoiceResult
            {
                Success = false,
                Message = data["RtnMsg"]?.ToString()
            };
        }

        private static long GetTimeStamp()
        {
            var unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            return unixTime;
        }

        private static string Encrypt(string plainText, string key, string iv)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.ASCII.GetBytes(key);
            aes.IV = Encoding.ASCII.GetBytes(iv);

            using var encryptor = aes.CreateEncryptor();
            byte[] encryptedBytes = encryptor.TransformFinalBlock(
                plainBytes, 0, plainBytes.Length);

            return Convert.ToBase64String(encryptedBytes);
        }

        private static string Decrypt(string cipherText, string key, string iv)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.ASCII.GetBytes(key);
            aes.IV = Encoding.ASCII.GetBytes(iv);

            using var decryptor = aes.CreateDecryptor();
            byte[] plainBytes = decryptor.TransformFinalBlock(
                cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
