using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Shos.Chatter.NetFramework.Server.Controllers
{
    using Models;

    public class UsersController : ApiController
    {
        ChatterContext db = new ChatterContext();

        // GET: api/Users
        public async Task<IEnumerable<UserBase>> GetUsers()
            => await db.Users
                       .Where(user => !user.HasDeleted)
                       .OrderBy(user => user.Name)
                       .Select(user => new UserBase { Id = user.Id, Name = user.Name })
                       .ToListAsync();

        // GET: api/Users/5
        [ResponseType(typeof(UserBase))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            var user = await db.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            return Ok(new UserBase { Id = user.Id, Name = user.Name });
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, UserBase user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != user.Id)
                return BadRequest();

            var userInDb = await db.Users.FindAsync(user.Id);
            if (userInDb == null)
                return BadRequest();

            userInDb.Name = user.Name;
            db.Entry(userInDb).State = EntityState.Modified;
            try {
                await db.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!UserExists(id))
                    return NotFound();
                else
                    throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(UserBase))]
        public async Task<IHttpActionResult> PostUser(UserBase user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newUser = new User { Name = user.Name };
            db.Users.Add(newUser);
            await db.SaveChangesAsync();
            user.Id = newUser.Id;

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(UserBase))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            var user = await db.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            user.HasDeleted = true;
            db.Entry(user).State = EntityState.Modified;
            //db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(new UserBase { Id = user.Id, Name = user.Name });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        bool UserExists(int id) => db.Users.Count(e => e.Id == id && !e.HasDeleted) > 0;
    }
}