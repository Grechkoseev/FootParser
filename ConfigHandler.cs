using System.Xml;

namespace FootParser;

public static class ConfigHandler
{
    public static string Database { get; private set; }
    public static string Username { get; private set; }
    public static string Password { get; private set; }
    public static string Address { get; private set; }
    public static string Port { get; private set; }
    public static readonly Dictionary<string, int> LeagueDictionary;
    public static readonly Dictionary<string, int> SeasonDictionary;

    static ConfigHandler()
    {
        var doc = new XmlDocument();
        doc.Load("Config.xml");
        LeagueDictionary = new();
        SeasonDictionary = new();
        var connectionStringNodeList = doc.SelectNodes("/configuration/leagues/kfl55/connectionString/KFL55");
        var leaguesNode = doc.SelectSingleNode("/configuration/leagues");

        try
        {
            foreach (XmlNode childNode in leaguesNode.ChildNodes)
            {
                var name = childNode.Attributes["name"].Value;
                var id = int.Parse(childNode.Attributes["id"].Value);
                LeagueDictionary.Add(name, id);

                var seasonsNode = childNode.SelectSingleNode("seasons");

                foreach (XmlNode seasonNode in seasonsNode.SelectNodes("season")!)
                {
                    var seasonName = seasonNode.Attributes["name"].Value;
                    var seasonId = int.Parse(seasonNode.Attributes["id"].Value);
                    SeasonDictionary.Add(seasonName, seasonId);
                }
            }

            if (connectionStringNodeList is { Count: > 0 })
            {
                var element = (XmlElement)connectionStringNodeList[0]!;
                Database = element.GetAttribute("database");
                Username = element.GetAttribute("username");
                Password = element.GetAttribute("password");
                Address = element.GetAttribute("address");
                Port = element.GetAttribute("port");
            }
        }
        catch (Exception)
        {
            MessageBox.Show("Ошибка при попытке считать данные из конфига для подключения к базе данных", "FootParser",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}