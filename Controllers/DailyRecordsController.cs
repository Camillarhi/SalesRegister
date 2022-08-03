using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesRegister.ApplicationDbContex;
using SalesRegister.DTOs;
using SalesRegister.HelperClass;
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
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.CreatedById).FirstOrDefault();
            IEnumerable<DailyRecordsModel> objList = _db.DailyRecords.Where(x => x.AdminId == currentUser);
            return Ok(objList);
        }

        [HttpGet("{Id}")]

        public async Task<ActionResult> Get(int? Id)
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.CreatedById).FirstOrDefault();
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            var obj = _db.DailyRecords.Where(x => x.AdminId == currentUser && x.Id == Id).FirstOrDefault();
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
                var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.CreatedById).FirstOrDefault();
                var record = new CustomerInvoiceModel();
                var records = new List<DailyRecordsModel>();
                var totAmt = new List<float>();
                record.SoldById = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
                record.AdminId = currentUser;
                record.CustomerName = customerInvoiceModel.CustomerName;
                record.PhoneNumber = customerInvoiceModel.PhoneNumber;
                record.Date = DateTime.Now;
                var custName =customerInvoiceModel.CustomerName.Substring(0, 3);
                var rnd = new Random();
                int num = rnd.Next(100);
                var getCompanyName = _db.CompanyName.FirstOrDefault();
                var companyName = getCompanyName.CompanyName.Substring(0, 3);
                record.InvoiceId = companyName + custName + num;
                record.InvoiceDetail = new List<CustomerInvoiceDetailModel>();

                foreach (var item in customerInvoiceModel.InvoiceDetail)
                {
                    var recordDetail = new CustomerInvoiceDetailModel
                    {
                        AdminId = record.AdminId,
                        Quantity = item.Quantity,
                        Measure = item.Measure,
                        MeasureId = item.MeasureId,
                        ProductId = item.ProductId,
                        Product = item.Product,
                        Date = record.Date,
                        InvoiceId = record.InvoiceId
                    };
                    var product = _db.Products.Where(x => x.AdminId == currentUser && x.Id == item.ProductId).Include(y => y.ProductMeasures).FirstOrDefault();
                    var unitPrice = product.ProductMeasures.Find(x => x.Id == item.MeasureId && x.ProductId == item.ProductId).UnitPrice;
                    var updateProductQty = product.ProductMeasures.Find(x => x.Id == item.MeasureId && x.ProductId == item.ProductId);
                    updateProductQty.Quantity -= recordDetail.Quantity;
                    recordDetail.UnitPrice = unitPrice;
                    recordDetail.Amount = unitPrice * recordDetail.Quantity;
                    var productsQty = _db.StockBalances.Where(u => u.Measure == recordDetail.Measure && u.Product == recordDetail.Product).Select(u => u.Id).FirstOrDefault();
                    var update = _db.StockBalances.Find(productsQty);
                    update.Quantity -= recordDetail.Quantity;
                    //i put update fun for stock and nit for product
                    _db.StockBalances.Update(update);
                    _db.Products.Update(product);
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
                    dailySales.SoldById = record.SoldById;  
                    dailySales.AdminId = record.AdminId;
                    dailySales.Quantity = item.Quantity;
                    dailySales.Product = item.Product;
                    dailySales.Measure = item.Measure;
                    dailySales.Date = DateTime.Now;
                    dailySales.PhoneNumber = customerInvoiceModel.PhoneNumber;
                    dailySales.CustomerName = record.CustomerName;
                    var product = _db.Products.Where(x => x.AdminId == currentUser && x.Id == item.ProductId).Include(y => y.ProductMeasures).FirstOrDefault();
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
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.CreatedById).FirstOrDefault();
            var records = _db.DailyRecords.Where(x => x.AdminId == currentUser && x.Id == Id).FirstOrDefault();
            _db.DailyRecords.Remove(records);
            _db.SaveChanges();
            return Ok();
        }
    }
}
