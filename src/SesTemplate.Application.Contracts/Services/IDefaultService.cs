using SesTemplate.Application.Contracts.Dto;
using SesTemplate.Domain.Shared.Filters;

namespace SesTemplate.Application.Contracts.Services;

public interface IDefaultService<TDto, TCadastroDto, TFilter, Tkey>
where TDto : class
where TCadastroDto : class
where TFilter : Filter
{
    public Task<TDto> AddAsync(TCadastroDto dto, CancellationToken cancellationToken = default);
    public Task AddRangeAsync(IList<TCadastroDto> dto, CancellationToken cancellationToken = default);
    public Task<PagedResultDto<TDto>> GetAllAsync(TFilter filter, CancellationToken cancellationToken = default);
    public Task<TDto> GetByIdAsync(Tkey id, CancellationToken cancellationToken = default);
    public Task<TDto> FindAsync(Tkey id, CancellationToken cancellationToken = default);
    public Task UpdateAsync(TCadastroDto dto, Tkey id, CancellationToken cancellationToken = default);
    public Task<TDto> DeleteAsync(Tkey id, CancellationToken cancellationToken = default);
    public Task<bool> HasAnyAsync(CancellationToken cancellationToken = default);
}