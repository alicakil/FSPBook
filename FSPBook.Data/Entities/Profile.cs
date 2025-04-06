using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSPBook.Data.Entities;
public class Profile
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string JobTitle { get; set; }

    [NotMapped]
    public string FullName => FirstName + " " + LastName;

    public virtual ICollection<Post> Posts { get; set; } = [];
}
