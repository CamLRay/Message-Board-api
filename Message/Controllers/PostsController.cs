using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Message.Models;
using Message.DTO;
using Message.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using AutoMapper.QueryableExtensions;


namespace Message.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly MessageContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        

        public PostsController(MessageContext context, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPost(string author)
        {
            var posts = _context.Posts
                .AsQueryable();

            if(author != null)
            {
                posts = posts.Where(entry => entry.Author == author);
            }
            var model = await posts.ProjectTo<PostDTO>(_mapper.ConfigurationProvider).ToListAsync();
            
            return Ok(model);
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(int id)
        {
            var post = await _context.Posts
                .Where(x => x.PostId == id)
                .ProjectTo<PostDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
            if (post == null)
            {
                return NotFound();
            }
            return post;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostDTO post, string userId)
        {
            var thisPost = _context.Posts.FirstOrDefault(entry => entry.PostId == id);
            if(thisPost == null) return NotFound();
            if(post.UserId != userId)
            {
                return BadRequest();
            }
            if (id != post.PostId)
            {
                return BadRequest();
            }
            _mapper.Map(post, thisPost);

            _context.Posts.Update(thisPost);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PostDTO>> PostPost(AddPostDTO model)
        {
            var currentUser = await _userManager.FindByIdAsync(model.UserId);
    
            Post post = new Post() {
                Message = model.Message,
                Author = currentUser.UserName,
                User = currentUser,
                ReplyId = model.ReplyId,
                ThreadId = model.ThreadId
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPost), new { id = post.PostId });
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id, string userId)
        {
           
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            if (userId == post.User.Id)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
                return NoContent();
            }
    
            return BadRequest();
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
