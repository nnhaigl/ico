using ICOCore.Entities.Extra;
using ICOCore.Utils.Encrypt;
using ICOCore.Utils.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ICOCore.ServicesAPIs
{
    public class BTCServiceAPI
    {

        /// <summary>
        /// Lấy tỉ giá BTC với USD
        /// Ví dụ kết quả trả về là 625 => 1 BTC = 625 USD
        /// </summary>
        /// <returns></returns>
        public static double BTCRateInDollar()
        {
            return double.Parse(GetBTCRates().Where(x => x.CurrencyCode == "USD").FirstOrDefault().Last);
        }

        /// <summary>
        /// Lấy tỉ giá BTC theo từng đơn vị tiền tệ
        /// </summary>
        /// <returns></returns>
        public static List<BTCRateWrapper> GetBTCRates()
        {
            List<BTCRateWrapper> rates = new List<BTCRateWrapper>();
            BTCRateWrapper btcRate = null;

            HttRequestData requestData = new HttRequestData();
            requestData.Method = HttpConstant.RequestMethod.GET;
            requestData.Endpoint = "https://blockchain.info/vi/ticker";

            string jsonResult = HttpUtils.MakeRequest(requestData); //@"{""key1"":""value1"",""key2"":""value2""}";

            var o = JObject.Parse(jsonResult);

            foreach (JToken child in o.Children())
            {
                btcRate = new BTCRateWrapper();
                var currencyProperty = child as JProperty;
                btcRate.CurrencyCode = currencyProperty.Name;

                foreach (JToken grandChild in child)
                {
                    foreach (JToken grandGrandChild in grandChild)
                    {
                        var property = grandGrandChild as JProperty;

                        if (property != null)
                        {
                            //Console.WriteLine(property.Name + ":" + property.Value);
                            string value = property.Value.ToString();
                            switch (property.Name)
                            {
                                case "15m": // 15 phút delay trên chợ Bitcoin
                                    break;
                                case "last":
                                    btcRate.Last = value;
                                    break;
                                case "buy":
                                    btcRate.Buy = value;
                                    break;
                                case "sell":
                                    btcRate.Sell = value;
                                    break;
                                case "symbol":
                                    btcRate.Symbol = value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                rates.Add(btcRate);
            }

            //Dictionary<string, string> rateDics = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);

            //foreach (var pair in rateDics)
            //{
            //    BTCRateWrapper wrapper = new BTCRateWrapper { CurrencyCode = pair.Key };
            //    BTCRateDetail detail = JsonConvert.DeserializeObject<BTCRateDetail>(pair.Value); // lỗi éo parse được do json format không có key
            //    wrapper.Details = detail;

            //    rates.Add(wrapper);
            //}

            return rates;
        }

        /// <summary>
        /// Chuyển Dollar ra BTC
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ConvertDollarToBTC(float value)
        {
            return ConvertCurrencyToBTC("USD", value);
        }

        /// <summary>
        /// Chuyển đổi tiền tệ thành BTC.
        /// Ví dụ muốn đổi 500 USD sang BTC => currencyCode = USD, value = 500
        /// </summary>
        /// <param name="currencyCode">Mã đơn vị tiền tệ</param>
        /// <param name="value">Số tiền cần chuyển</param>
        /// <returns></returns>
        public static double ConvertCurrencyToBTC(string currencyCode, float value)
        {
            string apiEndpoint = string.Format("https://blockchain.info/tobtc?currency={0}&value={1}", currencyCode, value);

            HttRequestData requestData = new HttRequestData();
            requestData.Method = HttpConstant.RequestMethod.GET;
            requestData.Endpoint = apiEndpoint;

            string jsonResult = HttpUtils.MakeRequest(requestData);
            return double.Parse(jsonResult);
        }

        //public static string CreateNewWalletID(string username)
        //{
        //    return SaltHelper.GetUniqueKey();
        //}

        public static string CreateNewBIAddress(string username)
        {
            return SaltHelper.GetUniqueKey();
        }

        public static bool IsValidBitAddress(string bitAddress)
        {
            int satoshi = 0;
            return IsValidBitAddress(bitAddress, out satoshi);
        }

        public static bool IsValidBitAddress(string bitAddress, out int satoshi)
        {
            satoshi = 0;
            if (string.IsNullOrWhiteSpace(bitAddress)) return false;

            var apiEndpoint = "https://blockchain.info/it/q/addressbalance/" + bitAddress;

            HttRequestData requestData = new HttRequestData();
            requestData.Method = HttpConstant.RequestMethod.GET;
            requestData.Endpoint = apiEndpoint;

            string result = string.Empty;

            try
            {
                result = HttpUtils.MakeRequest(requestData);
            }
            catch { }

            // nếu đúng địa chỉ bit address thì response là số integer thể hiện lượng BTC Amount (Đơn vị Satoshi)
            try
            {
                satoshi = int.Parse(result);
                return true;
            }
            catch { return false; }
        }

    }
}
