using System.ComponentModel.DataAnnotations;

namespace Holoul.Models
{
    public class Problem
    {
        [Key]
        public int ProblemID { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public string ProblemImage { get; set; }
        public string ProblemDescription { get; set; }
        public DateTime ReportDate { get; set; }
        public string YouTubeVideoURL { get; set; }

        //relations
        public Category Category { get; set; }
        public ICollection<AISolutionStep> AISolutionSteps { get; set; }
    }
}
