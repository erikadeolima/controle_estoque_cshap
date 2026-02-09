using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _service;

    public ProductController(ProductService service)
    {
        _service = service;
    }

    [HttpGet("active")]
    public ActionResult<List<ProductActiveDto>> GetActive()
    {
        var result = _service.GetActiveProducts();
        return Ok(result);
    }
}
