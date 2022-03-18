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
    public class CustomerInvoiceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CustomerInvoiceController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]

        public ActionResult<CustomerInvoiceModel> GetAll()
        {
            IEnumerable<CustomerInvoiceModel> objList = _db.CustomerInvoice;
            return Ok(objList);
        }


        [HttpGet("{Id}")]

        public ActionResult Get(int? Id)
        {
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            var obj = _db.CustomerInvoice.Find(Id);

            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

       

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int Id)
        {
            var customer = _db.CustomerInvoice.Find(Id);

            _db.CustomerInvoice.Remove(customer);
            _db.SaveChanges();
            return Ok();

        }
    }
}
