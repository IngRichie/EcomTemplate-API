using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocerySupermarket.Domain.Entities;

[Table("favourites")]
public class Favourites
{
    [Key] public Guid FavouritesId { get; set; }
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public DateTime CreatedAt { get; set; }

    
}