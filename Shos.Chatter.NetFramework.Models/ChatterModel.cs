namespace Shos.Chatter.NetFramework.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Runtime.Serialization;

    public class ChatterModel : DbContext
    {
        public ChatterModel() : base("name=ChatterModel")
        {}

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
    }

    [DataContract]
    public class UserBase : IEquatable<UserBase>
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            var another = obj as UserBase;
            return !(another is null)        &&
                   Id  .Equals(another.Id  ) &&
                   Name.Equals(another.Name);
        }

        public bool Equals(UserBase another)
            => !(another is null)        &&
               Id  .Equals(another.Id  ) &&
               Name.Equals(another.Name);

        public override int GetHashCode()
        {
            int hashCode = -1919740922;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }

        public static bool operator ==(UserBase user1, UserBase user2) => !(user1 is null) && user1.Equals(user2);
        public static bool operator !=(UserBase user1, UserBase user2) => !(user1 == user2);
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public bool HasDeleted { get; set; } = false;
        public virtual ICollection<Chat> Chats { get; set; }
    }

    [DataContract]
    public class ChatBase : IEquatable<ChatBase>
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

        public override bool Equals(object obj)
        {
            var another = obj as ChatBase;
            if (another is null)
                return false;

            return Id.Equals(another.Id) &&
                   InsertDateTime.Equals(another.InsertDateTime) &&
                   UpdateDateTime.Equals(another.UpdateDateTime) &&
                   Content.Equals(another.Content) &&
                   UserId.Equals(another.UserId);
        }

        public bool Equals(ChatBase another)
            => !(another is null) &&
                InsertDateTime.Equals(another.InsertDateTime) &&
                UpdateDateTime.Equals(another.UpdateDateTime) &&
                Content       .Equals(another.Content       ) &&
                UserId        .Equals(another.UserId        );

        public override int GetHashCode()
        {
            int hashCode = -1919740922;
            hashCode = hashCode * -1521134295 + Id            .GetHashCode();
            hashCode = hashCode * -1521134295 + InsertDateTime.GetHashCode();
            hashCode = hashCode * -1521134295 + UpdateDateTime.GetHashCode();
            hashCode = hashCode * -1521134295 + Content       .GetHashCode();
            hashCode = hashCode * -1521134295 + UserId        .GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ChatBase chat1, ChatBase chat2) => !(chat1 is null) && chat1.Equals(chat2);
        public static bool operator !=(ChatBase chat1, ChatBase chat2) => !(chat1 == chat2);
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
}