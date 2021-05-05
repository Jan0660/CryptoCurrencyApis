# CryptoCurrencyApis
[![NuGet](https://img.shields.io/nuget/v/CryptoCurrencyApis)](https://www.nuget.org/packages/CryptoCurrencyApis/)
[![NuGet Prerelease](https://img.shields.io/nuget/vpre/CryptoCurrencyApis)](https://www.nuget.org/packages/CryptoCurrencyApis/)
[![chat on discord](https://img.shields.io/discord/749601186155462748?logo=discord)](https://discord.gg/zBbV56e)
A small library for using APIs for cryptos in .NET. Currently only ethermine.org and go-ethereum APIs are available in an unfinished state.

## Use
```csharp
using CryptoCurrencyApis;
```
### EtherMine
```csharp
var client = new EtherMineApiClient(EtherMineApiClient.EtcApiUrl);
// or if you're using the ETH api:
// var client == new EtherMineApiClient(EtherMineApiClient.EthApiUrl)
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
```
### Go Ethereum
```csharp
var client = new GethClient("URL:PORT");
var balance = await client.GetBalance("0x125690E5322AAF0B56aAa698C1E0FAf5CE6bbdE9");
```

## Support
- ethermine
  - Everything except `/servers/history` and the `Worker` category is implemented.
- Go Ethereum
  - only the `GetBalance` method is available
