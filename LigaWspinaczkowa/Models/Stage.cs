using System.ComponentModel.DataAnnotations;

namespace LigaWspinaczkowa.Models
{
    [Display(Name = "Rozdanie")]
    public class Stage
    {
        public int Id { get; set; }
        [Display(Name = "Od")]
        public DateTime DataFrom { get; set; }
        [Display(Name = "Do")]
        public DateTime DataTo { get; set; }
        [Display(Name = "Nazwa")]
        public string? Name { get; set; }
        public ICollection<UserStage> UserStage { get; } = new List<UserStage>();
    }
}
