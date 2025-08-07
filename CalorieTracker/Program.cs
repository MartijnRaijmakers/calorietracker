using App.CommandLine;
using BusinessLogic.Managers;
using CommandLine;

namespace App
{
    internal class Program
    {
        private static AccountManager? _accountManager;

        static void Main(string[] args)
        {
            /* Example command line arguments:
             * Create an account
             *      .\App.exe -a create --name Steve --dateOfBirth 2000-12-31 --gender Male --height 180 --weight 80
             *      .\App.exe -a create --name Steve --lastName Stevenson --dateOfBirth 2000-12-31 --gender Male --height 180 --weight 80
             * Get an account
             *      .\App.exe -a get --name Steve --dateOfBirth 2000-12-31
             */

            try
            {
                Initialise();

                Parser.Default.ParseArguments<Options>(args).WithParsed(options =>
                {
                    if (!Enum.TryParse<CmdAction>(options.Action, true, out var cmdAction)){
                        throw new ArgumentException("Invalid action specified. Use 'create' or 'get'.");
                    }

                    switch (cmdAction)
                    {
                        case CmdAction.Create:
                            CreateAccount(options);
                            break;
                        case CmdAction.Get:
                            GetAccount(options);
                            break;
                        default:
                            throw new ArgumentException($"Unknown action: {options.Action}.");
                    }
                });
            }
            catch (AggregateException ex)
            {
                ex.InnerExceptions.ToList().ForEach(innerEx =>
                {
                    Console.WriteLine($"Error: {innerEx.Message}");
                });
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Argument error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Press any key to continue ...");
                Console.ReadLine();
            }
        }

        private static void CreateAccount(Options options)
        {
            if (_accountManager == null)
            {
                throw new InvalidOperationException("AccountManager is not initialized. Please ensure Initialise() is called before creating an account.");
            }

            var exceptions = new List<Exception>();

            if (options.Name == null)
            {
                exceptions.Add(new ArgumentException("Name is required to create an account."));
            }
            if (options.Gender == null)
            {
                exceptions.Add(new ArgumentException("Gender is required to create an account."));
            }
            if (!options.DateOfBirth.HasValue)
            {
                exceptions.Add(new ArgumentException("Date of birth is required to create an account."));
            }
            if (!options.Height.HasValue)
            {
                exceptions.Add(new ArgumentException("Height is required to create an account."));
            }
            if (!options.Weight.HasValue)
            {
                exceptions.Add(new ArgumentException("Weight is required to create an account."));
            }

            if (exceptions.Count > 0) {
                throw new AggregateException("One or more errors occurred while creating the account.", exceptions);
            }

            _accountManager.CreateAccount(options.Name, options.LastName, options.Gender, options.DateOfBirth.Value, options.Height.Value, options.Weight.Value);
            Console.WriteLine($"Account for {options.Name} created successfully.");
        }

        private static void GetAccount(Options options)
        {
            if (_accountManager == null)
            {
                throw new InvalidOperationException("AccountManager is not initialized. Please ensure Initialise() is called before creating an account.");
            }

            var exceptions = new List<Exception>();

            if (options.Name == null)
            {
                exceptions.Add(new ArgumentException("Name is required to create an account."));
            }
            if (!options.DateOfBirth.HasValue)
            {
                exceptions.Add(new ArgumentException("Date of birth is required to create an account."));
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException("One or more errors occurred while creating the account.", exceptions);
            }

            try
            {
                var account = _accountManager.Login(options.Name, options.DateOfBirth.Value);
                Console.WriteLine("Account found: " + account.ToString());
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine($"Account not found: {ex.Message}");
            }
        }

        private static void Initialise()
        {
            _accountManager = Initialiser.InitialiseAccountManager();
        }
    }
}