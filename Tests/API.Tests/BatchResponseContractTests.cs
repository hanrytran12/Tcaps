using API.Controllers;
using Application.DTOs.Response;
using Application.Features.Batches.Queries.GetAllBatch;
using Application.Features.Batches.Queries.GetBatchForLead;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;
using Xunit;

namespace API.Tests;

public class BatchResponseContractTests
{
    [Fact]
    public async Task Batch_query_maps_root_and_assignment_fields_without_navigation_properties()
    {
        var batch = Batch.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "BATCH-001",
            100,
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 31));
        var assignment = Assignment.Create(
            batch.Id,
            Guid.NewGuid(),
            1,
            100,
            new DateOnly(2026, 1, 1),
            new DateOnly(2026, 1, 10),
            new DateOnly(2026, 1, 5),
            12.5m,
            true);
        batch.AddAssignment(assignment);

        var handler = new GetAllBatchQueryHandler(new StubBatchRepository(batch));
        var result = await handler.Handle(new GetAllBatchQuery(), CancellationToken.None);

        var dto = Assert.Single(result);
        Assert.Equal(batch.Id, dto.Id);
        Assert.Equal(batch.ProductId, dto.ProductId);
        Assert.Equal(batch.UserId, dto.UserId);
        Assert.Equal(batch.Code, dto.Code);
        Assert.Equal(batch.Quantity, dto.Quantity);
        Assert.Equal(batch.ActualQuantity, dto.ActualQuantity);
        Assert.Equal(batch.LostQuantity, dto.LostQuantity);
        Assert.Equal(batch.StartDate, dto.StartDate);
        Assert.Equal(batch.EndDate, dto.EndDate);
        Assert.Equal(batch.CreatedAt, dto.CreatedAt);
        Assert.Equal(batch.Status, dto.Status);
        Assert.Equal(batch.Note, dto.Note);
        Assert.Equal(batch.isDeleted, dto.IsDeleted);

        var assignmentDto = Assert.Single(dto.Assignments);
        Assert.Equal(assignment.Id, assignmentDto.Id);
        Assert.Equal(assignment.BatchId, assignmentDto.BatchId);
        Assert.Equal(assignment.WorkshopId, assignmentDto.WorkshopId);
        Assert.Equal(assignment.StepOrder, assignmentDto.StepOrder);
        Assert.Equal(assignment.Quantity, assignmentDto.Quantity);
        Assert.Equal(assignment.UnitPrice, assignmentDto.UnitPrice);
        Assert.Equal(assignment.StartDate, assignmentDto.StartDate);
        Assert.Equal(assignment.EndDate, assignmentDto.EndDate);
        Assert.Equal(assignment.ExpectedDeliveryDate, assignmentDto.ExpectedDeliveryDate);
        Assert.Equal(assignment.DateCompleted, assignmentDto.DateCompleted);
        Assert.Equal(assignment.RequiresMaterialDelivery, assignmentDto.RequiresMaterialDelivery);
        Assert.Equal(assignment.Status, assignmentDto.Status);
        Assert.Equal(assignment.CreatedAt, assignmentDto.CreatedAt);

        var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        Assert.Contains("\"isDeleted\"", json);
        Assert.DoesNotContain("\"product\"", json);
        Assert.DoesNotContain("\"products\"", json);
        Assert.DoesNotContain("\"evaluates\"", json);
        Assert.DoesNotContain("\"productions\"", json);
        Assert.DoesNotContain("\"materialUses\"", json);
        Assert.DoesNotContain("\"domainEvents\"", json);
    }

    [Fact]
    public async Task Lead_query_returns_only_batches_for_requested_lead()
    {
        var leadId = Guid.NewGuid();
        var matchingBatch = Batch.Create(Guid.NewGuid(), leadId, "MATCH", 10, new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 2));
        var otherBatch = Batch.Create(Guid.NewGuid(), Guid.NewGuid(), "OTHER", 20, new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 2));

        var handler = new GetBatchForLeadQueryHandler(new StubBatchRepository(matchingBatch, otherBatch));
        var result = await handler.Handle(new GetBatchForLeadQuery { UserId = leadId }, CancellationToken.None);

        var dto = Assert.Single(result);
        Assert.Equal(matchingBatch.Id, dto.Id);
    }

    [Fact]
    public void Batch_entity_routes_preserve_route_and_authorization_metadata()
    {
        var allAction = typeof(BatchController).GetMethod(nameof(BatchController.GetAllBatch))!;
        var leadAction = typeof(BatchController).GetMethod(nameof(BatchController.GetBatchesByLeadIdAsync))!;

        Assert.Equal(
            typeof(Task<List<BatchResponseDTO>>),
            allAction.ReturnType);
        Assert.Equal(
            typeof(Task<List<BatchResponseDTO>>),
            leadAction.ReturnType);

        Assert.Null(allAction.GetCustomAttribute<HttpGetAttribute>()!.Template);
        Assert.Equal("lead/batches", leadAction.GetCustomAttribute<HttpGetAttribute>()!.Template);
        Assert.Equal(
            "Admin,Lead,QC,QCK,QCTransport,Staff",
            allAction.GetCustomAttribute<AuthorizeAttribute>()!.Roles);
        Assert.Equal("Lead", leadAction.GetCustomAttribute<AuthorizeAttribute>()!.Policy);
    }

    private sealed class StubBatchRepository : IBatchRepository
    {
        private readonly List<Batch> _batches;

        public StubBatchRepository(params Batch[] batches)
        {
            _batches = batches.ToList();
        }

        public Task<IEnumerable<Batch>> GetAllAsync() => Task.FromResult<IEnumerable<Batch>>(_batches);

        public Task<List<Batch>> GetBatchesByLeadIdAsync(Guid userId) =>
            Task.FromResult(_batches.Where(batch => batch.UserId == userId).ToList());

        public Task<Batch?> GetByIdAsync(Guid id) => throw new NotSupportedException();
        public Task<IEnumerable<Batch>> GetBatchesByIdsAsync(List<Guid> ids) => throw new NotSupportedException();
        public Task AddAsync(Batch batch) => Task.CompletedTask;
        public void Delete(Batch batch) => throw new NotSupportedException();
        public void Update(Batch batch) => throw new NotSupportedException();
        public Task<Batch?> GetByIdWithAssignmentsAsync(Guid id) => throw new NotSupportedException();
        public Task<Batch?> GetByCodeAsync(string code) => throw new NotSupportedException();
        public Task<bool> IsProductInUseAsync(Guid productId) => throw new NotSupportedException();
        public Task<bool> AreAllAssignmentsCompletedAsync(Guid batchId) => throw new NotSupportedException();
        public Task<Batch?> GetByAssignmentIdAsync(Guid assignmentId) => throw new NotSupportedException();
        public Task<int?> GetLastCodeIndexAsync(string prefix) => throw new NotSupportedException();
        public Task<Batch?> GetByIdAssignmentWithMaterialUse(Guid assignmentId) => throw new NotSupportedException();
    }
}
