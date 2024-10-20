using Library.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Додаємо підтримку XML-формату 
builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;

})
.AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
