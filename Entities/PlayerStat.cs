using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FootParser.Entities;

public class PlayerStat : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int SeasonId { get; set; }
    public string Lastname { get; set; }
    public string Firstname { get; set; }
    public string Middlename { get; set; }
    public int Games { get; set; }
    public int Goals { get; set; }
    public int PenaltyGoals { get; set; }
    public int Assists { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int ManOfTheMatch { get; set; }
}
