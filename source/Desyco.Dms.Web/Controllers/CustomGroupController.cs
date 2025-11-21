// using Microsoft.AspNetCore.Mvc;
// using Desyco.Dms.Application.Common.RequestPipelines;
// using Desyco.Dms.Application.CustomGroups.Commands;
// using Desyco.Dms.Application.CustomGroups.Models;
// using Desyco.Dms.Application.CustomGroups.Queries;
// using Desyco.Dms.Domain.Common.Filtering;
// using Desyco.Dms.Domain.CustomGroups;
// using Netto.Kassentableau.Web.Infrastructure;
// using Netto.Kassentableau.Web.ModelBinders;
//
// namespace Netto.Kassentableau.Web.Controllers;
//
// [ApiController]
// [Route("api/v1/custom-groups")]
// public class CustomGroupController(IRequestExecutor requestExecutor) : ControllerBase
// {
//     /// <summary>
//     /// Ruft eine paginierte Liste von benutzerdefinierten Gruppen ab.
//     /// </summary>
//     /// <param name="mainGroupNumber">Die Hauptgruppennummer.</param>
//     /// <param name="queryOptionsFilter">Abfrageoptionen.</param>
//     /// <param name="cancellationToken">Abbruchtoken.</param>
//     /// <returns>Eine paginierte Liste von benutzerdefinierten Gruppen.</returns>
//     [HttpGet]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     public Task<ActionResult<QueryResult<CustomGroupDto>>> GetMainGroup(
//         [FromQuery] int mainGroupNumber,
//         QueryOptionsFilter<CustomGroup> queryOptionsFilter,
//         CancellationToken cancellationToken = default
//     )
//         => requestExecutor.Handle(new CustomGroupByMainGroupNumberQuery(mainGroupNumber, queryOptionsFilter), cancellationToken);
//
//     /// <summary>
//     /// Ruft eine benutzerdefinierte Gruppe anhand der ID ab.
//     /// </summary>
//     /// <param name="groupId">Die ID der abzurufenden Gruppe.</param>
//     /// <param name="cancellationToken">Abbruchtoken.</param>
//     /// <returns>Die benutzerdefinierte Gruppe.</returns>
//     [HttpGet("{groupId:guid}")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public Task<ActionResult<CustomGroupDto>> GetGroup(Guid groupId, CancellationToken cancellationToken = default)
//         => requestExecutor.Handle(new CustomGroupByIdQuery(groupId), cancellationToken);
//
//     /// <summary>
//     /// Erstellt eine neue benutzerdefinierte Gruppe.
//     /// </summary>
//     /// <param name="group">Die zu erstellende benutzerdefinierte Gruppe.</param>
//     /// <param name="cancellationToken">Abbruchtoken.</param>
//     /// <returns>Die erstellte benutzerdefinierte Gruppe.</returns>
//     [HttpPost]
//     [ProducesResponseType(StatusCodes.Status201Created)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public Task<ActionResult<CustomGroupDto>> CreateGroup([FromBody] CustomGroupDto group,
//         CancellationToken cancellationToken = default)
//         => requestExecutor.Handle(new CreateCustomGroupCommand(group), cancellationToken);
//
//     /// <summary>
//     /// Aktualisiert eine vorhandene benutzerdefinierte Gruppe.
//     /// </summary>
//     /// <param name="groupId">Die ID der zu aktualisierenden Gruppe.</param>
//     /// <param name="group">Die aktualisierten Daten der benutzerdefinierten Gruppe.</param>
//     /// <param name="cancellationToken">Abbruchtoken.</param>
//     /// <returns>Ein leeres Ergebnis.</returns>
//     [HttpPut("{groupId:guid}")]
//     [ProducesResponseType(StatusCodes.Status204NoContent)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public Task<ActionResult<CustomGroupDto>> UpdateGroup(Guid groupId, [FromBody] CustomGroupDto group,
//         CancellationToken cancellationToken = default)
//         => requestExecutor.Handle(new UpdateCustomGroupCommand(groupId, group), cancellationToken);
//
//     /// <summary>
//     /// Löscht eine benutzerdefinierte Gruppe.
//     /// </summary>
//     /// <param name="groupId">Die ID der zu löschenden Gruppe.</param>
//     /// <param name="cancellationToken">Abbruchtoken.</param>
//     /// <returns>Ein leeres Ergebnis.</returns>
//     [HttpDelete("{groupId:guid}")]
//     [ProducesResponseType(StatusCodes.Status204NoContent)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public Task<ActionResult> DeleteGroup(Guid groupId, CancellationToken cancellationToken = default)
//         => requestExecutor.Handle(new DeleteCustomGroupCommand(groupId), cancellationToken);
//
//     /// <summary>
//     /// Ruft die verfügbaren Untergruppen für eine benutzerdefinierte Gruppe ab.
//     /// </summary>
//     /// <param name="groupId">Die ID der Gruppe.</param>
//     /// <param name="mainGroupNumber">Die Hauptgruppennummer zur Filterung.</param>
//     /// <param name="cancellationToken">Abbruchtoken.</param>
//     /// <returns>Liste der verfügbaren Untergruppen.</returns>
//     [HttpGet("{groupId:guid}/available-subgroups")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     public Task<ActionResult<IEnumerable<CustomGroupSubGroupDto>>> GetAvailableSubGroups(
//         [FromRoute] Guid groupId,
//         [FromQuery] int mainGroupNumber,
//         CancellationToken cancellationToken = default
//     )
//         => requestExecutor.Handle(new GetAvailableSubGroupsQuery(groupId, mainGroupNumber), cancellationToken);
//
//     /// <summary>
//     /// Ruft die verfügbaren Artikel für eine benutzerdefinierte Gruppe ab.
//     /// </summary>
//     /// <param name="groupId">Die ID der Gruppe.</param>
//     /// <param name="mainGroupNumber">Die Hauptgruppennummer zur Filterung.</param>
//     /// <param name="cancellationToken">Abbruchtoken.</param>
//     /// <returns>Liste der verfügbaren Artikel.</returns>
//     [HttpGet("{groupId:guid}/available-articles")]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     public Task<ActionResult<IEnumerable<CustomGroupArticleDto>>> GetAvailableArticles(
//         [FromRoute] Guid groupId,
//         [FromQuery] int mainGroupNumber,
//         CancellationToken cancellationToken = default
//     )
//         => requestExecutor.Handle(new GetAvailableCustomGroupArticleQuery(groupId, mainGroupNumber), cancellationToken);
// }
