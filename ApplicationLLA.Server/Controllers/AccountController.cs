using ApplicationLLA.Server.DBC;
using ApplicationLLA.Server.Dtos.Account;
using ApplicationLLA.Server.Helper;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLLA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly IWorkerRepository _workerRepo;
        private readonly ICustomerRepository _customerRepo;



        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IWorkerRepository workerRepo, ICustomerRepository customerRepo)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signInManager;
            _customerRepo = customerRepo;
            _workerRepo = workerRepo;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

            if (user == null) return Unauthorized("Username not found and/or password incorrect");

            var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

            var userRoles = await _userManager.GetRolesAsync(user);


            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    AccountType = userRoles.First(),
                    Token = _tokenService.CreateToken(user, userRoles)
                }
            );
        }
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                registerDto.Email = registerDto.Email.HandleSpaces();
                registerDto.Username = registerDto.Username.HandleSpaces();
                registerDto.Password = registerDto.Password.HandleSpaces();

                if (registerDto == null) return BadRequest("Register Object is not valid");

                if (!(registerDto.AccountType == "Customer" || registerDto.AccountType == "Worker"))
                {
                    return BadRequest("Account type can be just Customer or Worker");
                }

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };

                if (appUser == null) return BadRequest("There is something went wrong");

                if (registerDto.Password == null) { return BadRequest(); }
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (!createdUser.Succeeded) return StatusCode(500, createdUser.Errors);

                // Account Type Can be Just Customer Or Worker
                var roleResult = await _userManager.AddToRoleAsync(appUser, registerDto.AccountType);

                if (!roleResult.Succeeded) return StatusCode(500, roleResult.Errors);

                var userRoles = await _userManager.GetRolesAsync(appUser);

                if (userRoles == null) return BadRequest("Something went wrong");



                if (registerDto.AccountType == "Customer" && !(await _customerRepo.IsCustomerExistsAsync(appUser.Id.ToString())))
                {
                    var customer = await _customerRepo.CreateAsync(appUser.Id.ToString());
                    if (customer == null) return BadRequest();
                }
                else if (registerDto.AccountType == "Worker" && !(await _workerRepo.IsWorkerExistsAsync(appUser.Id.ToString())))
                {
                    var worker = await _workerRepo.CreateAsync(appUser.Id.ToString());
                    if (worker == null) return BadRequest();
                }
                else {
                    return BadRequest();
                }

                return Ok(
                    new NewUserDto
                    {
                        UserName = appUser.UserName,
                        Email = appUser.Email,
                        AccountType = userRoles.First(),
                        Token = _tokenService.CreateToken(appUser, userRoles)
                    }
                );
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
