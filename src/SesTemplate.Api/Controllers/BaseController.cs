using Microsoft.AspNetCore.Mvc;
using SesTemplate.Application.Contracts.Dto;
using SesTemplate.Application.Contracts.Services;
using SesTemplate.Domain.Shared.Filters;

namespace SesTemplate.Api.Controllers
{
    [ApiController]
    public abstract class BaseController
    <
        TDto,
        TCadastroDto,
        TFilter,
        TKey,
        TService
    >
    (TService service) : ControllerBase
        where TCadastroDto : class
        where TDto : class
        where TFilter : Filter
        where TService : IDefaultService<
            TDto,
            TCadastroDto,
            TFilter,
            TKey
        >
    {
        private TService _service = service;

        #region Public Methods
        [HttpPost]
        public virtual async Task<TDto> AddAsync(TCadastroDto cadastroDto)
        {
            var entity = await _service.AddAsync(cadastroDto);
            return entity;
        }

        [HttpGet]
        public virtual async Task<PagedResultDto<TDto>> GetAllAsync(
            [FromQuery] TFilter filter, CancellationToken cancellationToken = default)
        {
            var result = await _service.GetAllAsync(filter, cancellationToken);
            return result;
        }

        [HttpGet("{id}")]
        public virtual async Task<TDto> GetByIdAsync(
            [FromRoute] TKey id)
        {
            var entity = await _service.GetByIdAsync(id);
            return entity;
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> UpdateAsync(
            [FromRoute] TKey id, [FromBody] TCadastroDto dto)
        {
            await _service.UpdateAsync(dto, id);
            return Ok();
        }


        [HttpDelete("{id}")]
        public virtual async Task<TDto> DeleteAsync(
            [FromRoute] TKey id)
        {
            var entity = await _service.DeleteAsync(id);
            return entity;
        }

        #endregion

        #region Protected Methods

       

        #endregion
    }
}
