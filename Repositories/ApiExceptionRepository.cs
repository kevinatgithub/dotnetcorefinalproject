using DomainModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class ApiExceptionRepository : IApiExceptionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ApiExceptionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApiException> Create(ApiException apiException)
        {
            var record = await _dbContext.ApiExceptions.AddAsync(apiException);
            await _dbContext.SaveChangesAsync();
            return record.Entity;
        }

        public async Task<IList<ApiException>> GetAll()
        {
            return await _dbContext.ApiExceptions.ToListAsync();
        }
    }
}
