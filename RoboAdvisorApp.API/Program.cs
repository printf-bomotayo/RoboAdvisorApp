using Microsoft.EntityFrameworkCore;
using RoboAdvisorApp.API.Data;
using RoboAdvisorApp.API.Services.Interface;
using RoboAdvisorApp.API.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RoboAppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("RoboAppConnectionString")));

// In ConfigureServices method
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IQuestionnaireService, QuestionnaireService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();

// Configure HttpClient
builder.Services.AddHttpClient<IRecommendationService, RecommendationService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExternalApi:BaseUrl"]);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
