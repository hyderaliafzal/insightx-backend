namespace ConnektaViz.API.DTOs;

public class JwtSettingDto
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
