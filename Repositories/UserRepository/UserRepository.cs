using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using controle_estoque_cshap.Data;
using controle_estoque_cshap.Models;

namespace controle_estoque_cshap.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
  private readonly AppDbContext _context;

  public UserRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<IEnumerable<User>> GetAllAsync()
  {
    return await _context.Users
        .AsNoTracking()
        .ToListAsync();
  }

  public async Task<User?> GetByIdAsync(int id)
  {
    return await _context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.IdUser == id);
  }

  public async Task<User?> GetByEmailAsync(string email)
  {
    var trimmedEmail = email.Trim();
    return await _context.Users
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Email == trimmedEmail);
  }

  public async Task<User> CreateAsync(User user)
  {
    _context.Users.Add(user);
    await _context.SaveChangesAsync();
    return user;
  }
}
