using FluentValidation;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Tql.Agdata.NamedAddress.Domain.SeedWork;
using Tql.Agdata.NamedAddress.Infrastructure.DataMappings;
using Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework;

namespace Tql.Agdata.NamedAddress.Infrastructure.Services;

/// <inheritdoc cref="IAddressService" />
public class AddressEfService : IAddressService
{
    private readonly IUnitOfWork<AddressEfDbContext> unitOfWork;
    private readonly IValidator<Address> validator;
    private readonly IAddressRepository repository;

    public AddressEfService(IUnitOfWork<AddressEfDbContext> unitOfWork, IValidator<Address> validator)
    {
        this.unitOfWork = unitOfWork;
        this.validator = validator;
        repository = new AddressEfRepository(unitOfWork, new AddressRepositoryDataMapper());
    }

    /// <inheritdoc cref="IAddressService.GetAll" />
    public async Task<Result<IEnumerable<Address>>> GetAll(CancellationToken cancellation)
    {
        var result = new Result<IEnumerable<Address>>();

        // Get addresses from cache.
        // TODO:

        if (result.IsSuccessful) return result;

        return await CacheAddresses(cancellation).ConfigureAwait(false);
    }

    /// <inheritdoc cref="IAddressService.GetByName" />
    public async Task<Result<Address?>> GetByName(string name, CancellationToken cancellation)
    {
        var result = new Result<Address?>();

        // Get addresses from cache.
        // TODO:

        if (result.IsSuccessful) return result;

        var cacheResult = await CacheAddresses(cancellation).ConfigureAwait(false);
        if (!cacheResult.IsSuccessful || cacheResult.Data?.Any() == false)
        {
            result.Message = $"Address with name '{name}' was not found.";
            return result;
        }

        result.Data = cacheResult.Data?.FirstOrDefault(address => address.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        if (result.Data == null) result.Message = $"Address with name '{name}' was not found.";
        else result.IsSuccessful = true;

        return result;
    }

    /// <inheritdoc cref="IAddressService.Add" />
    public async Task<Result<DateTime>> Add(Address address, CancellationToken cancellation)
    {
        var result = new Result<DateTime> { Data = DateTime.Now };

        // Validate address.
        var validationResult = await validator.ValidateAsync(address, cancellation).ConfigureAwait(false);
        if (!validationResult.IsValid)
        {
            result.Message = $"Failed to add new address for '{address.Name}'.";
            result.Description = string.Join(Environment.NewLine, validationResult.Errors.Select(err => err.ErrorMessage));
            return result;
        }

        // Persist address to database.
        try
        {
            await repository.Insert(address, cancellation).ConfigureAwait(false);
            await unitOfWork.Save(cancellation).ConfigureAwait(false);
            result.IsSuccessful = true;
        }
        catch (Exception ex)
        {
            result.Message = "Failed to add address due to database error.";
            result.Description = ex.Message;
            result.Errors.Add(ex);
            return result;
        }

        // Update addresses cache.
        try
        {
            // TODO:
        }
        catch (Exception ex)
        {
            result.Message = "Failed to update addresses cache.";
            result.Description = ex.Message;
            result.Errors.Add(ex);
        }

        return result;
    }

    /// <inheritdoc cref="IAddressService.Update" />
    public async Task<Result<DateTime>> Update(Address address, CancellationToken cancellation)
    {
        var result = new Result<DateTime> { Data = DateTime.Now };

        // Validate address.
        var validationResult = await validator.ValidateAsync(address, cancellation).ConfigureAwait(false);
        if (!validationResult.IsValid)
        {
            result.Message = $"Failed to update address for '{address.Name}'.";
            result.Description = string.Join(Environment.NewLine, validationResult.Errors.Select(err => err.ErrorMessage));
            return result;
        }

        // Persist address to database.
        try
        {
            await repository.Update(address, cancellation).ConfigureAwait(false);
            await unitOfWork.Save(cancellation).ConfigureAwait(false);
            result.IsSuccessful = true;
        }
        catch (Exception ex)
        {
            result.Message = $"Failed to update address '{address.Name}' due to database error.";
            result.Description = ex.Message;
            result.Errors.Add(ex);
            return result;
        }

        // Update addresses cache.
        try
        {
            // TODO:
        }
        catch (Exception ex)
        {
            result.Message = "Failed to update addresses cache.";
            result.Description = ex.Message;
            result.Errors.Add(ex);
        }

        return result;
    }

    /// <inheritdoc cref="IAddressService.Delete" />
    public async Task<Result<DateTime>> Delete(Address address, CancellationToken cancellation)
    {
        var result = new Result<DateTime> { Data = DateTime.Now };

        // Persist address to database.
        try
        {
            await repository.Delete(address, cancellation).ConfigureAwait(false);
            await unitOfWork.Save(cancellation).ConfigureAwait(false);
            result.IsSuccessful = true;
        }
        catch (Exception ex)
        {
            result.Message = $"Failed to delete address '{address.Name}' due to database error.";
            result.Description = ex.Message;
            result.Errors.Add(ex);
            return result;
        }

        // Update addresses cache.
        try
        {
            // TODO:
        }
        catch (Exception ex)
        {
            result.Message = "Failed to update addresses cache.";
            result.Description = ex.Message;
            result.Errors.Add(ex);
        }

        return result;
    }

    private async Task<Result<IEnumerable<Address>>> CacheAddresses(CancellationToken cancellation)
    {
        var result = new Result<IEnumerable<Address>>();

        // Get addresses from database.
        try
        {
            result.Data = await repository.GetAll(cancellation).ConfigureAwait(false);
            result.IsSuccessful = true;
        }
        catch (Exception ex)
        {
            result.Message = "Failed to retrieve addresses from database.";
            result.Description = ex.Message;
            result.Errors.Add(ex);
        }

        if (!result.IsSuccessful) return result;

        // Cache addresses.
        try
        {
            // TODO:
        }
        catch (Exception ex)
        {
            result.Message = "Failed to create/update addresses cache.";
            result.Description = ex.Message;
            result.Errors.Add(ex);
        }

        return result;
    }
}