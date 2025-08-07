using Persistance.Dto;
namespace Persistance.Repositories
{
    public interface IAccountRepository : IRepository<AccountDto>
    {
        AccountDto GetByNameAndDateOfBirth(string name, DateOnly dateOfBirth);
    }
}
