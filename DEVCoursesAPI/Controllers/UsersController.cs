using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Data.Models;
using DEVCoursesAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DEVCoursesAPI.Controllers;

[Route("users")]
[ApiController]
public class UsersController : ControllerBase
{
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService, ILogger<UsersController> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }


        /// <summary>
        /// Inserir usuário
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Retorna Id do usuário inserido</returns>
        /// <response code = "201">Usuário inserido com sucesso</response>
        /// <response code = "400">Inserção não realizada</response>
        /// <response code = "500">Erro execução</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] DataUser user)
        {
            try
            {
                return StatusCode(201); ;

            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Controller:{nameof(UsersController)} - Method:{nameof(Post)}");
                return StatusCode(500, e.Message);
            }

        }
        
            
        /// <summary>
        /// Login de usuário
        /// </summary>
        /// <param name="LoginUser"></param>
        /// <returns>Retorna token e data de expiração do Token</returns>
        /// <response code="200">Login efetuado com sucesso</response>
        ///  <response code="500">Erro ao efetuar o login</response>
        [HttpGet("LoginUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> LoginUser([FromBody] LoginUser loginUser)
        {
            JWTResult jwtResult = _usersService.AuthUser(loginUser);
            _logger.LogInformation($"Controller: {nameof(UsersController)} - Método: {nameof(LoginUser)} - JwtResult: {jwtResult}");
                
            return StatusCode(200, jwtResult); ;

        }

        /// <summary>
        /// Resetar senha do usuário
        /// </summary>
        /// <param name="LoginUser"></param>
        /// <returns>Retorna se foi atualizado a senha no banco</returns>
        /// <response code="200">Senha resetada com sucesso</response>
        /// <response code="500">Erro ao efetuar a reinicialização da senha</response>
        [HttpPut("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ResetPassword([FromBody] LoginUser loginUser)
        {
            bool updatePassword = _usersService.ResetPassword(loginUser);
            _logger.LogInformation($"Controller: {nameof(UsersController)} - Método: {nameof(ResetPassword)} - UpdatePassword: {updatePassword}");

            return StatusCode(200, updatePassword); ;

        }



}