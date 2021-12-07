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
    public class TotalController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public TotalController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]

        public ActionResult<TotalModel> GetAll()
        {
            IEnumerable<TotalModel> objList = _db.Totals;
            return Ok(objList);
        }

        [HttpGet("{Id}")]

        public ActionResult Get(int? Id)
        {
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            var total = _db.Totals.Find(Id);

            if (total == null)
            {
                return NotFound();
            }
            return Ok(total);
        }

        [HttpPost]

        public ActionResult Post([FromBody] TotalModelDTO totalModel)
        {
            if (ModelState.IsValid)
            {
                var total = new TotalModel()
                {
                    Date = DateTime.Now,
                    Total=totalModel.Total
                
                };
                _db.Totals.Add(total);
                _db.SaveChanges();
            }
            return Ok();
        }

        
        [HttpPut]

        public ActionResult Update(int Id, [FromBody] TotalModel totalModel)
        {
            if (ModelState.IsValid)
            {
                var total = _db.Totals.Find(Id);
                total.Date = totalModel.Date;
                total.Total = totalModel.Total;
                _db.Totals.Update(total);
                _db.SaveChanges();
            }
            return Ok();
        }

        [HttpDelete]

        public ActionResult Delete(int Id)
        {
            var total = _db.Totals.Find(Id);

            _db.Totals.Remove(total);
            _db.SaveChanges();
            return Ok();
        }
    }
}
