using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YogaFitnessClub.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]{1,255}$",
         ErrorMessage = "Name is not valid.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{1,9}[a-zA-Z\-\.\,\s]{1,40}$",
            ErrorMessage = "Not a valid Address.")]
        public string Address { get; set; }

        [Required]
        [RegularExpression("^([Gg][Ii][Rr] 0[Aa]{2}|([A-Za-z][0-9]{1,2}|[A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2}|[A-Za-z][0-9][A-Za-z]|[A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]) ?[0-9][A-Za-z]{2})$",
         ErrorMessage = "Invalid UK Postcode.")]
        public string Postcode { get; set; }
    }

    public class TutorRegisterViewModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]{1,255}$",
         ErrorMessage = "Name is not valid.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{DD/MM/YYYY}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{1,9}[a-zA-Z\-\.\,\s]{1,40}$",
            ErrorMessage = "Not a valid Address.")]
        public string Address { get; set; }

        [Required]
        [StringLength(8)]
        [RegularExpression("^([Gg][Ii][Rr] 0[Aa]{2}|([A-Za-z][0-9]{1,2}|[A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2}|[A-Za-z][0-9][A-Za-z]|[A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]) ?[0-9][A-Za-z]{2})$",
         ErrorMessage = "Invalid UK Postcode.")]
        public string Postcode { get; set; }

        //https://www.gov.uk/hmrc-internal-manuals/national-insurance-manual/nim39110
        [Required]
        [RegularExpression(@"^(?!BG)(?!GB)(?!NK)(?!KN)(?!TN)(?!NT)(?!ZZ)(?:[A-Ca-cEGHeghJ-PjpR-Tr-tW-Zw-z][A-Ca-cEGHeghJ-Nj-nPpR-Tr-tW-Zw-z])(?:\s*\d\s*){6}([A-Da-d]|\s)$",
            ErrorMessage = "Invalid National Insurance Number.\n Format: LL NN NN NN L")]
        public string NiN { get; set; }

        [Required]
        [RegularExpression(@"^(07[\d]{9}|7[\d]{9}|447[\d]{9}|00447[\d]{9})$",
            ErrorMessage = "Invalid UK mobile number. Try 447 or without 0")]
        public long MobileNumber { get; set; }

        //[Required]
        //public string Gender { get; set; }

    }

    public class AdminRegisterViewModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]{1,255}$",
         ErrorMessage = "Name is not valid.")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{DD/MM/YYYY}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime Birthday { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{1,9}[a-zA-Z\-\.\,\s]{1,40}$",
            ErrorMessage = "Not a valid Address.")]
        public string Address { get; set; }

        [Required]
        [StringLength(8)]
        [RegularExpression("^([Gg][Ii][Rr] 0[Aa]{2}|([A-Za-z][0-9]{1,2}|[A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2}|[A-Za-z][0-9][A-Za-z]|[A-Za-z][A-Ha-hJ-Yj-y][0-9]?[A-Za-z]) ?[0-9][A-Za-z]{2})$",
         ErrorMessage = "Invalid UK Postcode.")]
        public string Postcode { get; set; }

        //https://www.gov.uk/hmrc-internal-manuals/national-insurance-manual/nim39110
        [Required]
        [RegularExpression(@"^(?!BG)(?!GB)(?!NK)(?!KN)(?!TN)(?!NT)(?!ZZ)(?:[A-Ca-cEGHeghJ-PjpR-Tr-tW-Zw-z][A-Ca-cEGHeghJ-Nj-nPpR-Tr-tW-Zw-z])(?:\s*\d\s*){6}([A-Da-d]|\s)$",
            ErrorMessage = "Invalid National Insurance Number.\n Format: LL NN NN NN L")]
        public string NiN { get; set; }

        [Required]
        [RegularExpression(@"^(07[\d]{9}|7[\d]{9}|447[\d]{9}|00447[\d]{9})$",
            ErrorMessage = "Invalid UK mobile number. Try 447 or without 0")]
        public long MobileNumber { get; set; }

        //[Required]
        //public string Gender { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
