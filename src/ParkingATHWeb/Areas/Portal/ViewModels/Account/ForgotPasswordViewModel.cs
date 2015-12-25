﻿using ParkingATHWeb.ViewModels.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ParkingATHWeb.Areas.Portal.ViewModels.Account
{
    public class ForgotPasswordViewModel : ParkingAthBaseViewModel
    {
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        [EmailAddress(ErrorMessage = "To nie jest adres email")]
        [DisplayName("Email")]
        public string Email { get; set; }
    }
}