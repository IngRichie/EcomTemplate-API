using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EcomTemplate.Domain.Entities.Admin;

namespace EcomTemplate.Domain.Entities;

[Table("refresh_tokens")]
public class RefreshToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid RefreshTokenId { get; set; }

  
    public Guid? CustomerProfileId { get; set; }
    public Guid? AdminId { get; set; }

    [Required]
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRevoked { get; set; }

    public CustomerProfile? CustomerProfile { get; set; }
    public AdminProfile? AdminProfile { get; set; }
}