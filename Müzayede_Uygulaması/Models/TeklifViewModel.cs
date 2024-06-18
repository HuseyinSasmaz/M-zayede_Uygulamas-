using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Müzayede_Uygulaması.Models
{
    public class TeklifViewModel
    {
        public int Id { get; set; }
        public double Fiyat { get; set; }
        [Required]
        public string IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public IdentityUser? User { get; set; }
        public int? ÜrünListelemeId { get; set; }
        [ForeignKey("ÜrünListelemeId")]
        public ÜrünListeleme? ÜrünListeleme { get; set; }
    }
}
