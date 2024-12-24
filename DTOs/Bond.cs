namespace MoexProxy.DTOs;

public record Bond(string Date, decimal Close, decimal Low, decimal High, int Volume);