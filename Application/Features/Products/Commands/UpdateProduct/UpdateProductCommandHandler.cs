using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IProductRepository _repository;
        public UpdateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);

            if (product is null)
            {
                return Result.Failure($"Không tìm thấy Product với Id: {request.Id}.");
            }

            if (request.Name != product.Name)
            {
                var existingProduct = await _repository.GetByNameAsync(request.Name);
                if (existingProduct is not null)
                {
                    return Result.Failure("Tên sản phẩm này đã tồn tại");
                }
            }

            product.UpdateDetails(request.Name, request.Description);

            return Result.Success();
        }
    }
}
