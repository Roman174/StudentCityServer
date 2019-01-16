namespace Holod.Models.Database
{
    public class Coordinates
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Coordinates() => Latitude = Longitude = 0;
    }
}
