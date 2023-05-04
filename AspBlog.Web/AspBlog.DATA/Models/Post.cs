using System.ComponentModel.DataAnnotations;

namespace AspBlog.DATA.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public AspNetUser User { get; set; }
        public List<Comment> Comments { get; set; }

    }
}
