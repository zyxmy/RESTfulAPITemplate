﻿using FluentValidation;
using RESTfulAPITemplate.Core.DTO;

namespace RESTfulAPITemplate.Core.Validator
{
    public class LoginRequestDTOValidator : AbstractValidator<LoginRequestDTO>
    {
        public LoginRequestDTOValidator()
        {
            RuleFor(l => l.Username).NotEmpty().WithName("用户名").WithMessage("“{PropertyName}”是必填项");
            RuleFor(l => l.Password).NotEmpty().WithName("密码").WithMessage("“{PropertyName}”是必填项");
        }

    }
}