using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Exceptions;
using GloboTicket.TicketManagement.Application.Models.Mail;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;

public static class CreateEvent
{
    public record CreateEventCommand(
        string Name, 
        int Price, 
        string? Artist, 
        DateTime Date, 
        string? Description, 
        string? ImageUrl, 
        Guid CategoryId) : IRequest<Guid>;

    public class Handler : IRequestHandler<CreateEventCommand, Guid>
    {
        private readonly IMapper mapper;
        private readonly IEventRepository eventRepository;
        private readonly IEmailService emailService;

        public Handler(IMapper mapper, IEventRepository eventRepository, IEmailService emailService)
        {
            this.mapper = mapper;
            this.eventRepository = eventRepository;
            this.emailService = emailService;
        }

        public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var @event = mapper.Map<Event>(request);
            var validator = new CreateEventCommandValidation(eventRepository);
            var validatorResult = await validator.ValidateAsync(request, cancellationToken);
            if (validatorResult.Errors.Count > 0) throw new ValidationException(validatorResult);
            @event = await eventRepository.AddAsync(@event);
            var email = new Email
            {
                To = "mevaajules9@gmail.com",
                Body = "A new event was create",
                Subject = "A new event was created"
            };
            try
            {
                await emailService.SendEmail(email);
            }catch (Exception ex)
            {

            }
            return @event.EventId;
        }
    }
}
