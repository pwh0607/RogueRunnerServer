
using Newtonsoft.Json;

namespace RogueRunnerServer
{

    class Matches
    {
        public int page { get; set; }
        public int per_page { get; set; }
        public int total { get; set; }
        public int total_pages { get; set; }
        public Competition[] data { get; set; }
    }

    class Competition
    {
        public string competition { get; set; }
        public int year { get; set; }
        public string round { get; set; }
        public string team1 { get; set; }
        public string team2 { get; set; }
        public string team1goals { get; set; }
        public string team2goals { get; set; }
    }
    public class Test
    {

        private static readonly HttpClient _httpClient = new HttpClient();
        private string url = "https://jsonmock.hackerrank.com/api/";

        private static async Task<int> GetCountFor(int year, int page)
        {
            int count = 0;

            return count;
        }

        public async Task<int> GetNumDraws(int year)
        {
            int count = 0;
            var response = await _httpClient.GetStringAsync(url + $"football_matches?year={year}");
            var result = JsonConvert.DeserializeObject<Matches>(response);

            foreach (var match in result.data)
            {
                if(match.team1goals == match.team2goals)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
}
