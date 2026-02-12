namespace GroceryApp.Models;

using System.ComponentModel.DataAnnotations;
using System.Linq;

public class DeliveryAddress
{
    [Required(ErrorMessage = "Required.")]
    public string AddressLine1 { get; set; } = string.Empty;

    public string AddressLine2 { get; set; } = string.Empty;

    [Required(ErrorMessage = "Required.")]
    [StringLength(10, ErrorMessage = "Postal code too long.")]
    public string PostalCode { get; set; } = string.Empty;

}