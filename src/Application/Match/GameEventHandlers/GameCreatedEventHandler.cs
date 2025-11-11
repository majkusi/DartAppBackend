using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartAppClean.Domain.Events;
using Microsoft.Extensions.Logging;

namespace DartAppClean.Application.Match.GameEventHandlers;
public class GameCreatedEventHandler : INotificationHandler<GameCreatedEvent>
{
    private readonly ILogger<GameCreatedEventHandler> _logger;

    public GameCreatedEventHandler(ILogger<GameCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(GameCreatedEvent notification, CancellationToken cancellationToke)
    {
        _logger.LogInformation("DartAppClean Domain Event: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
