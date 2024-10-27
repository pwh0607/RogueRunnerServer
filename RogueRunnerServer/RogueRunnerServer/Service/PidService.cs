
namespace RogueRunnerServer.Service
{
    public class PidService
    {
        public static string MakePid(string year, int cnt)
        {
            string userNum = (cnt+1).ToString();
            string pidTail = userNum.PadLeft(10 - userNum.Length + 1, '0');
            string newPid = year + "-" + pidTail;

            return newPid;
        }
    }
}
