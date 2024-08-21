using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        // 메모리 내 사용자 정보를 저장할 리스트
        private static readonly List<User> Users = new List<User>();

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            // 기본적인 데이터 유효성 검사
            if (string.IsNullOrEmpty(request.Id) || string.IsNullOrEmpty(request.Nickname) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("All fields are required.");
            }

            // 고유 식별번호 생성
            string p_id = System.Guid.NewGuid().ToString();

            // 사용자 데이터 생성
            var newUser = new User
            {
                P_Id = p_id,
                Id = request.Id,
                Nickname = request.Nickname,
                Password = request.Password // 비밀번호는 해시화하여 저장하는 것이 좋습니다
            };

            Console.WriteLine($"UserID : {request.Id}, Password : {request.Password}, Nickname : {request.Nickname},");

            // 리스트에 사용자 추가
            Users.Add(newUser);

            return Ok(new { message = "User registered successfully.", user = newUser });
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(Users);
        }
    }

    public class RegisterRequest
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
    }

    public class User
    {
        public string P_Id { get; set; }
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
    }
}