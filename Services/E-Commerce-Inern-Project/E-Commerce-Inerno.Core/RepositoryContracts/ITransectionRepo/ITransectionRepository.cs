using Microsoft.EntityFrameworkCore.Storage;
 

namespace E_Commerce_Inerno.Core.Domain.RepositoryContracts.ITransectionRepo
{
    public interface ITransectionRepository
    {
        public Task<IDbContextTransaction?> BeginTransactionAsync();
    }
}
