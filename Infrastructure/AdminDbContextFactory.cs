// ✅ デザインタイム専用ファクトリ（migrationsコマンド用）
// Program.csのDI設定に依存せず単独でDbContextを生成できる
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MyApi.Models;

public class AdminDbContextFactory : IDesignTimeDbContextFactory<AdminDbContext>
{
    public AdminDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var options = new DbContextOptionsBuilder<AdminDbContext>()
            .UseNpgsql(config.GetConnectionString("AdminContext"))
            .Options;

        return new AdminDbContext(options);
    }
}