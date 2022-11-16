using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;
using DEVCoursesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace DEVCoursesAPI.Controllers
{
    [Route("Trainings")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IOptions<TrainingsController> _tokenSettings;
        private readonly ITrainingRepository<Training> _repository;
        private readonly ITrainingService _service;

        public TrainingsController(IOptions<TrainingsController> tokenSettings,
            ILogger<UsersController> logger,
            ITrainingRepository<Training> repository,
            ITrainingService service)
        {
            _tokenSettings = tokenSettings;
            _logger = logger;
            _repository = repository;
            _service = service;
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
        [Route("Complete")]
        [HttpPut]
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
    }
}