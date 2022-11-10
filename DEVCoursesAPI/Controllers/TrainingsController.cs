using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DEVCoursesAPI.Controllers
{
    [Route("Trainings")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IOptions<TrainingsController> _tokenSettings;

        public TrainingsController(IOptions<TrainingsController> tokenSettings, ILogger<UsersController> logger)
        {
            _tokenSettings = tokenSettings;
            _logger = logger;
        }

    }
}
