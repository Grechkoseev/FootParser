namespace FootParser.Entities;

public class Coach : IEntity
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string FullName
    {
        set
        {
            var indexesOfSpaces = new[]
            {
                value.IndexOf(' '),
                value.LastIndexOf(' ')
            };

            LastName = value[..indexesOfSpaces[0]];
            FirstName = value.Substring(indexesOfSpaces[0] + 1, indexesOfSpaces[1] - indexesOfSpaces[0]).Trim();
            MiddleName = value[(indexesOfSpaces[1] + 1)..];
        }
    }
    public int CommandNumber { get; set; }
    public int Games { get; set; }
    public int Win { get; set; }
    public int Draw { get; set; }
    public int Lose { get; set; }
}