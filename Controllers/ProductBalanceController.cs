using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SalesRegister.ApplicationDbContex;
using SalesRegister.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductBalanceController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductBalanceController(ApplicationDbContext db)
        {

            _db = db;
        }

        [HttpGet]

        public ActionResult<StockBalanceModel> GetAll()
        {
            IEnumerable<StockBalanceModel> objList = _db.StockBalances;
            return Ok(objList);
        }


        [HttpGet("{Id}")]

        public ActionResult Get(int? Id)
        {
            if (Id == 0 || Id == null)
            {
                return NotFound();
            }
            var obj = _db.StockBalances.Find(Id);


            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }


        [HttpPost]

        public ActionResult Post(IFormFile productBalances)
        {
            if (ModelState.IsValid)
            {
                if (productBalances?.Length > 0)
                {
                    var stream = productBalances.OpenReadStream();

                    List<StockBalanceModel> productsQuantity = new List<StockBalanceModel>();

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
                                    var productCode = worksheet.Cells[row, 2].Value?.ToString();
                                    var product = worksheet.Cells[row, 3].Value?.ToString();
                                    var measure = worksheet.Cells[row, 4].Value?.ToString();
                                    var quantity = worksheet.Cells[row, 5].Value?.ToString();

                                    var qty = Convert.ToInt32(quantity);


                                    var productsQty = new StockBalanceModel()
                                    {
                                        ProductCode = productCode,
                                        Product = product,
                                        Measure = measure,
                                        Quantity = qty,

                                    };

                                    //productsQuantity.Add(productsQty);
                                    _db.StockBalances.Add(productsQty);
                                    _db.SaveChanges();
                                }
                                catch (Exception ex)
                                {
                                    return BadRequest(ex);
                                }

                            }
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

    }
}
