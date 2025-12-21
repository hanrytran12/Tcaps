using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence
{
    public class WorkshopConfiguration : IEntityTypeConfiguration<Workshop>
    {
        public void Configure(EntityTypeBuilder<Workshop> builder)
        {
            builder.Property(x => x.WorkshopType)
                .HasConversion<int>()
                .IsRequired();
        }
    }
}
