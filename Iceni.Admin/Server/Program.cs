using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddCors(feature =>
    feature.AddPolicy(
        "CorsPolicy",
        apiPolicy => apiPolicy
            //.AllowAnyOrigin()
            //.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(host => true)
            .AllowCredentials()
    ));
builder.WebHost.UseKestrel();
var app = builder.Build();

Console.WriteLine(builder.Environment.WebRootPath);

Console.WriteLine(builder.Environment.ContentRootPath);

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseHsts();
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Content-Security-Policy", string.Join(';',
        "base-uri 'self'",
        "default-src 'self' https://localhost https://www.google.com/recaptcha/api2/anchor https://www.google.com/recaptcha/api.js", 
        "img-src data: https:; object-src 'none'",
        "script-src 'self' https://maps.googleapis.com https://www.gstatic.com 'unsafe-hashes' 'unsafe-inline' 'unsafe-eval'",
        "font-src 'self' https://fonts.gstatic.com",
        "style-src 'self' 'unsafe-inline' https://fonts.googleapis.com",
        "upgrade-insecure-requests",
        $"connect-src 'self' wss://localhost:* https://localhost:* {builder.Configuration["BaseApiUrl"]}",
        "script-src-elem 'self' 'unsafe-inline' https://maps.googleapis.com https://*.google.com https://google.com https://www.gstatic.com https://*.gstatic.com"
    ));

    await next();
});

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

