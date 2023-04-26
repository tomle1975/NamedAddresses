using FluentValidation;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Tql.Agdata.NamedAddress.Domain.SeedWork;
using Tql.Agdata.NamedAddress.Infrastructure.DataMappings;
using Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework;
using Tql.Agdata.NamedAddress.Infrastructure.Services;
using Db = Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework.Tables;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAddressService, AddressEfService>();
builder.Services.AddScoped<IUnitOfWork<AddressEfDbContext>, EfUnitOfWork<AddressEfDbContext>>();
builder.Services.AddScoped<IAddressRepository<AddressEfDbContext>, AddressEfRepository>();
builder.Services.AddScoped<IRepositoryDataMapper<Address, Db.Address>, AddressRepositoryDataMapper>();
builder.Services.AddScoped<IValidator<Address>, AddressValidator>();
builder.Services.AddMemoryCache();

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
