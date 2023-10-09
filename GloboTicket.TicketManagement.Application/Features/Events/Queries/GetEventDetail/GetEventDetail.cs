using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;

public static class GetEventDetail
{
    public record Query(Guid Id) : IRequest<EventDetailVm>;
    public class Handler : IRequestHandler<Query, EventDetailVm>
    {
        private readonly IMapper mapper;
        private readonly IAsyncRepository<Event> eventRepo;
        private readonly IAsyncRepository<Category> categoryRepo;

        public Handler(IMapper mapper, IAsyncRepository<Event> eventRepo, IAsyncRepository<Category> categoryRepo)
        {
            this.mapper = mapper;
            this.eventRepo = eventRepo;
            this.categoryRepo = categoryRepo;
        }
        public async Task<EventDetailVm> Handle(Query request, CancellationToken cancellationToken)
        {
            var @event = await eventRepo.GetByIdAsync(request.Id);
            var eventDetailDto = mapper.Map<EventDetailVm>(@event);

            var category = await categoryRepo.GetByIdAsync(@event!.CategoryId);
            eventDetailDto.Category = mapper.Map<CategoryDto>(category);

            return mapper.Map<EventDetailVm>(eventDetailDto);
        }
    }
}
