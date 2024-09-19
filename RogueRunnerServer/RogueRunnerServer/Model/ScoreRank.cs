using System.ComponentModel.DataAnnotations.Schema;

namespace RogueRunnerServer.Model
{
    public class ScoreRank
    {
        //플레이어가 게임 종료 되었을 때, 랭킹 갱신용.

        [Column("p_id")]
        public string P_Id { get; set; }

        [Column("nickname")]
        public string Nickname { get; set; }

        [Column("score")]
        public float Score { get; set; }
    }
}