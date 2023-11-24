using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FootParser.Entities;

public class Player : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Lastname { get; set; }
    public string Firstname { get; set; }
    public string Middlename { get; set; }
    private string _fullName;
    public string FullName
    {
        get => _fullName;
        set
        {
            var spaceCount = value.Count(ch => ch == ' ');

            if (spaceCount < 2)
            {
                Lastname = value[..value.IndexOf(' ')];
                Firstname = value[(value.IndexOf(' ') + 1)..].Trim();
                Middlename = string.Empty;
            }
            else
            {
                var indexesOfSpaces = new[]
                {
                    value.IndexOf(' '),
                    value.IndexOf(' ', value.IndexOf(' ') + 1)
                };

                Lastname = value[..indexesOfSpaces[0]];
                Firstname = value.Substring(indexesOfSpaces[0] + 1, indexesOfSpaces[1] - indexesOfSpaces[0]).Trim();
                Middlename = value[(indexesOfSpaces[1] + 1)..];
            }
        }
    }
    public int Games { get; set; }
    private string _goalsTotal;
    public string GoalsTotal
    {
        get => _goalsTotal;
        set
        {
            if (value.Contains('(') || value.Contains(')'))
            {
                Goals = int.Parse(value[..value.IndexOf('(')].Trim());

                if (value.Contains(','))
                {
                    PenaltyGoals =
                        int.Parse(value.Substring(value.IndexOf('(') + 1,
                            value.IndexOf(',') - value.IndexOf('(') - 1).Trim()) + int.Parse(value
                            .Substring(value.IndexOf(',') + 1, value.IndexOf(')') - value.IndexOf(',') - 1).Trim());
                }
                else
                {
                    PenaltyGoals = int.Parse(value
                        .Substring(value.IndexOf('(') + 1, value.IndexOf(')') - value.IndexOf('(') - 1).Trim());
                }
            }
            else
            {
                Goals = int.Parse(value);
                PenaltyGoals = 0;
            }
        }
    }
    public int Goals { get; set; }
    public int PenaltyGoals { get; set; }
    public int Assists { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int Motm { get; set; }
    public int SeasonId { get; set; }
}