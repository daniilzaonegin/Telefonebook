using Microsoft.AspNetCore.Identity;

namespace telefone_book_api.Models.Dto;

public class TelephoneBookUser: IdentityUser { }

public class UserInfo
{
    public required string Username { get; set; }

    public required string Password { get; set; }
}

public class ExternalUserInfo
{
    public required string Username { get; set; }

    public required string ProviderKey { get; set; }
}
