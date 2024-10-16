using Microsoft.AspNetCore.Mvc;
using RogueRunnerServer.Data;
using System;
using RogueRunnerServer.Model;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace RogueRunnerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerDataController : ControllerBase
    {
        private readonly PlayerDbContext _context;

        public PlayerDataController(PlayerDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PostPlayerData([FromBody] PlayerDataRequest request)
        {
            Console.WriteLine("POST : PlayerData Update Request...");
            if (request == null){
                return BadRequest("Invalid data.");
            }

            //존재 여부를 확인하는 참조
            var existingData = await _context.PlayerDatas.FindAsync(request.P_Id);
            if (existingData != null) {
                existingData.Stage = request.Stage;
                existingData.SceneName = request.SceneName;
                existingData.HP = request.HP;
                existingData.Score = request.Score;
                existingData.Speed = request.Speed;
                existingData.Skills = JsonConvert.SerializeObject(request.Skills);

                _context.PlayerDatas.Update(existingData);
            }
            else
            {
                var playerData = new PlayerData
                {
                    P_Id = request.P_Id,
                    Stage = request.Stage,
                    SceneName = request.SceneName,
                    HP = request.HP,
                    Score = request.Score,
                    Speed = request.Speed,
                    Skills = JsonConvert.SerializeObject(request.Skills)
                };
                _context.PlayerDatas.Add(playerData);
            }
            await _context.SaveChangesAsync();
            return Ok(request);
        }

        [HttpGet("{p_id}")]
        public async Task<IActionResult> GetPlayerData(string p_id)
        {
            Console.WriteLine("GET : PlayerData Get Request...");
            var playerData = await _context.PlayerDatas.FindAsync(p_id);

            if (playerData == null)
            {
                return NotFound($"{p_id}의 PlayerData를 찾을 수 없습니다.");
            }

            var response = new PlayerDataResponse
            {
                Stage = playerData.Stage,
                SceneName = playerData.SceneName,
                HP = playerData.HP,
                Score = playerData.Score,
                Speed = playerData.Speed,
                Skills = playerData.Skills
            };
            return Ok(response);
        }

        [HttpDelete("{p_id}")]
        public async Task<IActionResult> DeletePlayerData(string p_id)
        {
            Console.WriteLine("DELETE : PlayerData Delete Request...");
            var playerData = await _context.PlayerDatas.FindAsync(p_id);
            if (playerData == null)
            {
                return NotFound($"{p_id}의 PlayerData를 찾을 수 없습니다.");
            }

            _context.PlayerDatas.Remove(playerData);
            await _context.SaveChangesAsync();

            return Ok($"{p_id}의 임시 PlayerData가 성공적으로 삭제되었습니다.");
        }

        public class PlayerDataRequest
        {
            public string P_Id { get; set; }                      
            public int Stage { get; set; }                        
            public string SceneName { get; set; }                   
            public int HP { get; set; }                            
            public float Score { get; set; }                        
            public float Speed { get; set; }                    
            public Dictionary<string, int> Skills { get; set; }   
        }

        public class PlayerDataResponse
        {
            public int Stage { get; set; }                             
            public string SceneName { get; set; }                       
            public int HP { get; set; }                                
            public float Score { get; set; }                           
            public float Speed { get; set; }                          
            public string Skills { get; set; }        
        }
    }
}