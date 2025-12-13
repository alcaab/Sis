using Desyco.Iam.Contracts.Interfaces;
using Desyco.Iam.Contracts.Permissions;
using Desyco.Iam.Contracts.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desyco.Iam.Web.Controllers;

[ApiController]
[Route("api/roles")]
[Authorize]
public class RolesController(IRoleService roleService) : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = Permissions.Roles.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RoleDto>>> GetAll()
    {
        return Ok(await roleService.GetAllAsync());
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = Permissions.Roles.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RoleDto>> GetById(Guid id)
    {
        var role = await roleService.GetByIdAsync(id);
        if (role == null) return NotFound();
        return Ok(role);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.Roles.Write)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleDto request)
    {
        try
        {
            var role = await roleService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = Permissions.Roles.Write)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleDto request)
    {
        try
        {
            await roleService.UpdateAsync(id, request);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = Permissions.Roles.Delete)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await roleService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
