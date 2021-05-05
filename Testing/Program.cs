using System;
using System.Threading.Tasks;
using CryptoCurrencyApis;

namespace Testing
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await GethTest(new GethClient("http://192.168.1.122:3334"));
            await EtherMineTest(new EtherMineApiClient(EtherMineApiClient.EtcApiUrl));
            Console.WriteLine("Hello World!");
        }

        public static async Task EtherMineTest(EtherMineApiClient client)
        {
            // Pool
            var poolStats = await client.GetPoolStats();
            var blockHistory = await client.GetBlockHistory();
            var networkStats = await client.GetNetworkStatistics();
            // Miner
            var dashboard = await client.GetDashboard("0xDd2c32F8c25Ae6e7aFC590593f5Dfd34639e4F14");
            var history = await client.GetHistory("0xDd2c32F8c25Ae6e7aFC590593f5Dfd34639e4F14");
            var payouts = await client.GetPayouts("0xDd2c32F8c25Ae6e7aFC590593f5Dfd34639e4F14");
            var rounds = await client.GetRounds("0xDd2c32F8c25Ae6e7aFC590593f5Dfd34639e4F14");
            var settings = await client.GetSettings("0xDd2c32F8c25Ae6e7aFC590593f5Dfd34639e4F14");
        }

        public static async Task GethTest(GethClient client)
        {
            var balance = await client.GetBalance("0x125690E5322AAF0B56aAa698C1E0FAf5CE6bbdE9");
        }
    }
}