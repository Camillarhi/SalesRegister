//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using SalesRegister.ApplicationDbContex;
//using SalesRegister.DTOs;
//using SalesRegister.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace SalesRegister.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CustomerInvoiceController : ControllerBase
//    {
//        private readonly ApplicationDbContext _db;

//        public CustomerInvoiceController(ApplicationDbContext db)
//        {

//            _db = db;
//        }

//        [HttpGet]

//        public ActionResult<CustomerInvoiceModel> GetAll()
//        {
//            IEnumerable<CustomerInvoiceModel> objList = _db.CustomerInvoice;
//            return Ok(objList);
//        }


//        [HttpGet("{Id}")]

//        public ActionResult Get(int? Id)
//        {
//            if (Id == 0 || Id == null)
//            {
//                return NotFound();
//            }
//            var obj = _db.CustomerInvoice.Find(Id);
//            var customerOrder = _db.CustomerInvoiceDetails.Where(u => u.InvoiceId == obj.Id).Select(u => u.Id).ToList();
            

//            if (obj == null)
//            {
//                return NotFound();
//            }
//            return Ok(new {obj=obj, customerOrder=customerOrder });
//        }

//        [HttpPost]

//        public ActionResult Post([FromBody] CustomerInvoiceModelDTO customerInvoiceModel)
//        {
//            if (ModelState.IsValid)
//            {
//                var customer = new CustomerInvoiceModel();

//                customer.CustomerName = customerInvoiceModel.CustomerName;
//                   customer.Date = DateTime.Now;
                

//                _db.CustomerInvoice.Add(customer);

//                var total = _db.CustomerInvoiceDetails.Where(u => u.InvoiceId == customer.Id).Select(u => u.Amount).ToList();
//                //var invoiceId = _db.CustomerInvoiceDetails.Find(total);
//                customer.Total = total.Sum();

//                _db.CustomerInvoice.Update(customer);

//                _db.SaveChanges();
//            }
//            return Ok();
//        }


//        [HttpPut("{id:int}")]


//        public ActionResult Put(int Id, [FromBody] CustomerInvoiceModel customerInvoiceModel)
//        {
//            if (ModelState.IsValid)
//            {
//                var customer = _db.CustomerInvoice.Find(Id);
//                customer.CustomerName = customerInvoiceModel.CustomerName;
//                customer.Date = DateTime.Now;
//                var total = _db.CustomerInvoiceDetails.Where(u => u.InvoiceId == customer.Id).Select(u => u.Amount).ToList();
//                //var invoiceId = _db.CustomerInvoiceDetails.Find(total);
//                customer.Total = total.Sum();

//                _db.CustomerInvoice.Update(customer);
//                _db.SaveChanges();

//            }
//            return Ok();
//        }

//        [HttpDelete("{id:int}")]
//        public ActionResult Delete(int Id)
//        {
//            var customer = _db.CustomerInvoice.Find(Id);

//            _db.CustomerInvoice.Remove(customer);
//            _db.SaveChanges();
//            return Ok();

//        }
//    }
//}
