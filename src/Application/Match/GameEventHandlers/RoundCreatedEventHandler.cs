using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartAppClean.Domain.Events;
using Microsoft.Extensions.Logging;

namespace DartAppClean.Application.Match.GameEventHandlers;
public class RoundCreatedEventHandler : INotificationHandler<RoundCreatedEvent>
{
    private readonly ILogger<RoundCreatedEventHandler> _logger;

    public RoundCreatedEventHandler(ILogger<RoundCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(RoundCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DartAppClean Domain Event: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
