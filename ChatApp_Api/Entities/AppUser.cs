﻿using ChatApp_Api.Extensions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChatApp_Api.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created {  get; set; }= DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
 
        public ICollection<Photo> Photos { get; set; }

        public ICollection<UserLike> LikedByUsers { get; set; }
        public ICollection<UserLike> LikedUsers { get; set; }
        public ICollection<Message> MessageSent { get; set; }
        public ICollection<Message> MessageReceived { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
