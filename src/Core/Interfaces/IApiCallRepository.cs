using Core.Entities;

namespace Core.Interfaces
{
    public interface IApiCallLogRepository
    {
        Task<IEnumerable<ApiCallLog>> GetAllAsync();
        Task<ApiCallLog> GetByIdAsync(int id);
        Task AddAsync(ApiCallLog log);
    }
}
