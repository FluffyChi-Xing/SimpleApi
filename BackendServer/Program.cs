using BackendServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var serverName = builder.Configuration["ServerName"];
if (string.IsNullOrWhiteSpace(serverName)) {
    throw new Exception("Server name is not set.");
}

Server.Name = serverName;

var app = builder.Build();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();