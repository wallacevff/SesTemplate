using Microsoft.EntityFrameworkCore;

namespace SesTemplate.Infra.Data.Interfaces;

public interface IContextEntityMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
{

}