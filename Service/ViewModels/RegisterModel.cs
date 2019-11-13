using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ViewModels
{
    // Модель представления для регистрации пользователя
    public class RegisterModel
    {
        [Required(ErrorMessage = "Введите адрес электронной почты")]
        [EmailAddress]
        public string Email { set; get; }

        [Required(ErrorMessage = "Введите логин")]
        [StringLength(100, ErrorMessage = "Логин должен содержать не меньше 6 и не больше 100 символов", MinimumLength  = 6)]
        public string UserName { set; get; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Пароль должен содержать не меньше 6 и не больше 100 символов", MinimumLength = 6)]
        public string Password { set; get; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { set; get; }
    }
}
