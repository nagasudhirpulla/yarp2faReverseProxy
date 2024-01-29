using Application;
using Application.Common;
using Application.Users.Commands.SeedUsers;
using Infra;
using MediatR;
using Microsoft.AspNetCore.Mvc.Authorization;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddControllers();
builder.Services.AddRazorPages()
    .AddMvcOptions(o => o.Filters.Add(new AuthorizeFilter()))
    .AddRazorRuntimeCompilation();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("gatewayAccess", policy => policy.RequireAuthenticatedUser());

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapReverseProxy();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

SeedData(app).Wait();

app.Run();

static async Task SeedData(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    _ = await mediator.Send(new SeedUsersCommand());
}