using FluentValidation;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;

public class CreateEventCommandValidation : AbstractValidator<CreateEvent.CreateEventCommand>
{
    private readonly IEventRepository eventRepository;
    public CreateEventCommandValidation(IEventRepository eventRepository)
    {
        this.eventRepository = eventRepository;

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

        RuleFor(p => p.Date)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .NotNull()
            .GreaterThan(DateTime.Now);

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .GreaterThan(0);

        RuleFor(x => x)
            .MustAsync(EventNameAndDateUnique)
            .WithMessage("An event with the same name and date already exists.");
    }



    private async Task<bool> EventNameAndDateUnique(CreateEvent.CreateEventCommand e, CancellationToken cancellationToken) => 
        !(await eventRepository.IsEventNameAndDateUnique(e.Name, e.Date, cancellationToken));
}
