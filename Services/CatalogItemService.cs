// // Services/CatalogItemService.cs
// using Microsoft.EntityFrameworkCore;
// using MyApi.Models;
// // Services/CatalogItemService.cs
// public class CatalogItemService
// {
//     private readonly StaffDbContext _db;

//     public CatalogItemService(StaffDbContext db)
//     {
//         _db = db;
//     }

//     /// <summary>
//     /// CatalogItemBase を継承する任意のアイテムを Catalog 自動生成付きで保存する
//     /// </summary>
//     public async Task<TItem> CreateAsync<TItem>(
//         int seriesId,
//         TItem item,
//         CancellationToken ct = default)
//         where TItem : CatalogItemBase
//     {
//         await using var tx = await _db.Database.BeginTransactionAsync(ct);
//         try
//         {
//             var series = await _db.Series
//                 .FirstOrDefaultAsync(s => s.Id == seriesId, ct)
//                 ?? throw new InvalidOperationException($"SeriesId:{seriesId} が存在しません");

//             series.LastNumber += 1;
//             _db.Series.Update(series);

//             var catalog = new Catalog
//             {
//                 Uuid = Guid.CreateVersion7(),
//                 SeriesId = seriesId,
//                 Number = series.LastNumber,
//                 IsLocked = false,
//             };
//             _db.Catalogs.Add(catalog);
//             await _db.SaveChangesAsync(ct); // catalog.Id を確定

//             item.CatalogUuid = catalog.Uuid;
//             item.Catalog = catalog;

//             // EF Core が TItem の実際の型を見て適切なテーブルに INSERT する
//             _db.Set<TItem>().Add(item);
//             await _db.SaveChangesAsync(ct);

//             await tx.CommitAsync(ct);
//             return item;
//         }
//         catch
//         {
//             await tx.RollbackAsync(ct);
//             throw;
//         }
//     }
// }