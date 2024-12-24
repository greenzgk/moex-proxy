namespace MoexProxy.DTOs;

public record MoexHistory(string[] Columns, object[][] Data);

public record MoexHistoryResponse(MoexHistory History);