using System;
using System.Collections.Generic;
namespace Message.DTO
{
  public class ThreadDTO
  {
    public int ThreadId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string UserId { get; set; }
    public DateTime DateCreated { get; set; }
    public ICollection<PostDTO> Posts { get; set; }
  }
}