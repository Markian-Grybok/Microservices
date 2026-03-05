using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Gateway",
        Version = "v1",
        Description = "Gateway для UserService та ContentService"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(
        "/users-swagger/swagger/v1/swagger.json",
        "UserService");
    options.SwaggerEndpoint(
        "/content-swagger/swagger/v1/swagger.json",
        "ContentService");
});

app.MapReverseProxy();

app.Run();