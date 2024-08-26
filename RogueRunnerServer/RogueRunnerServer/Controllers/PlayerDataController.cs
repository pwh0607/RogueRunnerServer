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
        //DB 참조 변수.
        private readonly PlayerDbContext _context;

        public PlayerDataController(PlayerDbContext context)
        {
            _context = context;
        }

        //Upsert 방식으로 P_Id를 기준으로 기존의 값이 존재하면 갱신하고, 없었으면 추가하는 방식이다.
        [HttpPost("playerData")]
        public async Task<IActionResult> PostPlayerData([FromBody] PlayerDataRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid data.");
            }

            // 여기에서 수신된 데이터를 처리하거나 데이터베이스에 저장할 수 있습니다.
            Console.WriteLine($"Received data: P_Id {request.P_Id}, Stage {request.Stage}, Scene {request.SceneName}, " +
                $"Character {request.PlayerCharacter}, HP {request.HP}, Score {request.Score}, Speed {request.Speed}");

            //존재 여부를 확인하는 참조
            var existingPlayerData = await _context.PlayerDatas.FindAsync(request.P_Id);

            if (existingPlayerData != null) {
                //기존 레코드 존재시 갱신하기.
                existingPlayerData.Stage = request.Stage;
                existingPlayerData.SceneName = request.SceneName;
                existingPlayerData.PlayerCharacter = request.PlayerCharacter;
                existingPlayerData.HP = request.HP;
                existingPlayerData.Score = request.Score;
                existingPlayerData.Speed = request.Speed;
                existingPlayerData.Skills = JsonConvert.SerializeObject(request.Skills);

                _context.PlayerDatas.Update(existingPlayerData);
            }
            else
            {
                //데이터가 없으면 새로 생성하기.
                var playerData = new PlayerData
                {
                    P_Id = request.P_Id,
                    Stage = request.Stage,
                    SceneName = request.SceneName,
                    PlayerCharacter = request.PlayerCharacter,
                    HP = request.HP,
                    Score = request.Score,
                    Speed = request.Speed,
                    Skills = JsonConvert.SerializeObject(request.Skills)         //Dictionary는 JSON(해당 서버에서는 string...)형식으로...
                };

                _context.PlayerDatas.Add(playerData);
            }

            await _context.SaveChangesAsync();

            // 예시로, 받은 데이터를 다시 반환
            return Ok(request);
        }

        //게임 이어하기시 사용자 데이터 가져오기.
        
        [HttpGet("playerData/{p_id}")]
        public async Task<IActionResult> GetPlayerData(int p_id)
        {
            var playerData = await _context.PlayerDatas.FindAsync(p_id);

            if (playerData == null)
            {
                // P_Id에 해당하는 레코드가 없으면 404 Not Found 반환
                return NotFound($"PlayerData with P_Id {p_id} not found.");
            }

            var response = new PlayerDataResponse
            {
                P_Id = playerData.P_Id,
                Stage = playerData.Stage,
                SceneName = playerData.SceneName,
                PlayerCharacter = playerData.PlayerCharacter,
                HP = playerData.HP,
                Score = playerData.Score,
                Speed = playerData.Speed,
                Skills = JsonConvert.DeserializeObject<Dictionary<string, int>>(playerData.Skills)
            };

            return Ok(response);
        }
        
        public class PlayerDataRequest
        {
            public string P_Id { get; set; }                           //고유 P_Id
            public int Stage { get; set; }                          //Stage 번호
            public string SceneName { get; set; }                   //씬 이름
            public string PlayerCharacter { get; set; }             //사용자 캐릭터명
            public int HP { get; set; }                             //체력
            public float Score { get; set; }                        //누적 점수
            public float Speed { get; set; }                        //누적 스피드
            public Dictionary<string, int> Skills { get; set; }     //스킬셋
        }

        public class PlayerDataResponse
        {
            public string P_Id { get; set; }                           
            public int Stage { get; set; }                             
            public string SceneName { get; set; }                      
            public string PlayerCharacter { get; set; }               
            public int HP { get; set; }                                
            public float Score { get; set; }                           
            public float Speed { get; set; }                          
            public Dictionary<string, int> Skills { get; set; }        
        }
    }
}