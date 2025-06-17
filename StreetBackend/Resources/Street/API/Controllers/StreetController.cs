using System;
using Microsoft.AspNetCore.Mvc;
using StreetBackend.Common.Interfaces;
using StreetBackend.Resources.Street.API.DTOs;
using StreetBackend.Resources.Street.Application.Commands;
using StreetBackend.Resources.Street.Application.Queries;

namespace StreetBackend.Resources.Street.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class StreetController : ControllerBase
    {

        private readonly ICommandHandler<CreateNewStreetCommand> _createNewStreetCommandHandler;
        private readonly ICommandHandler<DeletePointFromStreetCommand> _deletePointCommandHandler;
        private readonly ICommandHandler<DeleteStreetCommand> _deleteStreetCommandHandler;
        private readonly ICommandHandler<PostPointToStreetCommand> _addPointCommandHandler;
        private readonly IQueryHandler<GetAllStreetsQuery, List<StreetDto>> _getAllQueryHandler;
        private readonly IQueryHandler<GetStreetByFieldQuery, StreetDto> _getByIdQueryHandler;


        private readonly ILogger<StreetController> _logger;

        public StreetController(
            ILogger<StreetController> logger,
            ICommandHandler<CreateNewStreetCommand> createNewStreetCommandHandler,
            ICommandHandler<DeletePointFromStreetCommand> deletePointCommandHandler,
            ICommandHandler<DeleteStreetCommand> deleteStreetCommandHandler,
            ICommandHandler<PostPointToStreetCommand> addPointCommandHandler,
            IQueryHandler<GetAllStreetsQuery, List<StreetDto>> getAllQueryHandler,
            IQueryHandler<GetStreetByFieldQuery, StreetDto> getByIdQueryHandler
            )
        {
            _logger = logger;
            _createNewStreetCommandHandler = createNewStreetCommandHandler;
            _deletePointCommandHandler = deletePointCommandHandler;
            _deleteStreetCommandHandler = deleteStreetCommandHandler;
            _addPointCommandHandler = addPointCommandHandler;
            _getAllQueryHandler = getAllQueryHandler;
            _getByIdQueryHandler = getByIdQueryHandler;
        }

        [HttpGet]
        public async Task<ActionResult<List<StreetDto>>> GetAll()
        {
            var result = await _getAllQueryHandler.HandleAsync(new GetAllStreetsQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<StreetDto>> GetById(Guid id)
        {
            var result = await _getByIdQueryHandler.HandleAsync(new GetStreetByFieldQuery() { Id = id });
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNewStreetCommand command)
        {
            await _createNewStreetCommandHandler.HandleAsync(command);
            return Ok();
        }

        [HttpPost("{id:guid}/point")]
        public async Task<IActionResult> AddPoint(Guid id, [FromBody] PostPointToStreetCommand command)
        {
            if (id != command.StreetId) return BadRequest("Mismatched ID");
            await _addPointCommandHandler.HandleAsync(command);
            return NoContent();
        }

        /// <summary>
        /// Delete Point from a street
        /// TODO: not finished, because no such requirement yet
        ///       as we already have a update street point, for
        ///       now it may be redundent. (need re-check)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        //[HttpDelete("{id:guid}/point")]
        //public async Task<IActionResult> DeletePoint(Guid id, [FromBody] DeletePointFromStreetCommand command)
        //{
        //    if (id != command.StreetId) return BadRequest("Mismatched ID");
        //    await _deletePointCommandHandler.HandleAsync(command);
        //    return NoContent();
        //}

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _deleteStreetCommandHandler.HandleAsync(new DeleteStreetCommand() { StreetId = id});
            return NoContent();
        }
    }

}

