using System.ComponentModel.DataAnnotations;

namespace FootParser.Entities;

public class Season : IEntity
{
    [Key]
    public int Id { get; set; }
    public string Start_season { get; set; }
    public string End_season { get; set; }
    public int Year { get; set; }
}