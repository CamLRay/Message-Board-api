using System.ComponentModel.DataAnnotations;
using System;
using Message.Models;

namespace Message.DTO
{
  public class PostDTO
  {
    public int PostId { get; set; }
    public string UserId { get; set; }
    public int ThreadId { get; set; }
    public string Message { get; set; }
    public int ReplyId { get; set; }
    public string Author { get; set; }
    public DateTime DateCreated { get; set; }
  }
  
}