using WebAPIDotNet7.Dtos.DtoRequest;
using WebAPIDotNet7.Dtos.DtoResponse;

namespace WebAPIDotNet7.Interfaces.IRepos
{
    public interface ILoginRepository
    {
        Task<RegistrationResponse> Registration(RegistrationRequest request);
        Task<LoginResponse> Login(LoginRequest request);
    }
}
