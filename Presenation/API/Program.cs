var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddApplicationServices();


builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins(
                   "*",
                   "https://localhost:3000",
                   "http://localhost:3000",
                   "https://localhost:3001",
                   "http://localhost:3001",
                   "http://127.0.0.1:5500",
                   "http://192.168.11.80:5029",
                   "http://192.168.11.80:3001"
               )
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
        });
});

builder.Services.AddSwaggerGen();


builder.Services.AddControllers(opt => opt.Filters.Add<PermissionEndpointFilter>())
                .AddJsonOptions(opt =>
                    {
                        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });

var app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
// }
app.UseCors();

app.UseTokenAuthetication();
app.UseChangeTokenAuthetication();


app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();
app.UseExceptionHandling();
app.UseTokenAuthetication();
app.Run();