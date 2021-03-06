﻿using System.ComponentModel.DataAnnotations;
using ParkingATHWeb.ViewModels.Base;

namespace ParkingATHWeb.Areas.Portal.ViewModels.Manage
{
    public class ResetPasswordViewModel : SmartParkBaseViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        public string Token { get; set; }
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [Display(Name = "nowe hasło")]
        public string Password { get; set; }
        [Display(Name = "powtórz nowe hasło")]
        [Required(ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "Common_RequiredError")]
        [Compare("Password", ErrorMessageResourceType = typeof(ViewModelResources), ErrorMessageResourceName = "ResetPasswordViewModel_Password_CompareError")]
        public string RepeatPassword { get; set; }
    }
}
