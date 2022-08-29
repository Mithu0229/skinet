
using API.Errors;
using API.Helpers;
using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddScoped<IBasketRepository,BasketRepository>();

builder.Services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<StoreContext>(x=>
x.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<IConnectionMultiplexer>(c=>{
    var configuration=ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"),true);
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApiBehaviorOptions>(options=> //for error
{
    options.InvalidModelStateResponseFactory=actionContext=>
    {
        var errors=actionContext.ModelState.Where(e=>e.Value.Errors.Count>0)
        .SelectMany(x=>x.Value.Errors)
        .Select(x=>x.ErrorMessage).ToArray();
        var errorResponse = new ApiValidationErrorReponse{
            Errors=errors
        };
        return new BadRequestObjectResult(errorResponse);
    };
});

builder.Services.AddCors(opt=>{//for cors
    opt.AddPolicy("CorsPolicy",policy=>{
        policy.WithOrigins( "http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
    });
});

var app = builder.Build();

    // app.UseStaticFiles(new StaticFileOptions()
    // {
    //     FileProvider = new PhysicalFileProvider(
    //         Path.Combine(Directory.GetCurrentDirectory(), @"images")),
    //     RequestPath = new PathString("images/")
    // });
    app.UseStaticFiles();
// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();//for error
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStatusCodePagesWithReExecute("errors/{0}"); //for error

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");//for cors
app.UseAuthorization();

app.MapControllers();

app.Run();
