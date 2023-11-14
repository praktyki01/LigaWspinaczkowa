using Microsoft.AspNetCore.Identity;

namespace Liga.Models
{
    public class UserStage
    {
        public int Id { get; set; }
        public int StageId { get; set; }
        public Stage? Stage { get; set; }
        public DateTime Date { get; set; }
        public float Route1Points { get; set; }
        public float Route2Points { get; set; }
        public float RouteLead3Points { get; set; }
        public bool IsAccepted { get; set; }
        public string UserStageUserId { get; set; }
        public IdentityUser? UserStageUser { get; set; }
    }
}
