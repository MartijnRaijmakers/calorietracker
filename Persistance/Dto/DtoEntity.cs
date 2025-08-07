using CsvHelper.Configuration.Attributes;

namespace Persistance.Dto
{
    public abstract class DtoEntity
    {
        public int? Id { get; set; }
    }
}
