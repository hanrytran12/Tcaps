using Application.Common;
using Application.Common.Exceptions;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
    {
        private readonly IProductRepository _repository;
        private readonly IBatchRepository _batchRepository;

        public DeleteProductCommandHandler(IProductRepository repository, IBatchRepository batchRepository)
        {
            _repository = repository;
            _batchRepository = batchRepository;
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);

            if (product is null)
            {
                throw new NotFoundException($"Không tìm thấy Product với Id: {request.Id}.");
            }

            var isProductInUse = await _batchRepository.IsProductInUseAsync(request.Id);
            if (isProductInUse)
            {
                throw new ConflictException("Không thể xóa Product vì đang có lô đang hoặc đã sản xuất cho mã nón này.");
            }

            product.MarkAsDeleted();
            return Result.Success();
        }
    }
}
