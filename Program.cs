using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using VkWebApiApplication;
using VkWebApiApplication.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/users", async (ApplicationContext db) => await db.Users.ToListAsync());

app.MapGet("/api/users/{id:int}", async (int id, ApplicationContext db) =>
{
    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

    return Results.Json(user);
});

app.MapPost("/api/users", async (AddUser addUser, ApplicationContext db) =>
{
    int timeToAddUser = 5;
    await Task.Delay(TimeSpan.FromSeconds(timeToAddUser));

    User user = new User(addUser);

    UserGroup? userGroup = await db.UserGroups.FirstOrDefaultAsync(group => group.Id == user.UserGroupId);
    
    if (userGroup == null)
    {
        return Results.BadRequest(new { message = "Не существующий userGroupId" });
    }

    if (userGroup.Code == UserGroupCode.Admin)
    {
        User? admin = await db.Users.FirstOrDefaultAsync(u => u.UserGroupId == userGroup.Id);
        if (admin != null)
        {
            return Results.BadRequest(new { message = "Admin уже существует" });
        }
        
    }

    DateTime dateTimeToCompare = user.Created.AddSeconds(-timeToAddUser);
    User? sameLoginUser = await db.Users.FirstOrDefaultAsync(u => u.Login == user.Login & u.Created >= dateTimeToCompare);
    if (sameLoginUser != null)
    {
        return Results.BadRequest(new { message = "Пользователь с таким логином только что добавлен" });
    }


    await db.Users.AddAsync(user);
    await db.SaveChangesAsync();
    return Results.Json(user);

});

app.MapDelete("/api/users/{id:int}", async (int id, ApplicationContext db) =>
{
    User? user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

    if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });

    UserState? blockedState = await db.UserStates.FirstOrDefaultAsync(state => state.Code == UserStateCode.Blocked);

    if(blockedState == null) return Results.NotFound(new { message = "В базе отсутствует статус Blocked" });

    user.UserStateId = blockedState.Id;
    await db.SaveChangesAsync();
    return Results.Json(user);
});



app.Run();
