namespace AspBlog.DATA.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public AspNetUser User { get; set; }
    }
}
