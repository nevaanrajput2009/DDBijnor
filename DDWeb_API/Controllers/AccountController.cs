using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DD_Common;
using DD_DataAccess;
using DD_Models;
using DDWeb_API.Helper;
using DD_Business.Repository.IRepository;

namespace DDWeb_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly APISettings _aPISettings;
        private readonly IUserRepository _userRepository;


        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<APISettings> options,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager=roleManager;
            _aPISettings= options.Value;
            _userRepository= userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDTO signUpRequestDTO)
        {
            if(signUpRequestDTO==null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new ApplicationUser
            {
                UserName = signUpRequestDTO.Name,
                // for now we dont need emailid
                Email = signUpRequestDTO.Name,
                Name = signUpRequestDTO.Name,
                PhoneNumber = signUpRequestDTO.PhoneNumber,
                EmailConfirmed = true
            };

            var existingUser = await _userManager.FindByNameAsync(signUpRequestDTO.Name);
            if (existingUser != null)
            {
                return BadRequest(new SignUpResponseDTO
                {
                    IsRegisterationSuccessful = false,
                    Errors = new List<string>() { "username already exist"}
                }) ;
            }


            var result = await _userManager.CreateAsync(user,signUpRequestDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new SignUpResponseDTO()
                {
                    IsRegisterationSuccessful=false,
                    Errors= result.Errors.Select(u => u.Description)
                });
            }

            var roleResult = await _userManager.AddToRoleAsync(user, SD.Role_Customer);
            if (!roleResult.Succeeded)
            {
                return BadRequest(new SignUpResponseDTO()
                {
                    IsRegisterationSuccessful=false,
                    Errors= result.Errors.Select(u => u.Description)
                });
            }
            return StatusCode(201);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDTO signInRequestDTO)
        {
            if (signInRequestDTO==null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            
            var result = await _signInManager.PasswordSignInAsync(signInRequestDTO.UserName,signInRequestDTO.Password,false,false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(signInRequestDTO.UserName);
                if (user==null)
                {
                    return Unauthorized(new SignInResponseDTO
                    {
                        IsAuthSuccessful = false,
                        ErrorMessage = "Invalid Authentication"
                    });
                }

                //everything is valid and we need to login 
                var signinCredentials = GetSigningCredentials();
                var claims = await GetClaims(user);

                var tokenOptions = new JwtSecurityToken(
                    issuer: _aPISettings.ValidIssuer,
                    audience: _aPISettings.ValidAudience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: signinCredentials);

                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles == null || !userRoles.Any())
                {
                    throw new ArgumentNullException(nameof(userRoles));
                }

                bool isAdmin = userRoles.Any(x => x == SD.Role_Admin);

                return Ok(new SignInResponseDTO()
                {
                    IsAuthSuccessful=true,
                    Token = token,
                    UserDTO = new UserDTO()
                    {
                        Name = user.Name,
                        Id = user.Id,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        IsAdmin = isAdmin
                    }
                });

            }
            else
            {
                return Unauthorized(new SignInResponseDTO
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid Authentication"
                });
            }
           
            return StatusCode(201);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserAddress(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var userAddress = await _userRepository.GetUserAddress(userId);
            if (userAddress == null)
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(userAddress);
        }

        [HttpPost()]
        public async Task<IActionResult> UpdateUserAddress([FromBody] UserAddressDTO userAdressDTO)
        {
            if (string.IsNullOrEmpty(userAdressDTO.UserId))
            {
                return BadRequest(new ErrorModelDTO()
                {
                    ErrorMessage = "Invalid Id",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }

            var userAddress = await _userRepository.GetUserAddress(userAdressDTO.UserId);
            if (userAddress == null || userAddress.Id == default(int))
            {
                await _userRepository.CreateAddress(userAdressDTO);
            }
            else
            {
                userAdressDTO.Id = userAddress.Id;
                await _userRepository.UpdateAddress(userAdressDTO);
            }
            return Ok(userAddress);
        }


        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_aPISettings.SecretKey));

            return new SigningCredentials(secret,SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Email),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("Id",user.Id)
            };

            var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
