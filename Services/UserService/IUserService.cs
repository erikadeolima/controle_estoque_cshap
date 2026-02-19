using System.Collections.Generic;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs.UserDto;

namespace controle_estoque_cshap.Services.UserService;

public interface IUserService
{
  Task<IEnumerable<UserDto>> GetAllAsync();
  Task<UserDto?> GetByIdAsync(int id);
  Task<UserDto?> CreateAsync(UserCreateDto dto);
}
