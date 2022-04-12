using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SalesRegister.ApplicationDbContex;
using SalesRegister.DTOs;
using SalesRegister.HelperClass;
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
        UserManager<StaffModel> _userManager;
        public DailyRecordsController(ApplicationDbContext db, UserManager<StaffModel> userManager
            )
        {
            _db = db;
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<ActionResult<DailyRecordsModel>> GetAll()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            IEnumerable<DailyRecordsModel> objList = _db.DailyRecords.Where(x => x.AdminId == currentUser.Id);
            return Ok(objList);
        }

        [HttpGet("{Id}")]

        public async Task<ActionResult> Get(int? Id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            var obj = _db.DailyRecords.Where(x => x.AdminId == currentUser.Id && x.Id == Id).FirstOrDefault();
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }


        [HttpPost]

        public async Task<ActionResult> PostInvoice([FromBody] CustomerInvoiceModelDTO customerInvoiceModel)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var record = new CustomerInvoiceModel();
                var records = new List<DailyRecordsModel>();
                var totAmt = new List<float>();
                record.AdminId = currentUser.Id;
                record.CustomerName = customerInvoiceModel.CustomerName;
                record.Date = DateTime.Now;
                var custName =customerInvoiceModel.CustomerName.Substring(0, 3);
                var rnd = new Random();
                int num = rnd.Next(100);
                var getCompanyName = _db.CompanyName.FirstOrDefault();
                var companyName = getCompanyName.CompanyName.Substring(0, 3);
                record.InvoiceId = companyName + custName + num;

                foreach (var item in customerInvoiceModel.InvoiceDetail)
                {
                    var recordDetail = new CustomerInvoiceDetailModel();
                    recordDetail.AdminId = record.AdminId;
                    recordDetail.Quantity = item.Quantity;
                    recordDetail.Product = item.Product;
                    recordDetail.Date = record.Date;
                    recordDetail.InvoiceId = record.InvoiceId;
                    var product = _db.Products.Find(item.ProductId);
                    var unitPrice = product.ProductMeasures.Find(x => x.Id == item.MeasureId && x.ProductId == item.ProductId).UnitPrice;
                    //var unitPrice = _db.Products.Where(u => u.Measure == recordDetail.Measure && u.Product == recordDetail.Product).FirstOrDefault().UnitPrice;
                    recordDetail.UnitPrice = unitPrice;
                    recordDetail.Amount = unitPrice * recordDetail.Quantity;
                    var productsQty = _db.StockBalances.Where(u => u.Measure == recordDetail.Measure && u.Product == recordDetail.Product).Select(u => u.Id).FirstOrDefault();
                    var update = _db.StockBalances.Find(productsQty);
                    update.Quantity -= recordDetail.Quantity;

                    _db.StockBalances.Update(update);
                    record.InvoiceDetail.Add(recordDetail);
                }
                for (var i = 0; i < record.InvoiceDetail.Count; i++)
                {
                    totAmt.Add(record.InvoiceDetail[i].Amount);
                }
                record.Total = totAmt.Sum();
                foreach (var item in customerInvoiceModel.InvoiceDetail)
                {
                    var dailySales = new DailyRecordsModel();
                    dailySales.AdminId = record.AdminId;
                    dailySales.Quantity = item.Quantity;
                    dailySales.Product = item.Product;
                    dailySales.Measure = item.Measure;
                    dailySales.Date = DateTime.Now;
                    var product = _db.Products.Find(item.ProductId);
                    var unitPrice = product.ProductMeasures.Find(x => x.Id == item.MeasureId && x.ProductId == item.ProductId).UnitPrice;
                    dailySales.UnitPrice = unitPrice;
                    dailySales.Amount = unitPrice * dailySales.Quantity;
                    records.Add(dailySales);

                }
                _db.CustomerInvoice.AddRange(record);
                _db.DailyRecords.AddRange(records);
                _db.SaveChanges();
            }
            return Ok();
        }

        //[HttpPost]

        //public ActionResult Post([FromBody] List<DailyRecordsModel> dailyRecordsModel)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var record = new DailyRecordsModel();
        //        var records = new List<DailyRecordsModel>(); 
                
        //        foreach (var item in dailyRecordsModel)
        //        {
        //            record.Quantity = item.Quantity;
        //            record.Product = item.Product;
        //            record.Measure = item.Measure;
        //            record.Date = DateTime.Now;
        //            var product = _db.Products.Where(u => u.Measure == record.Measure && u.Product == record.Product).FirstOrDefault().UnitPrice;
        //            item.UnitPrice = product;
        //            item.Amount = product * record.Quantity;
        //            var productsQty = _db.ProductBalances.Where(u => u.Measure == record.Measure && u.Product == record.Product).Select(u => u.Id).FirstOrDefault();
        //            var update = _db.ProductBalances.Find(productsQty);
        //            update.Quantity = update.Quantity - record.Quantity;
        //            //  var updateQty = update.Quantity - records.Quantity;

        //            _db.ProductBalances.Update(update);

        //            records.Add(item);

        //        }
                
        //        //   records.Quantity = dailyRecordsModel.Quantity;

        //        //   records.Product = dailyRecordsModel.Product;
        //        //    records.Measure = dailyRecordsModel.Measure;


        //        //var products = _db.Products.Where(u => u.Measure == records.Measure&& u.Product==records.Product).FirstOrDefault();
        //        //var item = _db.Products.Find(products);
                

        //        //if (item.Product == records.Product&& item.Measure==records.Measure)
        //        //{
        //        //    records.UnitPrice = item.UnitPrice;
        //        //    records.Amount = records.UnitPrice * records.Quantity;


        //        //}

        //      //  var productsQty = _db.ProductBalances.Where(u => u.Measure == records.Measure && u.Product == records.Product).Select(u => u.Id).FirstOrDefault();
        //      //  var update = _db.ProductBalances.Find(productsQty);
        //      //  update.Quantity= update.Quantity - records.Quantity;
        //      ////  var updateQty = update.Quantity - records.Quantity;

        //      //  _db.ProductBalances.Update(update);

        //        _db.DailyRecords.AddRange(records);
        //        _db.SaveChanges();
        //    }
        //    return Ok();
        //}

        [HttpDelete]

        public async Task<ActionResult> Delete(int Id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var records = _db.DailyRecords.Where(x => x.AdminId == currentUser.Id && x.Id == Id).FirstOrDefault();
            _db.DailyRecords.Remove(records);
            _db.SaveChanges();
            return Ok();
        }
    }
}
