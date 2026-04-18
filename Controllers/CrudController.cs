using MyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;

namespace MyApi.Controllers
{
    /// <summary>
    /// CRUD操作を行うためのベースコントローラー
    /// StaffDbContextを注入して、共通のCRUD操作を提供する。具体的なエンティティごとのコントローラーはこれを継承して作成する。
    /// </summary>

    [ApiController]
    [Route("staff/[controller]")]
    [EnableRateLimiting("StaffLimit")]
    public abstract class CrudController<T>(StaffDbContext db) : ControllerBase where T : class, IEntity
    {
        protected readonly StaffDbContext _db = db;

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 50)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 200);
            var items = await _db.Set<T>()
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<T>> GetById(int id)
        {
            var entity = await _db.Set<T>().FindAsync(id);
            if (entity == null) return NotFound();
            return entity;
        }

        [HttpPost]
        public virtual async Task<ActionResult<IEnumerable<T>>> Create(List<T> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return BadRequest("Request body must be a non-empty array.");
            }

            _db.Set<T>().AddRange(entities);
            await _db.SaveChangesAsync();

            return Ok(entities);
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<T>> Update(int id, T entity)
        {
            if (id != entity.Id) return BadRequest("ID mismatch");

            var existing = await _db.Set<T>().FindAsync(id);
            if (existing == null) return NotFound();

            _db.Entry(existing).CurrentValues.SetValues(entity);
            await _db.SaveChangesAsync();
            return Ok(entity);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(int id)
        {
            var entity = await _db.Set<T>().FindAsync(id);
            if (entity == null) return NotFound();

            _db.Set<T>().Remove(entity);
            await _db.SaveChangesAsync(); // ← 必須！

            return NoContent();
        }
    }
}