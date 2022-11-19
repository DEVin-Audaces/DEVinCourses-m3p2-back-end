namespace DEVCoursesAPI.Data.DTOs
{
    public class TrainingReport
    {
        public string Name { get; set; }
        public double Duration { get; set; }
        public int TotalFinishedStudents { get; set; }
        public bool Active { get; set; }
    }
}