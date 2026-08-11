using Application.Features.MaterialWorkshops.Queries.GetAllMaterialWorkshop;
using Domain.Entities;
using Domain.Interfaces;
using System.Reflection;
using Xunit;

namespace API.Tests;

public class MaterialWorkshopQueryHandlerTests
{
    [Fact]
    public async Task Applies_status_filter_and_maps_all_summary_fields()
    {
        var confirmed = MaterialWorkshop.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            12);
        confirmed.Update(10);
        confirmed.Confirmed();

        var pending = MaterialWorkshop.Create(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            20);

        var handler = CreateHandler(confirmed, pending);
        var result = await handler.Handle(new GetAllMaterialWorkshopQuery { Status = "Confirmed" }, CancellationToken.None);

        var dto = Assert.Single(result);
        Assert.Equal(confirmed.Id, dto.Id);
        Assert.Equal(confirmed.WorkshopId, dto.WorkshopId);
        Assert.Equal(confirmed.AssignId, dto.AssignId);
        Assert.Equal(confirmed.AssignmentTransferRequestId, dto.AssignmentTransferRequestId);
        Assert.Equal(confirmed.SupplierId, dto.SupplierId);
        Assert.Equal(confirmed.QuantitySend, dto.QuantitySend);
        Assert.Equal(confirmed.QuantityReceive, dto.QuantityReceive);
        Assert.Equal(confirmed.ShipDate, dto.ShipDate);
        Assert.Equal(confirmed.CreatedAt, dto.CreatedAt);
        Assert.Equal(confirmed.Status, dto.Status);
    }

    [Fact]
    public async Task Orders_summary_results_by_created_at_descending()
    {
        var oldest = MaterialWorkshop.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 1);
        var newest = MaterialWorkshop.Create(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 2);
        SetCreatedAt(oldest, new DateTime(2026, 1, 1));
        SetCreatedAt(newest, new DateTime(2026, 1, 2));

        var handler = CreateHandler(oldest, newest);
        var result = await handler.Handle(new GetAllMaterialWorkshopQuery(), CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Equal(newest.Id, result[0].Id);
        Assert.Equal(oldest.Id, result[^1].Id);
    }

    private static GetAllMaterialWorkshopQueryHandler CreateHandler(params MaterialWorkshop[] items)
    {
        return new GetAllMaterialWorkshopQueryHandler(new StubMaterialWorkshopRepository(items));
    }

    private static void SetCreatedAt(MaterialWorkshop entity, DateTime createdAt)
    {
        typeof(MaterialWorkshop)
            .GetProperty(nameof(MaterialWorkshop.CreatedAt), BindingFlags.Instance | BindingFlags.Public)!
            .SetValue(entity, createdAt);
    }

    private sealed class StubMaterialWorkshopRepository : IMaterialWorkshopRepository
    {
        private readonly IEnumerable<MaterialWorkshop> _items;

        public StubMaterialWorkshopRepository(IEnumerable<MaterialWorkshop> items)
        {
            _items = items;
        }

        public Task<IEnumerable<MaterialWorkshop>> GetAllAsync() => Task.FromResult(_items);

        public Task<IEnumerable<MaterialWorkshop>> GetAllByWorkshopIdAsync(Guid? workshopId) =>
            throw new NotSupportedException();

        public Task<MaterialWorkshop> GetByIdAsync(Guid id) =>
            throw new NotSupportedException();

        public Task AddAsync(MaterialWorkshop materialWorkshop) => Task.CompletedTask;

        public void Update(MaterialWorkshop materialWorkshop)
        {
            throw new NotSupportedException();
        }

        public void Delete(MaterialWorkshop materialWorkshop)
        {
            throw new NotSupportedException();
        }
    }
}
