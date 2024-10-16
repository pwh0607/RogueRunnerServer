﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RogueRunnerServer.Model;
using RogueRunnerServer.Data;

namespace RogueRunnerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserDbContext _context;

        public LoginController(UserDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest request)
        {
            Console.WriteLine("POST : Login Request...");

            if (string.IsNullOrEmpty(request.Id) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "ID와 비밀번호를 모두 적어주세요." });           //400
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.Id);
            if (user == null)
            {
                return Unauthorized(new { message = "옳지 않은 ID 혹은 비밀번호입니다." });               //401
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "옳지 않은 ID 혹은 비밀번호입니다." });
            }

            return Ok(new
            {
                message = $"Login successful. p_id : {user.P_Id}",
                P_Id = user.P_Id,
                User_Id = user.Id,
                NickName = user.Nickname
            });
        }
    }
    public class LoginRequest
    {
        public string Id { get; set; }
        public string Password { get; set; }
    }
}