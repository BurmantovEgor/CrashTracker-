﻿using System.ComponentModel.DataAnnotations;

namespace TestApplication.Application.DTO_s
{
    public class UserRegisterDTO
    {

        [Required(ErrorMessage = "UserName обязателен.")]
        [StringLength(30, ErrorMessage = "UserName должен быть от 6 до 30 символов.", MinimumLength = 6)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [StringLength(100, ErrorMessage = "Пароль должен быть от 6 до 100 символов.", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
