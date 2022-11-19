namespace DEVCoursesAPI.Data.DTOs
{
    public class RegisteredUsers
    {
        public List<string> ActiveUsers { get; set; }
        public List<string> FinishedUsers { get; set; }

        public RegisteredUsers()
        {
            ActiveUsers= new List<string>();
            FinishedUsers = new List<string>();
        }
    }
}
