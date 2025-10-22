using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, Result<Guid>>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public AddProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = Product.Create(request.Code, request.Name, request.ImageURL, request.Description);
            await _repository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(product.Id);
        }
    }
}
