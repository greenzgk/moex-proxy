using System.Globalization;
using MoexProxy.DTOs;

namespace MoexProxy.Services;

public class BondRecalcService(HttpClient httpClient)
{
    private readonly HttpClient httpClient = httpClient;

    public async Task<IEnumerable<Bond>> GetBondHistoryData(string board, string symbol, int limit, string from)
    {
        var query = $"history/engines/stock/markets/bonds/boards/{board}/securities/{symbol}.json?history.columns=TRADEDATE,CLOSE,LOW,HIGH,VOLUME,FACEVALUE&iss.meta=off&limit={limit}&from={from}";

        var response = await httpClient.GetFromJsonAsync<MoexHistoryResponse>(query);

        if (response == null) return [];

        var result = new List<Bond>(response.History.Data.Length);

        foreach (var data in response.History.Data)
        {
            var date = data[0].ToString()!;
            var facevalue = Math.Round(decimal.Parse(data[5].ToString()!), 2);
            var close = Math.Round(decimal.Parse(data[1].ToString()!, CultureInfo.InvariantCulture) * facevalue / 100, 2);
            var low = Math.Round(decimal.Parse(data[2].ToString()!, CultureInfo.InvariantCulture) * facevalue / 100, 2);
            var high = Math.Round(decimal.Parse(data[3].ToString()!, CultureInfo.InvariantCulture) * facevalue / 100, 2);
            var vol = int.Parse(data[4].ToString()!, CultureInfo.InvariantCulture);

            result.Add(new Bond(date, close, low, high, vol));
        }

        return result;
    }

    public async Task<IEnumerable<Bond>> GetBondMarketData(string board, string symbol)
    {
        var query = $"engines/stock/markets/bonds/boards/{board}/securities/{symbol}.json?iss.meta=off&iss.only=marketdata,securities&marketdata.columns=SYSTIME,LAST,HIGH,LOW,VOLTODAY&securities.columns=FACEVALUE";

        var response = await httpClient.GetFromJsonAsync<MoexMarketResponse>(query);

        if (response == null || response.Marketdata.Data.Length == 0) return [];

        var result = new List<Bond>(response.Marketdata.Data.Length);

        var marketData = response.Marketdata.Data[0];
        var securitiesData = response.Securities.Data[0];

        var date = marketData[0].ToString()!;
        var facevalue = decimal.Parse(securitiesData[0].ToString()!, CultureInfo.InvariantCulture);
        var last = decimal.Parse(marketData[1].ToString()!, CultureInfo.InvariantCulture);

        var close = Math.Round(last * facevalue / 100, 2);
        var low = Math.Round(decimal.Parse(marketData[2].ToString()!, CultureInfo.InvariantCulture) * facevalue / 100, 2);
        var high = Math.Round(decimal.Parse(marketData[3].ToString()!, CultureInfo.InvariantCulture) * facevalue / 100, 2);
        var vol = int.Parse(marketData[4].ToString()!, CultureInfo.InvariantCulture);

        result.Add(new Bond(date, close, low, high, vol));

        return result;
    }
}

