using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Materials.Commands.AddMaterial
{
    public class AddMaterialCommandHandler : IRequestHandler<AddMaterialCommand, Result<Guid>>
    {
        private readonly IMaterialRepository _materialRepository;
        public AddMaterialCommandHandler(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<Result<Guid>> Handle(AddMaterialCommand request, CancellationToken cancellationToken)
        {
            var material = Material.Create(request.Name, request.Description, request.Unit);
            await _materialRepository.AddAsync(material);
            return Result<Guid>.Success(material.Id);
        }
    }
}
