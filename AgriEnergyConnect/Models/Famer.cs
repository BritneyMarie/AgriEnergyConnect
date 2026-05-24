using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Farmer
{
    [Key]
    public int FarmerId { get; set; }

    [Required]
    [StringLength(100)]
    public string FarmName { get; set; }

    [Required]
    [StringLength(100)]
    public string ContactPerson { get; set; }

    [Required]
    [StringLength(20)]
    public string PhoneNumber { get; set; }

    [Required]
    [StringLength(200)]
    public string Address { get; set; }

    [Required]
    [StringLength(100)]
    public string Email { get; set; }

    // Navigation property
    public ICollection<Product> Products { get; set; }

    // Link to Identity User
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; }
}