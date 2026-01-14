using Microsoft.EntityFrameworkCore;
using System.Text;

namespace BlueLagoon.Shared.DevTools.EntityFramework;

public static class Extensions
{
    public static void RestrictCascadeDelete(this ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
    }

    public static string ToSnakeCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(value[0]));
        for (int i = 1; i < value.Length; i++)
        {
            char c = value[i];
            if (char.IsUpper(c))
            {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }
}
