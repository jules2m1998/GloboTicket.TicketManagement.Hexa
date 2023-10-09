using GloboTicket.TicketManagement.Application.Contracts;
using GloboTicket.TicketManagement.Domain.Entities;
using GloboTicket.TicketManagement.Persistence;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;

namespace GloboTicket.TicketManagement.Persistance.Tests;

public class GloboTicketDbContextTests
{
    public readonly GloboTicketDbContext _globoTicketDBContext;
    private readonly Mock<ILoggedInUserService> _loggerInUserServiceMock;
    private readonly string _loggedInUserId;
    public GloboTicketDbContextTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<GloboTicketDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        _loggerInUserServiceMock = new Mock<ILoggedInUserService>();
        _loggedInUserId = Guid.NewGuid().ToString();
        _loggerInUserServiceMock.SetupGet(x => x.UserId).Returns(_loggedInUserId);

        _globoTicketDBContext = new GloboTicketDbContext(dbContextOptions, _loggerInUserServiceMock.Object);
    }

    [Fact]
    public async Task Save_SetCreatedByProperty()
    {
        var ev = new Event() { EventId = Guid.NewGuid(), Name = "Tests" };
        _globoTicketDBContext.Events.Add(ev);
        await _globoTicketDBContext.SaveChangesAsync();

        ev.CreatedBy.ShouldBe(_loggedInUserId);
    }

    [Fact]
    public async Task Update_SetLastModifiedByProperty()
    {
        var ev = new Event() { EventId = Guid.NewGuid(), Name = "Tests" };
        _globoTicketDBContext.Events.Add(ev);
        await _globoTicketDBContext.SaveChangesAsync();

        ev.Name = "Tester de test";
        _globoTicketDBContext.Events.Update(ev);
        await _globoTicketDBContext.SaveChangesAsync();

        ev.LastModifiedBy.ShouldBe(_loggedInUserId);
    }
}
