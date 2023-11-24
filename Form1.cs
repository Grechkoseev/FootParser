using FootParser.Entities;
using FootParser.ContentParsers;
using FootParser.DatabaseManager;

namespace FootParser;

public partial class FootParserForm : Form
{
    private readonly string _dbUsername;
    private readonly string _dbPassword;
    private readonly string _dbAddress;
    private readonly string _dbPort;
    private readonly string _dbName;

    private readonly Dictionary<string, string> _parametersDictionary = new();
    private List<IEntity> _entityList = new();

    public FootParserForm()
    {
        InitializeComponent();
        leagueComboBox.Items.AddRange(ConfigHandler.LeagueDictionary.Keys.ToArray());
        leagueComboBox.SelectedIndex = 0;
        var seasonDictionaryKeys = ConfigHandler.SeasonDictionary.Keys.ToList();
        seasonDictionaryKeys.Reverse();
        seasonComboBox.DataSource = seasonDictionaryKeys;
        seasonComboBox.SelectedIndex = 0;
        _dbUsername = ConfigHandler.Username;
        _dbPassword = ConfigHandler.Password;
        _dbAddress = ConfigHandler.Address;
        _dbName = ConfigHandler.Database;
        _dbPort = ConfigHandler.Port;
    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private async void ParseTeamsButton_Click(object sender, EventArgs e)
    {
        var path = new List<string>
        {
            "participants",
            "teams"
        };

        const string url = "https://kfl55.ru";
        var teamsParser = new Parser(path, _parametersDictionary, "Team", url);
        Cursor = Cursors.WaitCursor;
        _entityList = await teamsParser.GetEntityListTask();
        var teams = _entityList.OfType<Team>().ToList();

        await using MyDbContext context = new(_dbUsername, _dbPassword, _dbAddress, _dbPort, _dbName);

        foreach (var obj in teams.Where(obj => !context.Teams.Any(o => o.Name == obj.Name)))
        {
            context.Teams.Add(obj);
        }

        await context.SaveChangesAsync();
        Cursor = Cursors.Default;

        MessageBox.Show(context.Teams.Count().ToString());
    }

    private async void ParseRefereesButton_Click(object sender, EventArgs e)
    {
        var path = new List<string>
        {
            "participants",
            "referees"
        };

        const string url = "https://kfl55.ru";
        var refereesParser = new Parser(path, _parametersDictionary, "Referee", url);
        var _entityList = await refereesParser.GetEntityListTask();
    }

    private async void ParsePlayersButton_Click(object sender, EventArgs e)
    {
        var path = new List<string>
        {
            "participants",
            "players"
        };

        const string url = "https://kfl55.ru";
        var playersParser = new Parser(path, _parametersDictionary, "Player", url);
        Cursor = Cursors.WaitCursor;
        _entityList = await playersParser.GetEntityListTask();
        var players = _entityList.OfType<Player>().ToList();

        var playerStats = new List<PlayerStat>(players.Count);
        playerStats.AddRange(players.Select(player => new PlayerStat
        {
            Lastname = player.Lastname,
            Firstname = player.Firstname,
            Middlename = player.Middlename,
            Games = player.Games,
            Goals = player.Goals,
            PenaltyGoals = player.PenaltyGoals,
            Assists = player.Assists,
            YellowCards = player.YellowCards,
            RedCards = player.RedCards,
            ManOfTheMatch = player.Motm,
            SeasonId = player.SeasonId
        }));

        await using MyDbContext context = new(_dbUsername, _dbPassword, _dbAddress, _dbPort, _dbName);

        foreach (var obj in players.Where(obj => !context.Players.Any(o =>
                     o.Firstname == obj.Firstname && o.Middlename == obj.Middlename && o.Lastname == obj.Lastname)))
        {
            context.Players.Add(obj);
        }

        var playersInDb = context.Players.ToList();

        foreach (var playerStat in playerStats)
        {
            var player = playersInDb.FirstOrDefault(p =>
                p.Firstname == playerStat.Firstname && p.Lastname == playerStat.Lastname &&
                p.Middlename == playerStat.Middlename);

            if (player != null)
            {
                playerStat.PlayerId = player.Id;
                var existingPlayerStat = context.PlayerStats.FirstOrDefault(p =>
                    p.PlayerId == playerStat.PlayerId && p.SeasonId == playerStat.SeasonId);

                if (existingPlayerStat != null)
                {
                    existingPlayerStat.Games = playerStat.Games;
                    existingPlayerStat.Goals = playerStat.Goals;
                    existingPlayerStat.PenaltyGoals = playerStat.PenaltyGoals;
                    existingPlayerStat.Assists = playerStat.Assists;
                    existingPlayerStat.YellowCards = playerStat.YellowCards;
                    existingPlayerStat.RedCards = playerStat.RedCards;
                    existingPlayerStat.ManOfTheMatch = playerStat.ManOfTheMatch;

                    context.PlayerStats.Update(existingPlayerStat);
                }
                else
                {
                    context.PlayerStats.Add(playerStat);
                }
            }
        }

        await context.SaveChangesAsync();
        Cursor = Cursors.Default;

        MessageBox.Show(context.PlayerStats.Count().ToString());
    }

    private async void ParseCoachesButton_Click(object sender, EventArgs e)
    {
        var path = new List<string>
        {
            "participants",
            "coaches"
        };

        const string url = "https://kfl55.ru";
        var coachesParser = new Parser(path, _parametersDictionary, "Coach", url);
        _entityList = await coachesParser.GetEntityListTask();
    }

    private void LeagueComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_parametersDictionary.ContainsKey("league_id"))
        {
            _parametersDictionary.Remove("league_id");
        }

        _parametersDictionary.Add("league_id", ConfigHandler.LeagueDictionary[leagueComboBox.SelectedItem.ToString()].ToString());
    }

    private void SeasonComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_parametersDictionary.ContainsKey("season_id"))
        {
            _parametersDictionary.Remove("season_id");
        }

        _parametersDictionary.Add("season_id", ConfigHandler.SeasonDictionary[seasonComboBox.SelectedItem.ToString()].ToString());
        parseRefereesButton.Enabled = !seasonComboBox.Text.Contains("Зима");
    }

    private async void AddSeasonsToDbButton_Click(object sender, EventArgs e)
    {
        var seasonList = new List<Season>(ConfigHandler.SeasonDictionary.Count);

        seasonList.AddRange(from season in ConfigHandler.SeasonDictionary
            let startIndex = season.Key.IndexOf('-')
            let endIndex = season.Key.IndexOf(' ')
            select new Season
            {
                Id = season.Value, Start_season = season.Key[..startIndex],
                End_season = season.Key.Substring(startIndex + 1, endIndex - startIndex - 1),
                Year = int.Parse(season.Key[(endIndex + 1)..])
            });

        await using MyDbContext context = new(_dbUsername, _dbPassword, _dbAddress, _dbPort, _dbName);

        foreach (var season in seasonList.Where(season => !context.Seasons.Any(s => s.Id == season.Id)))
        {
            context.Seasons.Add(season);
        }

        await context.SaveChangesAsync();

        MessageBox.Show(context.Seasons.Count().ToString());
    }
}