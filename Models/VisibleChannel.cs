using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class VisibleChannel
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
}