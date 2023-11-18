using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LigaWspinaczkowa.Models
{
    [Display(Name = "Przejścia")]
    public class UserStage
    {
        public int Id { get; set; }
        [Display(Name = "Rozdanie")]
        public int StageId { get; set; }
        [Display(Name = "Rozdanie")]
        public Stage? Stage { get; set; }


        [Display(Name = "Data Przejścia Wędka 1")]
        public DateTime? DateRoute1 { get; set; }
        [Display(Name = "Punkty Wędka 1")]
        public float? Route1Points { get; set; }
        [Display(Name = "Czy Zaakceptowane Wędka 1")]
        public bool IsAcceptedRoute1 { get; set; }


        [Display(Name = "Data Przejścia Wędka 2")]
        public DateTime? DateRoute2 { get; set; }
        [Display(Name = "Punkty Wędka 2")]
        public float? Route2Points { get; set; }
        [Display(Name = "Czy Zaakceptowane Wędka 2")]
        public bool IsAcceptedRoute2 { get; set; }


        [Display(Name = "Data Przejścia Prowadzenie")]
        public DateTime? DateRoute3 { get; set; }
        [Display(Name = "Punkty Prowadzenie")]
        public float? RouteLead3Points { get; set; }
        [Display(Name = "Czy Zaakceptowane Prowadznie")]
        public bool IsAcceptedRoute3 { get; set; }


        [Display(Name = "Użytkownik")]
        public string UserStageUserId { get; set; }
        [Display(Name = "Użytkownik")]
        public ApplicationUser? UserStageUser { get; set; }
    }
}
