using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities;

public class LookUp : BaseEntity
{
    [Key]
    public int LookUpId { get; set; }
    public string NameEn { get; set; }
    public string NameAr { get; set; }

    public LookUpCategory LookupCategory { get; set; }
    public int LookupCategoryId { get; set; }
}
