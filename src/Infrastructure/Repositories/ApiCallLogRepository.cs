using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ApiCallLogRepository : IApiCallLogRepository
    {
        private readonly ApplicationDbContext _context;

        public ApiCallLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApiCallLog>> GetAllAsync()
        {
            return await _context.ApiCallLogs.ToListAsync();
        }

        public async Task<ApiCallLog> GetByIdAsync(int id)
        {
            return await _context.ApiCallLogs.FindAsync(id);
        }

        public async Task AddAsync(ApiCallLog log)
        {
            _context.ApiCallLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
