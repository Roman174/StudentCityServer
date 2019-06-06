using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Holod.Models.Database.QueueInfo
{
    public class CellQueue
    {
        public int Id { get; set; }
        public DateTime RecordingTime { get; set; }
        [ForeignKey("ResidentId")]
        public int ResidentId { get; set; }
        public Resident Resident { get; set; }
        [ForeignKey("QueueId")]
        public int QueueId { get; set; }
        public Queue Queue { get; set; }
    }
}