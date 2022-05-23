using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ApiMAL.EnumData;

namespace ApiMAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnimeListsController : ControllerBase
    {
        private readonly AnimeListContext _context;

        public AnimeListsController (AnimeListContext context) {
            _context = context;
        }

        // GET: api/AnimeLists/5/1
        [HttpGet("{userid}/{status}/{page}"), AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AnimeList>>> GetUserAnimeList (uint userid, Status? status, int? page = 1) {
            try {
                if (_context.AnimeList == null) {
                    return NotFound();
                }

                int cur = page ?? default(int);

                IQueryable<AnimeList> query;
                if (!status.HasValue) {
                    query = _context.AnimeList
                    .Where(x => x.UserId == userid)
                    .OrderBy(x => x.Title);
                } else {
                    query = _context.AnimeList
                    .Where(x => x.UserId == userid && x.Status == status)
                    .OrderBy(x => x.Title);
                }

                var pageResults = 20f;
                var pageCount = Math.Ceiling(query.Count() / pageResults);

                var list = await query
                    .Skip((cur - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();

                var response = new AnimeListResponse {
                    UserList = list,
                    CurrentPage = cur,
                    Pages = (int)pageCount
                };
                return !list.Any() ? NotFound() : Ok(response);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/AnimeLists/users
        [HttpGet("users/{page}"), AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AnimeList>>> GetUsersId (int? page = 1) {
            try {
                if (_context.AnimeList == null) {
                    return NotFound();
                }

                int cur = page ?? default(int);

                var query = _context.AnimeList.Select(x => x.UserId).Distinct();
                var pageResults = 10f;
                var pageCount = Math.Ceiling(query.Count() / pageResults);

                var list = await query
                    .Skip((cur - 1) * (int)pageResults)
                    .Take((int)pageResults)
                    .ToListAsync();

                var response = new AnimeListResponse {
                    UserId = list,
                    CurrentPage = cur,
                    Pages = (int)pageCount
                };
                return !list.Any() ? NotFound() : Ok(response);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/AnimeLists/5/1/1
        [HttpGet("{userid}/anime/{id}"), AllowAnonymous]
        public async Task<ActionResult<AnimeList>> GetUserAnimeByid (uint userid, uint id) {
            try {
                if (_context.AnimeList == null) {
                    return NotFound();
                }

                var list = await _context.AnimeList
                    .Where(x => x.UserId == userid && x.Id == id)
                    .ToListAsync();
                return !list.Any() ? NotFound() : Ok(list);
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/AnimeLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AnimeList>> PostAnimeList (AnimeList animeList) {
            if (_context.AnimeList == null) {
                return Problem("Entity set 'AnimeListContext.AnimeList' is null.");
            }
            _context.AnimeList.Add(animeList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserAnimeByid", new { userid = animeList.UserId, id = animeList.Id }, animeList);
        }

        // PUT: api/AnimeLists/5/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userid}/{id}")]
        public async Task<IActionResult> PutAnimeList (uint userid, uint id, AnimeList animeList) {
            if (id != animeList.Id || userid != animeList.UserId) {
                return BadRequest("Cant update User and Anime codes");
            }

            _context.Entry(animeList).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!AnimeListExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE: api/AnimeLists/5/5
        [HttpDelete("{userid}/{id}")]
        public async Task<IActionResult> DeleteAnimeList (uint userid, uint id) {
            try {
                if (_context.AnimeList == null) {
                    return NotFound();
                }

                var list = await _context.AnimeList
                .Where(x => x.UserId == userid && x.Id == id)
                .FirstAsync();
                _context.AnimeList.Remove(list);
                await _context.SaveChangesAsync();
                return NoContent();
            } catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        private bool AnimeListExists (uint id) {
            return (_context.AnimeList?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
