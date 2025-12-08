using Application.Common;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Materials.Commands.AddMaterial
{
    public class AddMaterialCommandHandler : IRequestHandler<AddMaterialCommand, Result<Guid>>
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IAppDbContext _appDbContext;

        public AddMaterialCommandHandler(IMaterialRepository materialRepository, IAppDbContext appDbContext)
        {
            _materialRepository = materialRepository;
            _appDbContext = appDbContext;
        }

        public async Task<Result<Guid>> Handle(AddMaterialCommand request, CancellationToken cancellationToken)
        {
            var materialDb = await _appDbContext.Materials.Where(m => m.Name.ToLower() == (request.Name.ToLower())).FirstOrDefaultAsync();
            if (materialDb is not null)
            {
                throw new ConflictException("Tên nguyên vật liệu đã có sẵn trong hệ thống");
            }

            var material = Material.Create(request.Name, request.Description, request.Unit);
            await _materialRepository.AddAsync(material);
            return Result<Guid>.Success(material.Id);
        }
    }
}
