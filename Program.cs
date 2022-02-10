using SistemaOnload.models;
using SistemaOnload.ContextDb;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();


//****************************Services*****************************
var connectionString = builder.Configuration.GetConnectionString("Conexao") ?? "Data Source=Onload.db";
builder.Services.AddCors();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddSqlite<ClienteDb>(connectionString);
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo {
         Title = "Cadastro API",
         Description = "Cadastro de clientes",
         Version = "v1" });
});


//******************************Middlewares***********************************
var app = builder.Build();
app.UseCors(p =>
{
    p.AllowAnyOrigin();
    p.AllowAnyMethod();
    p.AllowAnyHeader();
  
    
});

app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema API V1");
});


//*******************************Rotas***********************************
app.MapGet("/", async (ClienteDb db) => await db.Clientes.ToListAsync());

app.MapGet("/{id}", async (int id,ClienteDb db) => await db.Clientes.FindAsync(id));

app.MapPost("/", async (Cliente cl,ClienteDb db) =>{
     cl.Data = DateTime.Now.ToLocalTime();
     await db.Clientes.AddAsync(cl);
     db.SaveChanges();
});

app.MapPut("/{id}", async (int id,Cliente cl,ClienteDb db) =>{
     var clupdate = await db.Clientes.FindAsync(id);
     if (clupdate is null) return Results.NotFound();
     clupdate.Nome = cl.Nome;
     clupdate.Telefone = cl.Telefone;
     clupdate.Email = cl.Email;
     Console.WriteLine(cl);
     await db.SaveChangesAsync();
     return Results.NoContent();
});

app.MapDelete("/{id}", async (int id, ClienteDb db) =>{
    var cldelete = await db.Clientes.FindAsync(id);
    if(cldelete is null) return Results.NotFound();
    db.Clientes.Remove(cldelete);
    db.SaveChangesAsync();
    return Results.Ok(cldelete);
});



app.Run();
