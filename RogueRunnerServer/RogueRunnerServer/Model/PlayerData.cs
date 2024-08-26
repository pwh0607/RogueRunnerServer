using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace RogueRunnerServer.Model
{
    public class PlayerData
    {
        public string P_Id { get; set; }
        public int Stage { get; set; }                      //Stage (오름차)
        public string SceneName { get; set; }               //실행할 씬 이름
        public string PlayerCharacter { get; set; }         //사용할 캐릭터 명
        public int HP { get; set; }
        public float Score { get; set; }
        public float Speed { get; set; }
        public string Skills { get; set; }
    }
}
