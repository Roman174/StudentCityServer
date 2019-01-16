using System.ComponentModel.DataAnnotations.Schema;

namespace Holod.Models.Database
{
    public class Resident : Human
    {
        public string NumberRoom { get; set; }

        public int? HostelId { get; set; }
        [ForeignKey("HostelId")]
        public Hostel Hostel { get; set; }

        public Resident() => NumberRoom = string.Empty;
    }
}
