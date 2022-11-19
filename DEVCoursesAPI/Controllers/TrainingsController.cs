using DEVCoursesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using DEVCoursesAPI.Repositories;
using DEVCoursesAPI.Data.DTOs.TrainingDTO;
using Microsoft.AspNetCore.Authorization;
using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.Models;

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
        /// Busca todos os treinamentos
        /// </summary>
        /// <returns>Retorna lista de treinamentos</returns>
        /// <response code = "200">Retorna lista de treinamentos</response>
        /// <response code = "404">Nenhum treinamento encontrado</response>
        /// <response code = "500">Erro durante a execução</response>
        [HttpGet]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<Training>>> Get()
        {
            try
            {
                List<Training> trainings = await _repository.GetAll();

                _logger.LogInformation($"Controller: {nameof(TrainingsController)} - Method: {nameof(Get)}");

                return trainings.Any() ? Ok(trainings): NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Busca um treinamento pelo seu Id
        /// </summary>
        /// <param name="id">Id do treinamento</param>
        /// <returns>Retorna treinamento encontrado</returns>
        /// <response code = "200">Retorna o treinamento</response>
        /// <response code = "404">Nenhum treinamento encontrado com o Id fornecido</response>
        /// <response code = "500">Erro durante a execução</response>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ReadTrainingDto>> GetTrainingById(Guid id)
        {
            try
            {
                ReadTrainingDto? training = await _service.GetByIdAsync(id);

                _logger.LogInformation($"Controller: {nameof(TrainingsController)} - Method: {nameof(GetTrainingById)}");

                return training == null ? NotFound() : Ok(training);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Lista de todos os treinamentos
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Retorna os treinamentos de acordo com o Id do usuário inserido</returns>
        /// <response code = "200">Retorna Lista de Treinamentos</response>
        /// <response code = "500">Erro execução</response>
        [HttpGet("list/{userId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> GetTrainingsByUserId(Guid userId)
        {
            try
            {
                _logger.LogInformation($"Class:{nameof(TrainingsController)}-Method:{nameof(GetTrainingsByUserId)}");
                var trainings = _service.UserLoginTrainingsList(userId);

                if (trainings == null)
                    return NotFound();

                return Ok(trainings);

            }

            catch (Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(TrainingsController)}-Method:{nameof(GetTrainingsByUserId)}");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Concluir Treinamento
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="trainingId"></param>
        /// <returns>Retorna curso completado com sucesso</returns>
        /// <response code = "204">Concluído com sucesso</response>
        /// <response code = "400">Curso não pode ser concluído por que existem tópicos não concluídos</response>
        /// <response code = "404">Conclusão não realizada</response>
        /// <response code = "500">Erro execução</response>
        [Route("complete/{userId}/{trainingId}")]
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(Guid userId, Guid trainingId)
        {
            try
            {
                _logger.LogInformation($"Class:{nameof(TrainingsController)}-Method:{nameof(Put)}");

                var trainingUser = await _repository.GetTrainingUser(userId, trainingId);
                if (trainingUser == null) return StatusCode(404, $"Usuário {userId} não está matriculado no curso {trainingId}");

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
        [HttpDelete("registration/{userID}/{trainingID}")]
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

        /// <summary>
        /// Busca listas de usuários ativos e concluintes de um treinamento
        /// </summary>
        /// <param name="id">Id do treinamento</param>
        /// <returns>Retorna listas de usuários registrados no treinamento</returns>
        /// <response code = "200">Retorna as listas encontradas</response>
        /// <response code = "500">Erro durante a execução</response>
        [HttpGet("{trainingId}/registered-users/")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegisteredUsers>> GetUsersRegisteredInTraining(Guid trainingId)
        {
            try
            {
                RegisteredUsers result = await _service.GetUsersRegisteredInTraining(trainingId);

                _logger.LogInformation($"Controller: {nameof(TrainingsController)} - Method: {nameof(GetUsersRegisteredInTraining)}");

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Busca listas relatórios de treinamento
        /// </summary>
        /// <returns>Retorna lista relatórios de treinamento</returns>
        /// <response code = "200">Retorna lista de relatórios</response>
        /// <response code = "404">Nenhum treinamento encontrado</response>
        /// <response code = "500">Erro durante a execução</response>
        [HttpGet("report")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TrainingReport>>> GetTrainingReports()
        {
            try
            {
                List<TrainingReport> result = await _service.GetReports();

                return result.Any() ? Ok(result) : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
