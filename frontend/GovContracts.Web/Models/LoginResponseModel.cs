namespace GovContracts.Web.Models;

public class LoginResponseModel
{
    public string Token { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}
