using System.Collections.Specialized;

namespace DEVCoursesAPI.Services
{
    public interface ITrainingService
    {
        public bool DeleteRegistration(string userID, string trainingID, string[] topicsID);
    }
}
