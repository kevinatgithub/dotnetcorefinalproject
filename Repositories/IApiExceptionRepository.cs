using DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IApiExceptionRepository
    {
        Task<IList<ApiException>> GetAll();
        Task<ApiException> Create(ApiException apiException);
    }
}
