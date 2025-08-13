using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ConnektaVizContext>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("InsightX"));
}, ServiceLifetime.Scoped);

builder.Services.AddDbContext<ConnektaDBContext>(opt =>
{
    opt.UseSqlServer(configuration.GetConnectionString("ConnektaDb"));
}, ServiceLifetime.Scoped);

builder.Services.AddMemoryCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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
            []
        }
    });
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("*", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
builder.Services.AddTransient<IGraphRepository, GraphRepository>();
builder.Services.AddTransient<IDataRepository, DataRepository>();
builder.Services.AddTransient<IDashboardRepository, DashboardRepository>();
builder.Services.AddTransient<IDataMergeRepository, DataMergeRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAdoDatabaseConnektaViz, AdoDatabaseConnektaViz>();
builder.Services.AddSingleton<IAuthorizationHandler, CustomAuthorizationHandler>();
builder.Services.AddScoped<LoginSessionMiddleware>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettingDto>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpClient();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("*");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LoginSessionMiddleware>();

app.MapControllers();

app.Run();