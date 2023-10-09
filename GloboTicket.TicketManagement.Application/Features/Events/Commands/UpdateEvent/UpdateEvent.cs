using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;

public static class UpdateEvent
{
    public record UpdateEventCommand(
        Guid EventId,
        string Name,
        int Price,
        string? Artist,
        DateTime Date,
        string? Description,
        string? ImageUrl,
        Guid CategoryId
        ): IRequest;

    public class Handler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IMapper mapper;
        private readonly IAsyncRepository<Event> eventRepository;

        public Handler(IMapper mapper, IAsyncRepository<Event> eventRepository)
        {
            this.mapper = mapper;
            this.eventRepository = eventRepository;
        }

        public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventToUpdate = await eventRepository.GetByIdAsync(request.EventId);
            mapper.Map(request, eventToUpdate, typeof(UpdateEventCommand), typeof(Event));
            await eventRepository.UpdateAsync(eventToUpdate);
        }
    }
}
