using System.ComponentModel.DataAnnotations;

namespace Message.DTO
{
  public class AddPostDTO
  {
    public string UserId { get; set; }
    public int ThreadId { get; set; }
    public string Message { get; set; }
    public int ReplyId { get; set; }
  }
}