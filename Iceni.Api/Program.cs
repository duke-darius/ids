using System.Reflection;
using System.Security.Claims;
using System.Text;
using Iceni.Api.Services;
using Iceni.Lib.EfModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;
using Swashbuckle.AspNetCore.SwaggerUI;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(Log.Logger);

var config = builder.Configuration;

builder.Services.AddCors(feature =>
    feature.AddPolicy(
        "CorsPolicy",
        apiPolicy => apiPolicy
            //.AllowAnyOrigin()
            //.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(host => true)
            .AllowCredentials()
    ));
builder.Services.AddSingleton(config);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Iceni API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please paste JWT token into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped<PupilService>();
builder.Services.AddScoped<LessonService>();
builder.Services.AddScoped<PaymentService>();

// For Entity Framework
builder.Services.AddDbContext<IceniCtx>(options => options.UseSqlServer(config.GetConnectionString("Ctx")));
builder.Services.AddDbContextFactory<IceniCtx>(options => options.UseSqlServer(config.GetConnectionString("Ctx")),
    ServiceLifetime.Scoped);


// Identity options
builder.Services.Configure<IdentityOptions>(opts =>
{
    // Lockout
    opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
    opts.Lockout.MaxFailedAccessAttempts = 5;

    // Password
    opts.Password.RequiredLength = 8;
    opts.Password.RequireNonAlphanumeric = true;
    opts.Password.RequireDigit = true;
    opts.Password.RequireUppercase = true;
    opts.Password.RequireLowercase = true;
    opts.Password.RequiredUniqueChars = 0;
});
builder.Services.AddIdentity<IceniUser, IceniRole>()
    .AddEntityFrameworkStores<IceniCtx>()
    .AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = config["Jwt:Audience"],
            ValidIssuer = config["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Secret"] ?? throw new Exception())),
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });


var app = builder.Build();

using var scope = app.Services.CreateScope();

var ctx = scope.ServiceProvider.GetRequiredService<IceniCtx>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IceniUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IceniRole>>();

await ctx.Database.MigrateAsync();

var admRole = await roleManager.FindByNameAsync("adm");
if (admRole == null)
{
    await roleManager.CreateAsync(new IceniRole() { Name = "adm" });
}

var usrRole = await roleManager.FindByNameAsync("root");
if (usrRole == null)
{
    await roleManager.CreateAsync(new IceniRole() { Name = "root" });
}


app.UseSerilogRequestLogging(o =>
{
    o.IncludeQueryInRequestPath = true;
    o.EnrichDiagnosticContext =
        (context, httpContext) =>
        {
            var user = httpContext.User;
            var id = user.FindFirst(ClaimTypes.SerialNumber)?.Value;
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            if (!string.IsNullOrEmpty(username))
            {
                context.Set("user_name", username);
            }

            if (!string.IsNullOrEmpty(id))
            {
                context.Set("user_id", id);
            }
        };
});

app.Use(async (context, func) =>
{
    context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    context.Response.Headers.Add("Access-Control-Allow-Methods", "DELETE, POST, GET, PATCH, OPTIONS");
    context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization, X-Requested-With");
    await func();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.EnableDeepLinking();
        o.DocExpansion(DocExpansion.None);
        o.EnableFilter();
        o.DefaultModelRendering(ModelRendering.Example);
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.UseCors("CorsPolicy");

app.UseExceptionHandler(new ExceptionHandlerOptions
{
    AllowStatusCode404Response = true,
    ExceptionHandlingPath = "/error"
});

app.Run();