using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PixViewer.Project.Service.Token;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
  });

  options.SwaggerDoc("v1", new OpenApiInfo {
    Title = "PixViewer API",
    Version = "v1",
    Contact = new OpenApiContact {
      Email = "Lucasads18@outlook.com",
      Name = "Lucas Sousa",
      Url = new Uri("https://www.linkedin.com/feed/"),
    },
    Description = "Query transactions from the PIX API to check and confirm new entries."
  });

}); 

builder.Services.AddScoped<ITokenService, TokenService>();

// TOKEN CONFIGURATION
builder.Services.AddAuthentication(options => {
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(jwt => {
  jwt.RequireHttpsMetadata = false;
  jwt.SaveToken = true;
  jwt.TokenValidationParameters = new TokenValidationParameters {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(TokenService.GetSecret()),
    ValidateIssuer = false,
    ValidateAudience = false,
    TokenDecryptionKey = new SymmetricSecurityKey(TokenService.GetSecret())
  };
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(cors => {
  cors.AllowAnyOrigin(); // url origin to service
  cors.AllowAnyHeader();
  cors.WithMethods(new string[] { "POST", "GET", "DELETE", "PUT" });
});

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
