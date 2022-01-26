using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SalesRegister.ApplicationDbContex;
using SalesRegister.DTOs;
using SalesRegister.HelperClass;
using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace SalesRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly IConfiguration _configuration; 
        private readonly string containerName = "Staff";
        UserManager<StaffModel> _userManager;
        SignInManager<StaffModel> _signInManager;
        RoleManager<IdentityRole> _roleManager;


        public StaffController(ApplicationDbContext db, IFileStorageService fileStorageService, IMapper mapper, UserManager<StaffModel> userManager, 
       SignInManager<StaffModel> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _configuration = configuration;
        }



        [HttpGet]
        public ActionResult<StaffModelDTO> GetAll()
        {
            try
            {
                var staffs = (from user in _db.Users
                              select new StaffModel
                              {
                                  FirstName = user.FirstName,
                                  LastName = user.LastName,
                                  Gender=user.Gender,
                                  UserName=user.UserName,
                                  StaffId=user.StaffId,
                                  Address=user.Address,
                                  ProfilePicture=user.ProfilePicture,
                                  DateOfBirth=user.DateOfBirth,
                                  PhoneNumber=user.PhoneNumber
                                  
                                  
                              }
                         ).ToList();
                

                //  var users = await _userManager.FindByIdAsync(staffs);
                return Ok(staffs);
            }

            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            try
            {
                var staff = _db.Users.Where(u => u.StaffId == Id).Select(u => u.Id).FirstOrDefault();

                var users = await _userManager.FindByIdAsync(staff);


                if (staff == null)
                {
                    return NotFound();
                }
                //return _mapper.Map<StaffModelDTO>(users);
                return Ok(users);
            }

            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }


        [HttpPost("createUser")]
        public async Task<ActionResult<AuthenticationResponse>> CreateUser([FromBody] RegisterModelDTO register)
        {
            if (ModelState.IsValid)
            {
                var staff = new StaffModel
                {
                    UserName = register.UserName,
                    Email = register.UserName
                };

                var result = await _userManager.CreateAsync(staff, register.Password);
                if (result.Succeeded)
                {
                    return BuildToken(register);
                }
                else
                {
                    return BadRequest(result.Errors);
                }

            }
           
            return Ok();

        }
        private AuthenticationResponse BuildToken(RegisterModelDTO register)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", register.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration ["keyjwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddYears(1);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiration, signingCredentials: creds);
            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };

        }


        [HttpPost("register")]

        public async Task<IActionResult> Register([FromForm] StaffModelDTO model)

        {
            try
            {
                if (ModelState.IsValid)
                {
                    var staff = _mapper.Map<StaffModel>(model);

                    var newStaff = _db.Users.Where(u => u.UserName == model.UserName).Select(u => u.Id).FirstOrDefault();
                    var newStaff2 = await _userManager.FindByIdAsync(newStaff);
                    newStaff2.DateOfBirth = model.DateOfBirth;
                    newStaff2.FirstName = model.FirstName;
                    newStaff2.Gender = model.Gender;
                    newStaff2.LastName = model.LastName;
                    newStaff2.PhoneNumber = model.PhoneNumber;
                    newStaff2.Address = model.Address;
                   

                    var getCompanyName = _db.CompanyName.FirstOrDefault();
                    var companyName = getCompanyName.CompanyName.Substring(0, 3);

                    StaffModel user = new StaffModel();


                    var departmentName = model.Department.Substring(0, 3);
                    var rnd = new Random();
                    int num = rnd.Next(50);


                    newStaff2.StaffId = companyName + departmentName + num;


                    if (model.ProfilePicture != null)
                    {
                        newStaff2.ProfilePicture = await _fileStorageService.SaveFile(containerName, model.ProfilePicture);
                    }

                    var result = await _userManager.UpdateAsync(newStaff2);
                    if (result.Succeeded)
                    {

                        await _userManager.AddToRoleAsync(newStaff2, model.Department);
                        await _db.SaveChangesAsync();

                    }

                }
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }



        [HttpPut("updateStaffInfo")]
        public async Task<IActionResult> Update(string Id, [FromForm] StaffModelDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   var staff = _mapper.Map<StaffModel>(model);

                    var getCompanyName = _db.CompanyName.FirstOrDefault();
                    var companyName = getCompanyName.CompanyName.Substring(0, 3);



                    var departmentName = model.Department.Substring(0, 3);
                    var rnd = new Random();
                    int num = rnd.Next(50);


                    //to get the Id of the staff from the database
                    var del = _db.Users
                           .Where(u => u.StaffId == Id)
                           .Select(u => u.Id)
                           .FirstOrDefault();


                    //get User Data from del (using the Id to get the column)
                    var users = await _userManager.FindByIdAsync(del);

                    users.DateOfBirth = model.DateOfBirth;
                    users.FirstName = model.FirstName;
                    users.LastName = model.LastName;
                    users.Gender = model.Gender;
                    users.PhoneNumber = model.PhoneNumber;
                    users.Address = model.Address;
                    
                    users.StaffId = companyName + departmentName + num;



                    if (model.ProfilePicture != null)
                    {
                        staff.ProfilePicture = await _fileStorageService.SaveFile(containerName, model.ProfilePicture);
                    }

                    //update the column with the new information
                    var result = await _userManager.UpdateAsync(users);
                    var role = await _userManager.GetRolesAsync(users);

                    await _userManager.RemoveFromRolesAsync(users, role);



                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(users, model.Department);
                        await _db.SaveChangesAsync();
                    }

                }
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }


        [HttpPut("UpdateStaffLoginInfo")]
        public async Task<IActionResult> EditLoginInfo(string Email, [FromForm] RegisterModelDTO register)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new RegisterModel();
                    var staff = new StaffModel();

                    var editPassword = _db.Users
                              .Where(u => u.UserName == Email)
                              .Select(u => u.Id)
                              .FirstOrDefault();


                    var users = _userManager.FindByIdAsync(editPassword);



                    //var result = await _userManager.UpdateAsync(register);

                    await _userManager.RemovePasswordAsync(staff);
                    await _userManager.AddPasswordAsync(staff, "newpassword");




                }
                return Ok();
            }

            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }


        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login([FromBody] LoginModelDTO login)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password,isPersistent:false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return BuildToken(login);
                    }
                    else
                    {
                        return BadRequest(new { Error = "Invalid Username or Password" });
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private AuthenticationResponse BuildToken(LoginModelDTO login)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", login.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyjwt"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddYears(1);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiration, signingCredentials: creds);
            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };

        } 


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }



        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("reset-password-admin")]
        public async Task<IActionResult> resetPasswordForAdmin([FromForm] RegisterModelDTO reset)
        {
            var user = await _userManager.FindByEmailAsync(reset.UserName);
            if (user == null)
            {
                return NotFound();
            }
            if (string.Compare(reset.Password, reset.ConfirmPassword) != 0)
            {
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, reset.Password);

            if (!result.Succeeded)
            {
                var errors = new List<string>();

                foreach (var error in result.Errors)
                {
                    errors.Add(error.Description);
                }
                return StatusCode(StatusCodes.Status500InternalServerError);

            }
            await _userManager.DeleteAsync(user);
            await _db.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new ArgumentException($"'{nameof(Id)}' cannot be null or empty.", nameof(Id));
            }

            try
            {


                var del = _db.Users
                            .Where(u => u.StaffId == Id)
                            .Select(u => u.Id)
                            .FirstOrDefault();


                //get User Data from del
                 var user = await _userManager.FindByIdAsync(Id);



                if (del == null)
                {
                    return NotFound();
                }

                await _userManager.DeleteAsync(user);
                await _db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex);



            }
        }

    }
}
