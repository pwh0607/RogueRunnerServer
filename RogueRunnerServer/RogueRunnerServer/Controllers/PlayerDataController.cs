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
        [HttpPost]
        public async Task<IActionResult> PostPlayerData([FromBody] PlayerDataRequest request)
        {
            if (request == null){
                return BadRequest("Invalid data.");
            }

            //존재 여부를 확인하는 참조
            var existingData = await _context.PlayerDatas.FindAsync(request.P_Id);
            if (existingData != null) {
                existingData.Stage = request.Stage;
                existingData.SceneName = request.SceneName;
                //existingData.PlayerCharacter = request.PlayerCharacter;
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
                    //PlayerCharacter = request.PlayerCharacter,
                    HP = request.HP,
                    Score = request.Score,
                    Speed = request.Speed,
                    Skills = JsonConvert.SerializeObject(request.Skills)                                //Dictionary는 JSON(해당 서버에서는 string...)형식으로...
                };
                _context.PlayerDatas.Add(playerData);
            }
            await _context.SaveChangesAsync();
            return Ok(request);
        }

        //게임 이어하기시 사용자 데이터 가져오기.
        [HttpGet("{p_id}")]
        public async Task<IActionResult> GetPlayerData(string p_id)
        {
            var playerData = await _context.PlayerDatas.FindAsync(p_id);

            if (playerData == null)
            {
                // P_Id에 해당하는 레코드가 없으면 404 Not Found 반환
                return NotFound($"{p_id}의 PlayerData를 찾을 수 없습니다.");
            }

            var response = new PlayerDataResponse
            {
                Stage = playerData.Stage,
                SceneName = playerData.SceneName,
                //PlayerCharacter = playerData.PlayerCharacter,
                HP = playerData.HP,
                Score = playerData.Score,
                Speed = playerData.Speed,
                Skills = playerData.Skills
            };
            return Ok(response);
        }
        
        public class PlayerDataRequest
        {
            public string P_Id { get; set; }                           //고유 P_Id
            public int Stage { get; set; }                          //Stage 번호
            public string SceneName { get; set; }                   //씬 이름
           // public string PlayerCharacter { get; set; }             //사용자 캐릭터명
            public int HP { get; set; }                             //체력
            public float Score { get; set; }                        //누적 점수
            public float Speed { get; set; }                        //누적 스피드
            public Dictionary<string, int> Skills { get; set; }     //스킬셋
        }

        public class PlayerDataResponse
        {
            //p_id를 url을 통해 데이터를 받아오기 때문에 불필요
            public int Stage { get; set; }                             
            public string SceneName { get; set; }                      
           // public string PlayerCharacter { get; set; }               
            public int HP { get; set; }                                
            public float Score { get; set; }                           
            public float Speed { get; set; }                          
            public string Skills { get; set; }        
        }
    }
}