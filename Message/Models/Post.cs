using System;

namespace Message.Models
{
  public class Post
  {
    public int PostId { get; set; }
    public string Message { get; set; }
    public string Author { get; set; }
    public DateTime DateCreated { get; set; }
    public virtual ApplicationUser User { get; set; }
    public int ReplyId { get; set; }
    public int ThreadId { get; set; }
    public Post()
    {
      DateCreated = DateTime.Now;
    }
  }
}