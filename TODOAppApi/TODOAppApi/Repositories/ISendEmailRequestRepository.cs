using TODOAppApi.Db.Entities;

namespace TODOAppApi.Repositories
{
    public interface ISendEmailRequestRepository
    {
        void Insert(SendEmailRequestEntity entity);
        Task SaveChangesAsync();
    }
}
