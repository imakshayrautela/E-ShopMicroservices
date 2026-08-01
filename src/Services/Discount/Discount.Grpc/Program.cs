//using Discount.Grpc.Data;
//using Discount.Grpc.Services;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//var dataDirectory = Path.Combine(builder.Environment.ContentRootPath, "Data");
//Directory.CreateDirectory(dataDirectory);

//var dbPath = Path.Combine(dataDirectory, "discountdb.sqlite");
//var connectionString = builder.Configuration.GetConnectionString("Database");

//// Relative SQLite paths depend on the process working directory (e.g. bin/Debug),
//// which causes "unable to open database file". Always use an absolute path.
//if (string.IsNullOrWhiteSpace(connectionString) ||
//    connectionString.Contains("Data/discountdb", StringComparison.OrdinalIgnoreCase))
//{
//    connectionString = $"Data Source={dbPath}";
//}

//// Add services to the container.
//builder.Services.AddGrpc();
//builder.Services.AddDbContext<DiscountContext>(options =>
//    options.UseSqlite(connectionString));

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//app.UseMigration();
//app.MapGrpcService<DiscountService>();
//app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

//app.Run();

using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<DiscountContext>(opts =>
        opts.UseSqlite(builder.Configuration.GetConnectionString("Database")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMigration();
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();

