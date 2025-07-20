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

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("mobile", new OpenApiInfo { Title = "Yu Mobile API", Version = "v1", Description = "Mobile tətbiq üçün API endpoint-ləri" });
    c.SwaggerDoc("admin", new OpenApiInfo { Title = "Yu Admin API", Version = "v1", Description = "Admin panel üçün API endpoint-ləri" });
    
    // Admin və Mobile controller-ları üçün filter
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (docName == "admin")
        {
            return apiDesc.RelativePath?.StartsWith("api/admin") == true;
        }
        else if (docName == "mobile")
        {
            return apiDesc.RelativePath?.StartsWith("api/admin") != true;
        }
        return false;
    });
});


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
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/mobile/swagger.json", "Yu Mobile API v1");
    c.SwaggerEndpoint("/swagger/admin/swagger.json", "Yu Admin API v1");
    c.RoutePrefix = string.Empty;
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();
app.UseExceptionHandling();
app.UseTokenAuthetication();
app.Urls.Add("http://192.168.100.222:5040");
app.Run();