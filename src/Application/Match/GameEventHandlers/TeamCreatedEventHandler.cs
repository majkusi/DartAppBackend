using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartAppClean.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DartAppClean.Application.Match.GameEventHandlers;
public class TeamCreatedEventHandler : INotificationHandler<TeamCreatedEvent>
{
    private readonly ILogger<TeamCreatedEventHandler> _logger;

    public TeamCreatedEventHandler(ILogger<TeamCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TeamCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DartAppClean Domain Event: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}



