using filtertion_Task.DTO;
using filtertion_Task.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace filtertion_Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Model.Task>>> GetTasks()
        {
            return await _context.Tasks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Model.Task>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        [HttpPost]
        public async Task<ActionResult<Model.Task>> PostTask(TaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var teamMember = await _context.TeamMembers.FindAsync(taskDto.MemberId);
                if (teamMember == null)
                {
                    return BadRequest(new { message = "The MemberId does not exist in the TeamMembers table." });
                }

                var task = new Model.Task
                {
                    Name = taskDto.Name,
                    Description = taskDto.Description,
                    Status = taskDto.Status,
                    StartDate = taskDto.StartDate,
                    EndDate = taskDto.EndDate,
                    MemberId = taskDto.MemberId
                };

                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTask), new { id = task.TaskId }, task);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving the task.", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskDto taskDto)
        {
            if (id != taskDto.MemberId)  
            {
                return BadRequest(new { message = "The ID in the URL does not match the MemberId in the body" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            task.Name = taskDto.Name;
            task.Description = taskDto.Description;
            task.Status = taskDto.Status;
            task.StartDate = taskDto.StartDate;
            task.EndDate = taskDto.EndDate;
            task.MemberId = taskDto.MemberId;

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}
