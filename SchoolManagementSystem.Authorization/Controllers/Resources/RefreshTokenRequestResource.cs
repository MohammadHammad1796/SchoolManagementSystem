﻿using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Authorization.Controllers.Resources;

public class RefreshTokenRequestResource
{
	[Required]
	public string AccessToken { get; set; }
	[Required]
	public string RefreshToken { get; set; }
}