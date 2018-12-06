using ICOCore.Entities.Extra;
using ICOCore.Infrastructures.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICOCore.Utils.Common
{
    public class CommonUtils
    {

        /// <summary>
        /// Tạo Random One Time Password cho Transaction
        /// </summary>
        /// <returns></returns>
        public static string GenerateOTP()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numbers = "1234567890";

            string characters = numbers;
            characters += alphabets + numbers;

            int length = 5;

            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }

            return otp;
        }


        /// <summary>
        /// Trả về số lớn hơn và gần nhất so với số target
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double ClosesGreaterThanTo(IEnumerable<double> collection, double target)
        {
            double closest = -1;
            var minDifference = double.MaxValue;

            foreach (var element in collection)
            {
                if (element >= target)
                {
                    var difference = element - target;
                    if (minDifference > difference)
                    {
                        minDifference = difference;
                        closest = element;
                    }
                }
            }

            return closest;
        }

        public static bool ToDateTime(string datetime, string format, out DateTime outDate)
        {
            outDate = DateTime.Now;
            try
            {
                outDate = DateTime.ParseExact(datetime, format, CultureInfo.InvariantCulture);
                return true;
            }
            catch { return false; }
        }

        public static bool ToDate(string date, out DateTime outDate)
        {
            outDate = DateTime.Now;
            try
            {
                outDate = DateTime.ParseExact(date, CommonConstants.DATE_FORMAT, CultureInfo.InvariantCulture);
                return true;
            }
            catch { return false; }
        }

        public static bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Length < 6 || username.Length > 50)
                return false;
            return Regex.IsMatch(username, @"^[a-zA-Z0-9]+$"); // chữ hoặc số
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            return Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public static bool IsValidBankAddress(string bankAddress)
        {
            if (string.IsNullOrWhiteSpace(bankAddress)) return false;
            if (bankAddress.Length > 100 && bankAddress.Length < 6) return false;
            return Regex.IsMatch(bankAddress, @"^[0-9]+$"); // chỉ số
        }

        public static bool IsDouble(string number)
        {
            try
            {
                double.Parse(number); return true;
            }
            catch { return false; }
        }

        public static bool IsInt(string number)
        {
            try
            {
                int.Parse(number); return true;
            }
            catch { return false; }
        }

        public static bool IsLong(string number)
        {
            try
            {
                long.Parse(number); return true;
            }
            catch { return false; }
        }

        //http://www.csharp-examples.net/string-format-double/
        /// <summary>
        /// Format số BTC , lấy 7 số 0 đằng sau dấy phẩy
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string FormatBTC(double amount)
        {
            return string.Format("{0:0.0000000}", amount);
        }

        public static double FoatBTCAmount(double amount)
        {
            return System.Math.Round(amount, 7);
        }

        /// <summary>
        /// Format giá với 0 chữ số sau dấu phẩy
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static double FormatPriceZeroDigit(double price)
        {
            return System.Math.Round(price, 0);
        }

        // https://msdn.microsoft.com/en-us/library/dwhawy9k.aspx
        public static string FormatPrice(double price)
        {
            return String.Format("{0:N2}", price);
        }

        public static string FormatPriceCurrencySymbol(double price)
        {
            return string.Format("{0:C}", price);
        }

        public static bool IsValidGoogleRecaptcha(string g_recaptcha_response)
        {
            // ------------------ kiểm tra captcha
            bool isInProduction = CommonConstants.IS_IN_PRODUCTION;
            var captchaErrorMess = string.Empty;
            bool isCaptchaErr = false;
            if (isInProduction)
            {
                var response = g_recaptcha_response;
                if (string.IsNullOrWhiteSpace(response))
                {
                    captchaErrorMess = "The response parameter is missing...";
                    isCaptchaErr = true;
                }
                else
                {
                    //secret that was generated in key value pair
                    const string secret = "6Ld1YgkUAAAAAIbSQSBeM2uSV7sN5dUc3AWXV4-a";

                    var client = new WebClient();
                    var reply =
                        client.DownloadString(
                            string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                        secret, response));

                    var captchaResponse = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(reply);

                    //when response is false check for the error message
                    bool isCapChaSuccess = captchaResponse.Success;
                    if (!isCapChaSuccess)
                    {
                        isCaptchaErr = captchaResponse.ErrorCodes.Count <= 0 ? false : true;

                        if (isCaptchaErr)
                        {
                            // nếu có lỗi capcha
                            var error = captchaResponse.ErrorCodes[0].ToLower();
                            switch (error)
                            {
                                case ("missing-input-secret"):
                                    captchaErrorMess = "The secret parameter is missing.";
                                    break;
                                case ("invalid-input-secret"):
                                    captchaErrorMess = "The secret parameter is invalid or malformed.";
                                    break;
                                case ("missing-input-response"):
                                    captchaErrorMess = "The response parameter is missing.";
                                    break;
                                case ("invalid-input-response"):
                                    captchaErrorMess = "The response parameter is invalid or malformed.";
                                    break;
                                default:
                                    captchaErrorMess = "Error occured. Please try again";
                                    break;
                            }
                        }
                    }
                }
            }
            return !isCaptchaErr;
        }

        public static void AppendToTextFile(string filePath, string text)
        {
            File.AppendAllText(
                filePath,
                text);
        }
    }
}
