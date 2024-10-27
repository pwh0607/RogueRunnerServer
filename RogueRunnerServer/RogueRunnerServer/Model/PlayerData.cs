using System.ComponentModel.DataAnnotations.Schema;
namespace RogueRunnerServer.Model
{
    public class PlayerData
    {
        [Column("P_id")]
        public string P_Id { get; set; }

        [Column("Stage")]
        public int Stage { get; set; }

        [Column("SceneName")]
        public string SceneName { get; set; }

        [Column("HP")]
        public int HP { get; set; }

        [Column("Score")]
        public float Score { get; set; }

        [Column("Speed")]
        public float Speed { get; set; }

        [Column("Skills")]
        public string Skills { get; set; }
    }
}
