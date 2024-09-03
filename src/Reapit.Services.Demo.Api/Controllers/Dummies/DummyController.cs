using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reapit.Services.Demo.Api.Controllers.Abstract;
using Reapit.Services.Demo.Api.Controllers.Dummies.Examples;
using Reapit.Services.Demo.Api.Controllers.Dummies.Models;
using Reapit.Services.Demo.Core.UseCases.Dummies.CreateDummy;
using Reapit.Services.Demo.Core.UseCases.Dummies.GetDummies;
using Reapit.Services.Demo.Core.UseCases.Dummies.GetDummyById;
using Swashbuckle.AspNetCore.Filters;

namespace Reapit.Services.Demo.Api.Controllers.Dummies;

/// <summary>
/// Endpoints for interacting with Dummies.
/// </summary>
public class DummyController : ReapitApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    
    /// <summary>Initializes a new instance of the <see cref="DummyController"/> class.</summary>
    /// <param name="mapper"></param>
    /// <param name="mediator"></param>
    public DummyController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    /// <summary>
    /// Fetch a collection of Dummies.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReadDummyModel>), 200)]
    [SwaggerResponseExample(200, typeof(ReadDummyModelCollectionExample))]
    public async Task<IActionResult> GetMany()
    {
        var dummies = await _mediator.Send(new GetDummiesQuery(), default);
        return Ok(_mapper.Map<IEnumerable<ReadDummyModel>>(dummies));
    }

    /// <summary>
    /// Fetch a single Dummy by it's unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Dummy.</param>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReadDummyModel), 200)]
    [ProducesResponseType(typeof(void), 404)]
    [SwaggerResponseExample(200, typeof(ReadDummyModelExample))]
    public async Task<IActionResult> GetOne(string id)
    {
        var query = new GetDummyByIdQuery(id);
        var dummy = await _mediator.Send(query, default);
        return Ok(_mapper.Map<ReadDummyModel>(dummy));
    }

    /// <summary>
    /// Create a new Dummy.
    /// </summary>
    /// <param name="model">Model describing the Dummy to create.</param>
    [HttpPost]
    [ProducesResponseType(typeof(ReadDummyModel), 201)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateOne([FromBody] WriteDummyModel model)
    {
        var command = _mapper.Map<CreateDummyCommand>(model);
        var dummy = await _mediator.Send(command, default);
        return Ok(_mapper.Map<ReadDummyModel>(dummy));
    }
    
    /// <summary>
    /// Update an existing Dummy.
    /// </summary>
    /// <param name="id">The unique identifier of the Dummy.</param>
    /// <param name="model">Model describing the Dummy to update.</param>
    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(422)]
    public IActionResult UpdateOne(string id, [FromBody] WriteDummyModel model)
        => NotFound();
    
    /// <summary>
    /// Delete an existing Dummy. 
    /// </summary>
    /// <param name="id">The unique identifier of the Dummy.</param>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public IActionResult DeleteOne(string id)
        => NotFound();
}