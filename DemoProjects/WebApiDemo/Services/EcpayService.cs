using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebApiDemo.ViewModels.Ecpay;

namespace WebApiDemo.Services
{
    public class EcpayService
    {
        /// <summary>
        /// 建立交易並取得交易Token
        /// </summary>
        /// <param name="ecpayViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateTradeGetToken(EcpayViewModel ecpayViewModel)
        {
            string partialGuid = Guid.NewGuid().ToString("N").Substring(0, 10);
            string todayDate = DateTime.Now.ToString("yyyyMMdd");
            string dateTimeString = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            string guidString = $"{todayDate}{partialGuid}";
 
            int pointAmount = Convert.ToInt32(ecpayViewModel.PointAmount);
            var getTokenContent = new Dictionary<string, object>
                {
                    { "MerchantID", ConfigurationManager.AppSettings["EcPayMerchantID"] },
                    { "RememberCard", 0 },
                    { "PaymentUIType", 2 },
                    { "ChoosePaymentList", "1" },
                    { "OrderInfo",  new Dictionary<string, object>
                        {
                            { "MerchantTradeDate", dateTimeString },
                            { "MerchantTradeNo", guidString },
                            { "TotalAmount", pointAmount },
                            { "ReturnURL", $"https://app.houseflow.tw/Sms/GetResultReturnUrl" },
                            { "TradeDesc", "三竹簡訊點數資訊服務" },
                            { "ItemName", "資訊服務" }
                        }
                    },
                    { "CardInfo", new Dictionary<string, object>
                        {
                            { "OrderResultURL", $"https://app.houseflow.tw/Sms/SmsOrderResultURL" }
                        }
                    },
                    { "ConsumerInfo", new Dictionary<string, object>
                        {
                            { "Email", ecpayViewModel.Email },
                            { "Phone", ecpayViewModel.Phone },
                            { "CountryCode", "158" }
                        }
                    },
                    { "CustomField", $"CompanyAddr={ecpayViewModel.CompanyAddr}&InvoiceCode={ecpayViewModel.CarruerNum}&CompanyTitle={ecpayViewModel.CompanyTitle}&CompanyTaxId={ecpayViewModel.CompanyTaxId}" }
                };

            var getTokenContentJson = JsonConvert.SerializeObject(getTokenContent, new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            });

            var encryptedData = AES_Encrypt(getTokenContentJson,
                ConfigurationManager.AppSettings["EcPayPaymentHashKey"], ConfigurationManager.AppSettings["EcPayPaymentHashIV"]);

            var requestData = new Dictionary<string, object>
                {
                    { "MerchantID", ConfigurationManager.AppSettings["EcPayMerchantID"] },
                    { "RqHeader", new { Timestamp = GetTimeStamp() } },
                    { "Data", encryptedData }
                };
            var requestJson = JsonConvert.SerializeObject(requestData);

            var client = new HttpClient();
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json")
            {
                CharSet = "utf-8"
            };
            var response = client.PostAsync("https://ecpg-stage.ecpay.com.tw/Merchant/GetTokenbyTrade ", content).Result;

            var responseString = response.Content.ReadAsStringAsync().Result;
            var responseJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);

            if (responseJson.ContainsKey("Data"))
            {
                var encryptedResponseData = responseJson["Data"].ToString();
                var decryptedData = AES_Decrypt(encryptedResponseData,
                   ConfigurationManager.AppSettings["EcPayPaymentHashKeyTest"], ConfigurationManager.AppSettings["EcPayPaymentHashIVTest"]);
                string decodedData = HttpUtility.UrlDecode(decryptedData);
                var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(decodedData);

                if (dataJson["RtnCode"].ToString() == "1")
                {
                    var responseToFront = new
                    {
                        MerchantID = dataJson["MerchantID"].ToString(),
                        MerchantTradeNo = guidString,
                        Token = dataJson["Token"].ToString(),
                        TokenExpireDate = dataJson["TokenExpireDate"].ToString()
                    };

                    return Json(responseToFront, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        private static long GetTimeStamp()
        {
            var unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            return unixTime;
        }

        private static string AES_Encrypt(string plainText, string key, string iv)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        private static string AES_Decrypt(string cipherText, string key, string iv)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (var aes = new AesManaged())
            {
                aes.Key = keyBytes;
                aes.IV = ivBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var transform = aes.CreateDecryptor())
                {
                    byte[] plainBytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                    return Encoding.UTF8.GetString(plainBytes);
                }
            }
        }
    }
}
