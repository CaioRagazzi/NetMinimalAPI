using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Recupera string conexão com banco de dados
var connectionString = builder.Configuration.GetConnectionString("AppDb");
//Realiza injeção de dependência do Entity Framework
builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionString));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/Aubilous", (AppDbContext context, AubilousDto aubilousDto) =>
{
    var aubilous = new Aubilous
    {
        FirstName = aubilousDto.FirstName,
        LastName = aubilousDto.LastName
    };
    context.Aubilous.Add(aubilous);
    context.SaveChanges();
    return Results.Ok();
});

app.MapGet("/Aubilous", (AppDbContext context) =>
{
    var aubilous = context.Aubilous;
    return aubilous is not null ? Results.Ok(aubilous) : Results.NotFound();
});

app.UseSwagger();
app.UseSwaggerUI();
app.Run();
