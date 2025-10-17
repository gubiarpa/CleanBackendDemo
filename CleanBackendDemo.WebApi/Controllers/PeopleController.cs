using CleanBackendDemo.Domain.Entities;
using CleanBackendDemo.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleanBackendDemo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController(AppDbContext db) : ControllerBase
{
    // GET: api/people
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Person>>> GetAll()
    {
        var people = await db.People.AsNoTracking().ToListAsync();
        return Ok(people);
    }

    // GET: api/people/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Person>> GetById(Guid id)
    {
        var person = await db.People.FindAsync(id);
        return person is null ? NotFound() : Ok(person);
    }

    // POST: api/people
    [HttpPost]
    public async Task<ActionResult<Person>> Create([FromBody] Person person)
    {
        if (person.Id == Guid.Empty) person.Id = Guid.NewGuid();
        db.People.Add(person);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = person.Id }, person);
    }

    // PUT: api/people/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Person update)
    {
        if (id != update.Id) return BadRequest("Id mismatch");

        var exists = await db.People.AnyAsync(p => p.Id == id);
        if (!exists) return NotFound();

        db.Entry(update).State = EntityState.Modified;
        await db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/people/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var person = await db.People.FindAsync(id);
        if (person is null) return NotFound();
        db.People.Remove(person);
        await db.SaveChangesAsync();
        return NoContent();
    }
}

