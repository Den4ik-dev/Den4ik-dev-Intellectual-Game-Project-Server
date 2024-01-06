using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Application.Interfaces;
using server.Domain.Database;
using server.Domain.DTOs;
using server.Infrastructure.Services;
using server.Infrastructure.Validation;

/* @connection to database */
var builder = WebApplication.CreateBuilder(args);
string? connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApplicationContext>(
    options => options.UseLazyLoadingProxies().UseSqlite(connectionString)
);

builder.Services.AddCors();

/* @authentication and authorization */
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        options =>
            options.TokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["JWT:ISSUER"],
                ValidAudience = builder.Configuration["JWT:AUDIENCE"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:KEY"])
                )
            }
    );
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/* @services */
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<ICategoriesQuestionsService, CategoriesQuestionsService>();
builder.Services.AddScoped<IQuestionsService, QuestionsService>();
builder.Services.AddScoped<IAnswersService, AnswersService>();
builder.Services.AddScoped<IUsersQuestionsService, UsersQuestionsService>();
builder.Services.AddScoped<IUsersStatisticsService, UsersStatisticsService>();

/* @validation */
builder.Services.AddTransient<IValidator<TokenDto>, TokenDtoValidator>();
builder.Services.AddTransient<IValidator<RegisteredUserDto>, RegisteredUserDtoValidator>();
builder.Services.AddTransient<IValidator<LoginUserDto>, LoginUserDtoValidator>();
builder.Services.AddTransient<
    IValidator<AddedCategoryQuestionDto>,
    AddedCategoryQuestionDtoValidator
>();
builder.Services.AddTransient<
    IValidator<ChangedCategoryQuestionDto>,
    ChangedCategoryQuestionDtoValidator
>();
builder.Services.AddTransient<IValidator<AddedQuestionDto>, AddedQuestionDtoValidator>();
builder.Services.AddTransient<IValidator<ChangedQuestionDto>, ChangedQuestionDtoValidator>();
builder.Services.AddTransient<IValidator<AddedAnswerDto>, AddedAnswerDtoValidator>();
builder.Services.AddTransient<IValidator<ChangedAnswerDto>, ChangedAnswerDtoValidator>();

var app = builder.Build();

app.UseCors(
    options =>
        options
            .WithOrigins("http://localhost:5173", "https://localhost:5173")
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("x-total-count")
);

/* @authentication and authorization */
app.UseAuthentication();
app.UseAuthorization();

/* @use static files */
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
