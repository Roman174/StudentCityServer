using System.ComponentModel.DataAnnotations.Schema;

namespace Holod.Models.Database.Pass
{
    public class PassInfo
    {
        public int Id { get; set; }
        [ForeignKey("IdResident")]
        public int IdResident { get; set; }
        public Resident Resident { get; set; }
    }
}