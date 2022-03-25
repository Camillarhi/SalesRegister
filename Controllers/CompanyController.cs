using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesRegister.ApplicationDbContex;
using SalesRegister.DTOs;
using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        UserManager<StaffModel> _userManager;
        public CompanyController(ApplicationDbContext db, UserManager<StaffModel> userManager
            )
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<CompanyModel>> GetAll()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            IEnumerable<CompanyModel> objList = _db.CompanyName.Where(x => x.AdminId == currentUser.Id);
            return Ok(objList);
        }


        [HttpGet("{Id}")]

        public async Task<ActionResult> Get(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(User);
            var obj = _db.CompanyName.Where(x => x.AdminId == currentUser.Id && x.Id == Id).FirstOrDefault();
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }


        [HttpPost]

        public async Task<ActionResult> Post([FromBody] CompanyModelDTO companyModel)
        {
            if (ModelState.IsValid)
            {
                string uniqueId = System.Guid.NewGuid().ToString();
                var currentUser = await _userManager.GetUserAsync(User);
                var companyName = new CompanyModel()
                {
                    Id = uniqueId,
                    AdminId = currentUser.Id,
                    CompanyName = companyModel.CompanyName
                };
                if (companyName == null)
                {
                    return BadRequest();
                }
                _db.CompanyName.Add(companyName);
                _db.SaveChanges();
            }
            return Ok();
        }


        [HttpPut("{id}")]


        public async Task<ActionResult> Put(string Id, [FromBody] CompanyModel companyModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var companyName = _db.CompanyName.Where(x => x.AdminId == currentUser.Id && x.Id == Id).FirstOrDefault();
                companyName.CompanyName = companyModel.CompanyName;
                _db.CompanyName.Update(companyName);
                _db.SaveChanges();

            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string Id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var companyName = _db.CompanyName.Where(x => x.AdminId == currentUser.Id && x.Id == Id).FirstOrDefault();
            _db.CompanyName.Remove(companyName);
            _db.SaveChanges();
            return Ok();

        }

    }
}
