using BusinessLogic.Managers;
using Persistance.Repositories;
namespace App
{
    internal static class Initialiser
    {
        public static AccountManager InitialiseAccountManager()
        {
            var path = "Accounts.csv";
            var repository = new AccountRepository(path);
            return new AccountManager(repository);
        }
    }
}
