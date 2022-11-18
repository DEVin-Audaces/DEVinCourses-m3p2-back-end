using DEVCoursesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using DEVCoursesAPI.Repositories;
using DEVCoursesAPI.Data.DTOs.TrainingDTO;
using Microsoft.AspNetCore.Authorization;

namespace DEVCoursesAPI.Controllers
{
    [Route("trainings")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly ILogger<TrainingsController> _logger;
        private readonly IOptions<TrainingsController> _tokenSettings;
        private readonly ITrainingRepository _repository;
        private readonly ITrainingService _service;
        public TrainingsController(
            IOptions<TrainingsController> tokenSettings,
            ILogger<TrainingsController> logger,
            ITrainingRepository repository,
            ITrainingService service)
        {
            _service = service;
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Visualizar a tela de treinamentos
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>Retorna os treinamentos de acordo com o Id do usuário inserido</returns>
        /// <response code = "200">Retorna Lista de Treinamentos</response>
        /// <response code = "500">Erro execução</response>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> Get([FromQuery][Required] Guid UserId)
        {
            try
            {
                _logger.LogInformation($"Class:{nameof(TrainingsController)}-Method:{nameof(Get)}");
                var trainings = _service.UserLoginTrainingsList(UserId);

                if (trainings == null)
                    return NotFound();

                return Ok(trainings);

            }

            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(TrainingsController)}-Method:{nameof(Get)}");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Concluir Treinamento
        /// </summary>
        /// <param></param>
        /// <returns>Retorna curso completado com sucesso</returns>
        /// <response code = "204">Concluído com sucesso</response>
        /// <response code = "400">Curso não pode ser concluído por que existem tópicos não concluídos</response>
        /// <response code = "404">Conclusão não realizada</response>
        /// <response code = "500">Erro execução</response>
        [Route("complete")]
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put([Required] Guid UserId, [Required] Guid TrainingId)
        {
            try
            {
                _logger.LogInformation($"Class:{nameof(TrainingsController)}-Method:{nameof(Put)}");

                var trainingUser = await _repository.GetTrainingUser(UserId, TrainingId);
                if (trainingUser == null) return StatusCode(404, $"Usuário {UserId} não está matriculado no curso {TrainingId}");

                var isCompletedTraining = await _service.CompleteTraining(trainingUser);
                if (!isCompletedTraining)
                    return StatusCode(400,
                        "Nem todos os contéudos foram concluídos. " +
                        "Favor concluir todas os tópicos do curso para completar o treinamento."
                        );

                return StatusCode(204);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(TrainingsController)} - Method:{nameof(Put)}");
                return StatusCode(500, e.Message);
            }

        }

        /// <summary>
        /// Cancelar a matrícula
        /// </summary>
        /// <param name="userID"> ID do usuário</param>
        /// <param name="trainingID"> ID do treinamento relacionado ao usuário</param>
        /// <param name="topicsID">IDs dos tópicos do treinamento relacionado ao usuário</param>
        /// <returns>Retorna se a matrícula foi cancelada ou não</returns>
        /// <response code = "204">Matrícula cancelada com sucesso</response> 
        /// <response code = "404">Matrícula não encontrada</response>
        /// <response code = "500">Ocorreu erro durante a execução</response> 
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteRegistration(Guid userID, Guid trainingID, [FromBody] Guid[] topicsID)
        {
            try
            {
                var registration = await _service.DeleteRegistration(userID, trainingID, topicsID);
                _logger.LogInformation($"Controller: {nameof(TrainingsController)} - Método: {nameof(DeleteRegistration)}");
                if (!registration) return NotFound();
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(TrainingsController)} - Method:{nameof(DeleteRegistration)}");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Criar matrícula
        /// </summary>
        /// <param name="trainingRegistrationDto"> DTO com ID do usuário, treinamento e tópicos</param>
        /// <returns>Retorna informando se a matrícula foi realizada</returns>
        /// <response code = "201">Matrícula realizada com sucesso</response>
        /// <response code = "400">Inserção não realizada, pois treinamento inativo</response>
        /// <response code = "500">Ocorreu erro durante a execução</response> 
        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTrainingRegistration([FromBody] TrainingRegistrationDto trainingRegistrationDto)
        {
            try
            {
                bool trainingStatus = await _service.CreateTrainingRegistrationAsync(trainingRegistrationDto);
                if (trainingStatus == false) return BadRequest("Treinamento inativo");

                _logger.LogInformation($"Controller: {nameof(TrainingsController)} - Method: {nameof(CreateTrainingRegistration)}");

                return CreatedAtAction("get", "matrícula realizada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
