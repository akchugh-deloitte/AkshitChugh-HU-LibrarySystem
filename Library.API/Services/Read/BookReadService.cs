using Dapper;
using Microsoft.Data.Sqlite;
using Library.Core.DTO;

namespace Library.API.Services.Read
{
    public class BookReadService : IBookReadService
    {
        private readonly string _connectionString;

        public BookReadService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "Data Source=library.db";
        }

        public async Task<PagedResult<BookDto>> GetAvailableBooksAsync(BookFilter filter)
        {
            using var connection = new SqliteConnection(_connectionString);
            var conditions = new List<string> { "IsAvailable = 1" };
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                conditions.Add("(TITLE LIKE @Search OR Author LIKE @Search)");
                parameters.Add("@Search", $"%{filter.Search}%");
            }

            if (!string.IsNullOrEmpty(filter.Author)){
                conditions.Add("Author = @Author");
                parameters.Add("@Author", filter.Author);
            }
            if (filter.Year.HasValue)
            {
                conditions.Add("Year = @Year");
                parameters.Add("@Year", filter.Year.Value);
            }

            var whereClause = string.Join(" AND ", conditions);
            var count = $"SELECT COUNT(*) FROM Books WHERE {whereClause}";
            var total = await connection.ExecuteScalarAsync<int>(count, parameters);

            var offset = (filter.Page - 1) * filter.PageSize;
            var data = $"SELECT * FROM Books WHERE {whereClause} LIMIT @PageSize OFFSET @Offset";

            parameters.Add("@PageSize", filter.PageSize);
            parameters.Add("@Offset", offset);

            var items = await connection.QueryAsync<BookDto>(data, parameters);

            return new PagedResult<BookDto>
            {
                Items = items.ToList(),
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalCount = total
            };
        }

        public async Task<BookDto?> GetBookByIdAsync(Guid id)
        {
            using var connection = new SqliteConnection(_connectionString);
            var query = "SELECT * FROM Books WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<BookDto>(query, new { Id = id });
        }

        public async Task<List<BookDto>> GetBooksByAuthorAsync(string author)
        {
            using var connection = new SqliteConnection(_connectionString);
            var query = "SELECT * FROM Books WHERE Author = @Author";
            return (await connection.QueryAsync<BookDto>(query, new { Author = author })).ToList();
        }
    }
}
