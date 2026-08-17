using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Features.Batches.Mapping;

public static class BatchResponseMapper
{
    public static BatchResponseDTO ToResponse(Batch batch)
    {
        return new BatchResponseDTO
        {
            Id = batch.Id,
            ProductId = batch.ProductId,
            UserId = batch.UserId,
            Code = batch.Code,
            Quantity = batch.Quantity,
            ActualQuantity = batch.ActualQuantity,
            LostQuantity = batch.LostQuantity,
            StartDate = batch.StartDate,
            EndDate = batch.EndDate,
            CreatedAt = batch.CreatedAt,
            Status = batch.Status,
            Note = batch.Note,
            IsDeleted = batch.IsDeleted,
            Assignments = batch.Assignments.Select(assignment => new BatchAssignmentResponseDTO
            {
                Id = assignment.Id,
                BatchId = assignment.BatchId,
                WorkshopId = assignment.WorkshopId,
                StepOrder = assignment.StepOrder,
                Quantity = assignment.Quantity,
                UnitPrice = assignment.UnitPrice,
                StartDate = assignment.StartDate,
                EndDate = assignment.EndDate,
                ExpectedDeliveryDate = assignment.ExpectedDeliveryDate,
                DateCompleted = assignment.DateCompleted,
                RequiresMaterialDelivery = assignment.RequiresMaterialDelivery,
                Status = assignment.Status,
                CreatedAt = assignment.CreatedAt
            }).ToList()
        };
    }
}
