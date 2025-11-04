using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using AutoMapper;
using Domain.Entities;
using Domain.Events;
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
        private readonly IMediator _mediator;

        public AddEvaluateCommandHandler(IUnitOfWork unitOfWork, IEvaluateRepository evaluateRepository,
            IUserRepository userRepository, IComponentDefectRepository componentDefectRepository
            ,IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _evaluateRepository = evaluateRepository;
            _userRepository = userRepository;
            _componentDefectRepository = componentDefectRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Result<Guid>> Handle(AddEvaluateCommand request, CancellationToken cancellationToken)
        {
            var evaluate = Evaluate.Create(
                request.ProductionId,
                request.UserId.Value,
                request.QuantityError,
                request.Note,
                request.Image,
                request.Status);

            if (request.Status != "Passed")
            {
                foreach (var item in request.Defects)
                {
                    var component = Domain.Entities.ComponentDefect.Create(
                        evaluate.Id,
                        item.DefectType,
                        item.Serverity,
                        item.Description,
                        item.Solution,
                        item.Quantity,
                        item.Status);
                    await _componentDefectRepository.AddAsync(component);
                }
            }

            await _evaluateRepository.AddAsync(evaluate);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Trigger event sau khi DB đã có record
            await _mediator.Publish(new EvaluateCreatedEvent(
                evaluate.Id,
                evaluate.ProductionId,
                evaluate.UserId.Value,
                evaluate.QuantityError,
                evaluate.Note,
                evaluate.Status
            ));
            return Result<Guid>.Success(evaluate.Id);
        }
    }
}
