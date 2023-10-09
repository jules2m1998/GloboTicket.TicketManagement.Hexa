using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Exceptions;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;

public static class DeleteEvent
{
    public record Command(Guid EventId): IRequest;
    public class Handler : IRequestHandler<Command>
    {
        private readonly IMapper mapper;
        private readonly IAsyncRepository<Event> eventRepository;

        public Handler(IMapper mapper, IAsyncRepository<Event> eventRepository)
        {
            this.mapper = mapper;
            this.eventRepository = eventRepository;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var eventToDelete = await eventRepository.GetByIdAsync(request.EventId) ?? throw new NotFoundException(nameof(Event), nameof(request.EventId));
            await eventRepository.DeleteAsync(eventToDelete);
        }
    }
}
