using Desyco.Iam.Contracts.Interfaces;
using Desyco.Iam.Contracts.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Desyco.Iam.Web.Controllers;

[ApiController]
[Route("api/permissions")]
[Authorize]
public class PermissionsController(IPermissionService permissionService) : ControllerBase
{
    [HttpGet("features")]
    [Authorize(Policy = Permissions.Security.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<FeatureDto>>> GetAllFeatures()
    {
        var features = await permissionService.GetAllFeaturesAsync();
        return Ok(features);
    }

    [HttpGet("roles/{roleId:guid}")]
    [Authorize(Policy = Permissions.Security.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RoleClaimDto>>> GetRolePermissions(Guid roleId)
    {
        var permissions = await permissionService.GetRoleClaimsAsync(roleId);
        return Ok(permissions);
    }

    [HttpPut("roles/{roleId:guid}")]
    [Authorize(Policy = Permissions.Security.Write)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateRolePermissions(Guid roleId, [FromBody] List<FeaturePermission> permissions)
    {
        await permissionService.UpdateRolePermissionsAsync(roleId, permissions);
        return NoContent();
    }
    
    [HttpGet("schema/role/{roleId:guid}")]
    [Authorize(Policy = Permissions.Security.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PermissionSchemaDto>> GetPermissionSchemaForRole(Guid roleId)
    {
        var permissionSchema = await permissionService.GetPermissionSchemaForRoleAsync(roleId);
        return Ok(permissionSchema);
    }

    [HttpGet("schema/user/{userId:guid}")]
    [Authorize(Policy = Permissions.Security.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PermissionSchemaDto>> GetPermissionSchemaForUser(Guid userId)
    {
        var permissionSchema = await permissionService.GetPermissionSchemaForUserAsync(userId);
        return Ok(permissionSchema);
    }

    [HttpGet("role-features/{roleId:guid}")]
    [Authorize(Policy = Permissions.Security.Read)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Guid>>> GetRoleFeatures(Guid roleId)
    {
        var features = await permissionService.GetAssignedFeatureIdsAsync(roleId);
        return Ok(features);
    }

    [HttpPut("role-features/{roleId:guid}")]
    [Authorize(Policy = Permissions.Security.Write)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateRoleFeatures(Guid roleId, [FromBody] List<Guid> featureIds)
    {
        await permissionService.UpdateAssignedFeaturesAsync(roleId, featureIds);
        return NoContent();
    }
}
