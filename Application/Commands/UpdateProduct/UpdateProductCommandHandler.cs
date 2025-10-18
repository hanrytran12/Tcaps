using Application.Common;
using Domain.Interfaces;
using MediatR;

namespace Application.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateProductCommandHandler(IProductRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);

            if (product is null)
            {
                return Result.Failure($"Không tìm thấy Product với Id: {request.Id}.");
            }

            product.UpdateDetails(request.Name, request.ImageURL, request.Description);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
