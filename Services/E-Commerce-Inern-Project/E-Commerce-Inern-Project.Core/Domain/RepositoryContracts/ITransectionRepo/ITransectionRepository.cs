using Microsoft.EntityFrameworkCore.Storage;
 

namespace E_Commerce_Inern_Project.Core.Domain.RepositoryContracts.ITransectionRepo
{
    public interface ITransectionRepository
    {
        public Task<IDbContextTransaction?> BeginTransactionAsync();

    }
}
