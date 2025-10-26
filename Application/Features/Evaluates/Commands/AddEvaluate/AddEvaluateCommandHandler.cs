using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http.Metadata;

namespace Application.Features.Evaluates.Commands.AddEvaluate
{
    public class AddEvaluateCommandHandler : IRequestHandler<AddEvaluateCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly IUserRepository _userRepository;
        private readonly IComponentDefectRepository _componentDefectRepository;
        private readonly IMapper _mapper;

        public AddEvaluateCommandHandler(IUnitOfWork unitOfWork, IEvaluateRepository evaluateRepository,
            IUserRepository userRepository, IComponentDefectRepository componentDefectRepository
            ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _evaluateRepository = evaluateRepository;
            _userRepository = userRepository;
            _componentDefectRepository = componentDefectRepository;
            _mapper = mapper;
        }

        public async Task<Result<Guid>> Handle(AddEvaluateCommand request, CancellationToken cancellationToken)
        {
            var evaluate = Evaluate.Create(
                request.ProductionId,
                request.UserId.Value,
                request.QuantityError,
                request.Note,
                request.Image);

            foreach (var item in request.Defects)
            {
                var component = ComponentDefect.Create(
                    evaluate.Id,
                    item.DefectType,
                    item.Serverity,
                    item.Description,
                    item.Solution,
                    item.Status);
                await _componentDefectRepository.AddAsync(component);
            }

            await _evaluateRepository.AddAsync(evaluate);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Guid>.Success(evaluate.Id);
        }
    }
}
