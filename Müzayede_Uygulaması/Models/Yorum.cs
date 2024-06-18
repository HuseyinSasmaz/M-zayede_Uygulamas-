using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Müzayede_Uygulaması.Models
{
    public class Yorum
    {
        public int Id { get; set; }
        public string? İçerik { get; set; }
        [Required]
        public string IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public IdentityUser? User { get; set; }
        public int? ÜrünListelemeId { get; set; }
        [ForeignKey("ÜrünListelemeId")]
        public ÜrünListeleme? ÜrünListeleme { get; set; }
    }
}
