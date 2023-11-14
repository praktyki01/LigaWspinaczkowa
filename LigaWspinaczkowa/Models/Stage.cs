namespace Liga.Models
{
    public class Stage
    {
        public int Id { get; set; }
        public DateTime DataFrom { get; set; }
        public DateTime DataTo { get; set; }
        public int Route1Holds { get; set; }
        public int Route2Holds { get; set; }
        public int Route3Lead { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<UserStage> UserStage { get; } = new List<UserStage>();
    }
}
