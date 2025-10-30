using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Productions.Command.AddProduction
{
    public class AddProductionCommandHandler : IRequestHandler<AddProductionCommand, Result<Guid>>
    {
        private readonly IProductionRepository _productionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;

        public AddProductionCommandHandler(IProductionRepository productionRepository, IUnitOfWork unitOfWork,
            IUserRepository userRepository)
        {
            _productionRepository = productionRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<Result<Guid>> Handle(AddProductionCommand request, CancellationToken cancellationToken)
        {
            var production = new Production(Guid.NewGuid(), request.AssignId, request.UserId, request.Quantity);
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return Result<Guid>.Failure($"User với ID {request.UserId} không tồn tại.");
            }

            //user.AddProduction(production);
            await _productionRepository.AddAsync(production);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(production.Id);
        }
    }
}
