# 安装工具  SEE： https://learn.microsoft.com/zh-cn/ef/core/cli/dotnet#installing-the-tools

```bash
dotnet tool install --global dotnet-ef --version 8.0.0
```

# 强制更新数据库

```bash
dotnet ef database update
```

# 创建迁移 SEE：https://learn.microsoft.com/zh-cn/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
# 应用数据库迁移

```bash
dotnet ef migrations add InitialCreate --context ApplicationDbContext -o Migrations/ApplicationDb --project src/OpenClawWalletServer.csproj
```

