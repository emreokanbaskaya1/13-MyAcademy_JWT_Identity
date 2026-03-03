using _13_MyAcademy_JWT_Identity.Authorization;
using _13_MyAcademy_JWT_Identity.Context;
using _13_MyAcademy_JWT_Identity.Entities;
using _13_MyAcademy_JWT_Identity.Services.JwtServices;
using _13_MyAcademy_JWT_Identity.Services.PackageAccess;
using _13_MyAcademy_JWT_Identity.Services.Recommendation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPackageAccessService, PackageAccessService>();
builder.Services.AddSingleton<IAuthorizationHandler, PackageAuthorizationHandler>();
builder.Services.AddSingleton<ISongRecommendationService, SongRecommendationService>();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddIdentity<AppUser, AppRole>(config =>
{
    config.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;  
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.Name
    };

    // Allow JWT from query string for audio streaming
    // The browser's <audio> element cannot send Authorization headers,
    // so we read the token from the '?t=' query parameter for stream endpoints.
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = ctx =>
        {
            var path = ctx.HttpContext.Request.Path;
            if (path.StartsWithSegments("/api/songs/stream"))
            {
                var qToken = ctx.HttpContext.Request.Query["t"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(qToken))
                    ctx.Token = qToken;
            }
            return Task.CompletedTask;
        }
    };
});


// Paket bazlı yetkilendirme policy'leri
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EliteContent", policy =>
        policy.Requirements.Add(new PackageRequirement(1)));
    options.AddPolicy("PremiumContent", policy =>
        policy.Requirements.Add(new PackageRequirement(2)));
    options.AddPolicy("GoldContent", policy =>
        policy.Requirements.Add(new PackageRequirement(3)));
    options.AddPolicy("StandardContent", policy =>
        policy.Requirements.Add(new PackageRequirement(4)));
    options.AddPolicy("BasicContent", policy =>
        policy.Requirements.Add(new PackageRequirement(5)));
    options.AddPolicy("FreeContent", policy =>
        policy.Requirements.Add(new PackageRequirement(6)));
});


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});


builder.Services.AddControllersWithViews();
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

app.UseCors("AllowAll");

// Statik dosyalar (wwwroot)
app.UseStaticFiles();

// Bepop frontend template'ini /bepop yolu ile serve et
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Bepop")),
    RequestPath = "/bepop"
});

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

// ML.NET modelini uygulama başlatılırken eğit
using (var scope = app.Services.CreateScope())
{
    var recommendation = scope.ServiceProvider.GetRequiredService<ISongRecommendationService>();
    recommendation.TrainModelAsync().GetAwaiter().GetResult();
}

app.Run();
