using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Shos.Chatter.NetFramework.Models;

namespace Shos.Chatter.NetFramework.Server.Controllers
{
    public class ChatsController : ApiController
    {
        ChatterContext db = new ChatterContext();

        // GET: api/Chats
        public async Task<IEnumerable<ChatBase>> GetChats()
            => await db.Chats
                       .OrderByDescending(chat => chat.InsertDateTime)
                       .Select(chat => new ChatBase { Id = chat.Id, Content = chat.Content, InsertDateTime = chat.InsertDateTime, UpdateDateTime = chat.UpdateDateTime, UserId = chat.UserId })
                       .ToListAsync();

        // GET: api/Chats/5
        [ResponseType(typeof(ChatBase))]
        public async Task<IHttpActionResult> GetChat(int id)
        {
            var chat = await db.Chats.FindAsync(id);
            if (chat == null)
                return NotFound();
            return Ok(new ChatBase { Id = chat.Id, Content = chat.Content, InsertDateTime = chat.InsertDateTime, UpdateDateTime = chat.UpdateDateTime, UserId = chat.User.Id });
        }

        // PUT: api/Chats/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutChat(int id, ChatBase chat)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != chat.Id)
                return BadRequest();

            var chatInDb = await db.Chats.FindAsync(chat.Id);
            if (chatInDb == null)
                return BadRequest();
            var user = await db.Users.FindAsync(chat.UserId);
            if (user == null)
                return BadRequest();

            chatInDb.Content = chat.Content;
            chatInDb.UpdateDateTime = DateTime.Now;
            db.Entry(chatInDb).State = EntityState.Modified;

            try {
                await db.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!ChatExists(id))
                    return NotFound();
                else
                    throw;
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Chats
        [ResponseType(typeof(ChatBase))]
        public async Task<IHttpActionResult> PostChat(ChatBase chat)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await db.Users.FindAsync(chat.UserId);
            if (user is null || user.HasDeleted)
                return BadRequest();

            var now = DateTime.Now;
            var newChat = new Chat { Content = chat.Content, InsertDateTime = now, UpdateDateTime = now, User = user };
            db.Chats.Add(newChat);
            await db.SaveChangesAsync();
            chat.Id = newChat.Id;
            return CreatedAtRoute("DefaultApi", new { id = chat.Id }, chat);
        }

        // DELETE: api/Chats/5
        [ResponseType(typeof(Chat))]
        public async Task<IHttpActionResult> DeleteChat(int id)
        {
            var chat = await db.Chats.FindAsync(id);
            if (chat == null)
                return NotFound();

            db.Chats.Remove(chat);
            await db.SaveChangesAsync();

            return Ok(chat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        bool ChatExists(int id) => db.Chats.Count(e => e.Id == id) > 0;
        bool UserExists(int id) => db.Users.Count(e => e.Id == id && !e.HasDeleted) > 0;
    }
}