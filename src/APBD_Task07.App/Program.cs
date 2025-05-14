using APBD_Task07.Logic.Repositories;
using APBD_Task07.Logic.Repositories.interfaces;
using APBD_Task07.Logic.Service;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AllowSynchronousIO = true;
});


var connectionString = builder.Configuration.GetConnectionString("Database");

if (connectionString == null) throw new Exception("connection string is null");


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDeviceService, DeviceService>();
builder.Services.AddScoped<IEmbeddedDeviceRepository, EmbeddedDeviceRepository>( 
    s => new EmbeddedDeviceRepository(connectionString) );
builder.Services.AddScoped<IPersonalComputerRepository, PersonalComputerRepository>( 
    s => new PersonalComputerRepository(connectionString) );
builder.Services.AddScoped<ISmartwatchRepository, SmartwatchRepository>( 
    s => new SmartwatchRepository(connectionString) );
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>( 
    s => new DeviceRepository(connectionString) );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();