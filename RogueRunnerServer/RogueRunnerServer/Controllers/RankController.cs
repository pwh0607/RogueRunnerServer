using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace RogueRunnerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RankController : ControllerBase
    {
        // Sample data for testing
        private static readonly List<RankEntry> rankList = new List<RankEntry>
        {
            new RankEntry { Nickname = "PlayerOne", Score = 1500 },
            new RankEntry { Nickname = "PlayerTwo", Score = 1200 },
            new RankEntry { Nickname = "PlayerThree", Score = 1100 },
            new RankEntry { Nickname = "PlayerFour", Score = 900 }
        };

        // GET: /Rank
        [HttpGet]
        public ActionResult<RankListResponse> GetRankList()
        {
            Console.WriteLine("랭크 리스트 Get 호출 : "+rankList.Count);

            var response = new RankListResponse
            {
                Ranks = rankList
            };
            return Ok(response);
        }
    }

    [Serializable]
    public class RankEntry
    {
        public string Nickname { get; set; }
        public int Score { get; set; }
    }

    [Serializable]
    public class RankListResponse
    {
        public List<RankEntry> Ranks { get; set; }
    }
}
