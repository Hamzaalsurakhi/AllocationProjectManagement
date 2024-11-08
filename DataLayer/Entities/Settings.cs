using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class Settings : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SettingId { get; set; }
        public int Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
