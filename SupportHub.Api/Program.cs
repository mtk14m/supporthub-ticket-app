
using Microsoft.EntityFrameworkCore;
using SupportHub.Application.Repositories;
using SupportHub.Application.Tickets;
using SupportHub.Application.UseCases.Tickets;
using SupportHub.Infrastructure;
using SupportHub.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddOpenApi();

// Enregistrer les usecases
builder.Services.AddScoped<ICreateTicketUseCase, CreateTicketUseCase>();
builder.Services.AddScoped<IGetTicketUseCase, GetTicketUseCase>();
builder.Services.AddScoped<IAssignTicketUseCase, AssignTicketUseCase>();

// Enregistre le DbContext
builder.Services.AddDbContext<SupportHubDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Enregistre les repositories
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();//Ajoute le mapping des controllers

app.Run();

