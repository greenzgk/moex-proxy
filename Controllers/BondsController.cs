using Microsoft.AspNetCore.Mvc;
using MoexProxy.DTOs;
using MoexProxy.Services;

namespace MoexProxy.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BondsController(BondRecalcService serv) : ControllerBase
{
    [HttpGet("history/boards/{board}/securities/{symbol}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Bond>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> History(string board, string symbol, int limit, string from)
    {
        // TODO
        // 1. Request data from ISS.MOEX (TRADEDATE,CLOSE,LOW,HIGH,VOLUME, FACEVALUE) throught HttpClient
        // 2. Calc bond price
        // 3. Return data with replaced price (% -> RUB)
        return Ok(await serv.GetBondHistoryData(board, symbol, limit, from));
    }

    [HttpGet("market/boards/{board}/securities/{symbol}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Market(string board, string symbol)
    {
        return Ok(await serv.GetBondMarketData(board, symbol));
    }
}

// https://iss.moex.com/iss/history/engines/stock/markets/bonds/boards/TQOB/securities/SU26238RMFS4.json?history.columns=TRADEDATE,CLOSE,LOW,HIGH,VOLUME&iss.meta=off&limit=100&from={DATE:yyyy-MM-01}
// https://iss.moex.com/iss/engines/stock/markets/bonds/boards/TQOB/securities/SU26230RMFS1.json?iss.meta=off&iss.only=marketdata&marketdata.columns=SYSTIME,LAST,HIGH,LOW,VOLTODAY

// https://iss.moex.com/iss/history/engines/stock/markets/bonds/boards/TQCB/securities/RU000A107UU5.json?history.columns=TRADEDATE,CLOSE,LOW,HIGH,VOLUME&iss.meta=off&limit=100&from={DATE:yyyy-MM-01}