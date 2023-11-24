using System.Text;
using FootParser.Entities;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace FootParser.ContentParsers;

public class Parser
{
    private readonly List<string> _path;
    private readonly Dictionary<string, string> _requestParams;
    private readonly string _className;
    private readonly string _url;
    private readonly Dictionary<string, List<string>> _classNameNodesDictionary;
    private readonly Dictionary<string, Func<Task<List<IEntity>>>> _getEntityMethods;

    public Parser(IEnumerable<string> path, IDictionary<string, string> parameters, string className, string url)
    {
        _path = new List<string>(path);
        _requestParams = new Dictionary<string, string>(parameters);
        _className = className;
        _url = url;
        _classNameNodesDictionary = new Dictionary<string, List<string>>
            {
                { "Team", new List<string> { "//p[contains(@class, 'participants-teams__name')]" } },
                {
                    "Player",
                    new List<string>
                    {
                        "//span[contains(@class, 'table__player-name')]",
                        "//td[contains(@class, 'table__cell--variable')]"
                    }
                },
                {
                    "Coach",
                    new List<string>
                    {
                        "//span[contains(@class, 'table__player-name')]",
                        "//td[contains(@class, 'table__cell--variable')]"
                    }
                },
                {
                    "Referee",
                    new List<string>
                    {
                        "//span[contains(@class, 'table__player-name')]",
                        "//td[contains(@class, 'table__cell--variable')]"
                    }
                }
            };
        _getEntityMethods = new Dictionary<string, Func<Task<List<IEntity>>>>
            {
                { "Team", GetTeamsListTask },
                { "Player", GetPlayerListTask },
                { "Coach", GetCoachListTask },
                { "Referee", GetRefereeListTask }
            };
    }

    private string GetUrl()
    {
        var url = new StringBuilder(_url);

        foreach (var parameter in _path)
        {
            url.Append('/' + parameter);
        }

        if (_requestParams.Count != 0)
        {
            url.Append('?');
        }

        foreach (var parameter in _requestParams)
        {
            url.Append(parameter.Key + '=' + parameter.Value + '&');
        }

        url.Remove(url.Length - 1, 1);

        return url.ToString();
    }

    private Task<string> GetHtmlContent(string url)
    {
        var httpClient = new HttpClient();
        return httpClient.GetStringAsync(url);
    }

    private HtmlDocument GetHtmlDocument(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        return doc;
    }

    public async Task<List<IEntity>> GetEntityListTask()
    {
        var dataList = await _getEntityMethods[_className].Invoke();
        MessageBox.Show(dataList.Count.ToString());
        return dataList;
    }

    private async Task<List<IEntity>> GetTeamsListTask()
    {
        int currentPage;
        var dataList = new List<IEntity>();
        var pagesTotal = 0;
        _requestParams.Add("page", "1");

        do
        {
            var htmlStr = GetUrl();
            var html = await GetHtmlContent(htmlStr);
            var doc = GetHtmlDocument(html);
            var names = doc.DocumentNode.SelectNodes(_classNameNodesDictionary[_className][0]);
            dataList.AddRange(names.Select(name => new Team { Name = name.InnerText.Trim() }));

            if (pagesTotal == 0)
            {
                pagesTotal = PaginationHandler.GetTotalPageNumber(doc);
            }

            currentPage = PaginationHandler.GetCurrentPageNumber(doc);
            _requestParams["page"] = (currentPage + 1).ToString();

        } while (pagesTotal != currentPage);

        return dataList;
    }

