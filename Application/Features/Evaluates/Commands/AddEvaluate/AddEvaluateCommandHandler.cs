using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Domain.Events;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Evaluates.Commands.AddEvaluate
{
    public class AddEvaluateCommandHandler : IRequestHandler<AddEvaluateCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEvaluateRepository _evaluateRepository;
        private readonly IComponentDefectRepository _componentDefectRepository;
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorageService;

        public AddEvaluateCommandHandler(IUnitOfWork unitOfWork, IEvaluateRepository evaluateRepository,
            IComponentDefectRepository componentDefectRepository, IMediator mediator, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _evaluateRepository = evaluateRepository;
            _componentDefectRepository = componentDefectRepository;
            _mediator = mediator;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<Guid>> Handle(AddEvaluateCommand request, CancellationToken cancellationToken)
        {
            List<string> imageUrls = await _fileStorageService.SaveFileAsync(request.Image, "evaluates", cancellationToken);
            string combineUrls = string.Join(",", imageUrls);

            var evaluate = Evaluate.Create(
                request.ProductionId,
                request.UserId.Value,
                request.QuantityError,
                request.QuantitySucess,
                request.Note,
                combineUrls,
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
                evaluate.QuantitySuccess,
                evaluate.Note,
                evaluate.Status
            ));
            return Result<Guid>.Success(evaluate.Id);
        }
    }
}
