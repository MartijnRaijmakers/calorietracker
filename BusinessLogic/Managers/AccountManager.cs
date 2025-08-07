using BusinessLogic.Entities;
using Persistance.Dto;
using Persistance.Repositories;

namespace BusinessLogic.Managers
{
    public class AccountManager(IAccountRepository accountRepository)
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        public void CreateAccount(string firstName, string? lastName, string gender, DateOnly dateOfBirth, decimal height, decimal weight)
        {
            var account = new Account(firstName, lastName, gender, dateOfBirth, height, weight);
            _accountRepository.Add(account.ToAccountDto());
        }

        // NOT IN REQUIREMENTS
        public Account Login(string name, DateOnly dateOfBirth)
        {
            var result = _accountRepository.GetByNameAndDateOfBirth(name, dateOfBirth);
            return new Account(result);
        }
    }
}
