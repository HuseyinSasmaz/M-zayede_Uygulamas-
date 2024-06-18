using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Müzayede_Uygulaması.Models
{
    public class ÜrünListelemeViewModel
    {
        public int Id { get; set; }
        public string? Başlık { get; set; }
        public string? Açıklama { get; set; }
        public double Fiyat { get; set; }
        [Required]
        public IFormFile Resim { get; set; }
        public bool Satıldımı { get; set; }=false;
        [NotMapped]
       
        [Required]
        public string IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public IdentityUser? User { get; set; }
    }
}
