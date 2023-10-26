using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Students_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("defaultSQlConnection"));
    option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen();

// added on demand
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<ICacheService, CacheService>();

// adding api versioning
builder.Services.AddApiVersioning(options => {
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        // new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("x-version")//,
        // new MediaTypeApiVersionReader("ver")
        );
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});


// applying rate limiting on api level
//builder.Services.AddRateLimiter(options =>
//{
//    options.AddFixedWindowLimiter("fixed",
//        policy =>
//        {
//            policy.PermitLimit = 2;
//            policy.Window = TimeSpan.FromSeconds(1);
//            policy.QueueLimit = 0;
//        });
//    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
//});


//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//    {
//        policy.AllowAnyOrigin();
//        policy.AllowAnyHeader();
//        policy.AllowAnyMethod();
//    });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseCors();
//app.UseRateLimiter();


app.UseAuthorization();

app.MapControllers();

app.Run();
