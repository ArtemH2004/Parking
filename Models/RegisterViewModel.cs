using System.ComponentModel.DataAnnotations;

namespace Parking.Models
{
    //    public class RegisterViewModel
    //    {
    //        [Required]
    //        [EmailAddress]
    //        public string Email { get; set; }

    //        [Required]
    //        [StringLength(100, ErrorMessage = "Пароль должен содержать более 6 символов.", MinimumLength = 6)]
    //        [DataType(DataType.Password)]
    //        public string Password { get; set; }

    //        [DataType(DataType.Password)]
    //        [Display(Name = "Confirm password")]
    //        [Compare("Password", ErrorMessage = "Пароль отсутствует.")]
    //        public string ConfirmPassword { get; set; }

    //        [Required]
    //        public string Name { get; set; }

    //        [Required]
    //        public string Surname { get; set; }
    //        public string MiddleName { get; set; }
    //        public string Phone { get; set; }
    //        public string Address { get; set; }
    //    }

    //}


    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
