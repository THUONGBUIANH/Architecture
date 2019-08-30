using Architecture_BE.Business.Services;
using Architecture_BE.Models.Dto;
using Architecture_BE.Models.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Architecture_BE.API.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(
            IUserService userService,
            IConfiguration config,
            IHttpContextAccessor _contextAccessor,
            ITokenService tokenService)
            : base(_contextAccessor, config)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterUserDto registerUserDto)
        {
            if (registerUserDto == null)
                return BadRequest(_config["Errors:Base:ModelInValid"]);

            //Validation model dto
            var validatorDto = new RegisterUserDtoValidator();

            var resultValidateDto = validatorDto.Validate(registerUserDto);

            if (!resultValidateDto.IsValid)
                return BadRequest(ValidationDto.GetMessagesByErrors(resultValidateDto.Errors));

            //Check user exists
            var userExists = await _userService.UserExists(registerUserDto.UserName);

            if (userExists)
                return BadRequest(_config["Errors:User:Exist"]);

            //Register user
            var isOk = await _userService.Register(registerUserDto);

            return Ok(isOk);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Edit(Guid id, [FromBody]EditUserDto editUserDto)
        {
            if (id == Guid.Empty || editUserDto == null)
                return BadRequest(_config["Errors:Base:ModelInValid"]);

            //Validation model dto
            var validatorDto = new EditUserDtoValidator();

            var resultValidateDto = validatorDto.Validate(editUserDto);

            if (!resultValidateDto.IsValid)
                return BadRequest(ValidationDto.GetMessagesByErrors(resultValidateDto.Errors));

            //Set Config
            SetConfig();

            //Edit user
            var isOk = await _userService.Edit(id, editUserDto);

            return Ok(isOk);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Active(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(_config["Errors:Base:ModelInValid"]);

            //Set Config
            SetConfig();

            //Active user
            var isOk = await _userService.Active(id);

            return Ok(isOk);
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            //Get all user
            var userDTOList = await _userService.GetUsers();

            return Ok(userDTOList);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginUserDto loginUserDto)
        {
            if (loginUserDto == null)
                return BadRequest(_config["Errors:Base:ModelInValid"]);

            // Validation model dto
            var validatorDto = new LoginUserDtoValidator();

            var resultValidateDto = validatorDto.Validate(loginUserDto);

            if (!resultValidateDto.IsValid)
                return BadRequest(ValidationDto.GetMessagesByErrors(resultValidateDto.Errors));

            // Verify Account
            var userDto = await _userService.VerifyAccount(loginUserDto);

            if (userDto == null)
                return BadRequest(_config["Errors:User:Incorrect"]);

            // Create Refresh Token
            RefreshTokenDto refreshTokenDto = await _tokenService.CreateRefreshTokenByUserDtoAsync(userDto);

            // Generate token
            var tokenDto = GenerateToken(userDto, refreshTokenDto.RefreshToken);

            return Ok(tokenDto);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(_config["Errors:Token:IsNullOrEmpty"]);

            // Verify Account
            var refreshTokenDto = await _tokenService.CheckRefreshTokenAsync(refreshToken);

            if (refreshTokenDto == null)
                return BadRequest(_config["Errors:Token:ValidOrExpired"]);

            // Get userDto by userId
            UserDto userDto = await _userService.GetUserActiveById(refreshTokenDto.UserId);

            if (userDto == null)
            {
                return BadRequest(_config["Errors:User:NotExist"]);
            }

            // Generate token
            var tokenDto = GenerateToken(userDto, refreshTokenDto.RefreshToken);

            return Ok(tokenDto);
        }

        private TokenDto GenerateToken(UserDto userDto, string refreshToken)
        {
            var claims = new[] 
            {
                new Claim(ClaimTypes.Name, userDto.UserName)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_config["Jwt:Expires"])),
                signingCredentials: credentials
            );

            return new TokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                TokenType = JwtBearerDefaults.AuthenticationScheme,
                RefreshToken = refreshToken,
                Expiration = token.ValidTo
            };
        }
    }
}
