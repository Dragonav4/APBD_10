using System.Text.Json;
using APBD_s31722_11.DataLayer;
using APBD_s31722_11.DataLayer.Models;
using APBD_s31722_11.Exceptions;
using APBD_s31722_11.Interfaces;
using APBD_s31722_11.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")
    ));
builder.Services.AddControllers();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IPatientService,PatientService>();
var app = builder.Build();

app.UseMiddleware<ApiExceptionMiddleware>();

app.MapControllers();
app.Run();