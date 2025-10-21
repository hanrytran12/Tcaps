using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IProductionService
    {
        Task<ResponseDTO> SubmitProductionAsync(ProductionDTO dto, CancellationToken cancellationToken);
        Task<ResponseDTO> IncreaseQuantityAsync(Guid productionId, CancellationToken cancellationToken);
        Task<ResponseDTO> DecreaseQuantityAsync(Guid productionId, CancellationToken cancellationToken);
        Task<ResponseDTO> UpdateQuantityAsync(Guid productionId, int newQuantity, CancellationToken cancellationToken);
    }
}
