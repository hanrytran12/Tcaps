using Azure.Core;
using Domain.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeders
{
    public static class UserSeeder
    {
        public static async Task SeedUsersAsync(AppDbContext context, CancellationToken cancellationToken = default)
        {
            if (await context.Users.AnyAsync(cancellationToken))
                return; // Skip if already seeded

            var _passwordHasher = new PasswordHasher();
            var passwordHash = _passwordHasher.Hash("123456789");

            var users = new List<User>
            {
                // Admin
                User.Create(
                    workshopId: Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000001"),
                    role: "Admin",
                    fullName: "Nguyễn Văn An",
                    email: "admin@tcaps.com",
                    passwordHash: passwordHash,
                    phone: "0901234567"
                ),

                // Lead
                User.Create(
                    workshopId: Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000002"),
                    role: "Lead",
                    fullName: "Trần Thị Bình",
                    email: "lead@tcaps.com",
                    passwordHash: passwordHash,
                    phone: "0902345678"
                ),

                // Staff xưởng 3
                User.Create(
                    workshopId: Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000002"),
                    role: "Staff",
                    fullName: "Lê Văn Cường",
                    email: "staff.cuong@tcaps.com",
                    passwordHash: passwordHash,
                    phone: "0903456789"
                ),
                User.Create(
                    workshopId: Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000002"),
                    role: "Staff",
                    fullName: "Phạm Thị Dung",
                    email: "staff.dung@tcaps.com",
                    passwordHash: passwordHash,
                    phone: "0904567890"
                ),

                // QC Transport
                User.Create(
                    workshopId: Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000003"),
                    role: "QCTransport",
                    fullName: "Nguyễn Thị Hạnh",
                    email: "qctransport@tcaps.com",
                    passwordHash: passwordHash,
                    phone: "0906789012"
                ),

                // QC users 1-2 hardcoded
                User.Create(
                    workshopId: Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000001"),
                    role: "QC",
                    fullName: "QC Xưởng 1",
                    email: "qc.workshop1@tcaps.com",
                    passwordHash: passwordHash,
                    phone: "0901000001"
                ),
                User.Create(
                    workshopId: Guid.Parse("A1C9B3A0-4F12-4E81-B17B-000000000002"),
                    role: "QC",
                    fullName: "QC Xưởng 2",
                    email: "qc.workshop2@tcaps.com",
                    passwordHash: passwordHash,
                    phone: "0901000002"
                ),
            };

            // Generate remaining QC users for workshops 3-16
            for (int i = 3; i <= 16; i++)
            {
                users.Add(User.Create(
                    workshopId: Guid.Parse($"A1C9B3A0-4F12-4E81-B17B-{i:D12}"),
                    role: "QC",
                    fullName: $"QC Xưởng {i}",
                    email: $"qc.workshop{i}@tcaps.com",
                    passwordHash: passwordHash,
                    phone: $"09010000{i:D2}"
                ));
            }

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
