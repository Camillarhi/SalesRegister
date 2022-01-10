using Microsoft.AspNetCore.Http;
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
    public class ProductBalanceUpdateController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductBalanceUpdateController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]

        public ActionResult<ProductBalanceUpdateModelDTO> GetAll()
        {
            IEnumerable<ProductBalanceUpdateModel> objList = _db.ProductBalanceUpdates;

            return Ok(objList);
        }


        [HttpGet("{Id}")]

        public ActionResult Get(int? Id)
        {
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            var obj = _db.ProductBalanceUpdates.Find(Id);


            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        //[HttpPost]

        //public ActionResult Post([FromBody] DailyRecordsModelDTO dailyRecordsModel)
        //{
        //    if (ModelState.IsValid)
        //    {

        //    }

        //}


    }
}
