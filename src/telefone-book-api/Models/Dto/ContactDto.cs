using System.ComponentModel.DataAnnotations;

namespace telefone_book_api.Models.Dto;

public record ContactDto(int? Id, string Name, string? Surname, [EmailAddress] string? Email, string? CellPhoneNumber, string? HomePhoneNumber);
