namespace RogueRunnerServer.Model
{
    public class ScoreRank
    {
        //플레이어가 게임 종료 되었을 때, 랭킹 갱신용.
        public string P_Id { get; set; }
        
        public string Nickname { get; set; }
        public float Score { get; set; }
    }
}