    private async Task<List<IEntity>> GetPlayerListTask()
    {
        int currentPage;
        var dataList = new List<IEntity>();
        var pagesTotal = 0;
        _requestParams.Add("page", "1");
        const string trNodeString = "//thead/tr[@class='table__head']";
        const string thNodesString = ".//th[contains(@class, 'table__cell')]";

        do
        {
            var index = 0;
            var htmlStr = GetUrl();
            var html = await GetHtmlContent(htmlStr);
            var doc = GetHtmlDocument(html);
            var names = doc.DocumentNode.SelectNodes(_classNameNodesDictionary[_className][0]);
            var stats = doc.DocumentNode.SelectNodes(_classNameNodesDictionary[_className][1]);
            var trNode = doc.DocumentNode.SelectSingleNode(trNodeString);
            var thNodes = trNode.SelectNodes(thNodesString);

            switch (thNodes.Count)
            {
                case 7:
                    {
                        foreach (var name in names)
                        {
                            dataList.Add(new Player
                            {
                                FullName = name.InnerText.Trim(),
                                Games = int.Parse(stats[index].InnerText.Trim()),
                                GoalsTotal = stats[index + 1].InnerText.Trim(),
                                Assists = 0,
                                YellowCards = int.Parse(stats[index + 3].InnerText.Trim()),
                                RedCards = int.Parse(stats[index + 4].InnerText.Trim()),
                                Motm = 0,
                                SeasonId = int.Parse(_requestParams["season_id"])
                            });

                            index += 5;
                        }

                        break;
                    }

                case 8:
                    {
                        foreach (var name in names)
                        {
                            dataList.Add(new Player
                            {
                                FullName = name.InnerText.Trim(),
                                Games = int.Parse(stats[index].InnerText.Trim()),
                                GoalsTotal = stats[index + 1].InnerText.Trim(),
                                Assists = 0,
                                YellowCards = int.Parse(stats[index + 3].InnerText.Trim()),
                                RedCards = int.Parse(stats[index + 4].InnerText.Trim()),
                                Motm = int.Parse(stats[index + 5].InnerText.Trim()),
                                SeasonId = int.Parse(_requestParams["season_id"])
                            });

                            index += 6;
                        }

                        break;
                    }

                case 9:
                    {
                        foreach (var name in names)
                        {
                            dataList.Add(new Player
                            {
                                FullName = name.InnerText.Trim(),
                                Games = int.Parse(stats[index].InnerText.Trim()),
                                GoalsTotal = stats[index + 1].InnerText.Trim(),
                                Assists = int.Parse(stats[index + 3].InnerText.Trim()),
                                YellowCards = int.Parse(stats[index + 5].InnerText.Trim()),
                                RedCards = int.Parse(stats[index + 6].InnerText.Trim()),
                                Motm = 0,
                                SeasonId = int.Parse(_requestParams["season_id"])
                            });

                            index += 7;
                        }

                        break;
                    }

                case 10:
                    {
                        foreach (var name in names)
                        {
                            dataList.Add(new Player
                            {
                                FullName = name.InnerText.Trim(),
                                Games = int.Parse(stats[index].InnerText.Trim()),
                                GoalsTotal = stats[index + 1].InnerText.Trim(),
                                Assists = int.Parse(stats[index + 3].InnerText.Trim()),
                                YellowCards = int.Parse(stats[index + 5].InnerText.Trim()),
                                RedCards = int.Parse(stats[index + 6].InnerText.Trim()),
                                Motm = int.Parse(stats[index + 7].InnerText.Trim()),
                                SeasonId = int.Parse(_requestParams["season_id"])
                            });

                            index += 8;
                        }
                    }
                    break;
            }

            if (pagesTotal == 0)
            {
                pagesTotal = PaginationHandler.GetTotalPageNumber(doc);
            }

            currentPage = PaginationHandler.GetCurrentPageNumber(doc);
            _requestParams["page"] = (currentPage + 1).ToString();

        } while (pagesTotal != currentPage);

        return dataList;
    }

    private async Task<List<IEntity>> GetCoachListTask()
    {
        int currentPage;
        var dataList = new List<IEntity>();
        var pagesTotal = 0;
        _requestParams.Add("page", "1");

        do
        {
            var index = 0;
            var htmlStr = GetUrl();
            var html = await GetHtmlContent(htmlStr);
            var doc = GetHtmlDocument(html);
            var names = doc.DocumentNode.SelectNodes(_classNameNodesDictionary[_className][0]);
            var stats = doc.DocumentNode.SelectNodes(_classNameNodesDictionary[_className][1]);

            foreach (var name in names)
            {
                dataList.Add(new Coach
                {
                    FullName = name.InnerText.Trim(),
                    CommandNumber = int.Parse(stats[index].InnerText.Trim()),
                    Games = int.Parse(stats[index + 1].InnerText.Trim()),
                    Win = int.Parse(stats[index + 2].InnerText.Trim()),
                    Draw = int.Parse(stats[index + 3].InnerText.Trim()),
                    Lose = int.Parse(stats[index + 4].InnerText.Trim())
                });

                index += 5;
            }

            if (pagesTotal == 0)
            {
                pagesTotal = PaginationHandler.GetTotalPageNumber(doc);
            }

            currentPage = PaginationHandler.GetCurrentPageNumber(doc);
            _requestParams["page"] = (currentPage + 1).ToString();

        } while (pagesTotal != currentPage);


        return dataList;
    }

    private async Task<List<IEntity>> GetRefereeListTask()
    {
        int currentPage;
        var dataList = new List<IEntity>();
        var pagesTotal = 0;
        _requestParams.Add("page", "1");

        do
        {
            var index = 0;
            var htmlStr = GetUrl();
            var html = await GetHtmlContent(htmlStr);
            var doc = GetHtmlDocument(html);
            var names = doc.DocumentNode.SelectNodes(_classNameNodesDictionary[_className][0]);
            var stats = doc.DocumentNode.SelectNodes(_classNameNodesDictionary[_className][1]);

            foreach (var name in names)
            {
                dataList.Add(new Referee
                {
                    FullName = name.InnerText.Trim(),
                    Games = int.Parse(stats[index].InnerText.Trim()),
                    YellowCards = int.Parse(stats[index + 1].InnerText.Trim()),
                    RedCards = int.Parse(stats[index + 3].InnerText.Trim())
                });

                index += 5;
            }

            if (pagesTotal == 0)
            {
                pagesTotal = PaginationHandler.GetTotalPageNumber(doc);
            }

            currentPage = PaginationHandler.GetCurrentPageNumber(doc);
            _requestParams["page"] = (currentPage + 1).ToString();

        } while (pagesTotal != currentPage);

        return dataList;
    }

}