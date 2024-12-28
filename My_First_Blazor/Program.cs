using My_First_Blazor.Components;
using Data;
using Data.Models.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

builder.Services.AddOptions<BlogApiJsonDirectAccessSettings>().Configure(options =>
{
    options.DataPath = @"..\..\..\Data\";
    options.BlogPostsFolder = "BlogPosts";
    options.TagsFolder = "tags";
    options.CatagoriesFolder = "Catagories";
    options.CommentsFolder = "Comments";
});

builder.Services.AddScoped<IBlogApi,BlogApiJsonDirectAccess>();

