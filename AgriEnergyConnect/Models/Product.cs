using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Product Name")]
    public string Name { get; set; }

    [Required]
    [StringLength(50)]
    public string Category { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Production Date")]
    public DateTime ProductionDate { get; set; }

    public string Description { get; set; }

    [Required]
    [Range(0.01, 10000)]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    // Foreign key to Farmer
    public int FarmerId { get; set; }

    [ForeignKey("FarmerId")]
    public virtual Farmer Farmer { get; set; }
}