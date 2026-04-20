
using SupportHub.Application.Tickets;
using SupportHub.Application.UseCases.Tickets;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddOpenApi();

// Enregistrer les usecases
builder.Services.AddScoped<ICreateTicketUseCase, CreateTicketUseCase>();
builder.Services.AddScoped<IGetTicketUseCase, GetTicketUseCase>();
builder.Services.AddScoped<IAssignTicketUseCase, AssignTicketUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();//Ajoute le mapping des controllers

app.Run();

