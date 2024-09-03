using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace RogueRunnerServer.Model
{
    public class PlayerData
    {
        public string P_Id { get; set; }
        public int Stage { get; set; }                      
        public string SceneName { get; set; }               
        public string PlayerCharacter { get; set; }         
        public int HP { get; set; }
        public float Score { get; set; }
        public float Speed { get; set; }
        public string Skills { get; set; }
    }
}
