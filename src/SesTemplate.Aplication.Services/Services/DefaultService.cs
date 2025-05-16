using System.Reflection;
using SesTemplate.Application.Contracts.Dto;
using SesTemplate.Application.Contracts.Services;
using SesTemplate.Domain.Repositories;
using SesTemplate.Domain.Shared.Exceptions;
using SesTemplate.Domain.Shared.Filters;
using Talonario.Domain.Repositories;

namespace SesTemplate.Aplication.Services.Services;

public abstract class DefaultService<TEntity, TDto, TCadastroDto, TFilter, TKey>(
    IDefaultRepository<TEntity, TFilter, TKey> repository,
    IUnityOfWork unityOfWork,
    IAutomapApi mapper) : IDefaultService<TDto, TCadastroDto, TFilter, TKey>
    where TEntity : class
    where TDto : class
    where TCadastroDto : class
    where TFilter : Filter, new()
{
    public virtual async Task<TDto> AddAsync(TCadastroDto dto, CancellationToken cancellationToken = default)
    {
        var entity = mapper.MapFrom<TEntity>(dto);
        var createdEntity = await repository.AddAsync(entity, cancellationToken);
        await unityOfWork.SaveChangesAsync(cancellationToken);
        var createdEntityDto = mapper.MapFrom<TDto>(createdEntity);
        return createdEntityDto;
    }

    public virtual async Task AddRangeAsync(IList<TCadastroDto> dto,
        CancellationToken cancellationToken = default)
    {
        await repository.AddRangeAsync(mapper.MapFrom<List<TEntity>>(dto), cancellationToken);
        await unityOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(true);
    }

    public virtual async Task<PagedResultDto<TDto>> GetAllAsync(TFilter filter,
        CancellationToken cancellationToken = default)
    {
        var entities = await repository
            .GetAllAsync(filter, cancellationToken);
        var dtos = mapper.MapFrom<PagedResultDto<TDto>>(entities);
        return dtos;
    }

    public virtual async Task<TDto> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var foundEntity = await repository.GetByIdAsync(id, cancellationToken);
        if (foundEntity is null)
            throw new RegistroNaoEncontradoException("Entidade não encontrada");
        var dto = mapper.MapFrom<TDto>(foundEntity);
        return dto;
    }

    public virtual async Task<TDto> FindAsync(TKey id, CancellationToken cancellationToken = default)
    {
        var foundEntity = await repository.FindAsync(id, cancellationToken);
        if (foundEntity is null)
            throw new RegistroNaoEncontradoException("Entidade não encontrada");
        var dto = mapper.MapFrom<TDto>(foundEntity);
        return dto;
    }

    public virtual async Task UpdateAsync(TCadastroDto dto, TKey id,
        CancellationToken cancellationToken = default)
    {
        TEntity? foundEntity = await repository.FindAsync(id);
        if (foundEntity is null)
            throw new RegistroNaoEncontradoException("Entidade não encontrada");
        mapper.MapTo(dto, foundEntity);
        PropertyInfo? propertyInfo = foundEntity.GetType().GetProperty("UpdatedAt");
        if (propertyInfo is not null)
            propertyInfo.SetValue(foundEntity, DateTime.Now);
        await repository.UpdateAsync(foundEntity);
        await unityOfWork.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<TDto> DeleteAsync(TKey id,
        CancellationToken cancellationToken = default)
    {
        TEntity? foundEntity = await repository.FindAsync(id, cancellationToken);
        if (foundEntity is null)
            throw new RegistroNaoEncontradoException("Entidade não encontrada");
        var deleted = await repository.DeleteAsync(foundEntity, cancellationToken);
        await unityOfWork.SaveChangesAsync(cancellationToken);
        var dto = mapper.MapFrom<TDto>(deleted);
        return dto;
    }

    public virtual Task<bool> HasAnyAsync(CancellationToken cancellationToken = default)
    {
        // var predicate = PredicateBuilder.New<TEntity>(true);
        // var excludedProperties = new List<string> { "TotalPages", "PageSize", "PageNumber" };
        // PropertyInfo[] properties = filter.GetType().GetProperties();
        // foreach (var property in properties)
        // {
        //     if(excludedProperties.Contains(property.Name))
        //         continue;
        //     var filterValue = property.GetValue(filter);
        //     if (filterValue is not null)
        //     {
        //         var filterValueType = filterValue.GetType();
        //     }
        // }
        return repository.HasAnyAsync(null, cancellationToken);
    }
}
