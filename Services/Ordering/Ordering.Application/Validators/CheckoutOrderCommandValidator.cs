﻿using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators;
public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
{
    public CheckoutOrderCommandValidator()
    {
        RuleFor(c => c.UserName)
            .NotEmpty().WithMessage("{UserName} is required")
            .NotNull()
            .MaximumLength(70)
            .WithMessage("{UserName} must not exceed 70  chars");
        RuleFor(c => c.TotalPrice)
            .NotEmpty().WithMessage("{TotalPrice} is required")
            .GreaterThan(-1).WithMessage("{TotalPrice} should not be -ve");
        RuleFor(c => c.EmailAddress)
            .NotEmpty().WithMessage("{EmailAddress} is required");

        RuleFor(o => o.FirstName)
        .NotEmpty()
        .NotNull()
        .WithMessage("{FirstName} is required");
        RuleFor(o => o.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("{LastName} is required");
    }
}
