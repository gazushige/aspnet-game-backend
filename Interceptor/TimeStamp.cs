using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MyApi.Models;

public class TimestampInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, InterceptionResult<int> result)
    {
        var entries = eventData.Context!.ChangeTracker.Entries<IHasTimestamps>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
            if (entry.State is EntityState.Added or EntityState.Modified)
                entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
        }
        return base.SavingChanges(eventData, result);
    }
}