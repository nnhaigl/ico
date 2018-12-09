using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ICOWebCore.Filters
{
    /// <summary>
    /// Xác thực bằng cơ chế Token-Based + Secret Key Generation
    /// Cần test kỹ kết hợp với code javascript ở client để sinh key dựa vào user + salt + timestamp v.v.v
    /// </summary>
    public class WebAPIAuthenticateFilterAttribute : ActionFilterAttribute
    {
        //private AuthorizationService _authorizationSvc;

        HttpActionContext _actionContext = null;

        private const string AuthenticationHeaderName = "Authentication";
        private const string TimestampHeaderName = "Timestamp";

        public WebAPIAuthenticateFilterAttribute()
        {
            //_authorizationSvc = new AuthorizationService();
        }

        private static string ComputeHash(string hashedPassword, string message)
        {
            var key = Encoding.UTF8.GetBytes(hashedPassword.ToUpper());
            string hashString;

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                hashString = Convert.ToBase64String(hash);
            }

            return hashString;
        }

        private static void AddNameValuesToCollection(List<KeyValuePair<string, string>> parameterCollection,
            NameValueCollection nameValueCollection)
        {
            if (!nameValueCollection.AllKeys.Any())
                return;

            foreach (var key in nameValueCollection.AllKeys)
            {
                var value = nameValueCollection[key];
                var pair = new KeyValuePair<string, string>(key, value);

                parameterCollection.Add(pair);
            }
        }

        /// <summary>
        /// Lấy request parameter từ query string và từ formCollection
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static List<KeyValuePair<string, string>> BuildParameterCollection(HttpActionContext actionContext)
        {
            // Use the list of keyvalue pair in order to allow the same key instead of dictionary
            var parameterCollection = new List<KeyValuePair<string, string>>();

            var queryStringCollection = actionContext.Request.RequestUri.ParseQueryString();
            var formCollection = HttpContext.Current.Request.Form;

            AddNameValuesToCollection(parameterCollection, queryStringCollection);
            AddNameValuesToCollection(parameterCollection, formCollection);

            // sắp xếp formdata và query string theo thứ tự để build correct message 
            //return parameterCollection.OrderBy(pair => pair.Key).ToList();
            return parameterCollection;
        }

        /// <summary>
        /// Lấy Parameter từ request (query string + formCollection) 
        /// và trả về dưới dạng key1=value1&key2=value2v.v.
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static string BuildParameterMessage(HttpActionContext actionContext)
        {
            var parameterCollection = BuildParameterCollection(actionContext);
            if (!parameterCollection.Any())
                return string.Empty;
            var parameterMessage = string.Empty;

            foreach (var pair in parameterCollection)
            {
                string pairParam = string.Concat(pair.Key, "=", pair.Value);
                parameterMessage = string.Concat(parameterMessage, pairParam, "&");
            }

            parameterMessage = parameterMessage.Substring(0, parameterMessage.Length - 1);

            return parameterMessage;
            //var keyValueStrings = parameterCollection.Select(pair => $"{pair.Key}={pair.Value}");

            //return string.Join("&", keyValueStrings);
        }

        /// <summary>
        /// Lấy Request Header Value dựa vào HeaderName
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="headerName"></param>
        /// <returns></returns>
        private static string GetHttpRequestHeader(HttpHeaders headers, string headerName)
        {
            if (!headers.Contains(headerName))
                return string.Empty;

            return headers.GetValues(headerName)
                            .SingleOrDefault();
        }

        /// <summary>
        /// Lấy message để hash => hased message là signature
        /// Message gồm : 
        /// 1 : HTTP Method
        /// 2 : Date (Timestamp)
        /// 3 : Request URI
        /// 4 : Request parameters (Query string + formCollection)
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        private static string BuildBaseString(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;
            string date = GetHttpRequestHeader(headers, TimestampHeaderName);

            string methodType = actionContext.Request.Method.Method;

            var absolutePath = actionContext.Request.RequestUri.AbsolutePath.ToLower();
            var uri = HttpContext.Current.Server.UrlDecode(absolutePath);

            string parameterMessage = BuildParameterMessage(actionContext);
            string message = string.Join("\n", methodType, date, uri, parameterMessage);

            return message;
        }

        private static bool IsAuthenticated(string hashedPassword, string message, string signature)
        {
            if (string.IsNullOrEmpty(hashedPassword))
                return false;

            var verifiedHash = ComputeHash(hashedPassword, message);
            if (signature != null && signature.Equals(verifiedHash))
                return true;

            return false;
        }

        /// <summary>
        /// Ngăn chặn replay Attack
        /// </summary>
        /// <param name="timestampString"></param>
        /// <returns></returns>
        private static bool IsDateValidated(string timestampString)
        {
            DateTime timestamp;

            bool isDateTime = DateTime.TryParseExact(timestampString, "yyyyMMddHHmmssfff", null,
                DateTimeStyles.AdjustToUniversal, out timestamp);

            if (!isDateTime)
                return false;

            var now = DateTime.Now;// DateTime.UtcNow;

            // TimeStamp should not be in 5 minutes behind
            if (timestamp < now.AddMinutes(-5))
                return false;

            if (timestamp > now.AddMinutes(5))
                return false;

            return true;
        }

        /// <summary>
        /// Nếu signature đã tồn tại trong cache => request is rejected
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        private static bool IsSignatureValidated(string signature)
        {
            var memoryCache =  MemoryCache.Default;
            if (memoryCache.Contains(signature))
                return false;

            return true;
        }

        /// <summary>
        /// Thêm signature vào cache.Hết hạn sau 5 minutes
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        private static void AddToMemoryCache(string signature)
        {
            var memoryCache = MemoryCache.Default;
            if (!memoryCache.Contains(signature))
            {
                var expiration = DateTimeOffset.UtcNow.AddMinutes(5);
                memoryCache.Add(signature, signature, expiration);
            }
        }

        /// <summary>
        /// Lấy hashed password của user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private string GetHashedPassword(string username)
        {
            return "password";
            //return _authorizationSvc.GetHashedPassword(username);
        }

        private bool IsAuthenticated(HttpActionContext actionContext)
        {

            //var body1 = actionContext.Request.Content.ToString();
            //var body2 =  actionContext.Request.Content.ReadAsByteArrayAsync();

            var headers = actionContext.Request.Headers;

            // 1. Kiểm tra Timestamp request'header
            var timeStampString = GetHttpRequestHeader(headers, TimestampHeaderName);

            if (!IsDateValidated(timeStampString))
                return false;

            // 2 . Kiểm tra authentication request's header
            var authenticationString = GetHttpRequestHeader(headers, AuthenticationHeaderName);
            if (string.IsNullOrEmpty(authenticationString))
                return false;

            // authentication có 2 phần : {username}:{signature}
            var authenticationParts = authenticationString.Split(new[] { ":" },
                    StringSplitOptions.RemoveEmptyEntries);

            if (authenticationParts.Length != 2)
                return false;

            var username = authenticationParts[0];
            var signature = authenticationParts[1];

            // 3 . Kiểm tra hợp lệ của signature , nếu đã có trong cache thì không hợp lệ.
            // Mỗi request thì ở client sẽ generate 1 signature khác nhau.Do đó 2 request không thể có trùng signature
            // Có thể hiểu Signature như là OTP (One Time Password)
            if (!IsSignatureValidated(signature))
                return false;

            // 4. Add signature vào cache,đánh dấu rằng signature đã được sử dụng
            // signature này sẽ expire sau 5 phút
            AddToMemoryCache(signature);

            // 5. lấy password của user dựa vào username.Password đc sử dụng như là private key
            var hashedPassword = GetHashedPassword(username);
            var baseString = BuildBaseString(actionContext); // ============== MESSAGE ============

            return IsAuthenticated(hashedPassword, baseString, signature);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //var contentType = actionContext.Request.Content.Headers.ContentType.MediaType;
            //var requestParams = actionContext.Request.Content.ReadAsStringAsync().Result;

            //if (contentType == "application/json")
            //{
            //    return Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, object>>(requestParams);
            //}

            this._actionContext = actionContext;

            string rawDataBody = getRawPostData().Result;
            //string rawData2 = new StreamReader(HttpContext.Current.Request.InputStream).ReadToEnd();

            var isAuthenticated = IsAuthenticated(actionContext);

            if (!isAuthenticated)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                actionContext.Response = response;
            }
        }

        /// <summary>
        /// Lấy raw posted data
        /// </summary>
        /// <returns></returns>
        private async Task<String> getRawPostData()
        {
            using (var contentStream = await this._actionContext.Request.Content.ReadAsStreamAsync())
            {
                contentStream.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(contentStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

    }
}
