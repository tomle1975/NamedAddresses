using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Tql.Agdata.NamedAddress.Domain.Aggregates.AddressAggregate;
using Tql.Agdata.NamedAddress.Domain.SeedWork;
using Tql.Agdata.NamedAddress.Infrastructure.Repositories.EntityFramework;

namespace Tql.Agdata.NamedAddress.Infrastructure.Services;

/// <inheritdoc cref="IAddressService" />
public class AddressEfService : IAddressService
{
    private readonly IUnitOfWork<AddressEfDbContext> unitOfWork;
    private readonly IValidator<Address> validator;
    private readonly IMemoryCache cache;
    private readonly IAddressRepository<AddressEfDbContext> repository;
    private const string AddressesCacheName = "Addresses"; // TODO: Configurable cache name.
    private const int MinutesCacheToExpire = 15; // TODO: Configurable cache expiration.

    public AddressEfService(
        IUnitOfWork<AddressEfDbContext> unitOfWork,
        IAddressRepository<AddressEfDbContext> repository,
        IValidator<Address> validator,
        IMemoryCache cache)
    {
        this.unitOfWork = unitOfWork;
        this.repository = repository;
        this.validator = validator;
        this.cache = cache;

        this.repository.Context = unitOfWork.Context;
    }

    /// <inheritdoc cref="IAddressService.GetAll" />
    public async Task<Result<IEnumerable<Address>>> GetAll(CancellationToken cancellation)
    {
        var result = new Result<IEnumerable<Address>>();
        var cachedAddresses = cache.Get<List<Address>>(AddressesCacheName);
        if (cachedAddresses == null) return await CacheAddresses(cancellation).ConfigureAwait(false);
        
        result.Data = cachedAddresses;
        result.IsSuccessful = true;
        return result;
    }

    /// <inheritdoc cref="IAddressService.GetByName" />
    public async Task<Result<Address?>> GetByName(string name, CancellationToken cancellation)
    {
        // Get address from cache.
        var result = new Result<Address?>();
        var cachedAddresses = cache.Get<List<Address>>(AddressesCacheName);
        if (cachedAddresses?.Any(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) == true)
        {
            result.Data = cachedAddresses.First(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            result.IsSuccessful = true;
            return result;
        }

        // Get all addresses from database, cache them, and find the matching address.
        var addressesResult = await CacheAddresses(cancellation).ConfigureAwait(false);
        if (!result.IsSuccessful || addressesResult.Data == null || addressesResult.Data.Any(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) == false)
        {
            result.Message = $"Address for '{name}' was not found.";
            result.Description = addressesResult.Description;
            result.Errors.AddRange(addressesResult.Errors);
            return result;
        }

        result.Data = addressesResult.Data.First(address => address.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        result.IsSuccessful = true;
        return result;
    }

    /// <inheritdoc cref="IAddressService.Add" />
    public async Task<Result<long>> Add(Address address, CancellationToken cancellation)
    {
        var result = new Result<long>();

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
            
            var insertedAddress = await repository.GetByName(address.Name, cancellation).ConfigureAwait(false);
            result.Data = insertedAddress?.Id ?? 0;
            address.Id = result.Data;
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
            var cachedAddresses = cache.Get<List<Address>>(AddressesCacheName);
            if (cachedAddresses == null)
            {
                await CacheAddresses(cancellation).ConfigureAwait(false);
                return result;
            }

            cachedAddresses.Add(address);
            cache.Set(AddressesCacheName, cachedAddresses, TimeSpan.FromMinutes(MinutesCacheToExpire));
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
    public async Task<Result<long>> Update(Address address, CancellationToken cancellation)
    {
        var result = new Result<long> { Data = address.Id };

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
            var cachedAddresses = cache.Get<List<Address>>(AddressesCacheName);
            if (cachedAddresses == null)
            {
                await CacheAddresses(cancellation).ConfigureAwait(false);
                return result;
            }

            cachedAddresses.RemoveAll(a => a.Id == address.Id || a.Name.Equals(address.Name, StringComparison.OrdinalIgnoreCase));
            cachedAddresses.Add(address);
            cache.Set(AddressesCacheName, cachedAddresses, TimeSpan.FromMinutes(MinutesCacheToExpire));
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
    public async Task<Result<long>> Delete(Address address, CancellationToken cancellation)
    {
        var result = new Result<long> { Data = address.Id };

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
            var cachedAddresses = cache.Get<List<Address>>(AddressesCacheName);
            if (cachedAddresses == null)
            {
                await CacheAddresses(cancellation).ConfigureAwait(false);
                return result;
            }

            cachedAddresses.RemoveAll(a => a.Id == address.Id || a.Name.Equals(address.Name, StringComparison.OrdinalIgnoreCase));
            cache.Set(AddressesCacheName, cachedAddresses, TimeSpan.FromMinutes(MinutesCacheToExpire));
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
            return result;
        }

        if (result.Data == null || result.Data?.Any() == false) return result;

        // Cache addresses.
        try
        {
            cache.Set(AddressesCacheName, result.Data?.ToList(), TimeSpan.FromMinutes(MinutesCacheToExpire));
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