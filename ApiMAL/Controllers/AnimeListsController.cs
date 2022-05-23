using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ApiMAL.EnumData;

namespace ApiMAL.Controllers
{
    /// <summary>
    /// Controller Class to handle the requests of the User Anime List
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnimeListsController : ControllerBase
    {
        /// <summary>
        /// The database context for queries of the Entity
        /// </summary>
        private readonly AnimeListContext _context;

        public AnimeListsController (AnimeListContext context) {
            _context = context;
        }

        /// <summary>
        /// List the items of an User Anime List with Pagination
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="status"></param>
        /// <param name="page"></param>
        /// <returns>An 200 HTTP Code if some anime is finded, 404 Code if not and 400 Code if something went Wrong</returns>
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

        /// <summary>
        /// List the Users Id of the database
        /// </summary>
        /// <param name="page"></param>
        /// <returns>An 200 HTTP Code if some anime is finded, 404 Code if not and 400 Code if something went Wrong</returns>
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

        /// <summary>
        /// Get Only 1 anime or nothing of the user list.
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns>An 200 HTTP Code if some anime is finded, 404 Code if not and 400 Code if something went Wrong</returns>
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

        /// <summary>
        /// Insert a new anime in the User Anime List
        /// </summary>
        /// /// <remarks>
        /// Exemplo:
        ///
        ///     POST /AnimeLists
        ///     {
        ///         "userId": 1,
        ///         "title": "New Anime",
        ///         "type": "OVA",
        ///         "nrEpisodes": 1,
        ///         "nrWatched": 0,
        ///         "status": "Watching",
        ///         "startDate": "2022-05-23T13:36:08.679Z",
        ///         "endDate": "",
        ///         "score": 0,
        ///         "timesWatched": 0
        ///     }
        ///
        /// </remarks>
        /// <param name="animeList"></param>
        /// <returns>An 201 HTTP Code if the Anime is created successfully or a Message with the problem</returns>
        // POST: api/AnimeLists
        [HttpPost]
        public async Task<ActionResult<AnimeList>> PostAnimeList (AnimeList animeList) {
            if (_context.AnimeList == null) {
                return Problem("Entity set 'AnimeListContext.AnimeList' is null.");
            }
            _context.AnimeList.Add(animeList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserAnimeByid", new { userid = animeList.UserId, id = animeList.Id }, animeList);
        }

        /// <summary>
        /// Update an anime in the User Anime List
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <param name="animeList"></param>
        /// <returns>An 204 HTTP Code if the Anime is updated successfully or a Message with the problem</returns>
        // PUT: api/AnimeLists/5/5
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

        /// <summary>
        /// Delete an anime from the user list
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Check if the anime id exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool AnimeListExists (uint id) {
            return (_context.AnimeList?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
