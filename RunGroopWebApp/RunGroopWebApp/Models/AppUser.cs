﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Models
{
    public class AppUser : IdentityUser
    {
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public Address? Address { get; set; }
        public ICollection<Race> Races { get; set; }
        public ICollection<Club> Clubs { get; set; }
    }
}
