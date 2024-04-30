using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sadet.autodb.api.Models;

[Table("games")]
public class Game
{
    [Key]
    [Column("appid")]
    public long Appid { get; set; }
    [Column("name")]
    public string Name { get; set; } 
}