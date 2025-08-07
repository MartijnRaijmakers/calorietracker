using CsvHelper.Configuration.Attributes;

namespace Persistance.Dto
{
    public class AccountDto : DtoEntity
    {
        public string Name { get; set; }
        [Optional]
        public string? LastName { get; set; }
        public string Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
    }
}
