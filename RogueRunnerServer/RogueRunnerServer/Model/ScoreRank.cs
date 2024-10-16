using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RogueRunnerServer.Model
{
    public class ScoreRank
    {
        [Column("p_id")]
        public string P_Id { get; set; }

        [Column("nickname")]
        public string Nickname { get; set; }

        [Column("score")]
        public float Score { get; set; }
    }
}