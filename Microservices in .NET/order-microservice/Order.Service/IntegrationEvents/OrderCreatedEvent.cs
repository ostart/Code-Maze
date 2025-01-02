using ECommerce.Shared.Infrastructure.EventBus;

namespace Order.Service.IntegrationEvents;

public record OrderCreatedEvent(string CustomerId) : Event;