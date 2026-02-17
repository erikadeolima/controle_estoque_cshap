using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using controle_estoque_cshap.DTOs.UserDto;
using controle_estoque_cshap.Models;
using controle_estoque_cshap.Repositories.UserRepository;

namespace controle_estoque_cshap.Services.UserService;

public class UserService : IUserService
{
  private readonly IUserRepository _userRepository;

  public UserService(IUserRepository userRepository)
  {
    _userRepository = userRepository;
  }

  public async Task<IEnumerable<UserDto>> GetAllAsync()
  {
    var users = await _userRepository.GetAllAsync();
    return users.Select(MapToDto);
  }

  public async Task<UserDto?> GetByIdAsync(int id)
  {
    var user = await _userRepository.GetByIdAsync(id);
    return user == null ? null : MapToDto(user);
  }

  public async Task<UserDto?> CreateAsync(UserCreateDto dto)
  {
    if (string.IsNullOrWhiteSpace(dto.Name))
      throw new System.ArgumentException("Name e obrigatorio.", nameof(dto.Name));

    if (string.IsNullOrWhiteSpace(dto.Email))
      throw new System.ArgumentException("Email e obrigatorio.", nameof(dto.Email));

    if (!dto.Email.Contains('@'))
      throw new System.ArgumentException("Email invalido.", nameof(dto.Email));

    if (string.IsNullOrWhiteSpace(dto.Profile))
      throw new System.ArgumentException("Profile e obrigatorio.", nameof(dto.Profile));

    var existing = await _userRepository.GetByEmailAsync(dto.Email);
    if (existing != null)
      return null;

    var user = new User
    {
      Name = dto.Name.Trim(),
      Email = dto.Email.Trim(),
      Profile = dto.Profile.Trim()
    };

    await _userRepository.CreateAsync(user);
    return MapToDto(user);
  }

  private static UserDto MapToDto(User user)
  {
    return new UserDto
    {
      IdUser = user.IdUser,
      Name = user.Name,
      Email = user.Email,
      Profile = user.Profile
    };
  }
}
