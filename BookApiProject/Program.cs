using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;

// Create a WebApplication builder instance
var builder = WebApplication.CreateBuilder(args);

// Register AutoMapper for object mapping (DTO ↔ Entities)
builder.Services.AddAutoMapper(typeof(Program));

// Add support for API controllers
builder.Services.AddControllers();

// Bind JWT settings from configuration file (appsettings.json)
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

// Retrieve JWT settings object from configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

// Convert JWT secret key to byte array
if (jwtSettings == null)
{
    throw new Exception("JwtSettings section is missing in configuration");
}

var keyBytes = Encoding.ASCII.GetBytes(jwtSettings.Key);

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    // Set default authentication scheme to JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure JWT token validation parameters
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, // Ensure token signature is valid
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes), // Secret key for signing
        ValidateIssuer = true, // Validate token issuer
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true, // Validate token audience
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true, // Ensure token is not expired
        ClockSkew = TimeSpan.Zero // No extra time tolerance for expiration
    };

    // Handle "Forbidden" (403) responses
    options.Events = new JwtBearerEvents
    {
        OnForbidden = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var result = JsonSerializer.Serialize(new
            {
                error = "Forbidden",
                detail = "You do not have permission to access this resource."
            });

            return context.Response.WriteAsync(result);
        }
    };
});

// Add FluentValidation for model validation
builder.Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

// Register validators for DTOs
builder.Services.AddValidatorsFromAssemblyContaining<BookCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AuthorCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

// Enable API endpoint metadata exploration
builder.Services.AddEndpointsApiExplorer();

// Register Swagger for API documentation
builder.Services.AddSwaggerGen();

// Configure Swagger with JWT authentication support
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT token in the format: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
});

// Include XML comments in Swagger (for method summaries, parameters, etc.)
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Register application services with dependency injection
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddSingleton<IAuthService, AuthService>();

// Build the application
var app = builder.Build();

// Enable Swagger only in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use custom exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Redirect HTTP to HTTPS
app.UseHttpsRedirection();

// Map API controllers to routes
app.MapControllers();

// Enable authentication & authorization
app.UseAuthentication();
app.UseAuthorization();

// Run the application
app.Run();

