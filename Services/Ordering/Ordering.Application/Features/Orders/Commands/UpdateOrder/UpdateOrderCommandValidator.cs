using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(a => a.BuyerEmail)
                .NotEmpty().WithMessage("{buyerEmail} is required")
                .NotNull()
                .EmailAddress().WithMessage("{buyerEmail} must not exceed 50 characters");

            RuleFor(a => a.DelieveryMethod)
              .NotEmpty().WithMessage("{DelieveryMethod} is required");

            RuleFor(a => a.Subtotal)
                .NotEmpty().WithMessage("{Subtotal} is required")
                .GreaterThan(0).WithMessage("{Subtotal} should be greater than 0");
        }
    }
}
