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
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        RoleManager<IdentityRole> _roleManager;


        public RolesController(ApplicationDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
        }

        [HttpGet]

        public ActionResult<RolesModel> Get()
        {
            IEnumerable<RolesModel> objList = _db.Department;
            return Ok(objList);
        }

        [HttpGet("{Id}")]

        public ActionResult Get(int? Id)
        {
            if (Id == 0|| Id==null)
            {
                return NotFound();
            }
            var obj = _db.Department.Find(Id);

            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost]

        public async Task<IActionResult> Post([FromBody] RolesModelDTO rolesModel)
        {
            if (ModelState.IsValid)
            {
                var roles = new RolesModel()
                {
                    Department = rolesModel.Department,


                };

                _db.Department.Add(roles);
                await _roleManager.CreateAsync(new IdentityRole(roles.Department));
                _db.SaveChanges();
            }
            return Ok();
        }


        [HttpPut("{id:int}")]


        public async Task<IActionResult> Put(string Name, [FromBody] RolesModel rolesModel)
        {
            if (ModelState.IsValid)
            {
                 var departmentId = _db.Department.Find(rolesModel.Id);
                //var roleId =  _db.Roles.Where(u => u)
                //              .Select(u => u.Id).FirstOrDefault();
                departmentId.Department = rolesModel.Department;
                var roleId = await _roleManager.FindByNameAsync(Name);

                if (_db.Roles.Any(r => r.Name == Name))
                {
                    roleId.Name = rolesModel.Department;
                    await _roleManager.UpdateAsync(roleId);
                    // var manager = new RoleManager<IdentityRole>(Name);

                }

                _db.Department.Update(departmentId);
                _db.SaveChanges();

            }
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Name)
        {
            var departmentId = _db.Department.Find(Name);
            var roleId = await _roleManager.FindByNameAsync(Name);

            if (_db.Roles.Any(r => r.Name == Name) && departmentId != null)
            {
                await _roleManager.DeleteAsync(roleId);
                // var manager = new RoleManager<IdentityRole>(Name);
                _db.Department.Remove(departmentId);
                _db.SaveChanges();

            }
            else
            {
                return NotFound();
            }
            return Ok();

        }
    }
}
