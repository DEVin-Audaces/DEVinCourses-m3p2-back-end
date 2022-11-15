using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DEVCoursesAPI.Controllers
{
    [Route("trainings")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly ITrainingService _trainingService;
        private readonly ILogger<TrainingsController> _logger;

        public TrainingsController(ITrainingService trainingService, ILogger<TrainingsController> logger)
        {
            _trainingService = trainingService;
            _logger = logger;
        }


        /// <summary>
        /// Cancelar a matrícula
        /// </summary>
        /// <param name="registrationID"></param>
        /// <returns>Retorna se a matrícula foi cancelada ou não</returns>
        /// <response code = "204">Matrícula cancelada com sucesso</response> 
        /// <response code = "404">Matrícula não encontrada</response>
        /// <response code = "500">Ocorreu erro durante a execução</response> 
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteRegistration([FromBody] string userID, string trainingID, string[]topicsID)
        {
            try
            {
                var registration = _trainingService.DeleteRegistration(userID,trainingID,topicsID);
                if (!registration) return NotFound();
                _logger.LogInformation($"Controller: {nameof(TrainingsController)} - Método: {nameof(DeleteRegistration)}");

                return NoContent();

            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(TrainingsController)} - Method:{nameof(DeleteRegistration)}");
                return StatusCode(500);
            }
        }

    }
}
