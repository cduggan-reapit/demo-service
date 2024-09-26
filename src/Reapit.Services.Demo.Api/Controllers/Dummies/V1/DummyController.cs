using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reapit.Packages.ErrorHandling.Models;
using Reapit.Platform.ApiVersioning.Attributes;
using Reapit.Services.Demo.Api.Controllers.Abstract;
using Reapit.Services.Demo.Api.Controllers.Dummies.V1.Examples;
using Reapit.Services.Demo.Api.Controllers.Dummies.V1.Models;
using Reapit.Services.Demo.Core.UseCases.Dummies.CreateDummy;
using Reapit.Services.Demo.Core.UseCases.Dummies.GetDummies;
using Reapit.Services.Demo.Core.UseCases.Dummies.GetDummyById;
using Swashbuckle.AspNetCore.Filters;

namespace Reapit.Services.Demo.Api.Controllers.Dummies.V1;

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
    [IntroducedInVersion(1,0)]
    [ProducesResponseType(typeof(IEnumerable<ReadDummyModel>), 200)]
    [SwaggerResponseExample(200, typeof(ReadDummyModelCollectionExample))]
    public async Task<IActionResult> GetDummies()
    {
        var dummies = await _mediator.Send(new GetDummiesQuery());
        return Ok(_mapper.Map<IEnumerable<ReadDummyModel>>(dummies));
    }

    /// <summary>
    /// Fetch a single Dummy by it's unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the Dummy.</param>
    [HttpGet("{id}")]
    [IntroducedInVersion(1,0)]
    [ProducesResponseType(typeof(ReadDummyModel), 200)]
    [ProducesResponseType(typeof(ApiErrorModel), 404)]
    [SwaggerResponseExample(200, typeof(ReadDummyModelExample))]
    public async Task<IActionResult> GetDummyById(string id)
    {
        var query = new GetDummyByIdQuery(id);
        var dummy = await _mediator.Send(query);
        return Ok(_mapper.Map<ReadDummyModel>(dummy));
    }

    /// <summary>
    /// Create a new Dummy.
    /// </summary>
    /// <param name="model">Model describing the Dummy to create.</param>
    [HttpPost]
    [IntroducedInVersion(1,0)]
    [ProducesResponseType(typeof(ReadDummyModel), 201)]
    [ProducesResponseType(typeof(ValidationErrorModel), 422)]
    public async Task<IActionResult> CreateDummy([FromBody] WriteDummyModel model)
    {
        var command = _mapper.Map<CreateDummyCommand>(model);
        var dummy = await _mediator.Send(command);
        var readModel = _mapper.Map<ReadDummyModel>(dummy);
        return CreatedAtAction(nameof(GetDummyById), new { id = readModel.Id },  readModel);
    }
    
    /// <summary>
    /// Update an existing Dummy.
    /// </summary>
    /// <param name="id">The unique identifier of the Dummy.</param>
    /// <param name="model">Model describing the Dummy to update.</param>
    [HttpPut("{id}")]
    [IntroducedInVersion(1,1)]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ApiErrorModel), 404)]
    [ProducesResponseType(typeof(ValidationErrorModel), 422)]
    public IActionResult UpdateOne(string id, [FromBody] WriteDummyModel model)
        => NotFound();
    
    /// <summary>
    /// Delete an existing Dummy. 
    /// </summary>
    /// <param name="id">The unique identifier of the Dummy.</param>
    [HttpDelete("{id}")]
    [IntroducedInVersion(1,2)]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ApiErrorModel), 403)]
    [ProducesResponseType(typeof(ApiErrorModel), 404)]
    public IActionResult DeleteOne(string id)
        => NotFound();
}