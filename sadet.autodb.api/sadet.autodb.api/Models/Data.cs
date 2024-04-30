using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace sadet.autodb.api.Models;
[Table("data")]
public class Data
{
    [Key]
    [Column("index")]
    public long Index { get; set; }
    [Column("createdat")]
    public DateTime CreateDate { get; set; }
    [Column("appid")]
    public int Appid { get; set; }
    [Column("completion")]
    public double Completion { get; set; }
    [Column("isaverage")]
    public bool IsAverage { get; set; }
}