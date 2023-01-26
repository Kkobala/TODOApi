using Microsoft.EntityFrameworkCore;
using TODOAppApi.Db;
using TODOAppApi.Db.Entities;
using TODOAppApi.Models.Requests;

namespace TODOAppApi.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _db;

        public TodoRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task InsertAsync(
            int userId,
            string title,
            string description,
            DateTime deadLine)
        {
            var entity = new TodoEntity();
            entity.UserId = userId;
            entity.Title = title;
            entity.Description = description;
            entity.DeadLine = deadLine;
            entity.Status = TodoStatus.New;
            entity.CreatedAt = DateTime.UtcNow;

            await _db.Todos.AddAsync(entity);
        }

        public async Task<IEnumerable<TodoEntity>> SearchAsync(int userId, string title)
        {
            return await _db.Todos
                .Where(t => t.UserId == 1)
                .Where(t => t.Title.Contains(title))
                .OrderBy(t => t.DeadLine)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(int id, UpdateTodoRequest request)
        {
            var todo = await _db.Todos.FindAsync(id);

            if (todo == null)
            {
                return false;
            }

            todo.Title = request.Title;
            todo.Description = request.Description;
            todo.DeadLine = request.DeadLine;

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
