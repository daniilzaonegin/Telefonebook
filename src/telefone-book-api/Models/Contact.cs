using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace telefone_book_api.Models;

public class Contact
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? CurrentUser { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    
    public string? Surname { get; set; }
    
    public string? Email { get; set; }
    
    public string? CellPhoneNumber { get; set; }
    
    public string? HomePhoneNumber { get; set; }
}
