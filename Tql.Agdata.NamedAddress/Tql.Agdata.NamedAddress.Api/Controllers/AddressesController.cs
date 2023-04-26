using Microsoft.AspNetCore.Mvc;
using Tql.Agdata.NamedAddress.Api.DataMappings;
using Tql.Agdata.NamedAddress.Api.Models;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;

namespace Tql.Agdata.NamedAddress.Api.Controllers;

[Route("api")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IApiDataMapper<Address, AddressDto> dataMapper;
    private readonly IAddressService service;

    public AddressesController(IAddressService service, IApiDataMapper<Address, AddressDto> dataMapper)
    {
        this.service = service;
        this.dataMapper = dataMapper;
    }

    [Route("addresses")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AddressDto>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status510NotExtended)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<AddressDto>>> GetAll(CancellationToken cancellation)
    {
        try
        {
            var serviceResult = await service.GetAll(cancellation);
            if (!serviceResult.IsSuccessful || serviceResult.Data == null || serviceResult.Data?.Any() == false) return StatusCode(StatusCodes.Status404NotFound);

            return serviceResult.Data?.Select(a => dataMapper.ToApiDto(a)).ToList() ?? new List<AddressDto>();
        }
        catch (TaskCanceledException tcx)
        {
            return StatusCode(StatusCodes.Status510NotExtended);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Route("addresses/{name}")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status510NotExtended)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AddressDto>> GetByName(string name, CancellationToken cancellation)
    {
        try
        {
            var serviceResult = await service.GetByName(name, cancellation);
            if (!serviceResult.IsSuccessful || serviceResult.Data == null) return StatusCode(StatusCodes.Status404NotFound);

            return dataMapper.ToApiDto(serviceResult.Data);
        }
        catch (TaskCanceledException tcx)
        {
            return StatusCode(StatusCodes.Status510NotExtended);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Route("addresses/add")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddressDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status510NotExtended)]
    public async Task<ActionResult<AddressDto>> Add(AddressDto address, CancellationToken cancellation)
    {
        try
        {
            var serviceResult = await service.Add(dataMapper.ToEntity(address), cancellation);
            if (!serviceResult.IsSuccessful || serviceResult.Data == 0) return StatusCode(StatusCodes.Status400BadRequest);

            return address with { Id = serviceResult.Data };
        }
        catch (TaskCanceledException tcx)
        {
            return StatusCode(StatusCodes.Status510NotExtended);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Route("addresses/update")]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(AddressDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status510NotExtended)]
    public async Task<ActionResult> Update(AddressDto address, CancellationToken cancellation)
    {
        try
        {
            var serviceResult = await service.Update(dataMapper.ToEntity(address), cancellation);
            if (!serviceResult.IsSuccessful || serviceResult.Data == 0) return StatusCode(StatusCodes.Status400BadRequest);

            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (TaskCanceledException tcx)
        {
            return StatusCode(StatusCodes.Status510NotExtended);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [Route("addresses/delete")]
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(AddressDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status510NotExtended)]
    public async Task<ActionResult> Delete(AddressDto address, CancellationToken cancellation)
    {
        try
        {
            var serviceResult = await service.Delete(dataMapper.ToEntity(address), cancellation);
            if (!serviceResult.IsSuccessful || serviceResult.Data == 0) return StatusCode(StatusCodes.Status400BadRequest);

            return StatusCode(StatusCodes.Status204NoContent);
        }
        catch (TaskCanceledException tcx)
        {
            return StatusCode(StatusCodes.Status510NotExtended);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}