using GloboTicket.TicketManagement.Api;

var builder = WebApplication.CreateBuilder(args);

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

// To apply migrations
await app.ResetDatabaseAsync();

app.Run();

public partial class Program { }
