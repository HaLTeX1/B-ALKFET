using BookFlow.Api.Dtos;
using BookFlow.Api.Models;
using BookFlow.Api.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BookFlow.Api.Services;

public class BookService
{
    private readonly IMongoCollection<Book> _booksCollection;

    public BookService(IOptions<MongoDbSettings> mongoDbSettings)
    {
        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Book>(
            mongoDbSettings.Value.BooksCollectionName);
    }

    public async Task<PagedResult<Book>> GetPagedAsync(int page, int pageSize, string? search)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 5 : pageSize;

        var filter = Builders<Book>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(search))
        {
            filter = Builders<Book>.Filter.Or(
                Builders<Book>.Filter.Regex(x => x.Title, new MongoDB.Bson.BsonRegularExpression(search, "i")),
                Builders<Book>.Filter.Regex(x => x.Author, new MongoDB.Bson.BsonRegularExpression(search, "i")),
                Builders<Book>.Filter.Regex(x => x.Genre, new MongoDB.Bson.BsonRegularExpression(search, "i"))
            );
        }

        var totalCount = await _booksCollection.CountDocumentsAsync(filter);

        var items = await _booksCollection
            .Find(filter)
            .SortByDescending(x => x.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync();

        return new PagedResult<Book>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<Book?> GetByIdAsync(string id) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<Book> CreateAsync(Book newBook)
    {
        await _booksCollection.InsertOneAsync(newBook);
        return newBook;
    }

    public async Task<bool> UpdateAsync(string id, Book updatedBook)
    {
        updatedBook.Id = id;
        var result = await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);
        return result.MatchedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _booksCollection.DeleteOneAsync(x => x.Id == id);
        return result.DeletedCount > 0;
    }
}