using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
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
    public class ThreadsController : ControllerBase
    {
        private readonly MessageContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public ThreadsController(MessageContext context,UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns thread
        /// </summary>
        /// <returns> A List of threads</returns>
        /// <remarks>
        ///
        /// Sample request
        /// GET /api/threads
        ///
       /// </remarks>
        ///<response code="200">Returns a list of Threads</response>
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThreadDTO>>> GetThread(string author)
        {
            var threads = _context.Threads
                .AsQueryable();

            if(author != null)
            {
                threads = threads.Where(entry => entry.Author == author);
            }
            var model = await threads.ProjectTo<ThreadDTO>(_mapper.ConfigurationProvider).ToListAsync();
            return Ok(model);
        }

        // GET: api/Threads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ThreadDTO>> GetThread(int id)
        {
            var thread = await _context.Threads
                .Where(x => x.ThreadId == id)
                .ProjectTo<ThreadDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (thread == null)
            {
                return NotFound();
            }

            return thread;
        }

        // PUT: api/Threads/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutThread(int id, ThreadDTO thread, string userId)
        {
            var thisThread = await _context.Threads.FindAsync(id);
            if(thisThread == null) return NotFound();
            if(thread.UserId != userId)
            {
                return BadRequest();
            }
            if (id != thread.ThreadId)
            {
                return BadRequest();
            }
            _mapper.Map(thread, thisThread);

            _context.Threads.Update(thisThread);
            // _context.Entry(model).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThreadExists(id))
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

        // POST: api/Threads
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ThreadDTO>> PostThread(AddThreadDTO model)
        {
            var currentUser = await _userManager.FindByIdAsync(model.UserId);
    
            Thread thread = new Thread() {
                Title = model.Title,
                Author = currentUser.UserName,
                User = currentUser,
            };
            _context.Threads.Add(thread);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetThread", new { id = thread.ThreadId });
        }

        // DELETE: api/Threads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThread(int id, string userId)
        {
            var thread = await _context.Threads.FindAsync(id);
            if (thread == null)
            {
                return NotFound();
            }
            if (userId == thread.User.Id)
            {
                _context.Threads.Remove(thread);
                await _context.SaveChangesAsync();
                return NoContent();
            }

            return BadRequest();
        }

        private bool ThreadExists(int id)
        {
            return _context.Threads.Any(e => e.ThreadId == id);
        }
    }
}
