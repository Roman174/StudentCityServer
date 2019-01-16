namespace Holod.Models.Database
{
    public class Human
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Patronymic { get; set; }
        public string Photo { get; set; }

        public Human() => Surname = Firstname = Patronymic = Photo = string.Empty;
    }
}
