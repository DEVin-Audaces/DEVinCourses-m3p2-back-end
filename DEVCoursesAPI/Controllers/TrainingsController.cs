﻿using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Repositories;
using DEVCoursesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace DEVCoursesAPI.Controllers
{
    [Route("trainings")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IOptions<TrainingsController> _tokenSettings;
        private readonly ITrainingRepository _repository;
        private readonly ITrainingService _service;

        public TrainingsController(IOptions<TrainingsController> tokenSettings,
            ILogger<UsersController> logger,
            ITrainingRepository repository,
            ITrainingService service)
        {
            _service = service;
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
        [Route("complete")]
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
    }
}