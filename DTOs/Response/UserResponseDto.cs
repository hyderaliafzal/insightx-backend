﻿namespace ConnektaViz.API.DTOs.Response;

public class UserResponseDto
{
    public long UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhotoURL { get; set; }
}
