using System.ComponentModel.DataAnnotations.Schema;

namespace Holod.Models.Database
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public Stuff Stuff { get; set; }

        public Post() => Title = string.Empty;

        public Post(int Id, string Title)
        {
            this.Id = Id;
            this.Title = Title;
        }
    }
}
