using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Holod.Models.Database
{
    public class Hostel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Photo { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public int NumberFloors { get; set; }
        public int NumberStudents { get; set; }
        public double Rating { get; set; }

        public Coordinates Coordinates { get; set; }

        public List<Stuff> Stuffs { get; set; }
        public List<Resident> Residents { get; set; }

        public int StudentCityId { get; set; }
        [ForeignKey("StudentCityId")]
        public StudentCity StudentCity { get; set; }

        public Hostel()
        {
            Title = Phone = Photo = Address = string.Empty;
            NumberFloors = NumberStudents = 0;
            Rating = 0;
            Coordinates = new Coordinates();
            Stuffs = new List<Stuff>();
            Residents = new List<Resident>();
        }
    }
}
