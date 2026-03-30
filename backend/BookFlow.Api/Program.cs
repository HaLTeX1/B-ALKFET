using BookFlow.Api.Dtos;
using BookFlow.Api.Models;
using BookFlow.Api.Services;
using BookFlow.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<BookService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("frontend");

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/api/books", async (
    int page,
    int pageSize,
    string? search,
    BookService service) =>
{
    var result = await service.GetPagedAsync(page, pageSize, search);
    return Results.Ok(result);
});

app.MapGet("/api/books/{id}", async (string id, BookService service) =>
{
    var book = await service.GetByIdAsync(id);
    return book is null ? Results.NotFound() : Results.Ok(book);
});

app.MapPost("/api/books", async (CreateBookRequest request, BookService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) ||
        string.IsNullOrWhiteSpace(request.Author) ||
        string.IsNullOrWhiteSpace(request.Genre) ||
        request.PublishedYear <= 0 ||
        request.AvailableCopies < 0)
    {
        return Results.BadRequest(new { message = "Invalid request payload." });
    }

    var book = new Book
    {
        Title = request.Title.Trim(),
        Author = request.Author.Trim(),
        Genre = request.Genre.Trim(),
        PublishedYear = request.PublishedYear,
        AvailableCopies = request.AvailableCopies,
        CreatedAtUtc = DateTime.UtcNow
    };

    var created = await service.CreateAsync(book);
    return Results.Created($"/api/books/{created.Id}", created);
});

app.MapPut("/api/books/{id}", async (string id, UpdateBookRequest request, BookService service) =>
{
    if (string.IsNullOrWhiteSpace(request.Title) ||
        string.IsNullOrWhiteSpace(request.Author) ||
        string.IsNullOrWhiteSpace(request.Genre) ||
        request.PublishedYear <= 0 ||
        request.AvailableCopies < 0)
    {
        return Results.BadRequest(new { message = "Invalid request payload." });
    }

    var existing = await service.GetByIdAsync(id);
    if (existing is null)
    {
        return Results.NotFound();
    }

    existing.Title = request.Title.Trim();
    existing.Author = request.Author.Trim();
    existing.Genre = request.Genre.Trim();
    existing.PublishedYear = request.PublishedYear;
    existing.AvailableCopies = request.AvailableCopies;

    var updated = await service.UpdateAsync(id, existing);
    return updated ? Results.Ok(existing) : Results.NotFound();
});

app.MapDelete("/api/books/{id}", async (string id, BookService service) =>
{
    var deleted = await service.DeleteAsync(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();
