using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VoteMovie.Entity;
using VoteMovie.Infrastructure.Security;
using VoteMovie.Model.Config;
using VoteMovie.Repositories;
using VoteMovie.Repositories.Abstract;
using VoteMovie.Services;
using VoteMovie.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);
// Configuration
var Configuration = builder.Configuration;
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MoviewAppContext>(options =>
{
    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging();
});
// Enable Cors
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder
        .AllowAnyOrigin()
         .WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader();
}));
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Add("sub", ClaimTypes.NameIdentifier);
builder.Services.AddControllers();
builder.Services.AddMvc(option => option.EnableEndpointRouting = false)
    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:SecretKey"]));
var issurer = Configuration["Jwt:JwtIssuerOptions:Issuer"];
var audience = Configuration["Jwt:JwtIssuerOptions:Audience"];
var tokenValidationParameters = new TokenValidationParameters
{
    NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = signingKey,
    ValidateIssuer = true,
    ValidIssuer = issurer,
    ValidateAudience = true,
    ValidAudience = audience,
    ValidateLifetime = true,
    RequireExpirationTime = true,
    ClockSkew = TimeSpan.Zero
};
builder.Services.AddMvc(config =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
    config.RespectBrowserAcceptHeader = true;
    config.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
});
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddJwtBearer(options => { options.TokenValidationParameters = tokenValidationParameters; });
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Account/Forbidden/";
        options.LoginPath = "/Account/Unauthorized/";
    });
// Configure JwtIssuerOptions
builder.Services.Configure<JwtConfiguration>(options =>
{
    options.Issuer = issurer;
    options.Audience = audience;
    options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IEncryptionRepository, EncryptionRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Services.AddSignalRCore();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v3", new OpenApiInfo { Title = "MovieApp", Version = "3.0.3" });
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    };
    c.AddSecurityRequirement(securityRequirement);
});
var app = builder.Build();
// Configure the HTTP request pipeline.
app.Logger.LogInformation(app.Environment.EnvironmentName);
app.UseHttpLogging();
if (app.Environment.IsDevelopment())
{
    // app.UseDeveloperExceptionPage();
    // using (var scope = app.Services.CreateScope())
    // {
    //     // auto delete and create new db on run
    //     var dbContext = scope.ServiceProvider.GetRequiredService<MoviewAppContext>();
    //     dbContext.Database.EnsureDeleted();
    //     dbContext.Database.EnsureCreated();
    // }
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v3/swagger.json", "MovieApp"));
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();