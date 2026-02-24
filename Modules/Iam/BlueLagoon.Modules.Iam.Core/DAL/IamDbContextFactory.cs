using BlueLagoon.Modules.Iam.Core.DAL.Entities;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlueLagoon.Modules.Iam.Core.DAL;

internal sealed class IamDbContextFactory : IDesignTimeDbContextFactory<IamDbContext>
{
    public IamDbContext CreateDbContext(string[] args)
    {
        if (Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") == "Development")
            Env.Load("../../../docker/.env");
        
        var connectionString =
            Environment.GetEnvironmentVariable("Blue_ConnectionStrings__IamDb");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException($"Missing IamDb connection string");

        var builder = new DbContextOptionsBuilder<IamDbContext>();

        builder.UseNpgsql(
                connectionString,
                x =>
                {
                    x.MigrationsAssembly(typeof(IamDbContext).Assembly.FullName);
                    x.MigrationsHistoryTable("__EFMigrationsHistory", "iam");
                });
        builder.UseOpenIddict<Application, Authorization, Module, Token, Guid>();
            
        return new IamDbContext(builder.Options);
    }
}