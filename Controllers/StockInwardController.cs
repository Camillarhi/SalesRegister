using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SalesRegister.ApplicationDbContex;
using SalesRegister.DTOs;
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
    public class StockInwardController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public StockInwardController(ApplicationDbContext db)
        {

            _db = db;
        }

        [HttpGet]

        public ActionResult<StockInwardModel> GetAll()
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
            IEnumerable<StockInwardModel> objList = _db.StockInwards.Where(x => x.AdminId == currentUser).Include(x => x.stockInwardDetails);
            return Ok(objList);
        }


        [HttpGet("{Id}")]

        public ActionResult Get(int? Id)
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            var obj = _db.StockInwards.Where(x => x.AdminId == currentUser && x.Id == Id).Include(x => x.stockInwardDetails).FirstOrDefault();


            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }


        [HttpPost]

        public ActionResult Post(IFormFile stockInward, [FromBody] StockInwardModelDTO stockInwardModelDTO )
        {
            if (ModelState.IsValid)
            {
                var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
                var stockInwards = new StockInwardModel
                {
                    AdminId = currentUser,
                    Date=DateTime.Now,
                    SupplierName=stockInwardModelDTO.SupplierName,
                    stockInwardDetails=new List<StockInwardDetailsModel>()
                };
                if (stockInward?.Length > 0)
                {
                    var stream = stockInward.OpenReadStream();

                    List<StockInwardDetailsModel> stockQuantity = new();

                    try
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;

                            for (var row = 2; row <= rowCount; row++)
                            {
                                try
                                {
                                   // var productCode = worksheet.Cells[row, 2].Value?.ToString();
                                    var product = worksheet.Cells[row, 3].Value?.ToString();
                                    var measure = worksheet.Cells[row, 4].Value?.ToString();
                                    var quantity = worksheet.Cells[row, 5].Value?.ToString();

                                    var qty = Convert.ToInt32(quantity);
                                    var produtName = (product).ToUpper().Substring(0, 3);
                                    var rnd = new Random();
                                    int num = rnd.Next(50);
                                    var date = DateTime.Now;
                                    var productsQty = new StockInwardDetailsModel()
                                    {
                                        Product = product,
                                        Measure = measure,
                                        Quantity = qty,
                                        AdminId=currentUser,
                                    };
                                    var productExistInDb = _db.Products.Where(x => x.AdminId == currentUser && x.ProductName == product).Include(x => x.ProductMeasures).FirstOrDefault();
                                    if(productExistInDb == null)
                                    {
                                        productsQty.ProductCode = produtName + num;
                                    }
                                    else
                                    {
                                        productsQty.ProductCode = productExistInDb.ProductCode;
                                    }
                                    stockInwards.stockInwardDetails.Add(productsQty);
                                    

                                }
                                catch (Exception ex)
                                {
                                    return BadRequest(ex);
                                }

                            }
                            _db.StockInwards.Add(stockInwards);
                            _db.SaveChanges();
                        }

                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }
                }
            }
            return Ok();
        }

       [HttpPatch("{Id}")]

       public ActionResult Approve (int id)
        {
            if (ModelState.IsValid)
            {
                if(id == 0)
                {
                    return BadRequest();
                }
                var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = _db.Users.Where(u => u.Email == currentUserEmail).Select(u => u.Id).FirstOrDefault();
                var stock = _db.StockInwards.Where(x => x.Id == id && x.AdminId == currentUser).Include(x => x.stockInwardDetails).FirstOrDefault();
                var stockBalances = new List<StockBalanceModel>();
                if (stock.Approve)
                {
                    return BadRequest();
                }
                else
                {
                    stock.Approve = true;
                    foreach(var item in stock.stockInwardDetails)
                    {
                        var stockBalance = new StockBalanceModel
                        {
                            Measure = item.Measure,
                            AdminId=item.AdminId,
                            Product=item.Product,
                            ProductCode=item.ProductCode,
                            Quantity=item.Quantity
                        };
                        stockBalances.Add(stockBalance);
                    }
                    _db.StockBalances.AddRange(stockBalances);
                    _db.StockInwards.Update(stock);
                    _db.SaveChanges();
                }


                string uniqueId = System.Guid.NewGuid().ToString();
                string measureUniqueId = System.Guid.NewGuid().ToString();
              //  var productExist = _db.Products.Where(x => x.AdminId == currentUser && x.ProductName == product).Include(x => x.ProductMeasures).FirstOrDefault();
                //if (productExist == null)
                //{
                //    var addProduct = new ProductsModel
                //    {
                //        AdminId = currentUser,
                //        ProductCode = produtName + num,
                //        Id = uniqueId,
                //        ProductName = product,
                //        ProductMeasures = new List<ProductMeasureModel>()
                //    };
                //    var addProductMeasure = new ProductMeasureModel
                //    {
                //        Id = measureUniqueId,
                //        Measure = measure,
                //        ProductId = addProduct.Id,
                //        Quantity = qty
                //    };
                //    addProduct.ProductMeasures.Add(addProductMeasure);
                //    _db.Products.Add(addProduct);
                //    _db.SaveChanges();
                //}
                //else
                //{
                //    var productMeasureExist = productExist.ProductMeasures.Where(x => x.Measure == measure).FirstOrDefault();
                //    if (productMeasureExist == null)
                //    {
                //        var addProductMeasure = new ProductMeasureModel
                //        {
                //            Id = measureUniqueId,
                //            Measure = measure,
                //            ProductId = productExist.Id,
                //            Quantity = qty
                //        };
                //        productExist.ProductMeasures.Add(addProductMeasure);
                //        _db.Products.Add(productExist);
                //        _db.SaveChanges();
                //    }
                //    else
                //    {
                //        productMeasureExist.Quantity = qty;
                //    }
                //}
            }
            return Ok();
        }
    }
}
