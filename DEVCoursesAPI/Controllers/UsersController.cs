using System.Text.Json;
using DEVCoursesAPI.Data.DTOs;
using DEVCoursesAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        /// <param name="DataUser"></param>
        /// <returns>Retorna Id do usuário inserido</returns>
        /// <response code = "201">Usuário inserido com sucesso</response>
        /// <response code = "400">Inserção não realizada</response>
        /// <response code = "500">Erro execução</response>
        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateUser([FromBody] DataUser user)
        {
            Guid idUser = _usersService.Add(user);
            _logger.LogInformation($"Controller: {nameof(UsersController)} - Método: {nameof(CreateUser)} - Id: {idUser}");
            return StatusCode(201, $"{idUser}"); ;

        }
        
            
        /// <summary>
        /// Login de usuário
        /// </summary>
        /// <param name="LoginUser"></param>
        /// <returns>Retorna token e data de expiração do Token</returns>
        /// <response code="200">Login efetuado com sucesso</response>
        ///  <response code="500">Erro ao efetuar o login</response>
        [HttpPost("LoginUser")]
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

        /// <summary>
        /// Atualizar usuário
        /// </summary>
        /// <param name="DataUser"></param>
        /// <returns>Retorna a situação se usuário foi atualizado</returns>
        /// <response code = "200">Usuário atualizado com sucesso</response>
        /// <response code = "500">Erro execução</response>
        [HttpPut("UpdateUser")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUser([FromBody] DataUser user)
        {
            
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            Guid id = _usersService.GetIdToken(authHeader);     
            
            bool updateUser = _usersService.Update(user, id);
            _logger.LogInformation($"Controller: {nameof(UsersController)} - Método: {nameof(UpdateUser)} - Atualizado: {updateUser}");
            return StatusCode(200, JsonSerializer.Serialize(updateUser));
        }


/// <summary>
    /// Upload de foto
    /// </summary>
    /// <param name="UploadIImgUser"></param>
    /// <returns>Retorna se foi efetuado com sucesso o upload</returns>
    /// <response code = "200">foto atualizada com sucesso</response>
    /// <response code = "500">Erro execução</response>
    [HttpPut("UploadImgUser")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UploadImgUser([FromForm] UploadImgUser model)
    {
        var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
        _logger.LogInformation($"Controller: {nameof(UsersController)} - Método: {nameof(UploadImgUser)} - authHeader: {authHeader}");

        Guid id = _usersService.GetIdToken(authHeader);     
        _logger.LogInformation($"Controller: {nameof(UsersController)} - Método: {nameof(UploadImgUser)} - id: {id}");

        bool uploadImg = _usersService.UploadImg(model, id);
        _logger.LogInformation($"Controller: {nameof(UsersController)} - Método: {nameof(UploadImgUser)} - Atualizou: {uploadImg}");
        return StatusCode(200, JsonSerializer.Serialize(uploadImg));
    }

    // <summary>
    /// Retorna informações do usuário 
    /// </summary>
    /// <returns>Retorna as informações do usuário</returns>
    /// <response code="200">Retorna usuário</response>
    /// <response code="500">Ocorreu erro durante a execução</response>
    [HttpGet("UserProfile")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UserProfile()
    {
        var authHeader = HttpContext.Request.Headers["authorization"].ToString();
        Guid id = _usersService.GetIdToken(authHeader);
 
        ProfileUser user = _usersService.Get(id);
        _logger.LogInformation($"Controller: {nameof(UsersController)} - Método: {nameof(UserProfile)} - user: {user}");
        return StatusCode(200, user); 
    }
}