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
    public class DailyRecordsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public DailyRecordsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]

        public ActionResult<DailyRecordsModel> GetAll()
        {
            IEnumerable<DailyRecordsModel> objList = _db.DailyRecords;
            
            return Ok(objList);
        }

        [HttpGet("{Id}")]

        public ActionResult Get(int? Id)
        {
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            var obj = _db.DailyRecords.Find(Id);


            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost]

        public ActionResult Post([FromBody] List<DailyRecordsModel> dailyRecordsModel)
        {
            if (ModelState.IsValid)
            {

                var record = new DailyRecordsModel();
                var records = new List<DailyRecordsModel>(); 
                
                foreach (var item in dailyRecordsModel)
                {
                    item.Id = 0;
                    record.Quantity = item.Quantity;

                    record.Product = item.Product;
                    record.Measure = item.Measure;

                    var product = _db.Products.Where(u => u.Measure == record.Measure && u.Product == record.Product).FirstOrDefault().UnitPrice;
                    item.UnitPrice = product;
                    item.Amount = product * record.Quantity;
                    var productsQty = _db.ProductBalances.Where(u => u.Measure == record.Measure && u.Product == record.Product).Select(u => u.Id).FirstOrDefault();
                    var update = _db.ProductBalances.Find(productsQty);
                    update.Quantity = update.Quantity - record.Quantity;
                    //  var updateQty = update.Quantity - records.Quantity;

                    _db.ProductBalances.Update(update);

                    records.Add(item);
                }
                
                //   records.Quantity = dailyRecordsModel.Quantity;

                //   records.Product = dailyRecordsModel.Product;
                //    records.Measure = dailyRecordsModel.Measure;


                //var products = _db.Products.Where(u => u.Measure == records.Measure&& u.Product==records.Product).FirstOrDefault();
                //var item = _db.Products.Find(products);
                

                //if (item.Product == records.Product&& item.Measure==records.Measure)
                //{
                //    records.UnitPrice = item.UnitPrice;
                //    records.Amount = records.UnitPrice * records.Quantity;


                //}

              //  var productsQty = _db.ProductBalances.Where(u => u.Measure == records.Measure && u.Product == records.Product).Select(u => u.Id).FirstOrDefault();
              //  var update = _db.ProductBalances.Find(productsQty);
              //  update.Quantity= update.Quantity - records.Quantity;
              ////  var updateQty = update.Quantity - records.Quantity;

              //  _db.ProductBalances.Update(update);

                _db.DailyRecords.AddRange(records);
                _db.SaveChanges();
            }
            return Ok();
        }

        [HttpPut]

        public ActionResult Put(int Id, [FromBody] DailyRecordsModel dailyRecordsModel)
        {
            if (ModelState.IsValid)
            {
                var records = _db.DailyRecords.Find(Id);
                records.Measure = dailyRecordsModel.Measure;
                records.Quantity = dailyRecordsModel.Quantity;
                records.Amount = records.Quantity * records.UnitPrice;

                _db.DailyRecords.Update(records);
                _db.SaveChanges();
            }
            return Ok();
        }

        [HttpDelete]

        public ActionResult Delete(int Id)
        {
            var records = _db.DailyRecords.Find(Id);

            _db.DailyRecords.Remove(records);
            _db.SaveChanges();
            return Ok();
        }
    }
}
