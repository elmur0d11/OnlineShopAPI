using Microsoft.EntityFrameworkCore;
using OnlineShopAPIFull.Data;
using OnlineShopAPIFull.Services;
using OnlineShopAPIFull.Services.Caching;
using OnlineShopAPIFull.Services.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContextPool<AppDbContext>(options => options.UseNpgsql(
    builder.Configuration.GetConnectionString("ShopConnection")
    ));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBuyedProductRepository, BuyedProductRepository>();
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Host.SerilogConfiguration();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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