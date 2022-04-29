using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesRegister.ApplicationDbContex;
using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SalesRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockBalanceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        UserManager<StaffModel> _userManager;
        public StockBalanceController(ApplicationDbContext db, UserManager<StaffModel> userManager
           )
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<StockBalanceModel>> GetAll()
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.CreatedById).FirstOrDefault();
            IEnumerable<StockBalanceModel> objList = _db.StockBalances.Where(x => x.AdminId == currentUser);
            return Ok(objList);
        }
    }
}
