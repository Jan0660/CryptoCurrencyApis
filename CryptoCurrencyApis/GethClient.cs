using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace CryptoCurrencyApis
{
    public class GethClient
    {
        public RestClient RestClient;
        public GethClient(string url) => RestClient = new(url);

        public async Task<decimal> GetBalance(string wallet, string tag = "latest")
        {
            var req = new RestRequest();
            req.AddJsonBody(JsonConvert.SerializeObject(new GethRequest("eth_getBalance", wallet, tag)));
            var response = (await RestClient.ExecutePostAsync(req));
            var res = JsonConvert.DeserializeObject<BalanceResponse>(response.Content)!.Result.Substring(2);
            var result = ulong.Parse(res, NumberStyles.HexNumber);
            return result * 0.000000000000000001M;
        }

        private class BalanceResponse
        {
            [JsonProperty("result")] public string Result;
        }
    }

    public class GethRequest
    {
        [JsonProperty("id")] public int Id = 0;
        [JsonProperty("jsonrpc")] public string JsonRpcVersion = "2.0";
        [JsonProperty("method")] public string Method;
        [JsonProperty("params")] public string[] Parameters;

        public GethRequest(string method, params string[] parameters)
        {
            Method = method;
            Parameters = parameters;
        }
    }
}