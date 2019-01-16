using System.Collections.Generic;

namespace Holod.Models.Database
{
    public class StudentCity
    {
        public int Id { get; set; }
        public string Photo { get; set; }
        public List<Hostel> Hostels { get; set; }
        public List<Stuff> Stuffs { get; set; }

        public StudentCity()
        {
            Photo = string.Empty;
            Hostels = new List<Hostel>();
            Stuffs = new List<Stuff>();
        }
    }
}
