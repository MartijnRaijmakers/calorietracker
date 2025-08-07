using Persistance.Dto;

namespace BusinessLogic.Entities
{
    public class Account
    {
        public Account(string name, string? lastName, string gender, DateOnly dateOfBirth, decimal height, decimal weight)
        {
            Name = name;
            LastName = lastName;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Height = height;
            Weight = weight;
        }

        public Account(AccountDto accountDto)
        {
            Name = accountDto.Name;
            LastName = accountDto.LastName;
            Gender = accountDto.Gender;
            DateOfBirth = accountDto.DateOfBirth;
            Height = accountDto.Height;
            Weight = accountDto.Weight;
        }

        public string Name;
        public string? LastName;
        public string Gender;
        public DateOnly DateOfBirth;
        public decimal Height;
        public decimal Weight;

        public AccountDto ToAccountDto()
        {
            return new AccountDto()
            {
                Name = Name,
                LastName = LastName,
                Gender = Gender,
                DateOfBirth = DateOfBirth,
                Height = Height,
                Weight = Weight
            };
        }

        public override string ToString()
        {
            return $"Name: {Name}, LastName: {LastName}, Gender: {Gender}, DateOfBirth: {DateOfBirth}, Height: {Height}, Weight: {Weight}";
        }
    }
}
