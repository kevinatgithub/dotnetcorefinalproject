using DomainModels;
using Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class ApiExceptionService : IApiExceptionService
    {
        private readonly IApiExceptionRepository _apiExceptionRepository;

        public ApiExceptionService(IApiExceptionRepository apiExceptionRepository)
        {
            _apiExceptionRepository = apiExceptionRepository;
        }

        public async Task<ApiException> Create(ApiException apiException)
        {
            return await _apiExceptionRepository.Create(apiException);
        }

        public async Task<IList<ApiException>> GetAll()
        {
            return await _apiExceptionRepository.GetAll();
        }

        public void ThrowTestException(TestException testException)
        {
            throw testException switch
            {
                TestException.NotImplementedException => new System.NotImplementedException(),
                TestException.IndexOutOfRangeException=> new System.IndexOutOfRangeException(),
                TestException.OverflowException => new System.OverflowException(),
            };
        }
    }
}
