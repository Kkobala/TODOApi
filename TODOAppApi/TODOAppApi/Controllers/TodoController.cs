using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TODOAppApi.Db.Entities;
using TODOAppApi.Models.Requests;
using TODOAppApi.Repositories;

namespace TODOAppApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController: ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly ITodoRepository _todoRepository;

        public TodoController(
            UserManager<UserEntity> userManager, 
            ITodoRepository todoRepository)
        {
            _userManager = userManager;
            _todoRepository = todoRepository;
        }

        [Authorize("ApiUser", AuthenticationSchemes = "Bearer")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateTodoRequest request)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) { 
            return NotFound("User Not Found"); 
            }

            var userId = user.Id;
            await _todoRepository.InsertAsync(userId, request.Title, request.Description, request.Deadline);
            await _todoRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody]SearchTodoRequest request)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User Not Found");
            }

            var userId = user.Id;
            await _todoRepository.SearchAsync(userId, request.Title);
            await _todoRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(int id, [FromBody]UpdateTodoRequest request)
        { 
            var user = await _userManager.GetUserAsync(User);

            if (user == null) 
            {
                return NotFound("User Not Found");
            }

            var userId = user.Id;
            await _todoRepository.UpdateAsync(userId, request);
            await _todoRepository.SaveChangesAsync();

            return Ok();
        }
    }
}
