using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;
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
        private readonly ITrainingRepository<Training> _repository;

        public TrainingsController(IOptions<TrainingsController> tokenSettings, 
            ILogger<UsersController> logger,
            ITrainingRepository<Training> repository)
        {
            _tokenSettings = tokenSettings;    
            _logger = logger;
            _repository = repository;
        }

        /// <summary>
        /// Concluir Treinamento
        /// </summary>
        /// <param></param>
        /// <returns>Retorna curso completado com sucesso</returns>
        /// <response code = "204">Concluído com sucesso</response>
        /// <response code = "404">Conclusão não realizada</response>
        /// <response code = "500">Erro execução</response>
        [Route("Complete")]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(Guid UserId, Guid TrainingId)
        {
            try
            {
                _logger.LogInformation($"Class:{nameof(TrainingsController)}-Method:{nameof(Put)}");

                var trainingUser = await _repository.GetTrainingUser(UserId, TrainingId);
                if (trainingUser == null) return StatusCode(404);

                var isCompletedTraining = await _repository.CompleteTraining(trainingUser);
                if (isCompletedTraining == false) return StatusCode(200); // precisa passar a mensagem

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

// Nem todos os contéudos foram concluídos. Favor concluir todas as atividades e aulas do curso para concluir o treinamento.

