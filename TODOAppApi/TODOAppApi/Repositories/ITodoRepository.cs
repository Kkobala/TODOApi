using TODOAppApi.Db.Entities;
using TODOAppApi.Models.Requests;

namespace TODOAppApi.Repositories
{
    public interface ITodoRepository
    {
        Task InsertAsync(int userId, string title, string description, DateTime deadline);
        Task<IEnumerable<TodoEntity>> SearchAsync(int userId, string title);
        Task<bool> UpdateAsync(int id, UpdateTodoRequest request);
        Task SaveChangesAsync();
    }
}
