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
using Shos.Chatter.Models;

namespace Shos.Chatter.Server.Controllers
{
    public class ChatsController : ApiController
    {
        ChatterContext context = new ChatterContext();

        // GET: api/Chats
        public async Task<IEnumerable<ChatBase>> GetChats()
            => await context.Chats
                            .OrderByDescending(chat => chat.InsertDateTime)
                            .Select(chat => new ChatBase { Id = chat.Id, Content = chat.Content, InsertDateTime = chat.InsertDateTime, UpdateDateTime = chat.UpdateDateTime, UserId = chat.UserId })
                            .ToListAsync();

        // GET: api/Chats/5
        [ResponseType(typeof(ChatBase))]
        public async Task<IHttpActionResult> GetChat(int id)
        {
            if (context.Chats is null)
                return NotFound();

            var chat = await context.Chats.FindAsync(id);

            if (chat is null || chat.User is null)
                return NotFound();

            return Ok(new ChatBase { Id = chat.Id, Content = chat.Content, InsertDateTime = chat.InsertDateTime, UpdateDateTime = chat.UpdateDateTime, UserId = chat.User.Id });
        }

        // PUT: api/Chats/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutChat(int id, ChatBase chat)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != chat.Id || context.Chats is null || context.Users is null)
                return BadRequest();

            var chatInDb = await context.Chats.FindAsync(chat.Id);
            if (chatInDb is null)
                return BadRequest();

            var user = await context.Users.FindAsync(chat.UserId);
            if (user is null)
                return BadRequest();

            chatInDb.Content = chat.Content;
            chatInDb.UpdateDateTime = DateTime.Now;
            context.Entry(chatInDb).State = EntityState.Modified;

            try {
                await context.SaveChangesAsync();
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

            if (context.Chats is null)
                return BadRequest();

            var user = await GetUser(chat.UserId);
            if (user is null)
                return BadRequest();

            var now = DateTime.Now;
            var newChat = new Chat { Content = chat.Content, InsertDateTime = now, UpdateDateTime = now, User = user };
            context.Chats.Add(newChat);
            await context.SaveChangesAsync();
            chat.Id = newChat.Id;
            return CreatedAtRoute("DefaultApi", new { id = chat.Id }, chat);
        }

        // DELETE: api/Chats/5
        [ResponseType(typeof(Chat))]
        public async Task<IHttpActionResult> DeleteChat(int id)
        {
            var chat = await context.Chats?.FindAsync(id);
            if (chat is null)
                return NotFound();

            context.Chats.Remove(chat);
            await context.SaveChangesAsync();

            return Ok(chat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                context.Dispose();
            base.Dispose(disposing);
        }

        bool ChatExists(int id) => !(context.Chats is null) && context.Chats.Any(chat => chat.Id == id);

        async Task<User?> GetUser(int id)
        {
            var user = await context.Users?.FindAsync(id);
            return user is null || user.HasDeleted ? null : user;
        }
    }
}