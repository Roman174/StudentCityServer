namespace Holod.Models.Database.Pass
{
    public class RequestPass
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Patronymic { get; set; }
        public string NumberOfHostel { get; set; }
        public int NumberOfRoom { get; set; }
        public string Faculty { get; set; }
    }
}