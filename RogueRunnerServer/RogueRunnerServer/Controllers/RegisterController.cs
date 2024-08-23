using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RogueRunnerServer.Model;
using System.Data;
using RogueRunnerServer.Data;

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

        //DB에 user정보를 저장하는 메서드.        POST
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            // 기본적인 데이터 유효성 검사
            if (string.IsNullOrEmpty(request.Id) || string.IsNullOrEmpty(request.Nickname) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("All fields are required.");
            }

            Console.WriteLine($"요청 정보 - UserID : {request.Id}, Password : {request.Password}, Nickname : {request.Nickname},");

            // 고유 식별번호 생성
            string p_id = System.Guid.NewGuid().ToString();

            //비밀번호 해싱
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(null, request.Password);

            // 사용자 데이터 생성
            var newUser = new User(p_id, request.Id, hashedPassword, request.Nickname);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully.", user = newUser });
        }

        //회원가입 후 DB에서 user정보들을 받아오는 메서드.
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
    }

    public class RegisterRequest
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
    }
}