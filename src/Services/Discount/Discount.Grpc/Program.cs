using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Database")
    ?? throw new InvalidOperationException("Connection string 'Database' is not configured.");

connectionString = EnsureSqliteDatabasePath(connectionString, builder.Environment.ContentRootPath);

builder.Services.AddGrpc();
builder.Services.AddDbContext<DiscountContext>(opts =>
    opts.UseSqlite(connectionString));

var app = builder.Build();

app.UseMigration();
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

static string EnsureSqliteDatabasePath(string connectionString, string contentRootPath)
{
    const string prefix = "Data Source=";
    var parts = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    var dataSourcePart = parts.FirstOrDefault(p => p.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

    if (dataSourcePart is null)
        return connectionString;

    var dataSource = dataSourcePart[prefix.Length..].Trim();
    var dbPath = Path.IsPathRooted(dataSource)
        ? dataSource
        : Path.GetFullPath(Path.Combine(contentRootPath, dataSource));

    var directory = Path.GetDirectoryName(dbPath);
    if (!string.IsNullOrWhiteSpace(directory))
        Directory.CreateDirectory(directory);

    var otherParts = parts.Where(p => !p.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    return string.Join(';', new[] { $"{prefix}{dbPath}" }.Concat(otherParts));
}
