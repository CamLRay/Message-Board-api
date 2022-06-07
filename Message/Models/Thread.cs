using System;
using System.Collections.Generic;

namespace Message.Models
{
  public class Thread
  {
    public int ThreadId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime DateCreated { get; set; }
    public virtual ApplicationUser User { get; set; }
    public int InitialPostId { get; set; }
    public virtual ICollection<Post> Posts { get; set; }
    public Thread()
    {
      DateCreated = DateTime.Now;
      this.Posts = new HashSet<Post>();
    }
  }
}