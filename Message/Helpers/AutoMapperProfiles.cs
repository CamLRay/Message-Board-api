using Message.DTO;
using Message.Models;
using AutoMapper;

namespace Message.Helpers
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<Post, PostDTO>();
      CreateMap<PostDTO, Post>();
      CreateMap<Thread, ThreadDTO>();
      CreateMap<ThreadDTO, Thread>();
    }
  }
}