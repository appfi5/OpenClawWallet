using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OpenClawWalletServer.Infrastructure;

/// <summary>
/// KeyDbContext
/// </summary>
public class KeyDbContext(DbContextOptions<KeyDbContext> options) : DbContext(options), IDataProtectionKeyContext
{
    /// <summary>
    /// 密钥
    /// </summary>
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = default!;
}