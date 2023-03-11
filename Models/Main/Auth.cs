using System.ComponentModel.DataAnnotations;

namespace GoodJoker.Models
{
    public class Login
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Pass { get; set; }
    }

    public class Reg
    {
        // Email
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        // Пароль
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Длина строки должна быть от 6 символов")]
        public string Pass { get; set; }

        // Подтверждение пароля
        [Required]
        [Compare("Pass", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Длина строки должна быть от 6 символов")]
        public string PassConfirm { get; set; }
    }

    public class AuthResultObject
    {
        public string user_id { get; set; }             // vk
        public string id { get; set; }                  // fb, gl
        public string uid { get; set; }                 // ok

        public string name { get; set; }                // полное имя
        public string first_name { get; set; }          // Имя
        public string last_name { get; set; }           // Фамилия

        public string given_name { get; set; }          // Имя от google
        public string family_name { get; set; }         // Фамилия от google

        public string birthday { get; set; }            // дата рождения ok
        public string gender { get; set; }              // пол
        public string email { get; set; }               // email, все кроме ok

        public string access_token { get; set; }        // токен доступа
        public string code { get; set; }                // код для получения токена
    }
}