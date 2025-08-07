using Persistance.Dto;

namespace Persistance.Repositories
{
    public class AccountRepository : CsvRepository<AccountDto>, IAccountRepository
    {
        public AccountRepository(string path) : base(path)
        {
        }

        public AccountDto GetByNameAndDateOfBirth(string name, DateOnly dateOfBirth)
        {
            var result = GetAll().FirstOrDefault(x => x.Name == name && x.DateOfBirth == dateOfBirth);

            if (result == null)
                throw new KeyNotFoundException($"No account found with Name: {name} and DateOfBirth: {dateOfBirth}");

            return result;
        }
    }
}
