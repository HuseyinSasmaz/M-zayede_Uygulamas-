using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Müzayede_Uygulaması.Models
{
    public class ÜrünListeleme
    {
        [Key]
        public int Id { get; set; }
        public string Başlık { get; set; }
        public string Açıklama { get; set; }
        public double Fiyat { get; set; }
        public string ResimYolu { get; set; }   
        public bool Satıldımı { get; set; }=false;
        [Required]
        public string IdentityUserId { get; set; }
        [ForeignKey("IdentityUserId")]
        public IdentityUser? User { get; set; }  
        public List<Teklif>? Teklifs { get; set; }   
        public List<Yorum>? Yorums { get; set; }   
    }
}
