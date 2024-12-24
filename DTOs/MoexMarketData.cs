namespace MoexProxy.DTOs;

public record MoexMarket(string[] Columns, object[][] Data);

public record MoexMarketResponse(MoexMarket Marketdata);