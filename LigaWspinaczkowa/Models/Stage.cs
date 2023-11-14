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
        [Display(Name = "Wędka 1 ilosc chwytów")]
        public int? Route1Holds { get; set; }
        [Display(Name = "Wędka 2 ilosc chwytów")]
        public int? Route2Holds { get; set; }
        [Display(Name = "Prowadzenie ilosc chwytów")]
        public int? Route3Lead { get; set; }
        [Display(Name = "Nazwa")]
        public string? Name { get; set; }
        [Display(Name = "Opis")]
        public string? Description { get; set; }
        public ICollection<UserStage> UserStage { get; } = new List<UserStage>();
    }
}
