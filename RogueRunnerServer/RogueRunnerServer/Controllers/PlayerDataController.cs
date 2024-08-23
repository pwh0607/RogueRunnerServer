using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace RogueRunnerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayerDataController : ControllerBase
    {
        [HttpPost("playerData")]
        public IActionResult PostPlayerData([FromBody] PlayerDataRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid data.");
            }

            // 여기에서 수신된 데이터를 처리하거나 데이터베이스에 저장할 수 있습니다.
            Console.WriteLine($"Received data: Stage {request.Stage}, Scene {request.SceneName}, " +
                $"Character {request.PlayerCharacter}, HP {request.HP}, Score {request.Score}, Speed {request.Speed}");

            // 예시로, 받은 데이터를 다시 반환
            return Ok(request);
        }

        public class PlayerDataRequest
        {
            //public int p_id { get; set; }
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