﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RogueRunnerServer.Model;
using RogueRunnerServer.Data;
using System.Data;

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

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            // 기본적인 데이터 유효성 검사
            if (string.IsNullOrEmpty(request.Id) || string.IsNullOrEmpty(request.Nickname) || string.IsNullOrEmpty(request.Password)){
                return BadRequest("모든 데이터를 넣어주세요...");
            }

            Console.WriteLine($"요청 정보 - UserID : {request.Id}, Password : {request.Password}, Nickname : {request.Nickname},");

            // 사용자 ID 중복 검사
            if (await IsUserIdDuplicated(request.Id)){
                return Conflict(new { message = "이미 사용 중인 ID입니다." });
            }
            // 닉네임 중복 검사
            if (await IsNicknameDuplicated(request.Nickname)){
                return Conflict(new { message = "이미 사용 중인 NickName입니다." });
            }

            string p_id = System.Guid.NewGuid().ToString();             // 고유 식별번호 생성
            var passwordHasher = new PasswordHasher<User>();            //비밀번호 해싱
            var hashedPassword = passwordHasher.HashPassword(null, request.Password);

            var newUser = new User(p_id, request.Id, hashedPassword, request.Nickname); // 사용자 데이터 모델 생성

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok(new { message = "회원 가입 성공.", user = newUser });
        }

        // 사용자 ID 중복 검사 메서드
        private async Task<bool> IsUserIdDuplicated(string userId){
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }

        // 닉네임 중복 검사 메서드
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