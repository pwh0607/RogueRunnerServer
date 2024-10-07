﻿using Microsoft.AspNetCore.Mvc;
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
                    Console.WriteLine("기존의 Score 보다 높아 갱신합니다.");
                    _context.ScoreDatas.Update(existingData);
                }
                else
                {
                    Console.WriteLine("기존의 Score 보다 낮아 갱신하지 않습니다.");
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

            await _context.SaveChangesAsync();

            // 예시로, 받은 데이터를 다시 반환
            return Ok(request);
        }

        [HttpGet]
        public async Task<IActionResult> GetScoreRank(){
            //정렬된 리스트의 형식으로 db에서 데이터 가져오기.
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
                Console.WriteLine("랭크 리스트가 없습니다.");
            }

            var rankResponse = new RankListResponse
            {
                Ranks = scoreRankList
            };

            /*
             * query 형식의 DB context.
                string query = @"
                                SELECT P_Id, Nickname, Score
                                FROM scoreboard
                                ORDER BY Score DESC";
            
                var scoreRankList = await _context.ScoreDatas.FromSqlRaw(query).ToListAsync();
            */
            return Ok(rankResponse);
        }

        //게임 완료후 데이터를 랭킹 리스트에 갱신 및 추가하는 Request
        public class ScoreRankRequest {
            public string P_Id { get; set; }
            public string NickName { get; set; }
            public float Score { get; set; }
        }

        //랭킹 리스트를 모두 가져오는 Res
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