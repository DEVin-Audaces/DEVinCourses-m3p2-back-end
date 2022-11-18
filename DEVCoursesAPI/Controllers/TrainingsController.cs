using DEVCoursesAPI.Data.Context;
using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Services;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DEVCoursesAPI.Repositories;

namespace DEVCoursesAPI.Controllers
{
    [Route("Trainings")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly ILogger<TrainingsController> _logger;
        private readonly IOptions<TrainingsController> _tokenSettings;
        private readonly ITrainingService _trainingService;


        public TrainingsController(IOptions<TrainingsController> tokenSettings, 
            ILogger<TrainingsController> logger, ITrainingService trainingService)
        {
            _tokenSettings = tokenSettings;
            _logger = logger;
            _trainingService = trainingService;
        }

        /// <summary>
        /// Visualizar a tela de treinamentos
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns>Retorna os treinamentos de acordo com o Id do usuário inserido</returns>
        /// <response code = "200">Retorna Lista de Treinamentos</response>
        /// <response code = "500">Erro execução</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> Get([FromQuery] [Required] Guid UserId)
        {

            try
            {
                _logger.LogInformation($"Class:{nameof(TrainingsController)}-Method:{nameof(Get)}");
                var trainings = _trainingService.UserLoginTrainingsList(UserId);
                
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
    }
}
