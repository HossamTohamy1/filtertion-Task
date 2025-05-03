using filtertion_Task.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace filtertion_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MemberController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamMember>>> GetTeamMembers()
        {
            return await _context.TeamMembers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamMember>> GetTeamMember(int id)
        {
            var teamMember = await _context.TeamMembers.FindAsync(id);

            if (teamMember == null)
            {
                return NotFound();
            }

            return teamMember;
        }

        [HttpPost]
        public async Task<ActionResult<TeamMember>> PostTeamMember(TeamMember teamMember)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TeamMembers.Add(teamMember);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTeamMember), new { id = teamMember.MemberId }, teamMember);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeamMember(int id, TeamMember teamMember)
        {
            if (id != teamMember.MemberId)
            {
                return BadRequest(new { message = "The ID in the URL does not match the MemberId in the body" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(teamMember).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamMemberExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeamMember(int id)
        {
            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember == null)
            {
                return NotFound();
            }

            var tasks = await _context.Tasks.Where(t => t.MemberId == id).ToListAsync();
            foreach (var task in tasks)
            {
                task.MemberId = null;
            }

            _context.TeamMembers.Remove(teamMember);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TeamMemberExists(int id)
        {
            return _context.TeamMembers.Any(e => e.MemberId == id);
        }
    }
}
