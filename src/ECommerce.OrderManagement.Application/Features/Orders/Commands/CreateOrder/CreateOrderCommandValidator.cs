using FluentValidation;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.Repositories;

namespace ECommerce.OrderManagement.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Coupon> _couponRepository;

        public CreateOrderCommandValidator(IRepository<Product> productRepository, IRepository<Coupon> couponRepository)
        {
            _productRepository = productRepository;
            _couponRepository = couponRepository;

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("O pedido deve conter pelo menos um item.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId)
                    .GreaterThan(0).WithMessage("ProductId inválido.")
                    .MustAsync(ProductExists).WithMessage("Produto não encontrado.");

                item.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Quantidade deve ser maior que zero.");
            });

            RuleFor(x => x.CouponId)
                .MustAsync(CouponExistsIfProvided).WithMessage("Cupom não encontrado.")
                .When(x => x.CouponId.HasValue);
        }

        private async Task<bool> ProductExists(int productId, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            return product != null;
        }

        private async Task<bool> CouponExistsIfProvided(int? couponId, CancellationToken cancellationToken)
        {
            if (!couponId.HasValue)
                return true;

            var coupon = await _couponRepository.GetByIdAsync(couponId.Value);
            return coupon != null;
        }
    }
}
