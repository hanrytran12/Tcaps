using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.MaterialWorkshops.Queries.GetMaterialWorkshopByQCId
{
    public class GetMaterialWorkshopByQCIdQueryHandler : IRequestHandler<GetMaterialWorkshopByQCIdQuery, List<MaterialWorkshop>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMaterialWorkshopRepository _materialWorkshopRepository;

        public GetMaterialWorkshopByQCIdQueryHandler(IUserRepository userRepository, IMaterialWorkshopRepository materialWorkshopRepository)
        {
            _userRepository = userRepository;
            _materialWorkshopRepository = materialWorkshopRepository;
        }
        public async Task<List<MaterialWorkshop>> Handle(GetMaterialWorkshopByQCIdQuery request, CancellationToken cancellationToken)
        {
            var qc = await _userRepository.GetByIdAsync(request.QC_Id);
            if (qc == null)
            {
                throw new Exception($"Không tìm thấy QC có Id = {request.QC_Id}");
            }

            var materialWorkshops = await _materialWorkshopRepository.GetAllByWorkshopIdAsync(qc.WorkshopId);
            if (materialWorkshops == null || !materialWorkshops.Any())
                return new List<MaterialWorkshop>();

            if (!string.IsNullOrEmpty(request.Status))
            {
                materialWorkshops = materialWorkshops.Where(m => m.Status == request.Status).ToList();
            }

            materialWorkshops = materialWorkshops.OrderByDescending(m => m.CreatedAt).ToList();
            return materialWorkshops.ToList();
        }
    }
}
