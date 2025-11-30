using Application.Common;
using Application.DTOs.Response;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Evaluates.Queries.GetEvaluatesByStaffId
{
    public class GetEvaluatesByStaffIdQueryHandler : IRequestHandler<GetEvaluatesByStaffIdQuery, Result<List<EvaluateDTO>>>
    {
        private readonly IAppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public GetEvaluatesByStaffIdQueryHandler(IAppDbContext context, IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }
        public async Task<Result<List<EvaluateDTO>>> Handle(GetEvaluatesByStaffIdQuery request, CancellationToken cancellationToken)
        {
            //var user = await _userRepository.GetByIdAsync(request.StaffId);
            //if (user == null)
            //{
            //    throw new Exception($"Không tìm thấy user với ID = {request.StaffId}");
            //}

            ////lấy ds productions
            //var productions = await _productionRepository.GetByUserAsync(user.Id);
            //if (!productions.Any())
            //    throw new Exception("Nhân viên chưa có sản xuất nào cho AssignId này.");

            //productions = productions.Where(p => p.AssignId == request.AssignId).ToList();
            ////lấy ds productionId
            //var productionIds = productions.Select(x => x.Id).ToList();

            ////lấy ds evaluate
            //var evaluates = await _evaluateRepository.GetByProductionIdsAsync(productionIds);

            var resultDtos = await (from user in _context.Users.AsNoTracking()
                                    where user.Id == request.StaffId

                                    join production in _context.Productions.AsNoTracking()
                                        on user.Id equals production.UserId
                                    where production.AssignId == request.AssignId

                                    join evaluate in _context.Evaluates.AsNoTracking()
                                        on production.Id equals evaluate.ProductionId


                                    select new EvaluateDTO
                                    {
                                        Id = evaluate.Id,
                                        ProductionId = evaluate.ProductionId,
                                        QuantityError = evaluate.QuantityError,
                                        QuantitySuccess = evaluate.QuantitySuccess,
                                        Note = evaluate.Note,
                                        Image = _fileStorageService.GetFileUrl(evaluate.Image),
                                        Status = evaluate.Status,
                                        Created_At = evaluate.CreatedAt,

                                        Defects = evaluate.ComponentDefects
                                            .Select(cd => new ComponentDefectsDTO
                                            {
                                                Id = cd.Id,
                                                Description = cd.Description,
                                                Quantity = cd.Quantity,
                                                Status = cd.Status,
                                            }).ToList()
                                    }).ToListAsync(cancellationToken);

            if (!resultDtos.Any())
            {
                var staffExists = await _context.Users.AnyAsync(u => u.Id == request.StaffId);

                if (!staffExists)
                {
                    return Result<List<EvaluateDTO>>.Failure($"Không tìm thấy User với ID = {request.StaffId}.");
                }
            }
            return Result<List<EvaluateDTO>>.Success(resultDtos);
        }
    }
}
