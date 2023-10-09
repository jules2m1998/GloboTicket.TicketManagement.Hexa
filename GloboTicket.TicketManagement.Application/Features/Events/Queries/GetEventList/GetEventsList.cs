using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventList;

public static class GetEventsList
{
    public record Query() : IRequest<List<EventListVm>>;
    public class Handler : IRequestHandler<Query, List<EventListVm>>
    {
        private readonly IMapper mapper;
        private readonly IAsyncRepository<Event> eventRepo;

        public Handler(IMapper mapper, IAsyncRepository<Event> eventRepo)
        {
            this.mapper = mapper;
            this.eventRepo = eventRepo;
        }

        public async Task<List<EventListVm>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = (await eventRepo.ListAllAsync()).OrderBy(x => x.Date);
            return mapper.Map<List<EventListVm>>(result);
        }
    }
}
