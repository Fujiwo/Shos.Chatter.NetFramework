using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Runtime.Serialization;

namespace Shos.Chatter.NetFramework.Models
{
    [DataContract]
    public class UserBase
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public bool HasDeleted { get; set; } = false;
        public virtual ICollection<Chat> Chats { get; set; }
    }

    [DataContract]
    public class ChatBase
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public DateTime InsertDateTime { get; set; }
        [DataMember]
        public DateTime UpdateDateTime { get; set; }
        [DataMember]
        public string Content { get; set; } = "";
        [DataMember]
        public int UserId { get; set; }
    }

    public class Chat
    {
        public int Id { get; set; }
        public DateTime InsertDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        [Required]
        public string Content { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }

    public class ChatterContext : DbContext
    {
        public ChatterContext() : base("name=ChatterContext")
        { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
    }
}