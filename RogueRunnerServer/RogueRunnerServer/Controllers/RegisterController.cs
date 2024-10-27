using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RogueRunnerServer.Model;
using RogueRunnerServer.Data;
using RogueRunnerServer.Service;
using System.Data;
using System.Reflection.Metadata.Ecma335;

namespace RogueRunnerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        //DB 참조 변수.
        private readonly UserDbContext _context;

        public RegisterController(UserDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.Id) || string.IsNullOrEmpty(request.Nickname) || string.IsNullOrEmpty(request.Password)){
                return BadRequest("User ID and Nickname are required and cannot be empty.");
            }

            if (await IsUserIdDuplicated(request.Id)){
                return Conflict(new { message = "The provided user ID already exists." });
            }

            if (await IsNicknameDuplicated(request.Nickname)){
                return Conflict(new { message = "The provided user Nickname already exists." });
            }

            string curYear = (DateTime.Today.Year % 100).ToString();
            int cnt = _context.Users.Count(u => u.P_Id.StartsWith(curYear));
            string p_id = PidService.MakePid(curYear,cnt);

            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(null, request.Password);

            var newUser = new User(p_id, request.Id, hashedPassword, request.Nickname); 

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Complete", user = newUser });
        }

        private async Task<bool> IsUserIdDuplicated(string userId){
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        private async Task<bool> IsNicknameDuplicated(string nickname){
            return await _context.Users.AnyAsync(u => u.Nickname == nickname);
        }
    }

    public class RegisterRequest
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
    }
}