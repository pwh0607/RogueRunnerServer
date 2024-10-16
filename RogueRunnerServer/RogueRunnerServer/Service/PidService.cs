using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;

namespace RogueRunnerServer.Service
{
    public class PidService
    {
        public static string MakePid(int cnt)
        {
            DateTime today = DateTime.Today;
            string year = (today.Year % 100).ToString();
            string userNum = (cnt+1).ToString();
            string pidTail = userNum.PadLeft(10 - userNum.Length + 1, '0');
            string newPid = year + "-" + pidTail;

            return newPid;
        }
    }
}
