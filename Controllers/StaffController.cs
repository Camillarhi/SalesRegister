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

        [Authorize]
        //Get all staffs
        [HttpGet("staff")]
        public async Task<ActionResult<StaffModelDTO>> GetAll()
        {
            try
            {
                var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
                //if (currentUser == null) return Challenge();
                var staffs =  _db.Users.Where(x => x.CreatedById == currentUser).ToList();
                         //     select new StaffModel
                         //     {
                         //         FirstName = user.FirstName,
                         //         LastName = user.LastName,
                         //         Gender = user.Gender,
                         //         UserName = user.UserName,
                         //         StaffId = user.StaffId,
                         //         Address = user.Address,
                         //         ProfilePicture = user.ProfilePicture,
                         //         DateOfBirth = user.DateOfBirth,
                         //         PhoneNumber = user.PhoneNumber
                         //     }
                         //);.ToList();
                

                //  var users = await _userManager.FindByIdAsync(staffs);
                return Ok(staffs);
            }

            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        //Get single staff
        [HttpGet("staff/{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            try
            {
                var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
                var staff = _db.Users.Where(u =>u.CreatedById == currentUser && u.Id == Id).FirstOrDefault();
                var role = await _userManager.GetRolesAsync(staff);
                if (staff == null)
                {
                    return NotFound();
                }
                //return _mapper.Map<StaffModelDTO>(users);
                return Ok(new { staff, role });
            }

            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        //Get admin Profile
        [HttpGet("admin/{Id}")]
        public async Task<IActionResult> GetAdmin(string Id)
        {
            try
            {
                var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
                var staff = _db.Users.Where(u => u.Id == currentUser && u.Id == Id).FirstOrDefault();
                var role=await _userManager.GetRolesAsync(staff);
                var company = _db.CompanyName.Where(x => x.AdminId == currentUser).FirstOrDefault();
                //var users = await _userManager.FindByIdAsync(staff);
                if (staff == null)
                {
                    return NotFound();
                }
                var profile = new {staff, role, company };
                //return _mapper.Map<StaffModelDTO>(users);
                return Ok(profile);
            }

            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        //admin login
        [HttpPost("createadmin")]
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
            var expiration = DateTime.UtcNow.AddMinutes(10);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiration, signingCredentials: creds);
            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                 Email= register.UserName,
                Expiration = expiration
            };

        }

        // setup admin
        [HttpPost("setupadmin")]

        public async Task<IActionResult> Register([FromForm] AdminFormDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var staff = _mapper.Map<StaffModel>(model);
                    var staff = new StaffModel();
                    var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                    var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
                    var newStaff = _db.Users.Where(u => u.UserName == model.UserName).Select(u => u.Id).FirstOrDefault();
                    var newStaff2 = await _userManager.FindByIdAsync(newStaff);
                    newStaff2.DateOfBirth = model.DateOfBirth;
                    newStaff2.FirstName = model.FirstName;
                    newStaff2.Gender = model.Gender;
                    newStaff2.LastName = model.LastName;
                    newStaff2.PhoneNumber = model.PhoneNumber;
                    newStaff2.Address = model.Address;
                    newStaff2.CreatedById = newStaff2.Id;
                    var user = new StaffModel();
                    var departmentName ="Admin".Substring(0, 3);
                    var rnd = new Random();
                    int num = rnd.Next(50);
                    newStaff2.StaffId = departmentName + num;
                    newStaff2.CreatedById = newStaff;

                    if (model.ProfilePicture != null)
                    {
                        newStaff2.ProfilePicture = await _fileStorageService.SaveFile(containerName, model.ProfilePicture);
                    }
                    string uniqueId = System.Guid.NewGuid().ToString();
                    var companyName = new CompanyModel()
                    {
                        Id = uniqueId,
                        AdminId = newStaff,
                        CompanyName = model.CompanyName
                    };
                    var result = await _userManager.UpdateAsync(newStaff2);
                    if (result.Succeeded)
                    {

                        await _userManager.AddToRoleAsync(newStaff2, model.Department);
                        _db.CompanyName.Add(companyName);
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
        //register staff
        [HttpPost("registerstaff")]

        public async Task<IActionResult> RegisterStaff([FromForm] StaffModelDTO model)

        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                    var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
                    var staff = new StaffModel
                    {
                        UserName = model.UserName,
                        Email = model.UserName,
                        DateOfBirth = model.DateOfBirth,
                        FirstName = model.FirstName,
                        Gender = model.Gender,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        CreatedById = currentUser
                    };
                    var getCompanyName = _db.CompanyName.Where(x => x.AdminId == currentUser).Select(x=>x.CompanyName).FirstOrDefault();
                    var companyName = getCompanyName.Substring(0, 3);
                    var departmentName = model.Department.Substring(0, 3);
                    var rnd = new Random();
                    int num = rnd.Next(50);
                    staff.StaffId = companyName + departmentName + num;
                    if (model.ProfilePicture != null)
                    {
                        staff.ProfilePicture = await _fileStorageService.SaveFile(containerName, model.ProfilePicture);
                    }
                    var result = await _userManager.CreateAsync(staff, model.Password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(staff, model.Department);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        return BadRequest(result.Errors);
                    }

                }
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        //edit Admin information
        [HttpPut("admin/{Id}")]
        public async Task<IActionResult> UpdateAdminProfile(string Id, [FromForm] AdminFormDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                    var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
                    //to get the Id of the staff from the database
                    var del = _db.Users
                           .Where(u => u.Id == Id)
                           .Select(u => u.Id)
                           .FirstOrDefault();
                    var company = _db.CompanyName.Where(x => x.AdminId == currentUser).FirstOrDefault();
                    company.CompanyName = model.CompanyName;
                    //get User Data from del (using the Id to get the column)
                    var users = await _userManager.FindByIdAsync(del);
                    users.DateOfBirth = model.DateOfBirth;
                    users.FirstName = model.FirstName;
                    users.LastName = model.LastName;
                    users.Gender = model.Gender;
                    users.PhoneNumber = model.PhoneNumber;
                    users.Address = model.Address;

                    if (model.ProfilePicture != null)
                    {
                        users.ProfilePicture = await _fileStorageService.SaveFile(containerName, model.ProfilePicture);
                    }
                    //update the column with the new information
                    var result = await _userManager.UpdateAsync(users);
                    var role = await _userManager.GetRolesAsync(users);
                    await _userManager.RemoveFromRolesAsync(users, role);
                    if (result.Succeeded)
                    {
                        _db.CompanyName.Update(company);
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

        //edit staff information
        [HttpPut("staff/{Id}")]
        public async Task<IActionResult> Update(string Id, [FromForm] StaffEditModelDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                    var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
                    //to get the Id of the staff from the database
                    var del = _db.Users
                           .Where(u => u.Id == Id)
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
                    if (model.ProfilePicture != null)
                    {
                        users.ProfilePicture = await _fileStorageService.SaveFile(containerName, model.ProfilePicture);
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

        //reset password
        [HttpPut("UpdateStaffLoginInfo")]
        public async Task<IActionResult> EditLoginInfo( [FromForm] RegisterModelDTO register)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                    var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
                    // var staff = new StaffModel();
                    var editPassword = _db.Users
                              .Where(u => u.UserName == register.UserName && u.CreatedById == currentUser)
                              .FirstOrDefault();
                  //  var users = _userManager.FindByIdAsync(editPassword);
                    //var result = await _userManager.UpdateAsync(register);
                    await _userManager.RemovePasswordAsync(editPassword);
                    await _userManager.AddPasswordAsync(editPassword, register.Password);
                    // await _userManager.AddPasswordAsync(staff, "newpassword");

                }
                return Ok();
            }

            catch (Exception ex)
            {

                return BadRequest(ex);
            }

        }

        //login
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
                        var user = await _userManager.FindByEmailAsync(login.Email);
                        var role = await _userManager.GetRolesAsync(user);
                        var logedIn = new AuthenticationResponse
                        {
                            Email = user.Email,
                            Id = user.Id,
                            Role = role[0]
                    };
                        return BuildToken(logedIn);
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

        private AuthenticationResponse BuildToken(AuthenticationResponse login)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", login.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyjwt"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddMinutes(20);
            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiration, signingCredentials: creds);
            return new AuthenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email= login.Email,
                Id=login.Id,
                Expiration = expiration,
                Role = login.Role
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
        public async Task<IActionResult> ResetPasswordForAdmin([FromForm] RegisterModelDTO reset)
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


        [HttpDelete("staff/{id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (string.IsNullOrEmpty(Id))
            {
                throw new ArgumentException($"'{nameof(Id)}' cannot be null or empty.", nameof(Id));
            }

            try
            {
                var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
                var del = _db.Users
                            .Where(u => u.CreatedById == currentUser && u.Id == Id)
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
