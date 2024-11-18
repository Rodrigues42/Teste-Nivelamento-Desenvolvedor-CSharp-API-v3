using System;

class Program
{
    static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    static async Task<int> getTotalScoredGoals(string team, int year)
    {
        var parametersTeams1 = new Dictionary<string, string>
        {
            { "team1", team },
            { "year", year.ToString() }
        };

        var parametersTeams2 = new Dictionary<string, string>
        {
            { "team2", team },
            { "year", year.ToString() }
        };

        var team1 = await calculateScoredGoals(team, parametersTeams1);
        var team2 = await calculateScoredGoals(team, parametersTeams2);

        return team1 + team2;
    }

    static async Task<int> calculateScoredGoals(String team, Dictionary<string, string> parameters)
    {
        string url = "https://jsonmock.hackerrank.com/api/football_matches";
        var baseClient = new BaseClient();

        int sum = 0;
        int totalPages = 1;

        for (int currentPage = 1; currentPage <= totalPages; currentPage++)
        {
            parameters["page"] = currentPage.ToString();

            var corveted = await baseClient.Get<BaseResponse<List<FootBallMatches>>>(url, parameters);
            totalPages = corveted.total_pages;

            foreach (var game in corveted.data)
            {
                if (team.Equals(game.team1))
                {
                    sum += int.Parse(game.team1goals);
                } else
                {
                    sum += int.Parse(game.team2goals);
                }
            }
        }

        return sum;
    }
}