using System;
using RogueRunnerServer.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DB ���� Context �߰�.
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),        //appsettings.json�� ���ǵ� �Ӽ�.
    new MySqlServerVersion(new Version(8, 0, 39))));

//Player DB ���� Context �߰�
builder.Services.AddDbContext<PlayerDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 39))));

//ScoreRank DB ���� Context �߰�
builder.Services.AddDbContext<ScoreRankDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 39))));

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

//DB Ŀ�ؼ� üũ �ڵ�
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<UserDbContext>();
    try
    {
        context.Database.CanConnect();
        Console.WriteLine("User DB connection successful.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"User DB connection failed: {ex.Message}");
    }
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PlayerDbContext>();
    try
    {
        context.Database.CanConnect();
        Console.WriteLine("Player DB connection successful.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Player DB connection failed: {ex.Message}");
    }
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ScoreRankDbContext>();
    try
    {
        context.Database.CanConnect();
        Console.WriteLine("Rank DB connection successful.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Rank DB connection failed: {ex.Message}");
    }
}


app.Run();