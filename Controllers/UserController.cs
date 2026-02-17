using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using controle_estoque_cshap.DTOs.UserDto;
using controle_estoque_cshap.Services.UserService;

namespace controle_estoque_cshap.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
  private readonly IUserService _userService;
  private readonly ILogger<UserController> _logger;

  public UserController(IUserService userService, ILogger<UserController> logger)
  {
    _userService = userService;
    _logger = logger;
  }

  /// <summary>
  /// Returns the list of users.
  /// </summary>
  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<UserDto>), 200)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
  {
    try
    {
      var users = await _userService.GetAllAsync();
      return Ok(users);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao obter usuarios.");
      return StatusCode(500, new { message = "Erro ao obter usuarios." });
    }
  }

  /// <summary>
  /// Returns a user by id.
  /// </summary>
  [HttpGet("{id:int}")]
  [ProducesResponseType(typeof(UserDto), 200)]
  [ProducesResponseType(404)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<UserDto>> GetById(int id)
  {
    try
    {
      var user = await _userService.GetByIdAsync(id);
      if (user == null)
        return NotFound(new { message = "Usuario nao encontrado." });

      return Ok(user);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao obter usuario.");
      return StatusCode(500, new { message = "Erro ao obter usuario." });
    }
  }

  /// <summary>
  /// Creates a new user.
  /// </summary>
  [HttpPost]
  [ProducesResponseType(typeof(UserDto), 201)]
  [ProducesResponseType(400)]
  [ProducesResponseType(409)]
  [ProducesResponseType(500)]
  public async Task<ActionResult<UserDto>> Create([FromBody] UserCreateDto dto)
  {
    try
    {
      var created = await _userService.CreateAsync(dto);
      if (created == null)
        return Conflict(new { message = "Email ja existe." });

      return CreatedAtAction(nameof(GetById), new { id = created.IdUser }, created);
    }
    catch (ArgumentException ex)
    {
      _logger.LogWarning(ex, "Dados invalidos ao criar usuario.");
      return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Erro ao criar usuario.");
      return StatusCode(500, new { message = "Erro ao criar usuario." });
    }
  }
}
