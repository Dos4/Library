using System.ComponentModel.DataAnnotations.Schema;

namespace Foxminded.Library.Domain.Entities;

public class Book : Entity
{
    public required string Title { get; set; }

    public int Pages { get; set; }

    [ForeignKey(nameof(Genre))]
    public Guid GenreId { get; set; }

    [ForeignKey(nameof(Author))]
    public Guid AuthorId { get; set;}

    [ForeignKey(nameof(Publisher))]
    public Guid PublisherId { get; set;}

    public DateTime ReleaseDate { get; set; }

    public virtual required Genre Genre { get; set; }
    public virtual required Author Author { get; set; }
    public virtual required Publisher Publisher { get; set; }
}
