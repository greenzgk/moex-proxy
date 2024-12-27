namespace MoexProxy.DTOs;

public record DataObj(string[] Columns, object[][] Data);

public record MoexMarketResponse(DataObj Marketdata, DataObj Securities);