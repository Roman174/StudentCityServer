using System.ComponentModel.DataAnnotations.Schema;

namespace Holod.Models.Database
{
    public class Stuff : Human
    {
        public int? HostelId { get; set; }
        [ForeignKey("HostelId")]
        public Hostel Hostel { get; set; }

        public int? StudentCityId { get; set; }
        [ForeignKey("StudentCityId")]
        public StudentCity StudentCity { get; set; }

        public Post Post { get; set; }
        [ForeignKey("PostId")]
        public int PostId { get; set; }

        public Stuff()
        {
            Post = new Post();
        }
    }
}