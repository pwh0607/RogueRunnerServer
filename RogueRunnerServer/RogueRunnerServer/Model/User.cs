using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RogueRunnerServer.Model
{
    public class User
    {
        [Column("p_id")]
        public string P_Id { get; set; }

        [Column("user_id")]
        public string Id { get; set; }

        [Column("user_password")]
        public string PasswordHash { get; set; }

        [Column("user_nickname")]
        public string Nickname { get; set; }

        public User() { }
        public User(string p_id, string id, string password, string nickname){
            P_Id = p_id;
            Id = id;
            PasswordHash = password;
            Nickname = nickname;
        }
    }
}