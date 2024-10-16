using Microsoft.AspNetCore.Mvc;
using RogueRunnerServer.Data;
using System;
using RogueRunnerServer.Model;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

using static RogueRunnerServer.Controllers.PlayerDataController;
using Microsoft.EntityFrameworkCore;

namespace RogueRunnerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoreRankController : ControllerBase
    {
        private readonly ScoreRankDbContext _context;

        public ScoreRankController(ScoreRankDbContext context) { 
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendScoreData([FromBody] ScoreRankRequest request)
        {
            Console.WriteLine("POST : Rank Score Update Request");
            if (request == null)
            {
                return BadRequest("Invalid data.");
            }

            var existingData = await _context.ScoreDatas.FindAsync(request.P_Id);

            if (existingData != null)
            {
                //기존 데이터가 존재하면 갱신. 하지만 좀더 높은 점수일 경우만 갱신하기.
                if (existingData.Score < request.Score)
                {
                    existingData.Score = request.Score;
                    Console.WriteLine("New Score Updating...");
                    _context.ScoreDatas.Update(existingData);
                }
                else
                {
                    Console.WriteLine("Score Not updating...");
                }
            }
            else
            {
                //데이터가 없으면 추가
                var ScoreData = new ScoreRank
                {
                    P_Id = request.P_Id,
                    Nickname = request.NickName,
                    Score = request.Score
                };

                _context.ScoreDatas.Add(ScoreData);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while saving data: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

            return Ok(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetScoreRank(){

            Console.WriteLine("GET : Score RankList Request");

            var scoreRankList = await _context.ScoreDatas.OrderByDescending(data => data.Score).Select(data => new ScoreRankResponse
            {
                P_Id = data.P_Id,
                NickName = data.Nickname,
                Score = data.Score
            }).ToListAsync();

            Console.WriteLine("랭크 리스트 Get 호출 : " + scoreRankList.Count);

            foreach(var rank in scoreRankList)
            {
                Console.WriteLine($"{rank.P_Id},{rank.NickName},{rank.Score}");
            }

            if (scoreRankList == null)
            {
                Console.WriteLine("Non RankList");
            }

            var rankResponse = new RankListResponse
            {
                Ranks = scoreRankList
            };

            return Ok(rankResponse);
        }

        public class ScoreRankRequest {
            public string P_Id { get; set; }
            public string NickName { get; set; }
            public float Score { get; set; }
        }

        public class ScoreRankResponse {
            public string P_Id { get; set; }
            public string NickName { get; set; }
            public float Score { get; set; }
        }

        public class RankListResponse
        {
            public List<ScoreRankResponse> Ranks { get; set; }
        }
    }
}