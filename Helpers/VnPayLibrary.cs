using MovieManagementSystem.Payloads.DataResponses;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace MovieManagementSystem.Helpers
{
    [JsonObject]
    public class VnPayLibrary
    {
        public const string VERSION = "2.1.0";
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private readonly SortedList<string, string> _responseData = new SortedList<string, string>(new VnPayCompare());

        public void AddRequestData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _requestData.Add(key, value);
            }
        }

        public void AddResponseData(string key, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _responseData.Add(key, value);
            }
        }

        public string GetResponseData(string key)
        {
            return _responseData.TryGetValue(key, out var retValue) ? retValue : string.Empty;
        }

        #region Request

        //public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        //{
        //    StringBuilder data = new StringBuilder();
        //    foreach (KeyValuePair<string, string> kv in _requestData)
        //    {
        //        if (!String.IsNullOrEmpty(kv.Value))
        //        {
        //            data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
        //        }
        //    }
        //    string queryString = data.ToString();

        //    baseUrl += "?" + queryString;
        //    String signData = queryString;
        //    if (signData.Length > 0)
        //    {
        //        signData = signData.Remove(data.Length - 1, 1);
        //    }
        //    string vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);
        //    baseUrl += "vnp_SecureHash=" + vnp_SecureHash;

        //    return baseUrl;
        //}       

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            var queryString = BuildQueryString(_requestData);
            var signData = queryString.TrimEnd('&');
            var vnp_SecureHash = Utils.HmacSHA512(vnp_HashSecret, signData);
            baseUrl += "?" + queryString + "vnp_SecureHash=" + vnp_SecureHash;
            return baseUrl;
        }

        private string BuildQueryString(SortedList<string, string> data)
        {
            StringBuilder queryString = new StringBuilder();
            foreach (KeyValuePair<string, string> kv in data)
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    queryString.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            return queryString.ToString();
        }

        #endregion

        #region Response process

        public bool ValidateSignature(string inputHash, string secretKey)
        {
            string rspRaw = GetResponseData();
            string myChecksum = Utils.HmacSHA512(secretKey, rspRaw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
        private string GetResponseData()
        {

            StringBuilder data = new StringBuilder();
            if (_responseData.ContainsKey("vnp_SecureHashType"))
            {
                _responseData.Remove("vnp_SecureHashType");
            }
            if (_responseData.ContainsKey("vnp_SecureHash"))
            {
                _responseData.Remove("vnp_SecureHash");
            }
            foreach (KeyValuePair<string, string> kv in _responseData)
            {
                if (!String.IsNullOrEmpty(kv.Value))
                {
                    data.Append(WebUtility.UrlEncode(kv.Key) + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                }
            }
            //remove last '&'
            if (data.Length > 0)
            {
                data.Remove(data.Length - 1, 1);
            }
            return data.ToString();
        }

        #endregion
    }

    public class Utils
    {
        public static String HmacSHA512(string key, String inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
        public static string GetIpAddress(HttpContext context)
        {
            var ipAddress = string.Empty;
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;
                if(remoteIpAddress != null)
                {
                    //if(remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    //{
                    //    remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                    //        .FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                    //}
                    //if(remoteIpAddress != null) ipAddress = remoteIpAddress.ToString();
                    //return ipAddress;

                    if (remoteIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        // Nếu là IPv6, thử chuyển đổi sang IPv4
                        var ipv4Address = remoteIpAddress.MapToIPv4();
                        ipAddress = ipv4Address.ToString();
                    }
                    else
                    {
                        // Nếu là IPv4 hoặc khác, lấy địa chỉ IP trực tiếp
                        ipAddress = remoteIpAddress.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP:" + ex.Message;
            }

            return ipAddress;
        }
    }

    public class VnPayCompare : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var vnpCompare = CompareInfo.GetCompareInfo("en-US");
            return vnpCompare.Compare(x, y, CompareOptions.Ordinal);
        }
    }
}
