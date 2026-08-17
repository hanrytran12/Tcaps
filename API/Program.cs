using API.Extensions;

HostExtensions.LoadDotEnvFromCurrentOrParentDirectory();

var builder = WebApplication.CreateBuilder(args);
builder.AddApiServices();
builder.ConfigureHost();

var app = builder.Build();
await app.MigrateAndSeedDatabaseAsync();
app.UseApiPipeline();

app.Run();
