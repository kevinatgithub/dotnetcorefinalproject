using DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IApiExceptionService
    {
        Task<ApiException> Create(ApiException apiException);
        Task<IList<ApiException>> GetAll();
        void ThrowTestException(TestException testException);
    }
}
