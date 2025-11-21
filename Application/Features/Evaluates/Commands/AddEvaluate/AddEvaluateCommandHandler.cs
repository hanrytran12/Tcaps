using System.Text.Json;
using Application.Common;
using Application.DTOs.Response;
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
        private readonly IMediator _mediator;
        private readonly IFileStorageService _fileStorageService;

        public AddEvaluateCommandHandler(IUnitOfWork unitOfWork, IEvaluateRepository evaluateRepository,
            IMediator mediator, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _evaluateRepository = evaluateRepository;
            _mediator = mediator;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<Guid>> Handle(AddEvaluateCommand request, CancellationToken cancellationToken)
        {
            // Deserialize Defects from JSON string
            if (!string.IsNullOrEmpty(request.DefectsJson))
            {
                try
                {
                    var json = request.DefectsJson.Trim();

                    // Bỏ dấu ngoặc kép ngoài nếu cần
                    if (json.StartsWith("\"") && json.EndsWith("\""))
                    {
                        json = System.Text.RegularExpressions.Regex.Unescape(json.Substring(1, json.Length - 2));
                    }

                    // Kiểm tra xem có phải array hay không
                    json = json.Trim();
                    if (!json.StartsWith("["))
                    {
                        json = $"[{json}]"; // Wrap single object trong array nếu cần
                    }

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        WriteIndented = true // Giúp debug dễ hơn
                    };

                    request.Defects = JsonSerializer.Deserialize<List<ComponentDefectsDTO>>(
                        json,
                        options
                    );

                    if (request.Defects == null || request.Defects.Count == 0)
                    {
                        return Result<Guid>.Failure("Defects list trống sau khi deserialize");
                    }
                }
                catch (JsonException ex)
                {
                    return Result<Guid>.Failure($"DefectsJson không hợp lệ: {ex.Message}. Path: {ex.Path}");
                }
                catch (Exception ex)
                {
                    return Result<Guid>.Failure($"Lỗi deserialize: {ex.Message}");
                }
            }

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

            if (request.Status != "Passed" && request.Defects?.Any() == true)
            {
                var componentDefects = request.Defects
                    .Select(item => Domain.Entities.ComponentDefect.Create(
                        evaluate.Id,
                        item.DefectType,
                        item.Severity,
                        item.Description,
                        item.Solution,
                        item.Quantity,
                        item.Status))
                    .ToList();
                evaluate.AddDefects(componentDefects);
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
