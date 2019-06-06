using Holod.Models.Database.Pass;
using Holod.Models.Database.QueueInfo;
using System.ComponentModel.DataAnnotations.Schema;

namespace Holod.Models.Database
{
    public class Resident : Human
    {
        public string NumberRoom { get; set; }

        public int? HostelId { get; set; }
        [ForeignKey("HostelId")]
        public Hostel Hostel { get; set; }
        [ForeignKey("PassInfoId")]
        public int? PassInfoId { get; set; }
        public PassInfo PassInfo { get; set; }

        public int CellQueueId { get; set; }
        public CellQueue CellQueue { get; set; }

        public Resident() => NumberRoom = string.Empty;
    }
}
