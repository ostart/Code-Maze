﻿using System.ComponentModel.DataAnnotations;

namespace CompanyEmployees.IDP.Entities.ViewModels;

public class ForgotPasswordModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}