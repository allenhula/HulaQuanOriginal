using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HulaQuanOriginal.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PortraitUrl { get; set; }

        public virtual ICollection<Pet> Pets { get; set; }
        public virtual ICollection<Publish> Publishs { get; set; }
    }

    public class Relationship
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }
    }

    public class FriendRequest
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public bool Confirmed { get; set; }
        public DateTime CreatedTime { get; set; }
    }

    public enum PetType
    {
        Dog
    }

    public class Pet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public PetType Type { get; set; }
        public DateTime BirthDate { get; set; }
        public string PictureUrl { get; set; }
    }
}