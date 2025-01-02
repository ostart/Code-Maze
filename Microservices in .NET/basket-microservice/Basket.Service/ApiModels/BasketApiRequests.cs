namespace Basket.Service.ApiModels;

public record CreateBasketRequest(string ProductId, string ProductName);

public record AddBasketProductRequest(string ProductId, string ProductName, int Quantity = 1);