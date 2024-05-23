﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAdaptive.Models
{
    public class UserModel
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        [StringLength(15)]
        public string? Username { get; set; }

        [Required]
        [StringLength(15)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(15)]
        public string? LastName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string? HashedPassword { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd-HH-mm}", ApplyFormatInEditMode = true)]
        public DateTime LastLoginDate { get; set; }

        [Required]
        public int LoginAttempts { get; set; }

    }
}
