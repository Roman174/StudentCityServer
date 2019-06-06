using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Holod.Models.Database.QueueInfo
{
    public class Queue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CellQueue> Cells { get; set; }
        [ForeignKey("HostelId")]
        public int HostelId { get; set; }
        public Hostel Hostel { get; set; }
    }
}
