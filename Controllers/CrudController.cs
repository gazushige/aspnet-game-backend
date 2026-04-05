using MyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;

namespace MyApi.Controllers
{
    /// <summary>
    /// CRUD操作を行うためのベースコントローラー
    /// ApiDbContextを注入して、共通のCRUD操作を提供する。具体的なエンティティごとのコントローラーはこれを継承して作成する。
    /// </summary>

    [ApiController]
    [Route("staff/[controller]")]
    [EnableRateLimiting("StaffLimit")]
    [SkipPlayFabAuth] // PlayFab認証をスキップ
    public abstract class CrudController<T>(StaffDbContext db) : ControllerBase where T : class, IEntity
    {
        protected readonly StaffDbContext _db = db;

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 50)
        {
            pageSize = Math.Min(pageSize, 200); // 上限を設ける
            var items = await _db.Set<T>()
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
        public virtual async Task<ActionResult<T>> Create(T entity)
        {
            _db.Set<T>().Add(entity);
            await _db.SaveChangesAsync(); // ← これが必須！

            // GetById メソッドを呼び出すための情報を返却 (RESTfulな作法)
            return CreatedAtAction(nameof(GetById), new { id = GetEntityId(entity) }, entity);
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

        // IDを動的に取得するためのヘルパー（継承先で override するか、Reflection等で取得）
        // とりあえず動かすなら単純な return Ok(entity) でも大丈夫です。
        protected virtual object GetEntityId(T entity) => entity.Id;
    }
}