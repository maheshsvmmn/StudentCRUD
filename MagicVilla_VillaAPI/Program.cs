using MagicVilla_VillaAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SqlKata.Compilers;
using SqlKata.Execution;
using Students_API.Configurations;
using Students_API.Services;
using Students_API.Services.Authorization;
using Students_API.Services.Health;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Security.Cryptography.Xml;
using System.Text;

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

// <<<<<<<<<<<<<<<<-------------------  added on demand ----------------------->>>>>>>>>>>>>>>>>>>

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

builder.Services.AddHealthChecks().AddCheck<ApiHealthCheck>("JokesApiChecks");

builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

// adding sql kata service

//sql server
//using var connection = new SqlConnection(builder.Configuration.GetConnectionString("defaultSQlConnection"));
//var compiler = new SqlServerCompiler();

// oracle server
using var connection = new OracleConnection(builder.Configuration.GetConnectionString("oracleConnectionString"));
var compiler = new OracleCompiler();
builder.Services.AddSingleton<QueryFactory>(new QueryFactory(connection , compiler));

//// adding authentication
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

//    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(
//    jwt =>
//    {
//        var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection(key: "JwtConfig:Secret").Value);
//        jwt.SaveToken = true;
//        jwt.TokenValidationParameters = new TokenValidationParameters()
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(key),
//            ValidateIssuer = false,//for development only
//            ValidateAudience = false,//for development only
//            RequireExpirationTime = false,//for development only, needs to be changed when tokens will be refreshed
//            ValidateLifetime = true,
//        };
//    });


// authentication
var key = "kygmtest12345678";

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddSingleton<JwtAuthenticationManager>(new JwtAuthenticationManager(key));


// applying rate limiting on api level
//builder.Services.AddRateLimiter(options =>
//{
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

app.MapHealthChecks("/health");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
