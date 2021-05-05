using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace CryptoCurrencyApis
{
    public class EtherMineApiClient
    {
        private RestClient _restClient;

        // todo
        private JsonSerializerSettings _jsonSettings = new()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public const string EthApiUrl = "https://api.ethermine.org/";
        public const string EtcApiUrl = "https://api-etc.ethermine.org/";
        public bool ThrowOnInvalidAddress = false;

        public EtherMineApiClient(string url) => _restClient = new(url);

        public Task<EtherMineDashboard> GetDashboard(string wallet)
            => _request<EtherMineDashboard>($"/miner/{wallet}/dashboard");

        public Task<EtherMineHistoricalStats[]> GetHistory(string wallet)
            => _request<EtherMineHistoricalStats[]>($"/miner/{wallet}/history");

        public Task<EtherMinePayout[]> GetPayouts(string wallet)
            => _request<EtherMinePayout[]>($"/miner/{wallet}/payouts");

        public Task<EtherMineRound[]> GetRounds(string wallet)
            => _request<EtherMineRound[]>($"/miner/{wallet}/rounds");

        public Task<EtherMineFullSettings> GetSettings(string wallet)
            => _request<EtherMineFullSettings>($"/miner/{wallet}/settings");

        // todo: /miner/:miner/currentStats
        public Task<EtherMinePoolStats> GetPoolStats()
            => _request<EtherMinePoolStats>("/poolStats");

        public Task<EtherMineMinedHistoryBlock[]> GetBlockHistory()
            => _request<EtherMineMinedHistoryBlock[]>("/blocks/history");

        public Task<EtherMineNetworkStatistics> GetNetworkStatistics()
            => _request<EtherMineNetworkStatistics>("/networkStats");

        private async Task<T> _request<T>(string endpoint) where T : class
        {
            var res = await _restClient.ExecuteGetAsync(new RestRequest(endpoint));
            var data = JsonConvert.DeserializeObject<EtherMineResponse<T>>(res.Content, _jsonSettings);
            if (data!.Status == "ERROR")
            {
                if (data.Error == "Invalid address")
                {
                    if (ThrowOnInvalidAddress)
                        throw new($"EtherMine returned {data.Status}: {data.Error}")
                        {
                            Data = {["Response"] = data}
                        };
                    return null;
                }

                throw new($"EtherMine returned {data.Status}: {data.Error}")
                {
                    Data = {["Response"] = data}
                };
            }

            return data.Data;
        }
    }

    public class EtherMineResponse<T>
    {
        [JsonProperty("status")] public string Status;
        [JsonProperty("error")] public string Error;
        [JsonProperty("data")] public T Data;
    }

    #region Miner

    public class EtherMineDashboard
    {
        [JsonProperty("statistics")] public EtherMineHistoricalStats[] Statistics;
        [JsonProperty("workers")] public EtherMineWorker[] Workers;
        [JsonProperty("currentStatistics")] public EtherMineCurrentStats CurrentStatistics;
        [JsonProperty("settings")] public EtherMineSettings Settings;
    }

    public class EtherMineStats
    {
        [JsonProperty("time")] public ulong Time;
        [JsonProperty("reportedHashrate")] public ulong? ReportedHashRate;
        [JsonProperty("currentHashrate")] public decimal? CurrentHashRate;
        [JsonProperty("validShares")] public int? ValidShares;
        [JsonProperty("invalidShares")] public int? InvalidShares;
        [JsonProperty("staleShares")] public int? StaleShares;
    }

    public class EtherMineHistoricalStats : EtherMineStats
    {
        [JsonProperty("averageHashrate")] public decimal AverageHashrate;
        [JsonProperty("activeWorkers")] public int? ActiveWorkers;
    }

    public class EtherMineWorker : EtherMineStats
    {
        [JsonProperty("worker")] public string Name;
        [JsonProperty("lastSeen")] public ulong LastSeen;
    }

    public class EtherMineCurrentStats : EtherMineHistoricalStats
    {
        [JsonProperty("lastSeen")] public ulong LastSeen;
        [JsonProperty("unpaid")] public ulong UnpaidRaw;
        [JsonIgnore] public decimal Unpaid => UnpaidRaw * 0.000000000000000001M;
    }

    public class EtherMineSettings
    {
        [JsonProperty("email")] public string Email;
        [JsonProperty("monitor")] public int MonitorRaw;
        [JsonIgnore] public bool Monitor => MonitorRaw == 1;
        [JsonProperty("minPayout")] public ulong MinimalPayoutRaw;
        [JsonIgnore] public decimal MinimalPayout => MinimalPayoutRaw * 0.000000000000000001M;
    }

    public class EtherMineFullSettings : EtherMineSettings
    {
        [JsonProperty("ip")] public string Ip;
    }

    public class EtherMinePayout
    {
        [JsonProperty("paidOn")] public long PaidOnRaw;
        [JsonIgnore] public DateTimeOffset PaidOn => DateTimeOffset.FromUnixTimeSeconds(PaidOnRaw);
        [JsonProperty("start")] public long StartRaw;
        [JsonProperty("end")] public long EndRaw;
        [JsonProperty("amount")] public ulong AmountRaw;
        [JsonIgnore] public decimal Amount => AmountRaw * 0.000000000000000001M;
        [JsonProperty("txHash")] public string TxHash;
    }

    public class EtherMineRound
    {
        [JsonProperty("block")] public ulong Block;
        [JsonProperty("amount")] public ulong AmountRaw;
    }

    #endregion

    #region Pool

    public class EtherMinePoolStats
    {
        public EtherMineMinedBlock[] MinedBlocks;
        public EtherMineGeneralPoolStats PoolStats;
        public EtherMinePoolStatsPrice Price;
    }

    public class EtherMineMinedBlock
    {
        public ulong Number;
        public string Miner;
        [JsonProperty("time")] public long TimeRaw;
        [JsonIgnore] public DateTimeOffset Time => DateTimeOffset.FromUnixTimeSeconds(TimeRaw);
    }

    public class EtherMineGeneralPoolStats
    {
        public ulong HashRate;
        public uint Miners;
        public uint Workers;
    }

    public class EtherMinePoolStatsPrice
    {
        public decimal Usd;
        public decimal Btc;
    }

    public class EtherMineMinedHistoryBlock
    {
        [JsonProperty("time")] public long TimeRaw;
        [JsonIgnore] public DateTimeOffset Time => DateTimeOffset.FromUnixTimeSeconds(TimeRaw);

        public ulong NbrBlocks;
        public ulong Difficulty;
    }

    public class EtherMineNetworkStatistics
    {
        [JsonProperty("time")] public long TimeRaw;
        [JsonIgnore] public DateTimeOffset Time => DateTimeOffset.FromUnixTimeSeconds(TimeRaw);
        public long BlockTime;
        public ulong Difficulty;
        public ulong HashRate;
        public decimal Usd;
        public decimal Btc;
    }

    #endregion
}