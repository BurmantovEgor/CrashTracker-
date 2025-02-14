using TestApplication.Application.DTO_s;
using TestApplication.DataBase.Entities;

namespace TestApplication.Core.Abstractions.User
{
    public interface IAuthService
    {
        string GenerateJwtToken(UserEntity  user);
    }
}
